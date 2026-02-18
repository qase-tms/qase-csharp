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
- The test method must have the `[Qase]` attribute for attachments to be captured
- Maximum file size: 32MB per file (Qase API limit)

See also: [Usage Guide - Attachments](usage.md#file-attachments-support)

## Basic Usage

The simplest way to attach a file is to provide its path:

```csharp
using Xunit;
using Qase.Csharp.Commons.Attributes;

public class AttachmentExamples
{
    [Fact]
    [Qase]
    public void TestWithBasicAttachment()
    {
        // Your test logic
        var result = PerformOperation();

        // Attach a file to the test result
        Metadata.Attach("path/to/file.txt");

        Assert.True(result);
    }
}
```

The file path is relative to the working directory where `dotnet test` runs (typically the test project directory).

## Attachment Types

### File Path

Attach a single file by providing its path:

```csharp
using Xunit;
using Qase.Csharp.Commons.Attributes;

public class SingleFileAttachment
{
    [Fact]
    [Qase]
    public void TestWithScreenshot()
    {
        // Capture a screenshot during test execution
        CaptureScreenshot("screenshot.png");

        // Attach the screenshot
        Metadata.Attach("screenshot.png");

        Assert.True(true);
    }

    private void CaptureScreenshot(string filename)
    {
        // Screenshot capture logic
    }
}
```

### Multiple Files

Attach multiple files at once using a list:

```csharp
using System.Collections.Generic;
using Xunit;
using Qase.Csharp.Commons.Attributes;

public class MultipleFilesAttachment
{
    [Fact]
    [Qase]
    public void TestWithMultipleAttachments()
    {
        // Generate multiple files during test execution
        GenerateLogFile("app.log");
        GenerateConfigFile("config.json");

        // Attach multiple files at once
        Metadata.Attach(new List<string>
        {
            "app.log",
            "config.json"
        });

        Assert.True(true);
    }

    private void GenerateLogFile(string filename) { }
    private void GenerateConfigFile(string filename) { }
}
```

### Byte Array

Create an attachment from in-memory data without writing to disk:

```csharp
using System.Text;
using Xunit;
using Qase.Csharp.Commons.Attributes;

public class ByteArrayAttachment
{
    [Fact]
    [Qase]
    public void TestWithInMemoryAttachment()
    {
        // Generate content in memory
        string apiResponse = GetApiResponse();

        // Attach content directly from memory
        Metadata.Attach(
            Encoding.UTF8.GetBytes(apiResponse),
            "api-response.json"
        );

        Assert.NotNull(apiResponse);
    }

    private string GetApiResponse()
    {
        return "{\"status\": \"success\", \"data\": []}";
    }
}
```

## Advanced Scenarios

### Attaching Files in Steps

Attachments can be added within step methods. The attachment will be associated with the specific step:

```csharp
using Xunit;
using Qase.Csharp.Commons.Attributes;

public class StepAttachmentExample
{
    [Fact]
    [Qase]
    public void TestWithStepAttachments()
    {
        LoginStep();
        PerformActionStep();
    }

    [Step]
    public void LoginStep()
    {
        // Perform login
        CaptureScreenshot("login-screen.png");

        // Attach screenshot to this step
        Metadata.Attach("login-screen.png");
    }

    [Step]
    public void PerformActionStep()
    {
        // Perform action
        CaptureScreenshot("action-screen.png");

        // Attach screenshot to this step
        Metadata.Attach("action-screen.png");
    }

    private void CaptureScreenshot(string filename) { }
}
```

### Conditional Attachments

Attach files only when specific conditions are met, such as test failure:

```csharp
using Xunit;
using Qase.Csharp.Commons.Attributes;

public class ConditionalAttachmentExample
{
    [Fact]
    [Qase]
    public void TestWithConditionalAttachment()
    {
        try
        {
            // Test logic that might fail
            PerformCriticalOperation();
        }
        catch (Exception ex)
        {
            // Capture debug information on failure
            CaptureDebugLog("error.log");
            CaptureScreenshot("failure-screenshot.png");

            // Attach failure artifacts
            Metadata.Attach("error.log");
            Metadata.Attach("failure-screenshot.png");

            throw; // Re-throw to fail the test
        }
    }

    private void PerformCriticalOperation() { }
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
