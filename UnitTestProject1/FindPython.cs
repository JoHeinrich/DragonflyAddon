using Microsoft.VisualStudio.TestTools.UnitTesting;
using DragonflyAddon;
using System;
using System.Reflection;
using Microsoft.Win32;
using VoiceControl;
using System.Linq;

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
            var resolved = LinkResolver.ResolveShortcut("C:\\Users\\laise\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Python 3.7\\IDLE (Python 3.7 64-bit).lnk");
            Assert.IsNotNull(resolved);
        }




    }
}

