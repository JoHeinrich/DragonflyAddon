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
            var path=pathManager.GetPath("Python");
            foreach (var file in Directory.EnumerateFiles(path))
            {
                var name = Path.GetFileNameWithoutExtension(file);
                mapping.Add(name, () => new DragonflyController(file));
            }
        }
        public IEnumerable<string> Available => mapping.Keys.ToList();

        public IEnumerator<KeyValuePair<string, Func<ICommandController>>> GetEnumerator()
        {
            return mapping.GetEnumerator();
        }

        public ICommandController Instantiate(string identifier)
        {
            return mapping[identifier]();
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
