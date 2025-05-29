using System.Linq;
using System.Reflection;
using AspectInjector.Broker;

namespace Qase.Csharp.Commons.Aspects
{
    [Aspect(Scope.Global)]
    public class UseQaseFeatureAspect
    {
        [Advice(Kind.Before, Targets = Target.Method)]
        public void BeforeMethod([Argument(Source.Metadata)] MethodBase method, [Argument(Source.Arguments)] object[] args)
        {
            var assemblyName = method.DeclaringType?.Assembly.GetName().Name;
            
            var stepParameters = method.GetParameters()
                .Zip(args, (parameter, value) => new
                {
                    parameter,
                    value
                })
                .ToDictionary(x => x.parameter.Name ?? "param", x => x.value?.ToString() ?? "null");

            var parametersString = stepParameters.Any()
                ? $"({string.Join(", ", stepParameters.Select(p => $"{p.Key}: {p.Value}"))})"
                : string.Empty;

            var testName = $"{assemblyName}.{method.DeclaringType?.Name}.{method.Name}{parametersString}";
            ContextManager.SetTestCaseName(testName);
        }

        [Advice(Kind.After, Targets = Target.Method)]
        public void AfterMethod([Argument(Source.Metadata)] MethodBase method)
        {
            ContextManager.SaveSteps();
        }
    }
}
