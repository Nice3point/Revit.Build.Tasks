# 3.0.0-preview.1.1

- Property `PublishAddinFiles` renamed to ``PublishRevitAddin`.
- Added new property `PublishRevitFiles`, to publish files to the `bin/publish` folder.

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