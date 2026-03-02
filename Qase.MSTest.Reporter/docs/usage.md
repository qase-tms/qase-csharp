# Qase MSTest Reporter - Usage Guide

## Table of Contents

- [Installation](#installation)
- [Configuration](#configuration)
- [Linking Test Cases](#linking-test-cases)
- [Test Metadata](#test-metadata)
- [Test Steps](#test-steps)
- [Attachments](#attachments)
- [Parameters](#parameters)
- [Ignoring Tests](#ignoring-tests)
- [Comments](#comments)

## Installation

To install the Qase MSTest Reporter, use the NuGet package manager:

### NuGet CLI

```bash
dotnet add package Qase.MSTest.Reporter
```

### PackageReference

Add the following to your `.csproj` file:

```xml
<PackageReference Include="Qase.MSTest.Reporter" Version="1.0.0" />
```

The `Qase.Csharp.Commons` package is included as a transitive dependency and provides the attributes and metadata APIs used throughout this guide.

## Configuration

The Qase MSTest reporter can be configured using two methods:

1. **Configuration file**: `qase.config.json` in your project root
2. **Environment variables**: Override values from the configuration file

### Configuration File

Create a `qase.config.json` file with the following structure:

```json
{
  "mode": "testops",
  "fallback": "report",
  "debug": true,
  "environment": "local",
  "logging": {
    "console": true,
    "file": false
  },
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

### Environment Variables

You can override configuration values using environment variables:

- `QASE_MODE` - Reporter mode (testops, report, off)
- `QASE_TESTOPS_API_TOKEN` - Your Qase API token
- `QASE_TESTOPS_PROJECT` - Your Qase project code

For the complete list of configuration options, see [Configuration Reference](../../Qase.Csharp.Commons/README.md#configuration).

## Linking Test Cases

Use the `[QaseIds]` attribute to link your automated tests to existing test cases in Qase. This ensures a reliable binding even if you rename, move, or parameterize your tests.

### Single Test Case ID

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qase.Csharp.Commons.Attributes;

[TestClass]
public class LoginTests
{
    [TestMethod]
    [QaseIds(123)]
    public void UserCanLogin()
    {
        // Test implementation
    }
}
```

### Multiple Test Case IDs

```csharp
[TestMethod]
[QaseIds(123, 456, 789)]
public void UserCanLoginMultipleCases()
{
    // Test implementation
}
```

### Parameterized Tests with QaseIds

```csharp
[TestMethod]
[DataRow("user1", "password1")]
[DataRow("user2", "password2")]
[QaseIds(200)]
public void LoginWithDifferentUsers(string username, string password)
{
    // Test implementation
}
```

### Auto-Generation

If you don't specify `[QaseIds]`, the reporter will automatically generate test cases in Qase based on your test names and file paths. Subsequent runs will match the same test cases as long as names and paths remain unchanged.

## Test Metadata

Enhance your test cases with metadata using attributes.

### Title

Set a custom title for your test case:

```csharp
[TestMethod]
[Title("User can successfully log in with valid credentials")]
public void UserCanLogin()
{
    // Test implementation
}
```

### Fields

Add custom fields to your test case:

```csharp
[TestMethod]
[Fields("priority", "high")]
[Fields("component", "authentication")]
[Fields("severity", "critical")]
public void CriticalLoginTest()
{
    // Test implementation
}
```

### Suites

Organize test cases into suites:

```csharp
[TestMethod]
[Suites("API", "Authentication", "Login")]
public void TestInSuite()
{
    // Test implementation
}
```

### Class-Level Attributes

Apply attributes at the class level to affect all tests in the class:

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qase.Csharp.Commons.Attributes;

[TestClass]
[Suites("API", "User Management")]
[Fields("component", "user-api")]
public class UserApiTests
{
    [TestMethod]
    [Title("Create User with Valid Data")]
    public void CreateUser()
    {
        // Inherits Suites and Fields from class level
    }

    [TestMethod]
    [Title("Delete User")]
    [Suites("API", "User Management", "Deletion")]
    public void DeleteUser()
    {
        // Overrides Suites, inherits Fields from class level
    }
}
```

### Combined Metadata Example

```csharp
[TestMethod]
[QaseIds(1001)]
[Title("Create User - Valid Data")]
[Fields("priority", "high")]
[Fields("component", "User API")]
[Suites("API", "User Management", "Creation")]
public void CreateUser_WithValidData_ShouldSucceed()
{
    // Test implementation
    Assert.IsTrue(true);
}
```

## Test Steps

The Qase MSTest reporter supports structured test steps using the `[Step]` attribute.

**Important**: You must add the `[Qase]` attribute to the test method to enable step tracking.

### Basic Steps

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qase.Csharp.Commons.Attributes;

[TestClass]
public class LoginTests
{
    [TestMethod]
    [Qase]
    public void UserCanLogin()
    {
        OpenLoginPage();
        EnterCredentials();
        ClickLoginButton();
        VerifyDashboard();
    }

    [Step]
    public void OpenLoginPage()
    {
        // Step implementation
    }

    [Step]
    public void EnterCredentials()
    {
        // Step implementation
    }

    [Step]
    public void ClickLoginButton()
    {
        // Step implementation
    }

    [Step]
    public void VerifyDashboard()
    {
        // Step implementation
    }
}
```

### Nested Steps

Steps can call other steps to create a hierarchy:

```csharp
[TestMethod]
[Qase]
public void ComplexUserWorkflow()
{
    LoginAsUser();
    PerformActions();
    Logout();
}

[Step]
public void LoginAsUser()
{
    OpenLoginPage();
    EnterCredentials();
    ClickLoginButton();
}

[Step]
public void OpenLoginPage()
{
    // Step implementation
}

[Step]
public void EnterCredentials()
{
    // Step implementation
}

[Step]
public void ClickLoginButton()
{
    // Step implementation
}

[Step]
public void PerformActions()
{
    // Step implementation
}

[Step]
public void Logout()
{
    // Step implementation
}
```

For more details, see [Test Steps Guide](STEPS.md).

## Attachments

Attach files, screenshots, and other content to your test results.

**Important**: You must add the `[Qase]` attribute to the test method to enable attachments.

### Attach a File by Path

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qase.Csharp.Commons.Attributes;
using Qase.Csharp.Commons;

[TestClass]
public class ReportTests
{
    [TestMethod]
    [Qase]
    public void TestWithAttachment()
    {
        // Test implementation
        Metadata.Attach("path/to/screenshot.png");
    }
}
```

### Attach Multiple Files

```csharp
[TestMethod]
[Qase]
public void TestWithMultipleAttachments()
{
    // Test implementation
    Metadata.Attach(new List<string>
    {
        "path/to/screenshot1.png",
        "path/to/screenshot2.png",
        "path/to/log.txt"
    });
}
```

### Attach Content from Byte Array

```csharp
using System.Text;

[TestMethod]
[Qase]
public void TestWithByteArrayAttachment()
{
    // Generate or capture content
    byte[] logContent = Encoding.UTF8.GetBytes("Test execution log data");

    // Attach with a custom filename
    Metadata.Attach(logContent, "execution.log");
}
```

For more details, see [Attachments Guide](ATTACHMENTS.md).

## Parameters

MSTest tests with `[DataRow]` and `[DynamicData]` parameters are automatically captured and reported to Qase.

### DataRow with Parameters

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qase.Csharp.Commons.Attributes;

[TestClass]
public class CalculatorTests
{
    [TestMethod]
    [DataRow(2, 3, 5)]
    [DataRow(10, 20, 30)]
    [DataRow(-5, 5, 0)]
    [QaseIds(300)]
    [Title("Addition Test")]
    public void AdditionTest(int a, int b, int expected)
    {
        int result = a + b;
        Assert.AreEqual(expected, result);
    }
}
```

Each set of parameters creates a separate test result in Qase with the parameter values captured and displayed. This allows you to track results for each data variation independently.

### DynamicData with Parameters

Use `[DynamicData]` for data-driven tests where test data is provided by a method or property. This is useful when you need to generate test data dynamically or when you have complex test data that doesn't fit in `[DataRow]` attributes.

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qase.Csharp.Commons.Attributes;

[TestClass]
public class PaginationTests
{
    [TestMethod]
    [DynamicData(nameof(PaginationData), DynamicDataSourceType.Method)]
    [QaseIds(503)]
    [Title("Pagination returns correct page size")]
    public void Pagination_ReturnsCorrectPageSize(int pageSize, int pageNumber)
    {
        var totalItems = 120;
        var startIndex = (pageNumber - 1) * pageSize;
        var itemsOnPage = Math.Min(pageSize, totalItems - startIndex);

        Assert.IsTrue(itemsOnPage > 0, $"Page {pageNumber} with size {pageSize} should have items");
        Assert.IsTrue(itemsOnPage <= pageSize);
    }

    private static IEnumerable<object[]> PaginationData()
    {
        yield return new object[] { 10, 1 };
        yield return new object[] { 25, 2 };
        yield return new object[] { 50, 3 };
    }
}
```

`[DynamicData]` works similarly to NUnit's `[TestCaseSource]`. The source method must be `static` and return `IEnumerable<object[]>`. You can use `DynamicDataSourceType.Method` to reference a method, or `DynamicDataSourceType.Property` to reference a static property.

## Ignoring Tests

Use the `[Ignore]` attribute from Qase to exclude specific tests from being reported to Qase. The test will still execute normally, but results won't be sent to Qase.

> **Warning:** MSTest has a built-in `[Ignore]` attribute for skipping tests. The Qase `[Ignore]` attribute (which excludes tests from Qase reporting but still runs them) **MUST** be fully qualified as `[Qase.Csharp.Commons.Attributes.Ignore]` to avoid ambiguity with MSTest's built-in `[Ignore]` attribute.

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qase.Csharp.Commons.Attributes;

[TestClass]
public class ExampleTests
{
    [TestMethod]
    [Qase.Csharp.Commons.Attributes.Ignore]
    public void TestNotReportedToQase()
    {
        // This test runs but results are not sent to Qase
        Assert.IsTrue(true);
    }

    [TestMethod]
    public void NormalTest()
    {
        // This test is reported normally
        Assert.IsTrue(true);
    }
}
```

**Note**: MSTest's built-in `[Ignore]` attribute prevents the test from running entirely and reports it to Qase with a "Skipped" status. The Qase `[Qase.Csharp.Commons.Attributes.Ignore]` attribute is different -- it lets the test run but excludes the result from Qase reporting.

## Comments

Add comments to your test results using the `Metadata.Comment()` method.

**Important**: You must add the `[Qase]` attribute to the test method to enable comments.

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qase.Csharp.Commons.Attributes;
using Qase.Csharp.Commons;

[TestClass]
public class CommentTests
{
    [TestMethod]
    [Qase]
    public void TestWithComment()
    {
        // Test implementation
        Metadata.Comment("This test verifies the login functionality");

        // Add more comments as needed
        Metadata.Comment("User logged in successfully");
    }
}
```

Comments appear in the test result details in Qase and are useful for adding context, debugging information, or execution notes.
