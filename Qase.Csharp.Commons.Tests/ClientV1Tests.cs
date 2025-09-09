using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Model;
using Qase.Csharp.Commons.Clients;
using Qase.Csharp.Commons.Config;
using Qase.Csharp.Commons.Core;
using FluentAssertions;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    public class ClientV1Tests
    {
        [Fact]
        public async Task CreateTestRunAsync_ShouldPassConfigurationsToRunCreate()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ClientV1>>();
            var mockRunApi = new Mock<IRunsApi>();
            var mockAttachmentsApi = new Mock<IAttachmentsApi>();
            var mockConfigurationsApi = new Mock<IConfigurationsApi>();

            var config = new QaseConfig
            {
                TestOps = new TestOpsConfig
                {
                    Project = "TEST",
                    Api = new ApiConfig
                    {
                        Token = "test-token",
                        Host = "qase.io"
                    },
                    Configurations = new ConfigurationsConfig
                    {
                        Values = new List<ConfigurationItem> 
                        { 
                            new ConfigurationItem { Name = "group1", Value = "value1" },
                            new ConfigurationItem { Name = "group2", Value = "value2" }
                        },
                        CreateIfNotExists = true
                    }
                }
            };

            var client = new ClientV1(mockLogger.Object, config, mockRunApi.Object, mockAttachmentsApi.Object, mockConfigurationsApi.Object);

            // Mock successful response
            var mockResponse = new Mock<ICreateRunApiResponse>();
            mockResponse.Setup(x => x.IsSuccessStatusCode).Returns(true);
            mockResponse.Setup(x => x.Ok()).Returns(new IdResponse
            {
                Result = new IdResponseAllOfResult
                {
                    Id = 12345
                }
            });

            // Mock configurations API response
            var mockConfigResponse = new Mock<IGetConfigurationsApiResponse>();
            mockConfigResponse.Setup(x => x.IsSuccessStatusCode).Returns(true);
            mockConfigResponse.Setup(x => x.Ok()).Returns(new ConfigurationListResponse
            {
                Result = new ConfigurationListResponseAllOfResult
                {
                    Entities = new List<ConfigurationGroup>
                    {
                        new ConfigurationGroup
                        {
                            Id = 1,
                            Title = "group1",
                            Configurations = new List<ModelConfiguration>
                            {
                                new ModelConfiguration { Id = 10, Title = "value1" }
                            }
                        },
                        new ConfigurationGroup
                        {
                            Id = 2,
                            Title = "group2",
                            Configurations = new List<ModelConfiguration>
                            {
                                new ModelConfiguration { Id = 20, Title = "value2" }
                            }
                        }
                    }
                }
            });

            mockConfigurationsApi.Setup(x => x.GetConfigurationsAsync(It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(mockConfigResponse.Object);

            mockRunApi.Setup(x => x.CreateRunAsync(It.IsAny<string>(), It.IsAny<RunCreate>(), It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(mockResponse.Object);

            // Act
            var result = await client.CreateTestRunAsync();

            // Assert
            result.Should().Be(12345);
            mockRunApi.Verify(x => x.CreateRunAsync(
                It.Is<string>(p => p == "TEST"),
                It.Is<RunCreate>(r => r.Configurations != null && r.Configurations.Count == 2 && 
                                     r.Configurations.Contains(10) && r.Configurations.Contains(20)),
                It.IsAny<System.Threading.CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateTestRunAsync_ShouldNotPassConfigurations_WhenConfigurationsIsEmpty()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ClientV1>>();
            var mockRunApi = new Mock<IRunsApi>();
            var mockAttachmentsApi = new Mock<IAttachmentsApi>();
            var mockConfigurationsApi = new Mock<IConfigurationsApi>();

            var config = new QaseConfig
            {
                TestOps = new TestOpsConfig
                {
                    Project = "TEST",
                    Api = new ApiConfig
                    {
                        Token = "test-token",
                        Host = "qase.io"
                    },
                    Configurations = new ConfigurationsConfig
                    {
                        Values = new List<ConfigurationItem>(), // Empty configurations
                        CreateIfNotExists = true
                    }
                }
            };

            var client = new ClientV1(mockLogger.Object, config, mockRunApi.Object, mockAttachmentsApi.Object, mockConfigurationsApi.Object);

            // Mock successful response
            var mockResponse = new Mock<ICreateRunApiResponse>();
            mockResponse.Setup(x => x.IsSuccessStatusCode).Returns(true);
            mockResponse.Setup(x => x.Ok()).Returns(new IdResponse
            {
                Result = new IdResponseAllOfResult
                {
                    Id = 12345
                }
            });

            mockRunApi.Setup(x => x.CreateRunAsync(It.IsAny<string>(), It.IsAny<RunCreate>(), It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(mockResponse.Object);

            // Act
            var result = await client.CreateTestRunAsync();

            // Assert
            result.Should().Be(12345);
            mockRunApi.Verify(x => x.CreateRunAsync(
                It.Is<string>(p => p == "TEST"),
                It.Is<RunCreate>(r => r.Configurations == null || r.Configurations.Count == 0),
                It.IsAny<System.Threading.CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateTestRunAsync_ShouldPassTagsAndConfigurations()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ClientV1>>();
            var mockRunApi = new Mock<IRunsApi>();
            var mockAttachmentsApi = new Mock<IAttachmentsApi>();
            var mockConfigurationsApi = new Mock<IConfigurationsApi>();

            var config = new QaseConfig
            {
                TestOps = new TestOpsConfig
                {
                    Project = "TEST",
                    Api = new ApiConfig
                    {
                        Token = "test-token",
                        Host = "qase.io"
                    },
                    Run = new RunConfig
                    {
                        Tags = new List<string> { "tag1", "tag2" }
                    },
                    Configurations = new ConfigurationsConfig
                    {
                        Values = new List<ConfigurationItem> 
                        { 
                            new ConfigurationItem { Name = "group1", Value = "value1" },
                            new ConfigurationItem { Name = "group2", Value = "value2" }
                        },
                        CreateIfNotExists = true
                    }
                }
            };

            var client = new ClientV1(mockLogger.Object, config, mockRunApi.Object, mockAttachmentsApi.Object, mockConfigurationsApi.Object);

            // Mock successful response
            var mockResponse = new Mock<ICreateRunApiResponse>();
            mockResponse.Setup(x => x.IsSuccessStatusCode).Returns(true);
            mockResponse.Setup(x => x.Ok()).Returns(new IdResponse
            {
                Result = new IdResponseAllOfResult
                {
                    Id = 12345
                }
            });

            // Mock configurations API response
            var mockConfigResponse = new Mock<IGetConfigurationsApiResponse>();
            mockConfigResponse.Setup(x => x.IsSuccessStatusCode).Returns(true);
            mockConfigResponse.Setup(x => x.Ok()).Returns(new ConfigurationListResponse
            {
                Result = new ConfigurationListResponseAllOfResult
                {
                    Entities = new List<ConfigurationGroup>
                    {
                        new ConfigurationGroup
                        {
                            Id = 1,
                            Title = "group1",
                            Configurations = new List<ModelConfiguration>
                            {
                                new ModelConfiguration { Id = 10, Title = "value1" }
                            }
                        },
                        new ConfigurationGroup
                        {
                            Id = 2,
                            Title = "group2",
                            Configurations = new List<ModelConfiguration>
                            {
                                new ModelConfiguration { Id = 20, Title = "value2" }
                            }
                        }
                    }
                }
            });

            mockConfigurationsApi.Setup(x => x.GetConfigurationsAsync(It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(mockConfigResponse.Object);

            mockRunApi.Setup(x => x.CreateRunAsync(It.IsAny<string>(), It.IsAny<RunCreate>(), It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(mockResponse.Object);

            // Act
            var result = await client.CreateTestRunAsync();

            // Assert
            result.Should().Be(12345);
            mockRunApi.Verify(x => x.CreateRunAsync(
                It.Is<string>(p => p == "TEST"),
                It.Is<RunCreate>(r => 
                    r.Tags != null && r.Tags.Count == 2 && r.Tags.Contains("tag1") && r.Tags.Contains("tag2") &&
                    r.Configurations != null && r.Configurations.Count == 2 && 
                    r.Configurations.Contains(10) && r.Configurations.Contains(20)),
                It.IsAny<System.Threading.CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateTestRunAsync_WithExternalLink_ShouldAttachExternalLink()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ClientV1>>();
            var mockRunApi = new Mock<IRunsApi>();
            var mockAttachmentsApi = new Mock<IAttachmentsApi>();
            var mockConfigurationsApi = new Mock<IConfigurationsApi>();

            var config = new QaseConfig
            {
                TestOps = new TestOpsConfig
                {
                    Project = "TEST",
                    Api = new ApiConfig
                    {
                        Token = "test-token",
                        Host = "qase.io"
                    },
                    Run = new RunConfig
                    {
                        Title = "Test Run",
                        Description = "Test Description",
                        ExternalLink = new TestOpsExternalLinkType
                        {
                            Type = ExternalLinkType.JiraCloud,
                            Link = "PROJ-123"
                        }
                    }
                }
            };

            var client = new ClientV1(mockLogger.Object, config, mockRunApi.Object, mockAttachmentsApi.Object, mockConfigurationsApi.Object);

            var runResponse = new IdResponse
            {
                Result = new IdResponseAllOfResult
                {
                    Id = 12345
                }
            };

            var apiResponse = new Mock<ICreateRunApiResponse>();
            apiResponse.Setup(x => x.IsSuccessStatusCode).Returns(true);
            apiResponse.Setup(x => x.Ok()).Returns(runResponse);

            mockRunApi.Setup(x => x.CreateRunAsync(
                It.IsAny<string>(),
                It.IsAny<RunCreate>(),
                It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(apiResponse.Object);

            var externalIssueResponse = new Mock<IRunUpdateExternalIssueApiResponse>();
            externalIssueResponse.Setup(x => x.IsSuccessStatusCode).Returns(true);

            mockRunApi.Setup(x => x.RunUpdateExternalIssueAsync(
                It.IsAny<string>(),
                It.IsAny<RunexternalIssues>(),
                It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(externalIssueResponse.Object);

            // Act
            var result = await client.CreateTestRunAsync();

            // Assert
            result.Should().Be(12345);
            mockRunApi.Verify(x => x.CreateRunAsync(
                It.Is<string>(p => p == "TEST"),
                It.IsAny<RunCreate>(),
                It.IsAny<System.Threading.CancellationToken>()), Times.Once);
            mockRunApi.Verify(x => x.RunUpdateExternalIssueAsync(
                It.Is<string>(p => p == "TEST"),
                It.Is<RunexternalIssues>(e => 
                    e.Type == RunexternalIssues.TypeEnum.JiraCloud &&
                    e.Links != null && e.Links.Count == 1 &&
                    e.Links[0].RunId == 12345 &&
                    e.Links[0].ExternalIssue == "PROJ-123"),
                It.IsAny<System.Threading.CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateTestRunAsync_WithJiraServerExternalLink_ShouldAttachCorrectType()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ClientV1>>();
            var mockRunApi = new Mock<IRunsApi>();
            var mockAttachmentsApi = new Mock<IAttachmentsApi>();
            var mockConfigurationsApi = new Mock<IConfigurationsApi>();

            var config = new QaseConfig
            {
                TestOps = new TestOpsConfig
                {
                    Project = "TEST",
                    Api = new ApiConfig
                    {
                        Token = "test-token",
                        Host = "qase.io"
                    },
                    Run = new RunConfig
                    {
                        Title = "Test Run",
                        Description = "Test Description",
                        ExternalLink = new TestOpsExternalLinkType
                        {
                            Type = ExternalLinkType.JiraServer,
                            Link = "PROJ-456"
                        }
                    }
                }
            };

            var client = new ClientV1(mockLogger.Object, config, mockRunApi.Object, mockAttachmentsApi.Object, mockConfigurationsApi.Object);

            var runResponse = new IdResponse
            {
                Result = new IdResponseAllOfResult
                {
                    Id = 67890
                }
            };

            var apiResponse = new Mock<ICreateRunApiResponse>();
            apiResponse.Setup(x => x.IsSuccessStatusCode).Returns(true);
            apiResponse.Setup(x => x.Ok()).Returns(runResponse);

            mockRunApi.Setup(x => x.CreateRunAsync(
                It.IsAny<string>(),
                It.IsAny<RunCreate>(),
                It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(apiResponse.Object);

            var externalIssueResponse = new Mock<IRunUpdateExternalIssueApiResponse>();
            externalIssueResponse.Setup(x => x.IsSuccessStatusCode).Returns(true);

            mockRunApi.Setup(x => x.RunUpdateExternalIssueAsync(
                It.IsAny<string>(),
                It.IsAny<RunexternalIssues>(),
                It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(externalIssueResponse.Object);

            // Act
            var result = await client.CreateTestRunAsync();

            // Assert
            result.Should().Be(67890);
            mockRunApi.Verify(x => x.RunUpdateExternalIssueAsync(
                It.Is<string>(p => p == "TEST"),
                It.Is<RunexternalIssues>(e => 
                    e.Type == RunexternalIssues.TypeEnum.JiraServer &&
                    e.Links != null && e.Links.Count == 1 &&
                    e.Links[0].RunId == 67890 &&
                    e.Links[0].ExternalIssue == "PROJ-456"),
                It.IsAny<System.Threading.CancellationToken>()), Times.Once);
        }
    }
} 
