using System;
using System.Diagnostics;
using System.Net;
using System.IO;

namespace DragonflyAddon
{
    public class ProgramInstaller
    {
        public static string Install(string url)
        {
            var fileName = Path.GetTempPath() + Path.GetFileName(url);
            Path.GetTempFileName();
            var path = Path.GetTempPath();
            DownloadFile(url, fileName);
            var extension = Path.GetExtension(url);
            if (extension == ".exe") return RunProcess(fileName);
            if (extension == ".msi") return InstallMsi(fileName);
            throw new NotImplementedException();
        }
        private static void DownloadFile(string url, string path)
        {
            if (File.Exists(path)) return;
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(url, path);
            }
        }


        public static string InstallMsi(string path)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "msiexec";
                process.StartInfo.WorkingDirectory = Path.GetDirectoryName(path);
                process.StartInfo.Arguments = $" /i {Path.GetFileName(path)} ADDLOCAL=ALL";
                process.StartInfo.Verb = "runas";
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
                var result = process.StandardOutput.ReadToEnd();
                return result;
            }
            catch (Exception e)
            {
                return e.Message;
            }


        }

        public static string RunElevatedProcess(string process, string arguments = "")
        {
            try
            {
                var p = Process.Start(new ProcessStartInfo(process, arguments)
                    {
                        Verb = "runas"
                    }
                );
                p.WaitForExit();
                if (p.ExitCode == 0) return "completed";
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }




        public static string RunProcess(string process,string arguments="")
        {
            try
            {
                var p = Process.Start(new ProcessStartInfo(process, arguments) { UseShellExecute = false, RedirectStandardOutput = true });
                
                p.WaitForExit();
                if (p.ExitCode != 0) return p.ExitCode.ToString();
                var result = p.StandardOutput.ReadToEnd();
                return result;
            }
            catch (Exception e)
            {
                return e.Message;
            }


        }

        public static string ExecutePythonCommand(string pythonPath,string file, params string[] commands)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = pythonPath;
            start.Arguments = file;
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardInput = true;

            using (Process process = Process.Start(start))
            {

                using (StreamWriter writer = process.StandardInput)
                {
                    foreach (var command in commands)
                    {
                        writer.Write(command);
                    }
                }
                

                using (StreamReader reader = process.StandardOutput)
                {
                    string read = "";
                    while (read != null)
                        read = reader.ReadLine();
                    string result = reader.ReadToEnd();
                    return result;
                }
            }
        }
        public static string ExecutePythonCommand(string pythonPath, string command)
            {
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = pythonPath;
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true;
                start.RedirectStandardInput = true;

                using (Process process = Process.Start(start))
                {
                    using (StreamWriter writer = process.StandardInput)
                    {
                        writer.Write(command);
                    }
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                        return result;
                    }
                }
            }
        
    }
}

