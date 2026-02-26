# Testing Patterns

**Analysis Date:** 2026-02-26

## Test Framework

**Runner:**
- xUnit 2.9.3 (primary)
- NUnit (secondary, for reporter-specific tests)
- Config: No explicit `xunit.runner.config` file; configuration via `csproj` or test attributes

**Assertion Library:**
- FluentAssertions 8.8.0 (primary modern assertions)
- xUnit Assert class (fallback for some tests)
- Example mixing:
  ```csharp
  result.Should().Be(expectedRunId);  // FluentAssertions
  Assert.ThrowsAsync<QaseException>(() => mockClient.Object.CreateTestRunAsync());  // xUnit
  ```

**Run Commands:**
```bash
dotnet test                                    # Run all tests
dotnet test --watch                           # Watch mode (when available)
dotnet test /p:CollectCoverage=true           # Coverage (via coverlet.collector)
```

**Test SDK:**
- Microsoft.NET.Test.Sdk 18.0.1
- coverlet.collector 6.0.4 for coverage collection

## Test File Organization

**Location:**
- Separate project pattern: Source in `Qase.Csharp.Commons/`, Tests in `Qase.Csharp.Commons.Tests/`
- Reporter tests co-located: `Qase.XUnit.Reporter/` → `Qase.XUnit.Reporter.Tests/`
- NUnit tests: `Qase.NUnit.Reporter.Tests/`

**Naming:**
- Test files: `[ComponentName]Tests.cs`
- Examples: `ClientTests.cs`, `CoreReporterTests.cs`, `QaseConfigTests.cs`, `StatusMappingUtilsTests.cs`, `QaseMessageSinkTests.cs`

**Structure:**
```
Qase.Csharp.Commons.Tests/
├── ClientTests.cs
├── CoreReporterTests.cs
├── QaseConfigTests.cs
├── StatusMappingUtilsTests.cs
├── TestopsReporterTests.cs
└── [...]Tests.cs
```

## Test Structure

**Suite Organization:**
```csharp
public class ClientTests
{
    [Fact]
    public async Task CreateTestRunAsync_ShouldReturnRunId()
    {
        // Arrange
        var mockClient = new Mock<IClient>();
        var expectedRunId = 12345L;
        mockClient.Setup(x => x.CreateTestRunAsync()).ReturnsAsync(expectedRunId);

        // Act
        var result = await mockClient.Object.CreateTestRunAsync();

        // Assert
        result.Should().Be(expectedRunId);
        mockClient.Verify(x => x.CreateTestRunAsync(), Times.Once);
    }
}
```

**Patterns:**
- **Setup/Arrange:** Mock setup and test data initialization
- **Act:** Execute the behavior being tested
- **Assert:** Verify results using FluentAssertions or xUnit Assert
- **Comments:** Arrange, Act, Assert comments are used consistently in `ClientTests.cs`, `CoreReporterTests.cs`, `QaseConfigTests.cs`

**Assertion Styles:**
- FluentAssertions (preferred): `result.Should().Be(expectedRunId)`, `config.Mode.Should().Be(Mode.Off)`, `reporter.Should().NotBeNull()`
- xUnit Assert: `Assert.Equal(2, result.Count)`, `Assert.Empty(result)`, `Assert.Throws<ArgumentException>(...)`
- Verification with Moq: `mockClient.Verify(x => x.CreateTestRunAsync(), Times.Once)`

**Async Testing:**
```csharp
[Fact]
public async Task CreateTestRunAsync_ShouldReturnRunId()
{
    // Arrange
    var mockClient = new Mock<IClient>();
    var expectedRunId = 12345L;
    mockClient.Setup(x => x.CreateTestRunAsync()).ReturnsAsync(expectedRunId);

    // Act
    var result = await mockClient.Object.CreateTestRunAsync();

    // Assert
    result.Should().Be(expectedRunId);
}
```

**Theory Tests (Parameterized):**
```csharp
[Theory]
[InlineData(null, Mode.Off)]
[InlineData("", Mode.Off)]
[InlineData("off", Mode.Off)]
[InlineData("testops", Mode.TestOps)]
[InlineData("report", Mode.Report)]
public void SetMode_WithValidValues_ShouldSetMode(string? mode, Mode expectedMode)
{
    // Arrange
    var config = new QaseConfig();

    // Act
    config.SetMode(mode);

    // Assert
    config.Mode.Should().Be(expectedMode);
}
```

## Mocking

**Framework:** Moq 4.20.72

**Patterns:**
```csharp
// Setup basic mock
var mockClient = new Mock<IClient>();
mockClient.Setup(x => x.CreateTestRunAsync()).ReturnsAsync(expectedRunId);
var result = await mockClient.Object.CreateTestRunAsync();

// Verify calls
mockClient.Verify(x => x.CreateTestRunAsync(), Times.Once);

// Setup with parameters
mockClient.Setup(x => x.CompleteTestRunAsync(runId)).Returns(Task.CompletedTask);

// Setup to throw
var exception = new QaseException("API Error");
mockClient.Setup(x => x.CreateTestRunAsync()).ThrowsAsync(exception);

// Setup complex behavior
mockClient.Setup(x => x.UploadResultsAsync(runId, results)).Returns(Task.CompletedTask);
```

**What to Mock:**
- External dependencies: API clients (`IClient`), loggers (`ILogger<T>`), reporters (`IInternalReporter`)
- Interface-based contracts used for testing
- Configuration objects passed as dependencies

**What NOT to Mock:**
- Configuration classes: Create real instances when testing config behavior (`new QaseConfig()`)
- Domain models: Use real `TestResult`, `Attachment`, etc. objects in test data
- Value objects and enums
- Utility methods in Utils classes

**Example from `CoreReporterTests.cs`:**
```csharp
private readonly Mock<ILogger<CoreReporter>> _loggerMock;
private readonly Mock<IInternalReporter> _primaryReporterMock;
private readonly Mock<IInternalReporter> _fallbackReporterMock;

public CoreReporterTests()
{
    _loggerMock = new Mock<ILogger<CoreReporter>>();
    _primaryReporterMock = new Mock<IInternalReporter>();
    _fallbackReporterMock = new Mock<IInternalReporter>();
    var config = new QaseConfig();  // Real instance
    _coreReporter = new CoreReporter(_loggerMock.Object, config, _primaryReporterMock.Object, _fallbackReporterMock.Object);
}
```

## Fixtures and Factories

**Test Data:**
- No separate fixture files detected
- Test data created inline in test methods
- Simple data structures:
  ```csharp
  var results = new List<TestResult>
  {
      new TestResult
      {
          Id = "1",
          Title = "Test 1",
          Message = "Test passed"
      },
      new TestResult
      {
          Id = "2",
          Title = "Test 2",
          Message = "Test failed"
      }
  };
  ```

**Location:**
- Test data defined within test method (Arrange section)
- No separate factory classes or builders
- Domain model instantiation is straightforward

**Pattern:**
```csharp
// Simple inline creation
var expectedIds = new List<long> { 1, 2, 3, 4, 5 };

// Collection initialization
var results = new List<TestResult> { ... };

// Configuration objects
var config = new QaseConfig();
config.Mode = Mode.TestOps;
```

## Coverage

**Requirements:** Not detected in codebase
- No explicit coverage thresholds in `csproj` files
- No coverage requirements in CI configuration

**View Coverage:**
```bash
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover
```

**Tool:** coverlet.collector 6.0.4 included as dev dependency

## Test Types

**Unit Tests:**
- Scope: Single class/method in isolation
- Approach: Mock external dependencies, test behavior
- Examples:
  - `ClientTests.cs` - Test IClient interface contract with mocks
  - `QaseConfigTests.cs` - Test QaseConfig property behavior
  - `StatusMappingUtilsTests.cs` - Test utility function behavior

**Integration Tests:**
- Not detected as separate test class
- Some reporter tests may have minimal integration aspects (e.g., `QaseMessageSinkTests.cs`)
- Reporter tests in `Qase.XUnit.Reporter.Tests/` and `Qase.NUnit.Reporter.Tests/` test integration with test frameworks

**E2E Tests:**
- Not used
- Separate example projects exist: `examples/NUnitExamples/`, `examples/xUnitExamples/`

## Common Patterns

**Async Testing:**
```csharp
[Fact]
public async Task CreateTestRunAsync_ShouldReturnRunId()
{
    // Arrange
    var mockClient = new Mock<IClient>();
    var expectedRunId = 12345L;
    mockClient.Setup(x => x.CreateTestRunAsync()).ReturnsAsync(expectedRunId);

    // Act
    var result = await mockClient.Object.CreateTestRunAsync();

    // Assert
    result.Should().Be(expectedRunId);
}
```

**Error Testing:**
```csharp
[Fact]
public async Task CreateTestRunAsync_WhenExceptionOccurs_ShouldThrowQaseException()
{
    // Arrange
    var mockClient = new Mock<IClient>();
    var exception = new QaseException("API Error");
    mockClient.Setup(x => x.CreateTestRunAsync()).ThrowsAsync(exception);

    // Act & Assert
    await Assert.ThrowsAsync<QaseException>(() => mockClient.Object.CreateTestRunAsync());
}
```

**Theory Tests with Multiple Cases:**
```csharp
[Theory]
[InlineData("invalid")]
[InlineData("unknown")]
public void SetMode_WithInvalidValues_ShouldThrowArgumentException(string? mode)
{
    // Arrange
    var config = new QaseConfig();

    // Act & Assert
    Assert.Throws<ArgumentException>(() => config.SetMode(mode));
}
```

**Constructor/Initialization Tests:**
```csharp
[Fact]
public void Constructor_ShouldInitializeWithDefaultValues()
{
    // Act
    var config = new QaseConfig();

    // Assert
    config.Mode.Should().Be(Mode.Off);
    config.Fallback.Should().Be(Mode.Off);
    config.Environment.Should().BeNull();
}
```

**Reflective/Method Invocation Testing:**
```csharp
public void IsAssertionFailure_WithAssertEqualInStackTrace_ShouldReturnTrue()
{
    // Arrange
    var mockTestFailed = new Mock<ITestFailed>();
    var stackTraces = new List<string> { ... };
    mockTestFailed.Setup(x => x.StackTraces).Returns(stackTraces.ToArray());

    // Act
    var result = InvokeIsAssertionFailure(mockTestFailed.Object);

    // Assert
    result.Should().BeTrue();
}
```

## Test Project Configuration

**Project Files:**
- `Qase.Csharp.Commons.Tests/Qase.Csharp.Commons.Tests.csproj` - Primary test project
- `Qase.XUnit.Reporter.Tests/Qase.XUnit.Reporter.Tests.csproj` - xUnit reporter tests
- `Qase.NUnit.Reporter.Tests/Qase.NUnit.Reporter.Tests.csproj` - NUnit reporter tests

**Implicit Usings (for test projects):**
```xml
<ItemGroup>
  <Using Include="Xunit" />
  <Using Include="FluentAssertions" />
  <Using Include="Moq" />
</ItemGroup>
```

**Target Frameworks:**
- Primary: net9.0 for commons tests
- Also targets: net10.0 for NUnit reporter tests

## Test Naming Convention

**Method names follow pattern:** `[MethodUnderTest]_[Scenario]_[ExpectedResult]`

Examples:
- `CreateTestRunAsync_ShouldReturnRunId` - Tests what method returns under normal conditions
- `CreateTestRunAsync_WhenExceptionOccurs_ShouldThrowQaseException` - Tests error handling
- `SetMode_WithValidValues_ShouldSetMode` - Tests method behavior with valid input
- `SetMode_WithInvalidValues_ShouldThrowArgumentException` - Tests method behavior with invalid input
- `Constructor_ShouldInitializeWithDefaultValues` - Tests initialization
- `IsAssertionFailure_WithAssertEqualInStackTrace_ShouldReturnTrue` - Tests detection logic

---

*Testing analysis: 2026-02-26*
