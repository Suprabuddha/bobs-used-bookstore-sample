# Next Steps

## Overview

The transformation has encountered a framework version mismatch issue. The `Bookstore.Domain.Tests` project is targeting .NET 6.0, while the `Bookstore.Domain` project it references has been migrated to .NET 8.0. This is preventing the solution from building successfully.

## Immediate Actions Required

### 1. Resolve Framework Version Mismatch

The primary issue is that `Bookstore.Domain.Tests` is targeting `net6.0` while `Bookstore.Domain` targets `net8.0`.

**Option A: Update Test Project to .NET 8.0 (Recommended)**

Open `Bookstore.Domain.Tests/Bookstore.Domain.Tests.csproj` and update the `TargetFramework` element:

```xml
<TargetFramework>net8.0</TargetFramework>
```

**Option B: Align All Projects to .NET 6.0**

If you need to maintain .NET 6.0 compatibility, update `Bookstore.Domain/Bookstore.Domain.csproj` to target `net6.0`:

```xml
<TargetFramework>net6.0</TargetFramework>
```

Then verify all other projects in the solution are also targeting `net6.0`.

### 2. Verify Solution-Wide Framework Consistency

After resolving the mismatch, ensure all projects target the same framework version:

```bash
dotnet sln list | xargs -I {} grep -H "TargetFramework" {}
```

Review each `.csproj` file and confirm they all specify the same target framework.

### 3. Restore and Build

Once framework versions are aligned:

```bash
dotnet restore
dotnet build
```

Address any additional errors that surface during the build process.

## Validation Steps

### 1. Run Unit Tests

Execute the test suite to verify functionality:

```bash
dotnet test
```

Review test results and investigate any failures. Pay particular attention to tests in `Bookstore.Domain.Tests` since this project was directly affected by the migration.

### 2. Verify Project References

Ensure all project-to-project references are correctly resolved:

```bash
dotnet list reference
```

Run this command in each project directory to confirm dependencies are properly configured.

### 3. Check for Runtime Compatibility Issues

Run the application locally to identify any runtime-specific issues:

```bash
cd Bookstore.Web
dotnet run
```

Test key functionality including:
- Database connectivity (Bookstore.Data)
- Domain logic execution
- Web application endpoints

### 4. Review Breaking Changes

Consult the official .NET migration documentation for breaking changes between your source and target frameworks:

- If migrating to .NET 8.0: Review [.NET 8.0 breaking changes](https://learn.microsoft.com/en-us/dotnet/core/compatibility/8.0)
- If staying on .NET 6.0: Review [.NET 6.0 breaking changes](https://learn.microsoft.com/en-us/dotnet/core/compatibility/6.0)

Pay attention to changes affecting:
- Entity Framework Core (used in Bookstore.Data)
- ASP.NET Core (used in Bookstore.Web)
- Testing frameworks

## Additional Considerations

### 1. Update NuGet Packages

After framework alignment, update NuGet packages to versions compatible with your target framework:

```bash
dotnet list package --outdated
dotnet add package <PackageName> --version <LatestVersion>
```

Focus on:
- Entity Framework Core packages
- Testing framework packages (xUnit, NUnit, or MSTest)
- ASP.NET Core packages

### 2. Review AWS CDK Compatibility

The `Bookstore.Cdk` project may require specific .NET runtime versions. Verify the AWS CDK for .NET supports your chosen target framework by checking the [AWS CDK documentation](https://docs.aws.amazon.com/cdk/api/latest/).

### 3. Update Configuration Files

Review and update configuration files that may reference framework versions:
- `global.json` (if present)
- `launchSettings.json`
- Any build scripts or deployment configurations

### 4. Code Analysis

Run static code analysis to identify potential issues:

```bash
dotnet format --verify-no-changes
dotnet build /p:TreatWarningsAsErrors=true
```

Address warnings that may indicate compatibility problems.

## Final Verification

Before considering the migration complete:

1. Perform a clean build of the entire solution:
   ```bash
   dotnet clean
   dotnet build --configuration Release
   ```

2. Run the full test suite with code coverage:
   ```bash
   dotnet test --collect:"XPlat Code Coverage"
   ```

3. Execute integration tests if available

4. Perform manual testing of critical business workflows

5. Review application logs for any runtime warnings or errors

## Documentation Updates

Update project documentation to reflect:
- New target framework version
- Any API changes required by the migration
- Updated development environment requirements
- Modified build and deployment procedures