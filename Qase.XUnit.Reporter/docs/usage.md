# Qase xUnit Reporter - Usage Guide

This guide explains how to use Qase attributes with xUnit to integrate your tests with Qase Test Management System.

## Available Attributes

### 1. QaseIds Attribute

Links your test to existing test cases in Qase by their IDs.

```csharp
[Fact]
[QaseIds(123, 456)]
public void TestWithMultipleIds()
{
    // Test implementation
}

[Theory]
[InlineData("test data")]
[QaseIds(789)]
public void TheoryTestWithId(string data)
{
    // Test implementation
}
```

### 2. Title Attribute

Sets a custom title for the test case in Qase.

```csharp
[Fact]
[Title("Custom Test Title")]
public void TestWithCustomTitle()
{
    // Test implementation
}
```

### 3. Fields Attribute

Adds custom fields to the test case.

```csharp
[Fact]
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
[Fact]
[Suites("API", "Authentication", "Login")]
public void TestInSpecificSuite()
{
    // Test implementation
}
```

### 5. Ignore Attribute

Marks the test to be ignored in Qase (test will still run but results won't be sent).

```csharp
[Fact]
[Ignore]
public void TestIgnoredInQase()
{
    // Test implementation
}
```

## Combining Attributes

You can combine multiple attributes on the same test:

```csharp
[Fact]
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
[Suites("API")]
public class AuthenticationTests
{
    [Fact]
    public void Test1()
    {
        // This test will inherit QaseIds(100) and Suites("API")
    }

    [Fact]
    [QaseIds(101)] // This will override the class-level QaseIds
    public void Test2()
    {
        // This test will use QaseIds(101) and inherit Suites("API")
    }
}
```

## Theory Tests with Parameters

For theory tests, parameters are automatically captured and included in the test signature:

```csharp
[Theory]
[InlineData("user1", "password1")]
[InlineData("user2", "password2")]
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
using Xunit;
using Qase.XUnit.Reporter;

[Suites("API", "User Management")]
public class UserApiTests
{
    [Fact]
    [QaseIds(1001)]
    [Title("Create User - Valid Data")]
    [Fields("Priority", "High")]
    [Fields("Component", "User API")]
    public void CreateUser_WithValidData_ShouldSucceed()
    {
        // Test implementation
        Assert.True(true);
    }

    [Theory]
    [InlineData("", "password")]
    [InlineData("username", "")]
    [InlineData("", "")]
    [QaseIds(1002)]
    [Title("Create User - Invalid Data")]
    [Fields("Priority", "Medium")]
    public void CreateUser_WithInvalidData_ShouldFail(string username, string password)
    {
        // Test implementation
        Assert.False(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password));
    }

    [Fact]
    [QaseIds(1003)]
    [Title("Get User by ID")]
    [Fields("Priority", "Low")]
    public void GetUser_ById_ShouldReturnUser()
    {
        // Test implementation
        Assert.True(true);
    }
}
```
