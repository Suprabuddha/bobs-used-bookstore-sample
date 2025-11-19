# Next Steps

## Overview

Your transformation has encountered a version mismatch issue in the `Bookstore.Domain.Tests` project. The error indicates that `Bookstore.Domain` is targeting .NET 8.0 while `Bookstore.Domain.Tests` is referencing .NET 6.0 runtime assemblies. This inconsistency needs to be resolved to complete the migration.

## Immediate Actions Required

### 1. Align Target Frameworks

**Check and update the Target Framework Moniker (TFM) across all projects:**

- Open each `.csproj` file and verify the `<TargetFramework>` property
- Ensure all projects target the same .NET version (preferably .NET 8.0 based on the `Bookstore.Domain` assembly)

**For `Bookstore.Domain.Tests.csproj`:**
```xml
<TargetFramework>net8.0</TargetFramework>
```

**Verify consistency in all other projects:**
- `Bookstore.Data.csproj`
- `Bookstore.Domain.csproj`
- `Bookstore.Web.csproj`
- `Bookstore.Cdk.csproj`

### 2. Update Package References

After aligning target frameworks, update NuGet package references in `Bookstore.Domain.Tests`:

```bash
cd app/Bookstore.Domain.Tests
dotnet list package --outdated
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit # or NUnit/MSTest depending on your test framework
dotnet add package xunit.runner.visualstudio # if using xUnit
```

### 3. Clean and Restore the Solution

Remove build artifacts and restore dependencies:

```bash
dotnet clean
dotnet restore
```

### 4. Rebuild the Solution

Attempt a full rebuild:

```bash
dotnet build
```

## Validation Steps

Once the build succeeds, perform the following validation:

### 1. Run Unit Tests

Execute all tests to ensure functionality remains intact:

```bash
dotnet test
```

Review test results and investigate any failures. Focus on:
- Tests in `Bookstore.Domain.Tests`
- Integration between domain and data layers

### 2. Verify Project References

Confirm all project-to-project references are correctly resolved:

```bash
dotnet list app/Bookstore.Domain.Tests/Bookstore.Domain.Tests.csproj reference
dotnet list app/Bookstore.Web/Bookstore.Web.csproj reference
```

### 3. Check for Runtime Issues

Run the web application locally:

```bash
cd app/Bookstore.Web
dotnet run
```

Test core functionality:
- Application startup
- Database connectivity (if applicable)
- Key business operations
- API endpoints or web pages

### 4. Review Configuration Files

Verify configuration files have been properly migrated:
- `appsettings.json` and environment-specific variants
- Connection strings
- Dependency injection registrations in `Program.cs` or `Startup.cs`

### 5. Validate Data Access

If using Entity Framework Core or another ORM:

```bash
cd app/Bookstore.Data
dotnet ef migrations list # Check existing migrations
dotnet ef database update --dry-run # Verify migrations without applying
```

## Additional Modernization Considerations

### 1. Update Language Features

Review code for opportunities to use modern C# features:
- Nullable reference types
- Record types for DTOs
- Pattern matching enhancements
- Init-only properties

Enable nullable reference types in each `.csproj`:
```xml
<Nullable>enable</Nullable>
```

### 2. Review Deprecated APIs

Check for compiler warnings about deprecated APIs:

```bash
dotnet build /p:TreatWarningsAsErrors=false > build-warnings.txt
```

Address warnings related to:
- Obsolete methods or types
- Platform-specific API usage
- Security-related warnings

### 3. Performance Profiling

Benchmark critical operations to identify performance regressions:
- Use BenchmarkDotNet for performance testing
- Profile memory usage with dotnet-counters
- Monitor startup time

### 4. Security Review

Update security-related packages and practices:
- Review authentication and authorization implementations
- Update cryptography APIs if using deprecated methods
- Scan for vulnerable package versions: `dotnet list package --vulnerable`

## Deployment Preparation

### 1. Publish Profiles

Test the publish process for each deployable project:

```bash
cd app/Bookstore.Web
dotnet publish -c Release -o ./publish
```

### 2. Runtime Configuration

Verify runtime configuration for the target environment:
- Ensure `RuntimeIdentifier` is set if targeting specific platforms
- Test with self-contained and framework-dependent deployment models

### 3. Environment Validation

Test the application in an environment similar to production:
- Verify all environment variables are correctly set
- Test with production-like data volumes
- Validate external service integrations

### 4. Documentation Updates

Update project documentation:
- README with new build and run instructions
- Deployment guides reflecting .NET 8.0 requirements
- Dependency matrix showing framework and package versions

## Troubleshooting

If issues persist after following these steps:

1. **Clear NuGet caches:**
   ```bash
   dotnet nuget locals all --clear
   ```

2. **Verify SDK installation:**
   ```bash
   dotnet --list-sdks
   dotnet --list-runtimes
   ```

3. **Check for binding redirects:** These should not be necessary in .NET 8.0, but verify no legacy `app.config` files contain them

4. **Review global.json:** If present, ensure it does not pin to an incompatible SDK version