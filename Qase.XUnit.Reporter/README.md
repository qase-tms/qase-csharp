# Qase TMS xUnit Reporter

Publish your test results easily and effectively with Qase TMS.

## Installation

To install the latest release version, follow the instructions below for NuGet.

### NuGet

Add the following package to your project:

```xml
<PackageReference Include="Qase.XUnit.Reporter" Version="1.0.1" />
```

## Getting Started

The xUnit reporter can auto-generate test cases and suites based on your test data. Test results from subsequent runs
will match the same test cases as long as their names and file paths remain unchanged.

You can also annotate tests with IDs of existing test cases from Qase.io before execution. This approach ensures a
reliable binding between your automated tests and test cases, even if you rename, move, or parameterize your tests.

### Metadata Annotations

- **`[QaseIds]`**: Set the IDs of the test case.
- **`[Title]`**: Set the title of the test case.
- **`[Fields]`**: Set custom fields for the test case.
- **`[Suites]`**: Specify the suite for the test case.
- **`[Ignore]`**: Ignore the test case in Qase. The test will execute, but results won't be sent to Qase.
- **`[Steps]`**: Add steps to the test case.

For detailed instructions on using attributes and methods, refer to [Usage](docs/usage.md).

### Example Test Case

Here's a simple example of using Qase annotations in an xUnit test:

```csharp
using Xunit;
using Qase.XUnit.Reporter;

[Suites("Example Suite", "Example Suite 2")]
[Fields("preconditions", "This is a precondition")]
public class SimpleTests
{
    [Fact]
    [QaseIds(1, 2)]
    [Title("Example Test")]
    [Fields("description", "This is an example test")]
    [Suites("Example Suite", "Example Suite 2")]
    public void Test()
    {
        Console.WriteLine("Running example test");
    }

    [Fact]
    [Ignore]
    public void Test2()
    {
        Console.WriteLine("This test will not be reported to Qase");
    }
    
    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(4, 5, 9)]
    public void Test3(int a, int b, int expected)
    {
        Console.WriteLine($"Running test with {a} and {b}");
    }
```

To execute your xUnit tests and report the results to Qase.io, use the following command:

```bash
dotnet test
```

After running the tests, results will be available at:

```
https://app.qase.io/run/QASE_PROJECT_CODE
```

## Configuration

The Qase xUnit reporter can be configured in multiple ways:

- **Configuration File**: Use a separate config file `qase.config.json`.
- **Environment Variables**: These override values from the configuration file.

### Example `qase.config.json`

```json
{
   "mode": "testops",
   "fallback": "report",
   "debug": true,
   "environment": "local",
   "report": {
      "driver": "local",
      "connection": {
         "local": {
            "path": "./build/qase-report",
            "format": "json"
         }
      }
   },
   "testops": {
      "api": {
         "token": "<token>",
         "host": "qase.io"
      },
      "run": {
         "title": "Regression Run",
         "description": "Description of the regression run",
         "complete": true
      },
      "defect": false,
      "project": "<project_code>",
      "batch": {
         "size": 100
      }
   }
}
```

## Requirements

- **xUnit**: Version 2.4.0 or higher is required.
- **.NET**: Version 6.0 or higher is required.

For further assistance, please refer to
the [Qase Authentication Documentation](https://developers.qase.io/#authentication).
