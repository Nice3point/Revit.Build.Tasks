using Nuke.Common.Git;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

sealed partial class Build
{
    [Parameter] [Secret] string NugetApiKey = EnvironmentInfo.GetVariable("NUGET_API_KEY");

    Target PublishNuget => _ => _
        .DependsOn(Pack)
        .Requires(() => NugetApiKey)
        .OnlyWhenStatic(() => IsServerBuild && GitRepository.IsOnMainBranch())
        .Executes(() =>
        {
            foreach (var package in ArtifactsDirectory.GlobFiles("*.nupkg"))
            {
                DotNetNuGetPush(settings => settings
                    .SetTargetPath(package)
                    .SetApiKey(NugetApiKey)
                    .SetSource("https://api.nuget.org/v3/index.json"));
            }
        });
}