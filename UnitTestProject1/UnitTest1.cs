using Microsoft.VisualStudio.TestTools.UnitTesting;
using DragonflyAddon;
using Python.Runtime;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace UnitTestProject1
{
    class MappingRule
    {
        public dynamic mapping;
    }
    [DeploymentItem("bridge.py")]
    [TestClass]
    public class UnitTest1
    {

        //get python path from environtment variable
        string GetPythonPathfromPath()
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
                        return pythonPathFromEnv;
                }
            }
            return null;
        }
        string GetPythonPath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu) + "\\Programs";
            string localpath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "\\Programs";

            var files = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories).ToList();
            var localfiles = Directory.EnumerateFiles(localpath, "*", SearchOption.AllDirectories).ToList();
            files.AddRange(localfiles);
            var pythonFiles = files.Where(x => x.Contains("Python")).ToHashSet();
            foreach (var potential in pythonFiles)
            {
                var directory = Path.GetDirectoryName(LinkResolver.ResolveShortcut(potential));
                var pythonExe = Path.Combine(directory, "python.exe");
                if (File.Exists(pythonExe))
                {
                    return pythonExe;
                }

            }
            return null;
        }


        [TestMethod]
        public void PythonPath()
        {
            var pythonPath = GetPythonPath();
            Assert.IsNotNull(pythonPath);

        }

        [TestMethod]
        public void TestResolve()
        {
            var resolved = LinkResolver.ResolveShortcut("C:\\Users\\laise\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Python 3.7\\IDLE (Python 3.7 64-bit).lnk");
            Assert.IsNotNull(resolved);

        }

        public void TestFunc()
        {

            var pythonPath = GetPythonPath();
            var pythonPathp = GetPythonPathfromPath();

            string pythonDir = @"C:\Users\laise\AppData\Local\Programs\Python\Python37";
            string path = pythonDir + ";" + pythonDir + "\\Scripts;" + Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
            Environment.SetEnvironmentVariable("PATH", path, EnvironmentVariableTarget.Process);
            //Environment.SetEnvironmentVariable("PYTHONHOME", @"c:\dev\libs\Anaconda3\envs\python2", EnvironmentVariableTarget.Process);
            //Environment.SetEnvironmentVariable("PYTHONPATH ", @"c:\dev\libs\Anaconda3\envs\python2\Lib", EnvironmentVariableTarget.Process);

            Process.Start(new ProcessStartInfo(pythonDir + "/Scripts/pip.exe", "install numpy"));
            var p = Process.Start(new ProcessStartInfo(pythonDir + "/Scripts/pip.exe", "install dragonfly2") { RedirectStandardOutput = true });
            var resut = p.StandardOutput.ReadToEnd();
            var pat = PythonEngine.PythonPath;
            PythonEngine.Initialize();

            // create a person object
            Person person = new Person("John", "Smith");



            // acquire the GIL before using the Python interpreter
            using (Py.GIL())
            {
                var fpath = @"C:/Users/laise/Documents/VoiceCoding/Natlink/_example1.py";
                //Py.Import();
                // create a Python scope


                var t = Assembly.GetExecutingAssembly().Location;
                Console.WriteLine(t);
                using (PyScope scope = Py.CreateScope())
                {
                    scope.Import("os");
                    //scope.Import("dragonfly");
                    scope.Exec("from dragonfly import *");
                    scope.Exec("path = os.getcwd()");
                    var s = scope.Get<string>("path");
                    scope.Exec("with open (\"" + fpath + "\", \"r\") as file:\n\texec(file.read())");
                    //var b = scope.Get<string>("bridge");
                    //scope.Exec(b);
                    //var tset = scope.Get("MyRule");
                    //scope.Exec("my_function(\"" + fpath + "\")");
                    var v = scope.Get("MyRule");
                    var mem = v.GetDynamicMemberNames();
                    var mapping = v.GetAttr("mapping");
                    var mem2 = mapping.GetDynamicMemberNames();
                    var items = mapping.GetAttr("keys");
                    var mem3 = items.GetDynamicMemberNames();
                    foreach (var item in mapping)
                    {
                        var target = mapping.GetItem(item.ToPython());
                        Console.WriteLine(item + ":" + target);
                        //target.InvokeMethod("execute");
                    }
                    var en = mapping.GetEnumerator();
                    var o = en.Current;
                    while (en.MoveNext())
                    {
                        o = en.Current;
                    }
                    // convert the Person object to a PyObject
                    PyObject pyPerson = person.ToPython();

                    // create a Python variable "person"
                    scope.Set("person", pyPerson);

                    // the person object may now be used in Python
                    string code = "fullName = person.FirstName + ' ' + person.LastName";
                    scope.Exec(code);
                }
            }
        }



        [DeploymentItem("bridge.py")]
        [TestMethod]
        public void TestMethod1()
        {
            TestFunc();
        }
    }
}

