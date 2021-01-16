using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VoiceControl;

namespace DragonflyAddon
{

    public class CheckPipPackage : ICheckSolve
    {
        public string Error { get; private set;} = "Missing Packages";
        public string AvailableAction { get; private set; } 

        string pythonDir => PathFinder.GetPythonPathfromPath().Directory;

        List<string> Packages; 

        public CheckPipPackage(params string[] packages)
        {
            Packages = packages.ToList();
            AvailableAction  = $"Installes {string.Join(" ", Packages)}";
        }

        string Pip(string command) => ProgramInstaller.RunProcess(pythonDir + "/Scripts/pip.exe", command);
        List<string> MissingPackages()
        {
            var result = Pip("list");
            List<string> missingPackages = new List<string>();
            foreach (var Package in Packages)
            {
                string package = Package.Split('=')[0];
                if (!result.Contains(package))
                {
                    missingPackages.Add(Package);
                }
            }
            return missingPackages;
        }
        public bool Check()
        {
            Error = "Could not find Python in Path";
            if (PathFinder.GetPythonPathfromPath() == null) return false;
            var missingPackages = MissingPackages();
            Error = "All necessary Packages are installed";
            if (missingPackages.Count == 0) return true;
            Error = "Missing Packages: " +  String.Join(" ", missingPackages);
            AvailableAction = "pip install " + String.Join(" ", missingPackages);
            return false;
        }




        public string Solve()
        {
            string error = "";
            var missingPackages = MissingPackages();

            foreach (var Package in missingPackages)
            {
                error += Pip("install " + Package);
            }

            return error;
        }


    }
}

