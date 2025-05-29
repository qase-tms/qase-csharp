# [Qase TestOps](https://qase.io) Reporters for C#

Monorepo containing [Qase TestOps](https://qase.io) integration tools for C# testing frameworks. This repository
includes reporters and API clients designed to streamline the process of reporting test results to Qase TestOps.

## Projects Overview

### Reporters

- **[Qase.xUnit.Reporter](/Qase.XUnit.Reporter)**  
  Reporter for xUnit.

- **[Qase.NUnit.Reporter](/Qase.NUnit.Reporter)**  
  Reporter for NUnit. (coming soon)

- **[Qase.MSTest.Reporter](/Qase.MSTest.Reporter)**  
  Reporter for MSTest. (coming soon)

- **[Qase.SpecFlow.Reporter](/Qase.SpecFlow.Reporter)**  
  Reporter for SpecFlow. (coming soon)

### Libraries

- **[Qase.Csharp.Commons](/Qase.Csharp.Commons/)**  
  Shared library containing common components and utilities used by Qase reporters.

### API Clients

- **[Qase.ApiClient.V1](/Qase.ApiClient.V1)**  
  Official client for interacting with the Qase TestOps API (v1). Recommended for most use cases.

- **[Qase.ApiClient.V2](/Qase.ApiClient.V2)**  
  API client supporting the new Qase TestOps API (v2). Use this client for projects leveraging the latest API features.
