## Tech Stack

| Component | Purpose | Version |
|-----------|---------|---------|
| .NET | Runtime & SDK | 8.0+ |
| NUnit | Test Framework | 4.1+ |
| HttpClient | HTTP API Communication | Built-in |
| FluentAssertions | Readable Assertions | 6.12+ |
| Allure.NUnit | Test Reporting | 2.15+ |
| GitHub Actions | CI/CD Pipeline | - |

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (or later)
- A terminal/PowerShell with `dotnet` CLI

### Optional (for HTML report generation)
- [Allure CLI](https://allurereport.org/docs/install/)

---

## Setup & Installation

### 1. Clone the Repository

```bash
git clone https://github.com/<your-username>/RestTestsKozlenko.git
cd RestTestsKozlenko
```

### 2. Restore NuGet Packages

```bash
dotnet restore
```

### 3. Verify the Build

```bash
dotnet build
```

---

## Running Tests

### Run All Tests

```bash
dotnet test
```

### Run with Verbose Output

```bash
dotnet test --logger "console;verbosity=detailed"
```

### Run a Specific Test Class

```bash
dotnet test --filter "ClassName=RestTestsKozlenko.Tests.PostsTests"
```

### Run a Specific Test Method

```bash
dotnet test --filter "FullyQualifiedName~GetPosts_ShouldReturn200OK"
```

---

## Configuration

### Base URL

The API base URL is configured in `appsettings.json` (no hardcoded values):

To test against a different environment, simply update this file before running tests.

---

## Test Reports

### Generate Allure Report
```bash
# Run tests (generates allure-results folder)
dotnet test

# Serve interactive HTML report
allure serve allure-results
```

The report includes:
- Test execution timeline
- Pass/fail statistics
- Detailed logs and attachments
- Trend analysis

---

## CI/CD Integration

### GitHub Actions Workflow
Automated testing on every push and pull request:

- Triggers on: push to main/develop, pull requests
- Runs on: Ubuntu latest
- Steps:
  1. Checkout code
  2. Setup .NET 8
  3. Restore dependencies
  4. Build project
  5. Run tests with detailed output
  6. Upload .trx test results
  7. Publish test report (interactive view)

View results:
- Check GitHub Actions tab after push
- Download artifact with `.trx` file
- Test report published as PR comment

---