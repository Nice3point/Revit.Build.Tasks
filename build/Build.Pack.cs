using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

sealed partial class Build
{
    Target Pack => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            ValidateRelease();

            DotNetPack(settings => settings
                .SetConfiguration("Release")
                .SetVersion(Version)
                .SetOutputDirectory(ArtifactsDirectory)
                .SetVerbosity(DotNetVerbosity.minimal)
                .SetPackageReleaseNotes(CreateNugetChangelog()));
        });
}