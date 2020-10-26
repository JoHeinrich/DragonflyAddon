using System;
using System.Diagnostics;
using System.Net;
using System.IO;
using VoiceControl;

namespace DragonflyAddon
{
    public class CheckNatlinkInstallationFor3_8 : ICheckSolve
    {
        string url = "https://github.com/dictation-toolbox/natlink.git";
        public string Error =>"Natlink not installed";
        public string AvailableAction =>$"Will clone Natlink from {url}";


        public bool Check()
        {
            return File.Exists(@"C:\DT\natlink\ConfigureNatlink\natlinkconfigfunctions.py");
        }


        public bool GitClone(string cloneUrl, string path)
        {
            string gitAddArgument = @"clone " + cloneUrl;
            try
            {
                var p = Process.Start(new ProcessStartInfo("git") { WorkingDirectory = path, Arguments = gitAddArgument });
                p.WaitForExit();
                return (p.ExitCode != 0);
            }
            catch (System.Exception e)
            {
                return false;
            }
        }


        public string Solve()
        {
            string path = @"C:\DT\";
            Directory.CreateDirectory(path);
            GitClone(url, path);
            return null;
        }

    }
}

