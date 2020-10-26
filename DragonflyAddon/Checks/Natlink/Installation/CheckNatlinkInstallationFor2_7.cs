using System.IO;
using VoiceControl;

namespace DragonflyAddon
{
    public class CheckNatlinkInstallationFor2_7 : ICheckSolve
    {
        string url = "https://kumisystems.dl.sourceforge.net/project/natlink/natlink/natlink4.2/setup-natlink-4.2.exe";
        public string Error => "Natlink not installed";
        public string AvailableAction => $"Will clone Natlink from {url}";

        public bool Check()
        {
            return File.Exists(@"C:\Natlink\Natlink\confignatlinkvocolaunimacro\natlinkconfigfunctions.py");
        }

        public string Solve()
        {
            return ProgramInstaller.Install(url);
        }
    }
}

