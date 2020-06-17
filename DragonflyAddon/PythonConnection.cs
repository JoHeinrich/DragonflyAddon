using System.Diagnostics;
using System.IO;

namespace DragonflyTest
{
    public class PythonConnection
    {

        public void ExecuteDragonflyKey(string key)
        {
            ExecuteCommand($"from dragonfly import Key\nKey(\"{key}\")._execute()");
        }
        public string ExecuteCommand(string command)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "C:/Python27/python.exe";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardInput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamWriter writer = process.StandardInput)
                {
                    writer.Write(command);

                }
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    return result;
                }
            }
        }

        //public void TestIronpython(string command)
        //{
        //    ScriptEngine engine = Python.CreateEngine();
        //    var paths = engine.GetSearchPaths();
        //    paths.Add(@"C:\Python27\Lib\site-packages\");
        //    paths.Add(@"C:\Python27\Lib\");
        //    engine.SetSearchPaths(paths);
        //    engine.ImportModule("dragonfly");
        //    engine.ImportModule("Key");
        //    engine.Execute("from dragonfly import Key");
        //    engine.Execute("ipy -X:Frames -m pip install html5lib");
        //    //engine.ExecuteFile(@"test.py");
        //    engine.Execute(command);
        //}
    }
}
