# 3.0.0-preview.3.0

- Added new property `PublishRevitAddin`, to publish files to the `bin/publish` folder.
- Added new target `RepackAddinFiles`, to repack the addin files.
- Added new property `IsRepackable`, to enable repacking.
- Property `PublishAddinFiles` renamed to `DeployRevitAddin`.
- Switched `DebugType` property for `Release` configuration to `portable`.
- Removed `EnableDynamicLoading` property. Enable for the project manually

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