using System.Text;
using Nuke.Common.Git;
using Nuke.Common.Tools.Git;
using Nuke.Common.Tools.GitHub;
using Octokit;

sealed partial class Build
{
    Target PublishGitHub => _ => _
        .DependsOn(Pack)
        .OnlyWhenStatic(() => IsServerBuild && GitRepository.IsOnMainBranch())
        .Executes(async () =>
        {
            var gitHubName = GitRepository.GetGitHubName();
            var gitHubOwner = GitRepository.GetGitHubOwner();

            ValidateRelease();

            var changelog = CreateGithubChangelog();
            var artifacts = Directory.GetFiles(ArtifactsDirectory, "*");
            Assert.NotEmpty(artifacts, "No artifacts were found to create the Release");

            var newRelease = new NewRelease(Version)
            {
                Name = Version,
                Body = changelog,
                TargetCommitish = GitRepository.Commit,
                Prerelease = Version.Contains("preview", StringComparison.OrdinalIgnoreCase)
            };

            var release = await GitHubTasks.GitHubClient.Repository.Release.Create(gitHubOwner, gitHubName, newRelease);
            await UploadArtifactsAsync(release, artifacts);
        });

    void ValidateRelease()
    {
        var tags = GitTasks.Git("describe --tags --abbrev=0 --always", logInvocation: false, logOutput: false);
        var latestTag = tags.First().Text;
        if (latestTag == GitRepository.Commit) return;

        Assert.False(latestTag == Version, $"A Release with the specified tag already exists in the repository: {Version}");
        Log.Information("Version: {Version}", Version);
    }

    static async Task UploadArtifactsAsync(Release createdRelease, IEnumerable<string> artifacts)
    {
        foreach (var file in artifacts)
        {
            var releaseAssetUpload = new ReleaseAssetUpload
            {
                ContentType = "application/x-binary",
                FileName = Path.GetFileName(file),
                RawData = File.OpenRead(file)
            };

            await GitHubTasks.GitHubClient.Repository.Release.UploadAsset(createdRelease, releaseAssetUpload);
            Log.Information("Artifact: {Path}", file);
        }
    }

    void WriteCompareUrl(StringBuilder changelog)
    {
        var tags = GitTasks.Git("describe --tags --abbrev=0 --always", logInvocation: false, logOutput: false);
        var latestTag = tags.First().Text;
        if (latestTag == GitRepository.Commit) return;

        if (changelog[^1] != '\r' || changelog[^1] != '\n') changelog.AppendLine(Environment.NewLine);
        changelog.Append("Full changelog: ");
        changelog.Append(GitRepository.GetGitHubCompareTagsUrl(Version, latestTag));
    }
}