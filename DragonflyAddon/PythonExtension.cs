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
                var content=File.ReadAllText(file);
                var data = RegularExpressionHelper.RegularExpressionDictionary(content, @"([\w_]+)\s=\s\{([^\{\}]*)\}", 1, 2);
                foreach (var item in data)
                {
                    mapping.Add("Python."+item.Key,()=>new PythonController(item.Key, item.Value));

                }
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

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return mapping.GetEnumerator();
        //}
    }

    public class PythonController : ICommandController
    {
        static string q = "\"";
        string Pattern = $@"{q}([\w\s\]\[\|\(\)<>]+){q}:";
        Dictionary<string, string> commands;
        public PythonController(string name,string data)
        {

            commands=RegularExpressionHelper.RegularExpressionDictionary(data, Pattern, 1, 1);
        }

        public void Build(ICommandBuilder builder)
        {
            foreach (var item in commands)
            {
                builder.AddCommand(item.Key, () => Console.WriteLine("Not implemented yet."));
            }
        }
    }
}
