# snyk-test

This repository is an intentionally vulnerable .NET 10 demo for Snyk only.

Every issue in this repo is deliberate:
- All secrets, credentials, API keys, tokens, passwords, URLs, and connection strings are fake.
- The insecure code paths exist only to trigger Snyk Code, secret scanning, and Open Source findings.
- The application is not intended for production, internal use, or realistic execution.
- Runtime failures are acceptable in this demo.

## Included intentional findings

- Hardcoded fake secret in `Program.cs`
- Fake secrets and connection string in `appsettings.json`
- Sensitive-looking configuration values printed to the console
- Raw SQL built from untrusted input with string concatenation
- EF Core `FromSqlRaw` and `ExecuteSqlRaw` with unsafe user input
- Predictable token generation using `Random`
- Broad exception handling that hides errors
- Insecure file path handling using user input
- One xUnit test with hardcoded fake credentials and unsafe SQL construction
- A known-vulnerable NuGet package included only to trigger a Snyk Open Source finding

## Demo dependency note

The vulnerable package reference in `snyk-test.csproj` is included solely to generate a Snyk Open Source result. It is not required for the demo logic and should never be copied into a real project.
