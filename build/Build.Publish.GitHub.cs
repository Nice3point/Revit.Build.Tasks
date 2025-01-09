using System.Text;
using Nuke.Common.Tools.Git;
using Nuke.Common.Tools.GitHub;
using Octokit;

sealed partial class Build
{
    Target PublishGitHub => _ => _
        .DependsOn(Pack)
        .OnlyWhenStatic(() => IsServerBuild)
        .Executes(async () =>
        {
            throw new Exception("Test");
            var gitHubName = GitRepository.GetGitHubName();
            var gitHubOwner = GitRepository.GetGitHubOwner();

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
        var tags = GitTasks
            .Git("tag --list", logInvocation: false, logOutput: false)
            .ToArray();

        if (tags.Length < 2) return;

        if (changelog[^1] != '\r' || changelog[^1] != '\n') changelog.AppendLine(Environment.NewLine);
        changelog.Append("Full changelog: ");
        changelog.Append(GitRepository.GetGitHubCompareTagsUrl(tags[^1].Text, tags[^2].Text));
    }
}