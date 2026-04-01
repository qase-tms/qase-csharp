using System.Reflection;
using Reqnroll.Plugins;
using Reqnroll.UnitTestProvider;
using Qase.Csharp.Commons.Reporters;

[assembly: RuntimePlugin(typeof(Qase.Reqnroll.Reporter.QaseReqnrollPlugin))]

namespace Qase.Reqnroll.Reporter
{
    /// <summary>
    /// Reqnroll runtime plugin that integrates Qase TMS reporting.
    /// Automatically discovered by Reqnroll via the RuntimePlugin assembly attribute.
    /// </summary>
    public class QaseReqnrollPlugin : IRuntimePlugin
    {
        /// <summary>
        /// Initializes the plugin by registering Qase reporter in the DI container
        /// and registering this assembly's bindings with Reqnroll.
        /// </summary>
        public void Initialize(
            RuntimePluginEvents runtimePluginEvents,
            RuntimePluginParameters runtimePluginParameters,
            UnitTestProviderConfiguration unitTestProviderConfiguration)
        {
            runtimePluginEvents.CustomizeGlobalDependencies += (sender, args) =>
            {
                args.ObjectContainer.RegisterInstanceAs<ICoreReporter>(
                    CoreReporterFactory.GetInstance());
            };

            runtimePluginEvents.ConfigurationDefaults += (sender, args) =>
            {
                args.ReqnrollConfiguration.AdditionalStepAssemblies.Add(
                    Assembly.GetExecutingAssembly().FullName);
            };
        }
    }
}
