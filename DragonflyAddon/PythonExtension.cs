using Python.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using VoiceControl;

namespace DragonflyAddon
{
   

    public class PythonProvider : IProvider<ICommandController>
    {
        Dictionary<string, Func<ICommandController>> mapping = new Dictionary<string, Func<ICommandController>>();
        public PythonProvider(IPaths pathManager)
        {

            PythonEngine.Initialize();

            var path =pathManager.GetPath("Dragonfly/Rules");
            var files = Directory.EnumerateFiles(path,"*.py");
            files = files.Where(x => !Path.GetFileNameWithoutExtension(x).StartsWith("_"));
            foreach (var file in files)
            {
                var name = Path.GetFileNameWithoutExtension(file);
                Console.WriteLine("Python File: "+name);
                string prefix = Assembly.GetExecutingAssembly().GetName().Name + ".Rule.";
                mapping[prefix+name] = () => new DragonflyController(file);
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
            if (analyzer.Error != null)
            {
                builder.AddCommand(analyzer.Error, () => { });
            }
            else
            {
                foreach (var command in analyzer.Commands)
                {
                    builder.AddCommand(command, () => analyzer.Execute(command));
                }
            }

        }
    }
}
