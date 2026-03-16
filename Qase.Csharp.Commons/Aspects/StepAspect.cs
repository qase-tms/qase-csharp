using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Text.Json;
using AspectInjector.Broker;
using Qase.Csharp.Commons.Attributes;
using Qase.Csharp.Commons.Models.Domain;

namespace Qase.Csharp.Commons.Aspects
{
    [Aspect(Scope.Global)]
    public class StepAspect
    {
        private static readonly MethodInfo AsyncGenericHandler =
            typeof(StepAspect).GetMethod(nameof(WrapGenericAsync), BindingFlags.NonPublic | BindingFlags.Static);
    
        private static readonly MethodInfo AsyncHandler =
            typeof(StepAspect).GetMethod(nameof(WrapAsync), BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly MethodInfo SyncGenericHandler =
            typeof(StepAspect).GetMethod(nameof(WrapGenericSync), BindingFlags.NonPublic | BindingFlags.Static);
        
        private static readonly MethodInfo SyncHandler =
            typeof(StepAspect).GetMethod(nameof(WrapSync), BindingFlags.NonPublic | BindingFlags.Static);

        [Advice(Kind.Around)]
        public object Around([Argument(Source.Name)] string name,
            [Argument(Source.Arguments)] object[] args,
            [Argument(Source.Target)] Func<object[], object> target,
            [Argument(Source.Metadata)] MethodBase metadata,
            [Argument(Source.ReturnType)] Type returnType)
        {
            var stepParameters = metadata.GetParameters()
                .Zip(args, (parameter, value) => new
                {
                    parameter,
                    value
                })
                .ToDictionary(x => x.parameter.Name ?? "param", x => x.value?.ToString() ?? "null");

            var stepName = metadata.GetCustomAttribute<TitleAttribute>()?.Title ?? name;
            
            StartStep(metadata, stepName, step =>
            {
                if (stepParameters.Count == 0)
                {
                    return;
                }

                step.Data!.InputData = JsonSerializer.Serialize(stepParameters);
            });

            var executionResult = GetStepExecutionResult(returnType, target, args, metadata);

            return executionResult;
        }

        private static void StartStep(MethodBase metadata, string stepName, Action<StepResult>? configure = null)
        {
            if (metadata.GetCustomAttribute<StepAttribute>() != null)
            {
                ContextManager.StartStep(stepName, configure);
            }
        }

        private static void PassStep(MethodBase metadata)
        {
            if (metadata.GetCustomAttribute<StepAttribute>() != null)
            {
                ContextManager.PassStep();
            }
        }

        private static void ThrowStep(MethodBase metadata)
        {
            if (metadata.GetCustomAttribute<StepAttribute>() != null)
            {
                ContextManager.FailStep();
            }
        }

        private object GetStepExecutionResult(Type returnType, Func<object[], object> target, object[] args, MethodBase metadata)
        {
            if (typeof(Task).IsAssignableFrom(returnType))
            {
                var wrappedAsync = returnType.IsConstructedGenericType
                    ? AsyncGenericHandler.MakeGenericMethod(returnType.GenericTypeArguments[0])
                    : AsyncHandler;
                
                return wrappedAsync.Invoke(this, new object[] { target, args, metadata });
            }

            var wrappedSync = typeof(void).IsAssignableFrom(returnType)
                ? SyncHandler
                : SyncGenericHandler.MakeGenericMethod(returnType);

            return wrappedSync.Invoke(this, new object[] { target, args, metadata });
        }

        private static void WrapSync(Func<object[], object> target, object[] args, MethodBase metadata)
        {
            try
            {
                target(args);
                PassStep(metadata);
            }
            catch (Exception)
            {
                ThrowStep(metadata);
                throw;
            }
        }
        
        private static T WrapGenericSync<T>(Func<object[], object> target, object[] args, MethodBase metadata)
        {
            try
            {
                var result = (T)target(args);
                PassStep(metadata);
                return result;
            }
            catch (Exception)
            {
                ThrowStep(metadata);
                throw;
            }
        }
        
        private static async Task WrapAsync(Func<object[], object> target, object[] args, MethodBase metadata)
        {
            try
            {
                await (Task)target(args);
                PassStep(metadata);
            }
            catch (Exception)
            {
                ThrowStep(metadata);
                throw;
            }
        }

        private static async Task<T> WrapGenericAsync<T>(Func<object[], object> target, object[] args, MethodBase metadata)
        {
            try
            {
                var result = await (Task<T>)target(args);
                PassStep(metadata);
                return result;
            }
            catch (Exception)
            {
                ThrowStep(metadata);
                throw;
            }
        }
    }
}
