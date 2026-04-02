# Attachments

## Overview

Attachments are files or data associated with test results in Qase TestOps. They help you investigate test failures by including screenshots, logs, API responses, debug output, or any other relevant files with your test results.

**Why use attachments?**
- Visual evidence of failures (screenshots, videos)
- Debug logs and application output
- API request/response data
- Configuration files or test data
- Any file that helps understand test execution

**Prerequisites:**
- Maximum file size: 32MB per file (Qase API limit)
- Use the `Metadata` class from `Qase.Csharp.Commons` in your step definitions

See also: [Usage Guide - Attachments](usage.md#attachments)

## Basic Usage

The simplest way to attach a file is to provide its path from within a step definition:

```csharp
using Reqnroll;
using Qase.Csharp.Commons;

[Binding]
public class AttachmentExamples
{
    [Then(@"a screenshot is captured")]
    public void ThenAScreenshotIsCaptured()
    {
        // Your test logic
        CaptureScreenshot("screenshot.png");

        // Attach a file to the test result
        Metadata.Attach("screenshot.png");
    }

    private void CaptureScreenshot(string filename) { }
}
```

The file path is relative to the working directory where `dotnet test` runs (typically the test project directory).

## Attachment Types

### File Path

Attach a single file by providing its path:

```csharp
[When(@"the test captures a screenshot")]
public void WhenTheTestCapturesAScreenshot()
{
    CaptureScreenshot("screenshot.png");
    Metadata.Attach("screenshot.png");
}
```

### Multiple Files

Attach multiple files at once using a list:

```csharp
using System.Collections.Generic;

[Then(@"all test artifacts are collected")]
public void ThenAllTestArtifactsAreCollected()
{
    GenerateLogFile("app.log");
    GenerateConfigFile("config.json");

    Metadata.Attach(new List<string>
    {
        "app.log",
        "config.json"
    });
}
```

### Byte Array

Create an attachment from in-memory data without writing to disk:

```csharp
using System.Text;

[Then(@"the API response is recorded")]
public void ThenTheApiResponseIsRecorded()
{
    string apiResponse = GetApiResponse();

    Metadata.Attach(
        Encoding.UTF8.GetBytes(apiResponse),
        "api-response.json"
    );
}

private string GetApiResponse()
{
    return "{\"status\": \"success\", \"data\": []}";
}
```

## Advanced Scenarios

### Attaching Files in Step Definitions

Attachments added within step definitions are associated with the test result of the current scenario:

```csharp
using Reqnroll;
using Qase.Csharp.Commons;

[Binding]
public class DetailedStepDefinitions
{
    [Given(@"the user is on the login page")]
    public void GivenTheUserIsOnTheLoginPage()
    {
        // Navigate to login page
        CaptureScreenshot("login-page.png");
        Metadata.Attach("login-page.png");
    }

    [When(@"the user submits the form")]
    public void WhenTheUserSubmitsTheForm()
    {
        // Submit form
        CaptureScreenshot("form-submitted.png");
        Metadata.Attach("form-submitted.png");
    }

    private void CaptureScreenshot(string filename) { }
}
```

### Conditional Attachments

Attach files only when specific conditions are met, such as a step failure:

```csharp
using System;

[Binding]
public class ConditionalAttachmentSteps
{
    private readonly ScenarioContext _scenarioContext;

    public ConditionalAttachmentSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [AfterScenario]
    public void AttachOnFailure()
    {
        if (_scenarioContext.TestError != null)
        {
            CaptureDebugLog("error.log");
            CaptureScreenshot("failure-screenshot.png");

            Metadata.Attach("error.log");
            Metadata.Attach("failure-screenshot.png");
        }
    }

    private void CaptureDebugLog(string filename) { }
    private void CaptureScreenshot(string filename) { }
}
```

### Supported File Formats

The Qase API accepts any file type, but commonly used formats include:

- **Images**: PNG, JPG, GIF, BMP, SVG
- **Documents**: PDF, TXT, MD, XML, JSON, HTML
- **Logs**: LOG, CSV, TSV
- **Archives**: ZIP, TAR, GZ
- **Videos**: MP4, AVI, MOV, WEBM
- **Code**: CS, XML, JSON, YAML

Remember that each file must be under 32MB in size. For larger files, consider compressing them or splitting the data into multiple files.
