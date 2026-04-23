#nullable enable

using Qase.Csharp.Commons.Models.Domain;

namespace Qase.Csharp.Commons.Utils
{
    public interface ITestResultBuilder
    {
        TestResult Build(RawTestData raw);
    }
}
