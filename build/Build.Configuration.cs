using Nuke.Common.CI.GitHubActions;

sealed partial class Build
{
    readonly AbsolutePath ArtifactsDirectory = RootDirectory / "output";
    readonly AbsolutePath ChangeLogPath = RootDirectory / "Changelog.md";

    string ReleaseVersion => ((GitRepository.Branch is null && GitRepository.Tags.Count > 0) ||
                              (GitRepository.Branch is not null && GitRepository.Branch.StartsWith("refs/tags"))) switch
    {
        true when GitHubActions.Instance is not null => GitHubActions.Instance.RefName,
        true => GitRepository.Tags.Single(),
        false => "1.0.0"
    };
}