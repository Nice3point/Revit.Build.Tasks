sealed partial class Build
{
    const string Version = "2.0.0";
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