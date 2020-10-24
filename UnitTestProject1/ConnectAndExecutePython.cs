using Microsoft.VisualStudio.TestTools.UnitTesting;
using Python.Runtime;
using System.Linq;
using VoiceControl;
using DragonflyAddon;
namespace DragonflyAddonTests
{
    [TestClass]
    public class ConnectAndExecutePython
    {
        string path = @"C:/Users/laise/Documents/VoiceCoding/Natlink/Example1.py";

        public ConnectAndExecutePython()
        {
            Check<CheckPythonInstallation>();
            Check<CheckPythonInPath>();

            PythonEngine.Initialize();
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

        public static void Check<T>() where T : ICheckSolve, new()
        {
            T installation = new T();
            if (!installation.Check())
            {
                installation.Solve();
            }
        }
    }
}

