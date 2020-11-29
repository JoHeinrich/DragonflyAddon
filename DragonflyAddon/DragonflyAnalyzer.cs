using Python.Runtime;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DragonflyAddon
{
    public class DragonflyAnalyzer
    {
        public string Error;
        Dictionary<string, PyObject> commands = new Dictionary<string, PyObject>();
        public DragonflyAnalyzer(string path)
        {
            try
            {
                Analyze(path);
            }
            catch (System.Exception e)
            {
                Error = e.Message;
            }

        }

        public IEnumerable<string> Commands => commands.Keys.Select(x => x.ToString());

        public void Execute(string command)
        {
            var target = commands[command];
            target.InvokeMethod("execute");
        }

        void Analyze(string path)
        {
            var data = File.ReadAllText(path);
            var name = Path.GetFileNameWithoutExtension(path);
            using (PyScope scope = Py.CreateScope())
            {
                scope.Import("sys");
                scope.Import("os");
                var dir = @"C:/Users/laise/Documents/EasyVoiceCodeTest2/Dragonfly/Rules";
                var com = $"sys.path.append({dir})";
                scope.Exec("p = os.getcwd()");
                var p = scope.Get("p");
                //scope.Exec(com);
                scope.Exec(data);
                var v = scope.Get(name);
                var mapping = v.GetAttr("mapping");
                var items = mapping.GetAttr("keys");

                foreach (var item in mapping)
                {
                    var command = item.ToPython();
                    var target = mapping.GetItem(command);
                    commands[command.ToString()] = target;
                }
            }
        }
    }
}

