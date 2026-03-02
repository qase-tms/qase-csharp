using Microsoft.Testing.Platform.Builder;
using Microsoft.Testing.Platform.Extensions;

namespace Qase.MSTest.Reporter
{
    public static class TestingPlatformBuilderHook
    {
        public static void AddExtensions(ITestApplicationBuilder testApplicationBuilder, string[] arguments)
        {
            var compositeFactory = new CompositeExtensionFactory<QaseMSTestExtension>(
                () => new QaseMSTestExtension());
            testApplicationBuilder.TestHost.AddDataConsumer(compositeFactory);
            testApplicationBuilder.TestHost.AddTestSessionLifetimeHandle(compositeFactory);
        }
    }
}
