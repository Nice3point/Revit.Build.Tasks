using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

sealed partial class Build
{
    Target Compile => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            ValidateRelease();

            DotNetBuild(settings => settings
                .SetConfiguration("Release")
                .SetOutputDirectory(ArtifactsDirectory)
                .SetVerbosity(DotNetVerbosity.minimal));
        });
}