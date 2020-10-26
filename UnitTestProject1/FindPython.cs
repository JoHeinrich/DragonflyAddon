using Microsoft.VisualStudio.TestTools.UnitTesting;
using DragonflyAddon;
using System;
using System.Reflection;
using Microsoft.Win32;
using VoiceControl;
using System.Linq;
using System.IO;

namespace DragonflyAddonTests
{
    [TestClass]
    public class FindPython
    {
        [TestMethod]
        public void PythonInPath()
        {
            var pythonPath = PathFinder.GetPythonInstallationPath();
            Assert.IsNotNull(pythonPath);
            Assert.IsTrue(!String.IsNullOrEmpty(pythonPath.Version));

        }

        [TestMethod]
        public void PythonVersion()
        {
            var pythonPath = PathFinder.GetPythonInstallationsFromStartMenu().ElementAt(0);
            Assert.IsTrue(!string.IsNullOrEmpty(pythonPath.Version));
        }

        [TestMethod]
        public void PythonArchitecture()
        {
            var pythonPath = PathFinder.GetPythonInstallationsFromStartMenu().ElementAt(0);
            Assert.AreNotEqual("",pythonPath.Architecture);
        }

        [TestMethod]
        public void FindPythonInstallationIfInstalled()
        {
            var pythonPath = PathFinder.GetPythonInstallations();
            Assert.AreNotEqual(0, pythonPath.Count());
        }

        [TestMethod]
        public void ResolveExistingShortcut()
        {
            var resolved = LinkResolver.ResolveShortcut("C:\\Users\\laise\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Python 3.8\\IDLE (Python 3.8 32-bit).lnk");
            Assert.IsFalse(String.IsNullOrEmpty(resolved));
        }

        [TestMethod]
        public void ResolvePython2_7Shortcut()
        {
            var path = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Python 2.7\Python (command line).lnk";
            if(!File.Exists(path))return;
            var resolved = LinkResolver.ResolveShortcut(path);
            Assert.IsFalse(String.IsNullOrEmpty(resolved));
        }




    }
}

