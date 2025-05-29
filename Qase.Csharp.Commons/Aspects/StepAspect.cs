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
        private static readonly MethodInfo AsyncHandler =
            typeof(StepAspect).GetMethod(nameof(WrapAsync), BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly MethodInfo SyncHandler =
            typeof(StepAspect).GetMethod(nameof(WrapSync), BindingFlags.NonPublic | BindingFlags.Static);

        [Advice(Kind.Around)]
        public object Around([Argument(Source.Name)] string name,
            [Argument(Source.Arguments)] object[] args,
            [Argument(Source.Target)] Func<object[], object> target,
            [Argument(Source.Metadata)] MethodBase metadata,
            [Argument(Source.ReturnType)] Type returnType)
        {
            object executionResult;

            var stepParameters = metadata.GetParameters()
                .Zip(args, (parameter, value) => new
                {
                    parameter,
                    value
                })
                .ToDictionary(x => x.parameter.Name ?? "param", x => x.value?.ToString() ?? "null");

            var stepName = metadata.GetCustomAttribute<TitleAttribute>()?.Title ?? name;
            try
            {
                StartStep(metadata, stepName, step =>
                {
                    if (stepParameters.Count == 0)
                    {
                        return;
                    }

                    step.Data!.InputData = JsonSerializer.Serialize(stepParameters);
                });

                executionResult = GetStepExecutionResult(returnType, target, args);

                PassStep(metadata);
            }
            catch (Exception)
            {
                ThrowStep(metadata);
                throw;
            }

            return executionResult;
        }

        private static void StartStep(MethodBase metadata, string stepName, Action<StepResult> configure = null)
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

        private object GetStepExecutionResult(Type returnType, Func<object[], object> target, object[] args)
        {
            if (typeof(Task).IsAssignableFrom(returnType))
            {
                var syncResultType = returnType.IsConstructedGenericType
                    ? returnType.GenericTypeArguments[0]
                    : typeof(object);
                return AsyncHandler.MakeGenericMethod(syncResultType)
                    .Invoke(this, new object[] { target, args });
            }

            if (typeof(void).IsAssignableFrom(returnType))
            {
                return target(args);
            }

            return SyncHandler.MakeGenericMethod(returnType)
                .Invoke(this, new object[] { target, args });
        }

        private static T WrapSync<T>(Func<object[], object> target, object[] args)
        {
            try
            {
                return (T)target(args);
            }
            catch (Exception e)
            {
                return default(T);
            }
        }

        private static async Task<T> WrapAsync<T>(Func<object[], object> target, object[] args)
        {
            try
            {
                return await (Task<T>)target(args);
            }
            catch (Exception e)
            {
                return default(T);
            }
        }
    }
}
