using System.Collections.Generic;
using System.Threading.Tasks;
using Qase.Csharp.Commons.Models.Domain;

public interface IInternalReporter : ICoreReporter
{
    Task<List<TestResult>> getResults();

    Task setResults(List<TestResult> results);
}
