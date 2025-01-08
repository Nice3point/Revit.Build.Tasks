using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

sealed partial class Build
{
    Target Pack => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            ValidateRelease();

            foreach (var configuration in GlobBuildConfigurations())
            {
                DotNetPack(settings => settings
                    .SetConfiguration(configuration)
                    .SetVersion(Version)
                    .SetOutputDirectory(ArtifactsDirectory)
                    .SetVerbosity(DotNetVerbosity.minimal)
                    .SetPackageReleaseNotes(CreateNugetChangelog()));
            }
        });
}