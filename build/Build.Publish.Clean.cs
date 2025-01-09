using Nuke.Common.Tools.Git;

sealed partial class Build
{
    Target CleanFailedPublish => _ => _
        .AssuredAfterFailure()
        .OnlyWhenDynamic(() => FailedTargets.Contains(PublishGitHub))
        .Executes(() =>
        {
            Log.Information("Cleaning failed GitHub release");
            GitTasks.Git($"push --delete origin {Version}", logInvocation: false);
        });
}