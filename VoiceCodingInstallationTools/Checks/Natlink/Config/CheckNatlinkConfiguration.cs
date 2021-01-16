using System.IO;
using System.Text.RegularExpressions;
using VoiceControl;

namespace DragonflyAddon
{
    public class CheckNatlinkConfiguration : ICheckSolve
    {
        string path;
        string userDir;

        public string Error { get; set; } = "Natlink not enabled";
        public string AvailableAction => "run natlink natlinkconfigfunctions with parameter e to enable natlink";

        public CheckNatlinkConfiguration(IPaths paths,string path)
        {
            this.path = path;
            this.userDir = paths.GetPath("Dragonfly");
        }
        public bool Check()
        {
            if (PathFinder.GetPythonPathfromPath() == null) return false;
            var result = Config("-i");
            var match = Regex.Match(result, "(Natlink|NatLink) is enabled");
            Error = "Natlink not enabled";
            if (!match.Success) return false;
            var escapedPath = userDir.Replace(@"\", @"\\");
            match = Regex.Match(result, $@"UserDirectory\s+{escapedPath}");
            Error = "Natlink user directory not set correctly";
            if (!match.Success) return false;
            string value = match.Value;
            Error = "Natlink is configured correctly";
            return true;
        }

        string Config(string command) => ProgramInstaller.RunProcess(PathFinder.GetPythonPathfromPath().Executable, path + " "+ command +" -q");
        string ConfigElevated(string command) => ProgramInstaller.RunElevatedProcess(PathFinder.GetPythonPathfromPath().Executable, path + " " + command +" -q");

        public string Solve()
        {
            return ConfigElevated("-e")
            + ConfigElevated($"-n {userDir}") ;
        }


    }
}

