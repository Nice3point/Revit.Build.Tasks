using Nuke.Common.Git;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

sealed partial class Build
{
    [Secret] [Parameter] string NugetApiKey;

    Target NuGetPush => _ => _
        .DependsOn(Pack)
        .Requires(() => NugetApiKey)
        .OnlyWhenStatic(() => IsServerBuild && GitRepository.IsOnMainBranch())
        .Executes(() =>
        {
            return;
            foreach (var package in ArtifactsDirectory.GlobFiles("*.nupkg"))
            {
                DotNetNuGetPush(settings => settings
                    .SetTargetPath(package)
                    .SetApiKey(NugetApiKey)
                    .SetSource("https://api.nuget.org/v3/index.json"));
            }
        });
}