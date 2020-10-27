
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VoiceControl;

namespace DragonflyAddon
{
    public class CheckPythonInPath2_7 : CheckPythonInPath
    {
        public CheckPythonInPath2_7() : base("2.7")
        {

        }
    }
    public class CheckPythonInPath3_8 : CheckPythonInPath
    {
        public CheckPythonInPath3_8() : base("3.8")
        {
        }
    }
    public class CheckPythonInPath : ICheckSolve
    {
        string version;
        public string Error => "Python not in path";
        public string AvailableAction => "Adding Python to Path";

        public CheckPythonInPath(string version)
        {
            this.version = version;
        }

        public bool Check()
        {
            try
            {
                var path = PathFinder.GetPythonPathfromPath();
                if (path == null) return false;
                if (path.IsVersion(version)) return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string Solve()
        {
            var installations = PathFinder.GetPythonInstallations();
            var currentPath = PathFinder.GetPythonInstallationsfromPath();

            var paths = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine).Split(';').ToList();
            var userPaths = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User).Split(';').ToList();

            List<string> pythonPath = new List<string>();
            foreach (var installation in installations)
            {
                if(installation.IsVersion(version))
                {
                    paths.Add(installation.Directory);
                    paths.Add(Path.Combine(installation.Directory, "Scripts"));
                    pythonPath.Add(Path.Combine(installation.Directory, "Lib"));
                    pythonPath.Add(Path.Combine(installation.Directory, "DLLs"));
                }
                else
                {
                    paths = paths.Where(x => !x.Contains(installation.Directory)).ToList();
                    userPaths = userPaths.Where(x => !x.Contains(installation.Directory)).ToList();
                }
                paths = paths.Where(x => !x.Contains("python.exe")).ToList();
                paths = new HashSet<string>(paths).ToList();
                userPaths = new HashSet<string>(userPaths).ToList();
            }

            //Environment.SetEnvironmentVariable("PYTHONHOME", @"c:\dev\libs\Anaconda3\envs\python2", EnvironmentVariableTarget.Process);
            //Environment.SetEnvironmentVariable("PYTHONPATH ", @"C:\Python27\Lib;C:\Python27\DLLs;C:\Python27\Lib\lib-tk;C:\NatLink\NatLink\MacroSystem\core", EnvironmentVariableTarget.Process);


            Environment.SetEnvironmentVariable("PATH", string.Join(";", paths), EnvironmentVariableTarget.Machine);
            Environment.SetEnvironmentVariable("PATH", string.Join(";", userPaths), EnvironmentVariableTarget.User);

            return "";
        }

    }
}

