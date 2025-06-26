using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Qase.Csharp.Commons;
using Qase.Csharp.Commons.Clients;
using Qase.Csharp.Commons.Config;
using Qase.Csharp.Commons.Reporters;
using Qase.Csharp.Commons.Writers;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddQaseServices_ShouldRegisterConfiguration()
        {
            // Arrange
            var services = new ServiceCollection();
            var config = new QaseConfig();

            // Act
            services.AddQaseServices(config);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var registeredConfig = serviceProvider.GetService<QaseConfig>();
            registeredConfig.Should().NotBeNull();
            registeredConfig.Should().BeSameAs(config);
        }

        [Fact]
        public void AddQaseServices_ShouldRegisterLogging()
        {
            // Arrange
            var services = new ServiceCollection();
            var config = new QaseConfig();

            // Act
            services.AddQaseServices(config);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<ServiceCollectionExtensionsTests>>();
            logger.Should().NotBeNull();
        }

        [Fact]
        public void AddQaseServices_ShouldRegisterClientV1()
        {
            // Arrange
            var services = new ServiceCollection();
            var config = new QaseConfig
            {
                TestOps = new TestOpsConfig
                {
                    Api = new ApiConfig
                    {
                        Host = "qase.io",
                        Token = "test-token"
                    }
                }
            };

            // Act
            services.AddQaseServices(config);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var clientV1 = serviceProvider.GetService<ClientV1>();
            clientV1.Should().NotBeNull();
        }

        [Fact]
        public void AddQaseServices_ShouldRegisterClientV2()
        {
            // Arrange
            var services = new ServiceCollection();
            var config = new QaseConfig
            {
                TestOps = new TestOpsConfig
                {
                    Api = new ApiConfig
                    {
                        Host = "qase.io",
                        Token = "test-token"
                    }
                }
            };

            // Act
            services.AddQaseServices(config);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var clientV2 = serviceProvider.GetService<ClientV2>();
            clientV2.Should().NotBeNull();
        }

        [Fact]
        public void AddQaseServices_ShouldRegisterIClient()
        {
            // Arrange
            var services = new ServiceCollection();
            var config = new QaseConfig
            {
                TestOps = new TestOpsConfig
                {
                    Api = new ApiConfig
                    {
                        Host = "qase.io",
                        Token = "test-token"
                    }
                }
            };

            // Act
            services.AddQaseServices(config);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var client = serviceProvider.GetService<IClient>();
            client.Should().NotBeNull();
            client.Should().BeOfType<ClientV2>();
        }

        [Fact]
        public void AddQaseServices_ShouldRegisterFileWriter()
        {
            // Arrange
            var services = new ServiceCollection();
            var config = new QaseConfig
            {
                Report = new ReportConfig
                {
                    Connection = new ConnectionConfig
                    {
                        Local = new LocalConfig
                        {
                            Path = "./test-report"
                        }
                    }
                }
            };

            // Act
            services.AddQaseServices(config);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var fileWriter = serviceProvider.GetService<FileWriter>();
            fileWriter.Should().NotBeNull();
        }

        [Fact]
        public void AddQaseServices_WithTestOpsMode_ShouldRegisterTestopsReporter()
        {
            // Arrange
            var services = new ServiceCollection();
            var config = new QaseConfig
            {
                Mode = Mode.TestOps,
                TestOps = new TestOpsConfig
                {
                    Api = new ApiConfig
                    {
                        Host = "qase.io",
                        Token = "test-token"
                    }
                }
            };

            // Act
            services.AddQaseServices(config);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var reporter = serviceProvider.GetService<IInternalReporter>();
            reporter.Should().NotBeNull();
            reporter.Should().BeOfType<TestopsReporter>();
        }

        [Fact]
        public void AddQaseServices_WithReportMode_ShouldRegisterFileReporter()
        {
            // Arrange
            var services = new ServiceCollection();
            var config = new QaseConfig
            {
                Mode = Mode.Report,
                Report = new ReportConfig
                {
                    Connection = new ConnectionConfig
                    {
                        Local = new LocalConfig
                        {
                            Path = "./test-report"
                        }
                    }
                }
            };

            // Act
            services.AddQaseServices(config);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var reporter = serviceProvider.GetService<IInternalReporter>();
            reporter.Should().NotBeNull();
            reporter.Should().BeOfType<FileReporter>();
        }

        [Fact]
        public void AddQaseServices_WithTestOpsFallback_ShouldRegisterFallbackFactory()
        {
            // Arrange
            var services = new ServiceCollection();
            var config = new QaseConfig
            {
                Mode = Mode.Report,
                Fallback = Mode.TestOps,
                TestOps = new TestOpsConfig
                {
                    Api = new ApiConfig
                    {
                        Host = "qase.io",
                        Token = "test-token"
                    }
                },
                Report = new ReportConfig
                {
                    Connection = new ConnectionConfig
                    {
                        Local = new LocalConfig
                        {
                            Path = "./test-report"
                        }
                    }
                }
            };

            // Act
            services.AddQaseServices(config);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var fallbackFactory = serviceProvider.GetService<Func<IInternalReporter>>();
            fallbackFactory.Should().NotBeNull();
            
            var fallbackReporter = fallbackFactory!();
            fallbackReporter.Should().BeOfType<TestopsReporter>();
        }

        [Fact]
        public void AddQaseServices_ShouldRegisterCoreReporter()
        {
            // Arrange
            var services = new ServiceCollection();
            var config = new QaseConfig
            {
                Mode = Mode.Report,
                Report = new ReportConfig
                {
                    Connection = new ConnectionConfig
                    {
                        Local = new LocalConfig
                        {
                            Path = "./test-report"
                        }
                    }
                }
            };

            // Act
            services.AddQaseServices(config);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var coreReporter = serviceProvider.GetService<ICoreReporter>();
            coreReporter.Should().NotBeNull();
            coreReporter.Should().BeOfType<CoreReporter>();
        }

        [Fact]
        public void AddQaseServices_ShouldReturnServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();
            var config = new QaseConfig();

            // Act
            var result = services.AddQaseServices(config);

            // Assert
            result.Should().BeSameAs(services);
        }

        [Fact]
        public void AddQaseServices_WithDebugEnabled_ShouldConfigureDebugLogging()
        {
            // Arrange
            var services = new ServiceCollection();
            var config = new QaseConfig
            {
                Debug = true
            };

            // Act
            services.AddQaseServices(config);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<ServiceCollectionExtensionsTests>>();
            logger.Should().NotBeNull();
        }
    }
} 
