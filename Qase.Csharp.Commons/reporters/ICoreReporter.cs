using System.Threading.Tasks;
using Qase.Csharp.Commons.Models.Domain;

public interface ICoreReporter
{
    Task startTestRun();

    Task completeTestRun();

    Task addResult(TestResult result);

    Task uploadResults();
}
