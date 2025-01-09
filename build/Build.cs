using Nuke.Common.Git;
using Nuke.Common.ProjectModel;

sealed partial class Build : NukeBuild
{
    [GitRepository] readonly GitRepository GitRepository;
    [Solution(GenerateProjects = true)] readonly Solution Solution;

    public static int Main() => Execute<Build>(build => build.Compile);
}