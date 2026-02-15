# Release Process

This document describes how to release new versions of CsharpTags packages to NuGet.org.

## Prerequisites

This repository uses **Trusted Publishing** (OIDC) to authenticate with NuGet.org. No long-lived API keys are required!

### Setup Trusted Publishing

You need to configure Trusted Publishing for each package on NuGet.org:

1. Go to [nuget.org](https://www.nuget.org/) and sign in
2. Click your username → **"Trusted Publishing"**
3. Click **"Add New Workflow"**
4. Configure for each package:
   - **Repository Owner**: `cheerio-pixel`
   - **Repository**: `CsharpTags`
   - **Workflow File**: `publish.yml` (filename only, no path)
   - **Environment**: (leave empty unless using GitHub environments)
5. Click **"Add"**

Repeat for all 4 packages:
- CsharpTags.Core
- CsharpTags.Htmx  
- CsharpTags.AspNetCore
- CsharpTags.Carter

### Setup GitHub Repository Variable (Optional)

The workflow uses your NuGet.org username. You can set this as a repository variable:

1. Go to your repository on GitHub
2. Navigate to **Settings** → **Secrets and variables** → **Actions**
3. Click **Variables** tab → **New repository variable**
4. Name: `NUGET_USER`
5. Value: Your NuGet.org username (profile name, not email)
6. Click **Add variable**

If not set, the workflow will use an empty username which still works with Trusted Publishing.

## Release Methods

### Method 1: Automatic Version Bump (Recommended)

This is the easiest way to release a new version:

1. Go to **Actions** tab in your GitHub repository
2. Select **Version Bump** workflow
3. Click **Run workflow**
4. Choose the bump type:
   - **patch**: Bug fixes (1.0.0 → 1.0.1)
   - **minor**: New features, backward compatible (1.0.0 → 1.1.0)
   - **major**: Breaking changes (1.0.0 → 2.0.0)
5. Optionally add a prerelease label (e.g., `beta-1`, `rc-1`)
6. Click **Run workflow**

The workflow will:
- Update version numbers in all `.csproj` files
- Commit the changes
- Create and push a git tag (e.g., `v1.2.3`)
- Automatically trigger the publish workflow

### Method 2: Manual Tag Push

If you prefer to control versions manually:

1. Update version numbers in all `.csproj` files:
   - `CsharpTags.Core/CsharpTags.Core.csproj`
   - `CsharpTags.Htmx/CsharpTags.Htmx.csproj`
   - `CsharpTags.AspNetCore/CsharpTags.AspNetCore.csproj`
   - `CsharpTags.Carter/CsharpTags.Carter.csproj`

2. Commit the changes:
   ```bash
   git add .
   git commit -m "Bump version to 1.2.3"
   git push
   ```

3. Create and push a tag:
   ```bash
   git tag -a v1.2.3 -m "Release v1.2.3"
   git push origin v1.2.3
   ```

The tag will automatically trigger the publish workflow.

## Version Numbering

We follow [Semantic Versioning](https://semver.org/):

- **MAJOR**: Breaking changes (public API changes)
- **MINOR**: New features, backward compatible
- **PATCH**: Bug fixes
- **Prerelease**: Optional suffix like `-beta-1`, `-rc-1`

Examples:
- `1.0.0` - Initial stable release
- `1.0.1` - Bug fix
- `1.1.0` - New feature added
- `2.0.0` - Breaking changes
- `1.1.0-beta-1` - Beta prerelease

## What Gets Published

When you trigger a release, the following packages are published:

1. **CsharpTags.Core** - Core HTML generation library
2. **CsharpTags.Htmx** - HTMX attributes extension
3. **CsharpTags.AspNetCore** - ASP.NET Core integration
4. **CsharpTags.Carter** - Carter framework integration

All packages will have the same version number and target:
- .NET 8.0
- .NET 10.0

## Monitoring Releases

You can monitor the release progress:

1. Go to **Actions** tab
2. Click on the running **Publish to NuGet** workflow
3. Watch the steps execute:
   - Build
   - Test
   - Pack
   - Publish to NuGet.org
   - Create GitHub Release

## Troubleshooting

### "Package already exists"
- You cannot overwrite an existing version on NuGet.org
- Bump the version number and try again

### "401 Unauthorized" or "Trusted Publishing is not enabled"
- Make sure Trusted Publishing is configured on NuGet.org for each package
- Verify the repository owner, repository name, and workflow filename match exactly
- Check that the `id-token: write` permission is set in the workflow
- Ensure the workflow file name is entered without path (just `publish.yml`, not `.github/workflows/publish.yml`)

### Build or test failures
- Check the CI workflow logs for details
- Fix any issues before attempting to publish again

### Manual package upload
If automatic publishing fails, you can download the packages from the workflow artifacts and upload them manually to NuGet.org.

## GitHub Release

A GitHub Release is automatically created for each published version, including:
- Release notes generated from commits
- All `.nupkg` files as downloadable artifacts

You can view releases at: `https://github.com/cheerio-pixel/CsharpTags/releases`
