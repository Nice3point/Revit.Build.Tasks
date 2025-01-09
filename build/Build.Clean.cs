sealed partial class Build
{
    Target Clean => _ => _
        .Executes(() =>
        {
            Log.Information(GitHubToken);
            Log.Information(NugetApiKey);
            Log.Information(GitRepository.Commit);
            Log.Information(GitRepository.Branch);
            Log.Information(GitRepository.Head);
            Log.Information(string.Join(", ", GitRepository.Tags));
            CleanDirectory(ArtifactsDirectory);

            foreach (var project in Solution.AllProjects.Where(project => project != Solution.Build))
                CleanDirectory(project.Directory / "bin");
        });

    static void CleanDirectory(AbsolutePath path)
    {
        Log.Information("Cleaning directory: {Directory}", path);
        path.CreateOrCleanDirectory();
    }
}