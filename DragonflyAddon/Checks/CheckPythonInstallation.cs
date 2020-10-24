using System;
using System.Diagnostics;
using System.Net;
using System.IO;

namespace DragonflyAddon
{
    public class CheckPythonInstallation : CheckSolve
    {
        string url = "https://www.python.org/ftp/python/3.7.9/python-3.7.9-amd64.exe";

        public CheckPythonInstallation() : base("Python not installed", "Will download python from https://www.python.org/ftp/python/3.7.9/python-3.7.9-amd64.exe and start installer")
        {

        }

        public override bool InternalCheck()
        {
            var path = PathFinder.GetPythonInstallationPath();
            return path != null;
        }

        private void DownloadFile(string url, string path)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(url, path);
            }
        }

        public override void InternalSolve()
        {
            var fileName = Path.GetTempPath()+ "python-3.7.9-amd64.exe";
            Path.GetTempFileName();
            var path = Path.GetTempPath();
            DownloadFile(url, fileName);
            try
            {
                Process.Start(fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }
}

