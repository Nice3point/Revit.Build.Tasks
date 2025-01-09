# Revit Build Tasks

[![Nuget](https://img.shields.io/nuget/vpre/Nice3point.Revit.Build.Tasks?style=for-the-badge)](https://www.nuget.org/packages/Nice3point.Revit.Build.Tasks)
[![Downloads](https://img.shields.io/nuget/dt/Nice3point.Revit.Build.Tasks?style=for-the-badge)](https://www.nuget.org/packages/Nice3point.Revit.Build.Tasks)
[![Last Commit](https://img.shields.io/github/last-commit/Nice3point/Revit.Build.Tasks/develop?style=for-the-badge)](https://github.com/Nice3point/Revit.Build.Tasks/commits/main)

This repository contains the MSBuild tasks for developing and publishing the plugin for multiple Revit versions.

## Installation

You can install Tasks as a [nuget package](https://www.nuget.org/packages/Nice3point.Revit.Build.Tasks).

```text
<PackageReference Include="Nice3point.Revit.Build.Tasks" Version="2.*"/>
```

How to use this package? Just add it to your add-in, and this package will setup the project for simplified maintenance and development.
About [MSBuild targets](https://learn.microsoft.com/en-us/visualstudio/msbuild/customize-your-build).

Package included by default in [Revit Templates](https://github.com/Nice3point/RevitTemplates).

## MSBuild Targets

### OR_GREATER preprocessor symbols

Included a target for generating the Define Constants needed to support code for multiple Revit versions.
`OR_GREATER` variants are accumulative in nature and provide a simpler way to write compilation conditions.

| Current configuration | Project configurations               | Generated define constants                                                  |
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

Constants are generated from the names of project configurations. If your project configurations do not contain metadata about the version, you can specify it explicitly:

```xml
<PropertyGroup>
    <RevitVersion>2025</RevitVersion>
</PropertyGroup>
```

**To disable preprocessor symbols, set:**

```xml
<PropertyGroup>
    <DisableImplicitRevitDefines>true</DisableImplicitRevitDefines>
</PropertyGroup>
```

### Publishing

Included a target for copying Revit add-in files to the `%AppData%\Autodesk\Revit\Addins` folder after building a project.

`Clean solution` or `Clean project` will delete the published files.

Copying files helps attach the debugger to the add-in when Revit starts. This makes it easier to test the application or can be used for local development.

Should only be enabled in projects containing the Revit manifest file (`.addin`). 

```xml
<PropertyGroup>
    <PublishRevitAddin>true</PublishRevitAddin>
</PropertyGroup>
```

**PublishRevitAddin disabled by default.**

If you need to create an installer or a bundle but don't want to publish files to the `%AppData%\Autodesk\Revit\Addins` directory, use the **PublishRevitFiles** property instead:

```xml
<PropertyGroup>
    <PublishRevitFiles>true</PublishRevitFiles>
</PropertyGroup>
```

Filed will be published to the `bin\publish` folder.

**PublishRevitFiles disabled by default.**

By default, all project files and dependencies required for the plugin to run, including the `.addin` manifest, are copied.
If you need to include additional files, such as configuration or family files, include them in the `Content` item.

```xml
<ItemGroup>
    <Content Include="Resources\Families\Window.rfa" CopyToPublishDirectory="Always"/>
    <Content Include="Resources\Music\Click.wav" CopyToPublishDirectory="PreserveNewest"/>
    <Content Include="Resources\Images\**" CopyToPublishDirectory="PreserveNewest"/>
</ItemGroup>
```

To enable copying Content files, set `CopyToPublishDirectory="Always"` or `CopyToPublishDirectory="PreserveNewest"`

The `PublishDirectory` property specifies which subfolder of the plugin the file should be copied to.
If it is not specified, the files will be copied to the root folder.

```xml
<ItemGroup>
    <Content Include="Resources\Families\Window.rfa" PublishDirectory="Families" CopyToPublishDirectory="PreserveNewest"/>
    <Content Include="Resources\Music\Click.wav" PublishDirectory="Music\Effects" CopyToPublishDirectory="PreserveNewest"/>
    <Content Include="Resources\Images\**" PublishDirectory="Images" CopyToPublishDirectory="PreserveNewest"/>
    <Content Include="Readme.md" CopyToPublishDirectory="PreserveNewest"/>
</ItemGroup>
```

Result:

```text
📂bin\publish ; %AppData%\Autodesk\Revit\Addins\2025
 ┣📜RevitAddIn.addin
 ┗📂RevitAddIn
   ┣📂Families
   ┃ ┗📜Family.rfa
   ┣📂Images
   ┃ ┣📜Image.png
   ┃ ┣📜Image2.png
   ┃ ┗📜Image3.jpg
   ┣📂Music
   ┃ ┗📂Effects
   ┃   ┗📜Click.wav
   ┣📜CommunityToolkit.Mvvm.dll
   ┣📜RevitAddIn.dll
   ┗📜Readme.md
```



### Implicit global usings

Included a target for generating implicit global Usings depending on the project references. Helps to reduce the frequent use of `using` in a project.

| Global Using                                | Enabled by reference            |
|---------------------------------------------|---------------------------------|
| using Autodesk.Revit.DB;                    | RevitAPI.dll                    |
| using JetBrains.Annotations;                | JetBrains.Annotations.dll       |
| using Nice3point.Revit.Extensions;          | Nice3point.Revit.Extensions.dll |
| using Nice3point.Revit.Toolkit;             | Nice3point.Revit.Toolkit.dll    |
| using CommunityToolkit.Mvvm.Input;          | CommunityToolkit.Mvvm.dll       |
| using CommunityToolkit.Mvvm.ComponentModel; | CommunityToolkit.Mvvm.dll       |

**To disable it, set:**

```xml
<PropertyGroup>
    <DisableImplicitRevitUsings>true</DisableImplicitRevitUsings>
    <!--OR-->
    <ImplicitUsings>false</ImplicitUsings>
</PropertyGroup>
```

## MSBuild Properties

By default, some properties that are optimal for publishing an application are overriden:

| Property                          | Default value | Description                                                                                                                                                         |
|-----------------------------------|---------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| CopyLocalLockFileAssemblies       | true          | Copies NuGet package dependencies to the output directory. Required to publish an application                                                                       |
| AppendTargetFrameworkToOutputPath | false         | Prevents the TFM from being appended to the output path. Required to publish an application                                                                         |

These properties are automatically applied to the `.csproj` file by default and can be overriden:

```xml
<PropertyGroup>
    <CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>
    <AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>
</PropertyGroup>
```