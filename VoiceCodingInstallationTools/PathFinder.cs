using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DragonflyAddon
{
    public class PathFinder
    {
        public static IEnumerable<PythonInstallation> GetPythonInstallationsfromPath()
        {
            var machinePathVariable = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
            var userPathVariable = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
            var pathVariable = machinePathVariable + ";" + userPathVariable;

            List<string> paths = new List<string>();
            string[] allPaths = pathVariable.Split(';');
            foreach (var path in allPaths)
            {
                string pythonPathFromEnv = Path.Combine(path, "python.exe");
                if (File.Exists(pythonPathFromEnv))
                {
                    paths.Add(pythonPathFromEnv);
                }
            }
            return paths.Select(x=>new PythonInstallation(x));
        }
        //get python path from environtment variable
        public static PythonInstallation GetPythonPathfromPath()
        {
            var paths = GetPythonInstallationsfromPath();
            
            foreach (var path in paths)
            {
                if (File.Exists(path.Executable))
                {
                    var p = Process.Start(new ProcessStartInfo(path.Executable, "--version") { UseShellExecute = false, RedirectStandardOutput = true });
                    p.WaitForExit();
                    if (p.ExitCode != 0) continue;
                    var result = p.StandardOutput.ReadToEnd();
                    return path;

                }
                        
            }
            
            return null;
        }
        public static IEnumerable<PythonInstallation> GetPythonInstallations()
        {
            var path = new HashSet<PythonInstallation>( GetPythonInstallationsfromPath());
            GetPythonInstallationsFromStartMenu().ToList().ForEach(x => path.Add(x));
            return path;
        }
        public static IEnumerable<PythonInstallation> GetPythonInstallationsFromStartMenu()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu) + "\\Programs";
            string localpath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "\\Programs";

            var files = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories).ToList();
            var localfiles = Directory.EnumerateFiles(localpath, "*", SearchOption.AllDirectories).ToList();
            files.AddRange(localfiles);
            var pythonFiles = new HashSet<string>(files.Where(x => x.Contains("Python")));
            HashSet<string> installations = new HashSet<string>();
            foreach (var potential in pythonFiles)
            {
                var directory = Path.GetDirectoryName(LinkResolver.ResolveShortcut(potential));
                var pythonExe = Path.Combine(directory, "python.exe");
                if (File.Exists(pythonExe))
                {
                    installations.Add(pythonExe);
                }

            }
            return installations.Select(x => new PythonInstallation(x));
        }
        public static PythonInstallation GetPythonInstallationPath()
        {
            return GetPythonInstallations().FirstOrDefault();
        }
    }
}

