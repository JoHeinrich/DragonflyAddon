using Microsoft.VisualStudio.TestTools.UnitTesting;
using Python.Runtime;
using System.Linq;
using VoiceControl;
using DragonflyAddon;
namespace DragonflyAddonTests
{


    [Ignore]
    [TestClass]
    public class ConnectAndExecutePython
    {
        string path = @"C:\Users\laise\Documents\EasyVoiceCodeTest2\Addons\DragonflyAddon\code\DragonflyAddon\UnitTestProject1\Example1.py";

        public ConnectAndExecutePython()
        {
            CheckSolve.Execute<CheckPythonInstallation3_8x32>();
            CheckSolve.Execute<CheckPythonInPath3_8>();

            //PythonEngine.PythonPath += @"C:\Users\laise\Documents\EasyVoiceCodeTest2\Dragonfly";
            //PythonEngine.PythonHome = @"C:\Users\laise\Documents\EasyVoiceCodeTest2\Dragonfly";

            PythonEngine.Initialize();

            var path = PythonEngine.PythonPath;

        }


        [TestMethod]
        public void HasCommands()
        {
            DragonflyAnalyzer analyzer = new DragonflyAnalyzer(path);
            var commands = analyzer.Commands;
            Assert.AreNotEqual(0, commands.Count());
        }

        [TestMethod]
        public void ExecuteCommand()
        {
            DragonflyAnalyzer analyzer = new DragonflyAnalyzer(path);
            analyzer.Execute("test");
        }
    }

    [TestClass]
    public class Installation2_7
    {
        [TestMethod]
        public void Python()
        {
            Assert.AreEqual("",CheckSolve.Execute<CheckPythonInstallation2_7x32>());
        }

        [TestMethod]
        public void Path()
        {
            Assert.AreEqual("", CheckSolve.Execute<CheckPythonInPath2_7>());
        }

        [TestMethod]
        public void Packages()
        {
            Assert.AreEqual("", CheckSolve.Execute<CheckPipPackage2_7>());
        }

        [TestMethod]
        public void Natlink()
        {
            Assert.AreEqual("", CheckSolve.Execute<CheckNatlinkInstallationFor2_7>());
        }


        [TestMethod]
        public void Configuration()
        {
            Assert.AreEqual("", CheckSolve.Execute<CheckNatlinkConfiguration2_7>());
        }
    }


    [TestClass]
    public class Installation3_8
    {
        [TestMethod]
        public void Python()
        {
            Assert.AreEqual("", CheckSolve.Execute<CheckPythonInstallation3_8x32>());
        }

        [TestMethod]
        public void Path()
        {
            Assert.AreEqual("", CheckSolve.Execute<CheckPythonInPath3_8>());
        }

        [TestMethod]
        public void Packages()
        {
            Assert.AreEqual("", CheckSolve.Execute<CheckPipPackage3_8>());
        }

        [TestMethod]
        public void Natlink()
        {
            Assert.AreEqual("", CheckSolve.Execute<CheckNatlinkInstallationFor3_8>());
        }


        [TestMethod]
        public void Configuration()
        {
            Assert.AreEqual("", CheckSolve.Execute<CheckNatlinkConfiguration3_8>());
        }
    }
}

