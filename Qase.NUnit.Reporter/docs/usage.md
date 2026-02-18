# Qase NUnit Reporter - Usage Guide

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

To install the Qase NUnit Reporter, use the NuGet package manager:

### NuGet CLI

```bash
dotnet add package Qase.NUnit.Reporter
```

### PackageReference

Add the following to your `.csproj` file:

```xml
<PackageReference Include="Qase.NUnit.Reporter" Version="1.1.1" />
```

The `Qase.Csharp.Commons` package is included as a transitive dependency and provides the attributes and metadata APIs used throughout this guide.

## Configuration

The Qase NUnit reporter can be configured using two methods:

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
using NUnit.Framework;
using Qase.Csharp.Commons.Attributes;

[TestFixture]
public class LoginTests
{
    [Test]
    [QaseIds(123)]
    public void UserCanLogin()
    {
        // Test implementation
    }
}
```

### Multiple Test Case IDs

```csharp
[Test]
[QaseIds(123, 456, 789)]
public void UserCanLoginMultipleCases()
{
    // Test implementation
}
```

### Parameterized Tests with QaseIds

```csharp
[Test]
[TestCase("user1", "password1")]
[TestCase("user2", "password2")]
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
[Test]
[Title("User can successfully log in with valid credentials")]
public void UserCanLogin()
{
    // Test implementation
}
```

### Fields

Add custom fields to your test case:

```csharp
[Test]
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
[Test]
[Suites("API", "Authentication", "Login")]
public void TestInSuite()
{
    // Test implementation
}
```

### Class-Level Attributes

Apply attributes at the class level to affect all tests in the class:

```csharp
using NUnit.Framework;
using Qase.Csharp.Commons.Attributes;

[TestFixture]
[Suites("API", "User Management")]
[Fields("component", "user-api")]
public class UserApiTests
{
    [Test]
    [Title("Create User with Valid Data")]
    public void CreateUser()
    {
        // Inherits Suites and Fields from class level
    }

    [Test]
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
[Test]
[QaseIds(1001)]
[Title("Create User - Valid Data")]
[Fields("priority", "high")]
[Fields("component", "User API")]
[Suites("API", "User Management", "Creation")]
public void CreateUser_WithValidData_ShouldSucceed()
{
    // Test implementation
    Assert.That(true, Is.True);
}
```

## Test Steps

The Qase NUnit reporter supports structured test steps using the `[Step]` attribute.

**Important**: You must add the `[Qase]` attribute to the test method to enable step tracking.

### Basic Steps

```csharp
using NUnit.Framework;
using Qase.Csharp.Commons.Attributes;

[TestFixture]
public class LoginTests
{
    [Test]
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
[Test]
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
using NUnit.Framework;
using Qase.Csharp.Commons.Attributes;
using Qase.Csharp.Commons;

[TestFixture]
public class ReportTests
{
    [Test]
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
[Test]
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

[Test]
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

NUnit tests with `[TestCase]` parameters are automatically captured and reported to Qase.

### TestCase with Parameters

```csharp
using NUnit.Framework;
using Qase.Csharp.Commons.Attributes;

[TestFixture]
public class CalculatorTests
{
    [Test]
    [TestCase(2, 3, 5)]
    [TestCase(10, 20, 30)]
    [TestCase(-5, 5, 0)]
    [QaseIds(300)]
    [Title("Addition Test")]
    public void AdditionTest(int a, int b, int expected)
    {
        int result = a + b;
        Assert.That(result, Is.EqualTo(expected));
    }
}
```

Each set of parameters creates a separate test result in Qase with the parameter values captured and displayed. This allows you to track results for each data variation independently.

## Ignoring Tests

Use the `[Ignore]` attribute from Qase to exclude specific tests from being reported to Qase. The test will still execute normally, but results won't be sent to Qase.

```csharp
using NUnit.Framework;
using Qase.Csharp.Commons.Attributes;

[TestFixture]
public class ExampleTests
{
    [Test]
    [Ignore]
    public void TestNotReportedToQase()
    {
        // This test runs but results are not sent to Qase
        Assert.That(true, Is.True);
    }

    [Test]
    public void NormalTest()
    {
        // This test is reported normally
        Assert.That(true, Is.True);
    }
}
```

**Note**: The `[Ignore]` attribute from Qase is different from NUnit's `[Ignore("reason")]` attribute. When you use NUnit's Ignore attribute, the test is skipped by NUnit itself and reported to Qase with a "Skipped" status.

## Comments

Add comments to your test results using the `Metadata.Comment()` method.

**Important**: You must add the `[Qase]` attribute to the test method to enable comments.

```csharp
using NUnit.Framework;
using Qase.Csharp.Commons.Attributes;
using Qase.Csharp.Commons;

[TestFixture]
public class CommentTests
{
    [Test]
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
