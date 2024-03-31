# Revit Build Tasks

[![Nuget](https://img.shields.io/nuget/vpre/Nice3point.Revit.Build.Tasks?style=for-the-badge)](https://www.nuget.org/packages/Nice3point.Revit.Build.Tasks)
[![Downloads](https://img.shields.io/nuget/dt/Nice3point.Revit.Build.Tasks?style=for-the-badge)](https://www.nuget.org/packages/Nice3point.Revit.Build.Tasks)
[![Last Commit](https://img.shields.io/github/last-commit/Nice3point/Revit.Build.Tasks/develop?style=for-the-badge)](https://github.com/Nice3point/Revit.Build.Tasks/commits/main)

This repository contains the MSBuild tasks for developing and publishing the plugin for multiple Revit versions.

## Installation

You can install Tasks as a [nuget package](https://www.nuget.org/packages/Nice3point.Revit.Build.Tasks).

```text
<PackageReference Include="Nice3point.Revit.Build.Tasks" Version="*"/>
```

How to use this package? Just add it to your add-in, and this package will setup the project for simplified maintenance and development.
About [MSBuild targets](https://learn.microsoft.com/en-us/visualstudio/msbuild/customize-your-build]).

Package included by default in [Revit Templates](https://github.com/Nice3point/RevitTemplates).

## MSBuild Properties

By default, some properties are set that are optimal for publishing an application.

| Property                          | Default value | Description                                                                                                                                                            |
|-----------------------------------|---------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| CopyLocalLockFileAssemblies       | true          | Copies NuGet package dependencies to the output directory. Required to publish an application                                                                          |
| AppendTargetFrameworkToOutputPath | false         | Prevents the TFM from being appended to the output path. Required to publish an application                                                                            |
| PublishAddinFiles                 | false         | Copies addin files to the **%AppData%\Autodesk\Revit\Addins** folder. Set `true` to enable copying. Handy for debugging the application instead of using AddinManager. |

## MSBuild Targets

### OR_GREATER preprocessor symbols

Included a target for generating the Define Constants needed to support code for multiple Revit versions.
`OR_GREATER` variants are accumulative in nature and provide a simpler way to write compilation conditions.

| Project configuration | Solution configurations              | Generated Define constants                                                  |
|-----------------------|:-------------------------------------|-----------------------------------------------------------------------------|
| Debug R20             | Debug R20, Release R21, Release 2022 | REVIT2020, REVIT2020_OR_GREATER                                             |
| Release R21           | Debug R20, Release R21, Release 2022 | REVIT2021, REVIT2020_OR_GREATER, REVIT2021_OR_GREATER                       |
| Release 2022          | Debug R20, Release R21, Release 2022 | REVIT2022, REVIT2020_OR_GREATER, REVIT2021_OR_GREATER, REVIT2022_OR_GREATER |

Usage:

```C#
#if REVIT2021_OR_GREATER
    UnitUtils.ConvertFromInternalUnits(69, UnitTypeId.Millimeters);
#else
    UnitUtils.ConvertFromInternalUnits(69, DisplayUnitType.DUT_MILLIMETERS);
#endif
```

To support removed APIs in newer versions of Revit, you can invert the constant:

```C#
#if !REVIT2023_OR_GREATER
    var builtinCategory = (BuiltInCategory) category.Id.IntegerValue;
#endif
```

To disable it, set `<DisableImplicitFrameworkDefines>false</DisableImplicitFrameworkDefines>`.

### Implicit global usings

Included a target for generating implicit global Usings depending on the installed Nuget packages. Helps to reduce the frequent use of `using` in a project.

| Using                              | Enabled by package          | Description                                                    |
|------------------------------------|-----------------------------|----------------------------------------------------------------|
| using Autodesk.Revit.DB;           | -                           | Always enabled                                                 |
| using Nice3point.Revit.Extensions; | Nice3point.Revit.Extensions | Added only if the required package is available in the project |
| using JetBrains.Annotations;       | Nice3point.Revit.Extensions | Added only if the required package is available in the project |

To disable it, set `<ImplicitUsings>false</ImplicitUsings>`.

### Publishing

Included a target for copying addin files to the `%AppData%\Autodesk\Revit\Addins folder` after building a project.
`Clean solution` or `Clean project` will delete the published files.

Disabled by default. To enable it, set `<PublishAddinFiles>true</PublishAddinFiles>`.