sealed partial class Build
{
    const string Version = "1.0.0-preview.1.4";
    readonly AbsolutePath ArtifactsDirectory = RootDirectory / "output";
    readonly AbsolutePath ChangeLogPath = RootDirectory / "Changelog.md";

    protected override void OnBuildInitialized()
    {
        Configurations = 
        [
            "Release"
        ];
    }
}