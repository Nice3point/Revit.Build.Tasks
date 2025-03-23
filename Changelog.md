# 3.0.0

- Added new property `PublishRevitAddin`, to publish files to the `bin/publish` folder.
- Added new property `DeployRevitAddin`, to deploy files to the `%AppData%\Autodesk\Revit\Addins` folder.
- Added new property `IsRepackable`, to enable [repacking](https://github.com/gluck/il-repack) for avoiding dependency conflicts between different add-ins on the .NET framework.
- Added new target `PatchManifest`, to patch the Revit `.addin` manifest, for backwards compatibility (enabled by default).
- Property `PublishAddinFiles` renamed to `DeployRevitAddin`.
- Property `DisableImplicitRevitUsings` renamed to `ImplicitRevitUsings`.
- Switched `DebugType` property for `Release` configuration to `portable`.
- Removed `EnableDynamicLoading` property. Enable for the project manually.
- Updated Readme and added new examples.

**Migration from v2 to v3:**

- Replace the `PublishAddinFiles` property with `DeployRevitAddin` in the `.csproj` file.
- Replace the `DisableImplicitRevitUsings` property with `ImplicitRevitUsings` in the `.csproj` file.
- Add the `EnableDynamicLoading` property and set it to `true` in the `.csproj` file.

# 2.0.2

- Import common properties

# 2.0.1

- Enabled EnableDynamicLoading by default for NetCore apps
- Disabled CopyLocalLockFileAssemblies for NetCore apps

# 2.0.0

- Disabled publishing Content files by default. To enable it, set `CopyToPublishDirectory=‘Always’` or `CopyToPublishDirectory=‘PreserveNewest’`.
- PreserveNewest and Always now affect whether existing files should be skipped or overwritten always
- Increased major version to avoid conflicts in existing projects

# 1.1.2

- Update Readme
- Add extra `clean` folder

# 1.1.1

- Changed publishing directory from bin to bin\publish

# 1.1.0

- Now you can add extra file to publishing. For description see Readme.md
- Add CommunityToolkit.Mvvm implicit usings
- Fix implicit usings generator

# 1.0.0

Initial release