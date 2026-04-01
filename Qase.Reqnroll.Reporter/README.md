# Qase.Reqnroll.Reporter

Qase TMS reporter for [Reqnroll](https://reqnroll.net/) — the open-source BDD test automation framework for .NET.

## Installation

```bash
dotnet add package Qase.Reqnroll.Reporter
```

The plugin is auto-discovered by Reqnroll. No additional configuration in `reqnroll.json` is needed.

## Configuration

Create a `qase.config.json` in your project root:

```json
{
  "mode": "testops",
  "testOps": {
    "api": {
      "token": "YOUR_QASE_API_TOKEN",
      "host": "qase.io"
    },
    "project": "YOUR_PROJECT_CODE",
    "run": {
      "title": "Reqnroll Test Run",
      "complete": true
    }
  }
}
```

Make sure to copy it to the output directory:

```xml
<ItemGroup>
  <Content Include="qase.config.json" Condition="Exists('qase.config.json')">
    <CopyToOutputDirectory>Always</CopyToOutputDirectory>
  </Content>
</ItemGroup>
```

## Usage

### Linking scenarios to Qase test cases

Use Gherkin tags to link scenarios to Qase:

```gherkin
@QaseId:123
Scenario: User can login with valid credentials
  Given the user is on the login page
  When the user enters valid credentials
  Then the user should see the dashboard
```

### Available tags

| Tag | Description | Example |
|-----|-------------|---------|
| `@QaseId:ID` | Link to Qase test case ID(s) | `@QaseId:123` or `@QaseId:123,456` |
| `@QaseTitle:Title` | Override test title (underscores become spaces) | `@QaseTitle:My_Test` |
| `@QaseFields:key:value` | Set custom field | `@QaseFields:severity:critical` |
| `@QaseSuite:Path` | Set suite hierarchy (backslash-separated) | `@QaseSuite:Auth\Login` |
| `@QaseIgnore` | Exclude from Qase reporting | `@QaseIgnore` |

All tag prefixes are case-insensitive.

### Automatic step reporting

Every Given/When/Then step is automatically reported as a test step in Qase with its own status and timing. No additional code is required.

### Scenario Outline parameters

Scenario Outline examples are automatically captured as test parameters:

```gherkin
@QaseId:200
Scenario Outline: Login validation
  When the user enters "<username>" and "<password>"
  Then the result should be "<result>"

  Examples:
    | username | password | result  |
    | admin    | pass123  | success |
    | invalid  | wrong    | failure |
```

### Suite hierarchy

By default, the Feature name becomes the test suite. Override with `@QaseSuite:Parent\Child` tag.

## Requirements

- .NET 6.0 or later
- Reqnroll 2.0.0 or later
