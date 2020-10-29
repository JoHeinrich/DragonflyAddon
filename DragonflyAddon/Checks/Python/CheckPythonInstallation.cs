using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using VoiceControl;

namespace DragonflyAddon
{
    public class PythonInstallation
    {
        public PythonInstallation(string path)
        {
            Executable = path;
        }

        public string Version
        {
            get
            {
                var version = ProgramInstaller.ExecutePythonCommand(Executable, "import sys\nprint (sys.version)");
                var match = Regex.Match(version, @"\d.\d.\d");
                if(match.Success)
                {
                    return match.Value;
                }
                return null;
            }
        }
        public string Architecture
        {
            get
            {
                string foundArchitecture = ProgramInstaller.ExecutePythonCommand(Executable, "import struct\nprint (struct.calcsize(\"P\") * 8)");
                if (foundArchitecture == null) return "";
                var match = Regex.Match(foundArchitecture, @"\d+");
                if (match.Success)
                {
                    return match.Value;
                }
                return null;
            }
        }

        public string Executable { get; }

        public string Directory => Path.GetDirectoryName(Executable);

        public bool IsVersion(string version)
        {
            var result = Version;
            if (result == null) return false;
            return result.Contains(version);
        }

        public bool IsArchitecture(string architecture)
        {
            return Architecture.Contains(architecture);
        }

        public override string ToString()
        {
            return $"Python x{Architecture} v{Version}";
        }
    }
    public class CheckPythonInstallation : ICheckSolve
    {
        string url;
        string version;
        string architecture;
        public string Error { get; private set; } = "Python not installed";
        public string AvailableAction => $"Will download python from {url} and start installer";
        public CheckPythonInstallation(string version, string architecture, string url)
        {
            this.version = version;
            this.architecture = architecture;
            this.url = url;
        }


        private string VersionError(IEnumerable<PythonInstallation> installations)
        {
            foreach (var installation in installations)
            {
                if (installation.IsVersion(version) && installation.IsArchitecture(architecture)) return "";
            }
            return $"Non of the installed versions is correct {string.Join(" ", installations)}";
        }

        public bool Check()
        {

            var installations = PathFinder.GetPythonInstallations();
            Error  = "Python not installed";
            if (installations.Count() == 0) return false;
            Error = VersionError(installations);
            if (!string.IsNullOrEmpty(Error)) return false;
            Error = "Python is installed";
            return true;
        }

        public string Solve()
        {
            return ProgramInstaller.Install(url);
        }

    }
}

