using System.Text.RegularExpressions;
using VoiceControl;

namespace DragonflyAddon
{
    public class CheckNatlinkConfiguration : ICheckSolve
    {
        string path;
        public string Error => "Natlink not enabled";
        public string AvailableAction => "run natlink natlinkconfigfunctions with parameter e to enable natlink";

        public CheckNatlinkConfiguration(string path)
        {
            this.path = path;
        }
        public bool Check()
        {
            if (PathFinder.GetPythonPathfromPath() == null) return false;
            var result = Config("-i");
            var match = Regex.Match(result, "(Natlink|NatLink) is enabled");
            return match.Success;
        }

        string Config(string command) => ProgramInstaller.RunProcess(PathFinder.GetPythonPathfromPath().Executable, path + " "+ command +" -q");
        string ConfigElevated(string command) => ProgramInstaller.RunElevatedProcess(PathFinder.GetPythonPathfromPath().Executable, path + " " + command +" -q");

        public string Solve()
        {
            return ConfigElevated("-e");
        }


    }
}

