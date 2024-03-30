using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;
using Task = Microsoft.Build.Utilities.Task;

namespace Nice3point.Revit.Build.Tasks;

public class GenerateRevitCompatibleDefineConstants : Task
{
    [Required] public string Configuration { get; set; }
    [Required] public string[] Configurations { get; set; }
    public string CurrentVersion { get; set; }

    [Output] public string[] DefineConstants { get; private set; }

    public override bool Execute()
    {
        try
        {
            int currentVersion;
            if (string.IsNullOrEmpty(CurrentVersion))
            {
                if (!TryGetRevitVersion(Configuration, out currentVersion)) return true;
            }
            else
            {
                if (!int.TryParse(CurrentVersion, out currentVersion)) return true;
            }

            var constants = new List<string>();
            
            AddVersionConstants(constants, currentVersion);
            AddGreaterConstants(currentVersion, constants);

            DefineConstants = constants
                .Select(constant => constant.Replace('.', '_'))
                .Distinct()
                .OrderBy(constant => constant)
                .ToArray();
            
            return true;
        }
        catch (Exception exception)
        {
            Log.LogErrorFromException(exception, false);
            return false;
        }
    }

    private static void AddVersionConstants(List<string> constants, int currentVersion)
    {
        constants.Add($"REVIT{currentVersion}");
    }

    private void AddGreaterConstants(int currentVersion, List<string> constants)
    {
        foreach (var configuration in Configurations)
        {
            if (!TryGetRevitVersion(configuration, out var version)) continue;
            if (version > currentVersion) continue;

            constants.Add($"REVIT{version}_OR_GREATER");
        }
    }
    
    private static bool TryGetRevitVersion(string configuration, out int version)
    {
        version = 0;

        if (configuration.Length >= 4)
        {
            if (int.TryParse(configuration.Substring(configuration.Length - 4), out version))
            {
                return true;
            }
        }

        if (configuration.Length >= 2)
        {
            if (int.TryParse(configuration.Substring(configuration.Length - 2), out version))
            {
                version += 2000;
                return true;
            }
        }

        return false;
    }
}