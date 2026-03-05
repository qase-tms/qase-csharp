using Microsoft.Testing.Platform.Builder;
using Microsoft.Testing.Platform.Extensions;

namespace Qase.XUnit.V3.Reporter
{
    public static class TestingPlatformBuilderHook
    {
        public static void AddExtensions(ITestApplicationBuilder testApplicationBuilder, string[] arguments)
        {
            var compositeFactory = new CompositeExtensionFactory<QaseXUnitV3Extension>(
                () => new QaseXUnitV3Extension());
            testApplicationBuilder.TestHost.AddDataConsumer(compositeFactory);
            testApplicationBuilder.TestHost.AddTestSessionLifetimeHandle(compositeFactory);
        }
    }
}
