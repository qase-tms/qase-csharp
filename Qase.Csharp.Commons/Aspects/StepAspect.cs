using System;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
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
            if (IsValueTask(returnType))
            {
                return WrapValueTask(returnType, target, args, metadata);
            }

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

            try
            {
                return wrappedSync.Invoke(this, new object[] { target, args, metadata });
            }
            catch (TargetInvocationException ex) when (ex.InnerException != null)
            {
                // Rethrow the step's own exception with its original stack trace.
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        // netstandard2.0 has no ValueTask, so it is recognised by name; without this a
        // ValueTask step would be marked passed before it ran to completion.
        private static bool IsValueTask(Type returnType)
        {
            var name = returnType.IsConstructedGenericType
                ? returnType.GetGenericTypeDefinition().FullName
                : returnType.FullName;
            return name == "System.Threading.Tasks.ValueTask" || name == "System.Threading.Tasks.ValueTask`1";
        }

        private object WrapValueTask(Type returnType, Func<object[], object> target, object[] args, MethodBase metadata)
        {
            object AsTask(object[] a)
            {
                var valueTask = target(a);
                return returnType.GetMethod("AsTask")!.Invoke(valueTask, null)!;
            }

            var handler = returnType.IsConstructedGenericType
                ? AsyncGenericHandler.MakeGenericMethod(returnType.GenericTypeArguments[0])
                : AsyncHandler;
            var task = handler.Invoke(this, new object[] { (Func<object[], object>)AsTask, args, metadata });

            return Activator.CreateInstance(returnType, task)!;
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
