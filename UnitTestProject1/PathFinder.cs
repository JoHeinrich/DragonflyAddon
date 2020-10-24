using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnitTestProject1
{
    public class PathFinder
    {
        //get python path from environtment variable
        public static string GetPythonPathfromPath()
        {
            var environmentVariables = Environment.GetEnvironmentVariables();
            string pathVariable = environmentVariables["Path"] as string;
            if (pathVariable != null)
            {
                string[] allPaths = pathVariable.Split(';');
                foreach (var path in allPaths)
                {
                    string pythonPathFromEnv = path + "\\python.exe";
                    if (File.Exists(pythonPathFromEnv))
                        return path;
                }
            }
            return null;
        }
        public static IEnumerable<string> GetPythonInstallations()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu) + "\\Programs";
            string localpath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "\\Programs";

            var files = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories).ToList();
            var localfiles = Directory.EnumerateFiles(localpath, "*", SearchOption.AllDirectories).ToList();
            files.AddRange(localfiles);
            var pythonFiles = files.Where(x => x.Contains("Python")).ToHashSet();
            HashSet<string> installations = new HashSet<string>();
            foreach (var potential in pythonFiles)
            {
                var directory = Path.GetDirectoryName(LinkResolver.ResolveShortcut(potential));
                var pythonExe = Path.Combine(directory, "python.exe");
                if (File.Exists(pythonExe))
                {
                    installations.Add(directory);
                }

            }
            return installations;
        }
        public static string GetPythonInstallationPath()
        {
            return GetPythonInstallations().FirstOrDefault();
        }
    }
}

