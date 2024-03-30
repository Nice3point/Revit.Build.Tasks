# Revit Build Tasks

[![Nuget](https://img.shields.io/nuget/vpre/Nice3point.Revit.Build.Tasks?style=for-the-badge)](https://www.nuget.org/packages/Nice3point.Revit.Build.Tasks)
[![Downloads](https://img.shields.io/nuget/dt/Nice3point.Revit.Build.Tasks?style=for-the-badge)](https://www.nuget.org/packages/Nice3point.Revit.Build.Tasks)
[![Last Commit](https://img.shields.io/github/last-commit/Nice3point/Revit.Build.Tasks/develop?style=for-the-badge)](https://github.com/Nice3point/Revit.Build.Tasks/commits/main)

This repository contains the MSBuild tasks and targets required for publishing Revit applications.

## MSBuild Properties

By default, some properties are set that are optimal for developing and publishing the plugin for multiple Revit versions.

| Property                          | Default value | Description                                                                                   |
|-----------------------------------|---------------|-----------------------------------------------------------------------------------------------|
| CopyLocalLockFileAssemblies       | true          | Copies NuGet package dependencies to the output directory. Required to publish an application |
| AppendTargetFrameworkToOutputPath | false         | Prevents the TFM from being appended to the output path. Required to publish an application   |
| PublishAddinFiles                 | false         | Copies addin files to the AppData\Autodesk\Revit\Addins folder                                |

## MSBuild Targets

**OR_GREATER preprocessor symbols target**

This target generates the Define Constants needed to support code for multiple Revit versions. 
`OR_GREATER` variants are accumulative in nature and provide a simpler way to write compilation conditions

| Project configuration | Solution configurations         | Define constants                                                            |
|-----------------------|:--------------------------------|-----------------------------------------------------------------------------|
| Debug R20             | Debug R20, Debug R21, Debug R22 | REVIT2020, REVIT2020_OR_GREATER                                             |
| Debug R21             | Debug R20, Debug R21, Debug R22 | REVIT2021, REVIT2020_OR_GREATER, REVIT2021_OR_GREATER                       |
| Debug R22             | Debug R20, Debug R21, Debug R22 | REVIT2022, REVIT2020_OR_GREATER, REVIT2021_OR_GREATER, REVIT2022_OR_GREATER |

Usage:

```C#
#if REVIT2021_OR_GREATER
    UnitUtils.ConvertFromInternalUnits(69, UnitTypeId.Millimeters);
#else
    UnitUtils.ConvertFromInternalUnits(69, DisplayUnitType.DUT_MILLIMETERS);
#endif
```

To disable it, set `<DisableImplicitFrameworkDefines>false</DisableImplicitFrameworkDefines>`.

**Implicit global usings target**

This target adds implicit global Usings depending on the installed Nuget packages. Helps to reduce the usings frequently encountered in a project

| Using                       | Enabled by package          |
|-----------------------------|-----------------------------|
| Autodesk.Revit.DB           | -                           |
| Nice3point.Revit.Extensions | Nice3point.Revit.Extensions |
| JetBrains.Annotations       | Nice3point.Revit.Extensions |

To disable it, set `<ImplicitUsings>false</ImplicitUsings>`.

**Publish target**

This target copies addin files to the `AppData\Autodesk\Revit\Addins folder` after project building.
Clearing the solution will delete the published files.

Disabled by default. To enable it, set `<PublishAddinFiles>true</PublishAddinFiles>`.