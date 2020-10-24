using Microsoft.VisualStudio.TestTools.UnitTesting;
using DragonflyAddon;
using Python.Runtime;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Win32;
using VoiceControl;

namespace UnitTestProject1
{
    [DeploymentItem("bridge.py")]
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void PythonPath()
        {
            var pythonPath = PathFinder.GetPythonInstallationPath();
            Assert.IsNotNull(pythonPath);

        }

        [TestMethod]
        public void TestResolve()
        {
            var resolved = LinkResolver.ResolveShortcut("C:\\Users\\laise\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Python 3.7\\IDLE (Python 3.7 64-bit).lnk");
            Assert.IsNotNull(resolved);

        }

        void Check<T>()where T : ICheckSolve, new()
        {
            T installation = new T();
            if (!installation.Check())
            {
                installation.Solve();
            }
        }

        public void TestFunc()
        {
            Check<CheckPythonInstallation>();
            Check<CheckPythonInPath>();

            PythonEngine.Initialize();

            // acquire the GIL before using the Python interpreter
            using (Py.GIL())
            {
                var fpath = @"C:/Users/laise/Documents/VoiceCoding/Natlink/_example1.py";


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

    public class CheckDragonflyPackage : CheckSolve
    {
        public CheckDragonflyPackage(string error, string action) : base("Dragonfly package not installed", "Runs pip install dragonfly2")
        {

        }

        public override bool InternalCheck()
        {
            throw new NotImplementedException();
        }

        public override void InternalSolve()
        {
            var pythonDir = PathFinder.GetPythonPathfromPath();
            Process.Start(new ProcessStartInfo(pythonDir + "/Scripts/pip.exe", "install numpy"));
            var p = Process.Start(new ProcessStartInfo(pythonDir + "/Scripts/pip.exe", "install dragonfly2") { RedirectStandardOutput = true });
            var resut = p.StandardOutput.ReadToEnd();
        }

    }

    public class CheckPythonInstallation : ICheckSolve
    {
        public CheckState State { get; }
        public string ErrorMessage => "Python not installed";
        public string ManualOptions => "Go to https://www.python.org/ download the windows version and install on your pc";
        public string AutomatedAction => "Will download python from https://www.python.org/ftp/python/3.7.9/python-3.7.9-amd64.exe and start installer";

        public event Action Changed;

        public bool Check()
        {
            var path = PathFinder.GetPythonInstallationPath();
            return path != null;
        }

        public void Solve()
        {
            throw new NotImplementedException();
        }
    }
    public class DragonflyAnalyzer
    {

    }
}

