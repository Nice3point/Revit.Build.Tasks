sealed partial class Build
{
    readonly AbsolutePath ArtifactsDirectory = RootDirectory / "output";
    readonly AbsolutePath ChangeLogPath = RootDirectory / "Changelog.md";

    [Parameter] string ReleaseVersion;

    protected override void OnBuildInitialized()
    {
        ReleaseVersion ??= GitRepository.Tags.SingleOrDefault();
    }
}