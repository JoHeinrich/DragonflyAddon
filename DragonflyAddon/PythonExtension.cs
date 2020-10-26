using Python.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VoiceControl;

namespace DragonflyAddon
{
    
    public class PythonProvider : IProvider<ICommandController>
    {
        Dictionary<string, Func<ICommandController>> mapping = new Dictionary<string, Func<ICommandController>>();
        public PythonProvider(IPaths pathManager)
        {
            Check<CheckPythonInstallation3_8x32>();
            Check<CheckPythonInPath3_8>();

            PythonEngine.Initialize();

            //var path = PythonEngine.PythonPath;
            var path =pathManager.GetPath("Dragonfly");
            foreach (var file in Directory.EnumerateFiles(path))
            {
                var name = Path.GetFileNameWithoutExtension(file);
                Console.WriteLine("Python File: "+name);
                mapping.Add(name, () => new DragonflyController(file));
            }
        }
        public IEnumerable<string> Available => mapping.Keys.ToList();

        public ICommandController Instantiate(string identifier)
        {
            return mapping[identifier]();
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

    internal class DragonflyController : ICommandController
    {
        DragonflyAnalyzer analyzer;
        public DragonflyController(string path)
        {
            analyzer = new DragonflyAnalyzer(path);
        }

        public void Build(ICommandBuilder builder)
        {
            foreach (var command in analyzer.Commands)
            {
                builder.AddCommand(command, () => analyzer.Execute(command));
            }
        }
    }
}
