﻿sealed partial class Build
{
    Target Clean => _ => _
        .Executes(() =>
        {
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