using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;
using Task = Microsoft.Build.Utilities.Task;

namespace Nice3point.Revit.Build.Tasks;

public class AddImplicitUsings : Task
{
    [Required] public ITaskItem[] AdditionalUsings { get; set; }
    [Required] public ITaskItem[] PackageReferences { get; set; }

    [Output] public string[] Usings { get; private set; }

    public override bool Execute()
    {
        try
        {
            var usings = new List<string>();
            foreach (var additionalUsing in AdditionalUsings)
            {
                var requiredPackage = additionalUsing.GetMetadata("RequiredPackage");
                if (string.IsNullOrEmpty(requiredPackage))
                {
                    usings.Add(additionalUsing.ItemSpec);
                }
                else
                {
                    var existedPackage = PackageReferences.FirstOrDefault(item => item.ItemSpec == requiredPackage);
                    if (existedPackage is not null)
                    {
                        usings.Add(additionalUsing.ItemSpec);
                    }
                }
            }

            Usings = usings.ToArray();
            return true;
        }
        catch (Exception exception)
        {
            Log.LogErrorFromException(exception, false);
            return false;
        }
    }
}