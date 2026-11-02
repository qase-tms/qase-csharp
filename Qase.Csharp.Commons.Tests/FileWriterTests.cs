using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Qase.Csharp.Commons.Writers;
using Xunit;

namespace Qase.Csharp.Commons.Tests
{
    public class FileWriterTests
    {
        [Fact]
        public async Task WriteRunAsync_ShouldWriteJsonWithoutByteOrderMark()
        {
            var root = Path.Combine(Path.GetTempPath(), $"qase-report-{Guid.NewGuid():N}");
            try
            {
                var writer = new FileWriter(root);
                writer.Prepare();

                await writer.WriteRunAsync("{}");

                var bytes = File.ReadAllBytes(Path.Combine(root, "run.json"));
                bytes.Should().Equal((byte)'{', (byte)'}');
            }
            finally
            {
                if (Directory.Exists(root))
                    Directory.Delete(root, true);
            }
        }
    }
}
