using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Model;
using Qase.Csharp.Commons.Clients;
using Qase.Csharp.Commons.Config;
using Xunit;
using DomainAttachment = Qase.Csharp.Commons.Models.Domain.Attachment;

namespace Qase.Csharp.Commons.Tests
{
    public class ClientV1AttachmentTests
    {
        private readonly Mock<IAttachmentsApi> _attachmentsApi = new Mock<IAttachmentsApi>();
        private readonly List<List<(Stream Stream, string FileName)>> _requests = new List<List<(Stream Stream, string FileName)>>();

        private ClientV1 CreateClient()
        {
            var config = new QaseConfig();
            config.TestOps.Project = "PRJ";

            _attachmentsApi
                .Setup(a => a.UploadAttachmentAsync("PRJ", It.IsAny<Option<List<(Stream Stream, string FileName)>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string _, Option<List<(Stream Stream, string FileName)>> files, CancellationToken _) =>
                {
                    _requests.Add(files.Value!);
                    var response = new Mock<IUploadAttachmentApiResponse>();
                    response.SetupGet(r => r.IsSuccessStatusCode).Returns(true);
                    response.Setup(r => r.Ok()).Returns(new AttachmentUploadsResponse(
                        result: files.Value!.Select(f => new Attachmentupload(hash: f.FileName)).ToList()));
                    return response.Object;
                });

            return new ClientV1(Mock.Of<ILogger<ClientV1>>(), config, Mock.Of<IRunsApi>(), _attachmentsApi.Object, Mock.Of<IConfigurationsApi>());
        }

        [Fact]
        public async Task UploadAttachmentsAsync_ShouldSplitMoreThanTwentyFilesIntoSeveralRequests()
        {
            var attachments = Enumerable.Range(0, 21)
                .Select(i => new DomainAttachment { FileName = "shot.png", ContentBytes = new[] { (byte)i } })
                .ToList();

            var hashes = await CreateClient().UploadAttachmentsAsync(attachments);

            hashes.Should().HaveCount(21);
            _requests.Select(r => r.Count).Should().Equal(20, 1);
        }

        [Fact]
        public async Task UploadAttachmentsAsync_ShouldNotTouchTheFileSystem_ForInMemoryContent()
        {
            var userFile = Path.Combine(Path.GetTempPath(), $"qase-user-{System.Guid.NewGuid():N}.txt");
            File.WriteAllText(userFile, "user data");
            try
            {
                var attachments = new List<DomainAttachment>
                {
                    new DomainAttachment { FileName = userFile, Content = "reporter data" }
                };

                await CreateClient().UploadAttachmentsAsync(attachments);

                File.ReadAllText(userFile).Should().Be("user data");
                _requests.Single().Single().FileName.Should().Be(Path.GetFileName(userFile));
            }
            finally
            {
                File.Delete(userFile);
            }
        }
    }
}
