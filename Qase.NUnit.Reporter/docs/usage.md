# Qase NUnit Reporter - Usage Guide

This guide explains how to use Qase attributes with NUnit to integrate your tests with Qase Test Management System.

## Available Attributes

### 1. QaseIds Attribute

Links your test to existing test cases in Qase by their IDs.

```csharp
[Test]
[QaseIds(123, 456)]
public void TestWithMultipleIds()
{
    // Test implementation
}

[Test]
[TestCase("test data")]
[QaseIds(789)]
public void TestCaseTestWithId(string data)
{
    // Test implementation
}
```

### 2. Title Attribute

Sets a custom title for the test case in Qase.

```csharp
[Test]
[Title("Custom Test Title")]
public void TestWithCustomTitle()
{
    // Test implementation
}
```

### 3. Fields Attribute

Adds custom fields to the test case.

```csharp
[Test]
[Fields("Priority", "High")]
[Fields("Component", "Authentication")]
public void TestWithCustomFields()
{
    // Test implementation
}
```

### 4. Suites Attribute

Specifies the suite structure for the test case.

```csharp
[Test]
[Suites("API", "Authentication", "Login")]
public void TestInSpecificSuite()
{
    // Test implementation
}
```

### 5. Ignore Attribute

Marks the test to be ignored in Qase (test will still run but results won't be sent).

```csharp
[Test]
[Ignore]
public void TestIgnoredInQase()
{
    // Test implementation
}
```

## Combining Attributes

You can combine multiple attributes on the same test:

```csharp
[Test]
[QaseIds(123)]
[Title("User Login Test")]
[Fields("Priority", "High")]
[Fields("Component", "Authentication")]
[Suites("API", "Authentication")]
public void ComprehensiveTest()
{
    // Test implementation
}
```

## Class-Level Attributes

You can apply attributes at the class level to affect all tests in the class:

```csharp
[TestFixture]
[Suites("API")]
public class AuthenticationTests
{
    [Test]
    public void Test1()
    {
        // This test will inherit Suites("API")
    }

    [Test]
    [QaseIds(101)] // This will override any class-level QaseIds
    [Suites("API", "Login")] // This will override class-level Suites
    public void Test2()
    {
        // This test will use QaseIds(101) and Suites("API", "Login")
    }
}
```

## Parameterized Tests

For parameterized tests using `[TestCase]` or `[TestCaseSource]`, parameters are automatically captured and included in the test signature:

```csharp
[Test]
[TestCase("user1", "password1")]
[TestCase("user2", "password2")]
[QaseIds(200)]
public void LoginTest(string username, string password)
{
    // Test implementation
    // Parameters will be automatically captured and sent to Qase
}
```

## Example Test Class

Here's a complete example of a test class using Qase attributes:

```csharp
using NUnit.Framework;
using Qase.Csharp.Commons.Attributes;

[TestFixture]
[Suites("API", "User Management")]
public class UserApiTests
{
    [Test]
    [QaseIds(1001)]
    [Title("Create User - Valid Data")]
    [Fields("Priority", "High")]
    [Fields("Component", "User API")]
    public void CreateUser_WithValidData_ShouldSucceed()
    {
        // Test implementation
        Assert.That(true, Is.True);
    }

    [Test]
    [TestCase("", "password")]
    [TestCase("username", "")]
    [TestCase("", "")]
    [QaseIds(1002)]
    [Title("Create User - Invalid Data")]
    [Fields("Priority", "Medium")]
    public void CreateUser_WithInvalidData_ShouldFail(string username, string password)
    {
        // Test implementation
        Assert.That(!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password), Is.True);
    }

    [Test]
    [QaseIds(1003)]
    [Title("Get User by ID")]
    [Fields("Priority", "Low")]
    public void GetUser_ById_ShouldReturnUser()
    {
        // Test implementation
        Assert.That(true, Is.True);
    }
}
```

## Steps

For using steps, you need to add the `Qase` attribute to the test method.

```csharp
[Test]
[Qase]
public void TestWithSteps()
{
    // Test implementation
    Step1();
    Step2();
}

[Step]
public void Step1()
{
    // Step implementation
}

[Step]
public void Step2()
{
    // Step implementation
}
```

## Comments

For using comments, you need to add the `Qase` attribute to the test method.

```csharp
[Test]
[Qase]
public void TestWithComments()
{
    // Test implementation
    Metadata.Comment("This is a comment");
}
```

## File Attachments Support

The Qase API clients support uploading file attachments to test cases and test runs. This feature allows you to:

- Upload files from file system paths
- Upload content from strings or byte arrays
- Properly handle file names and multipart/form-data requests
- Support multiple file formats and sizes (up to 32MB per file)

### Example Usage

```csharp
[Test]
[Qase]
public void TestWithAttachments()
{
    // Test implementation
    Metadata.Attach("path/to/your/file.xml");

    // or
    Metadata.Attach(new List<string> { "path/to/your/file.xml", "path/to/your/file2.xml" });

    // or
    Metadata.Attach(Encoding.UTF8.GetBytes("test data"), "log.txt");
}
```
