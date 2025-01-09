using Nuke.Common.CI.GitHubActions;

sealed partial class Build
{
    string Version => GitHubActions.Instance.RefName;
    
    readonly AbsolutePath ArtifactsDirectory = RootDirectory / "output";
    readonly AbsolutePath ChangeLogPath = RootDirectory / "Changelog.md";
}