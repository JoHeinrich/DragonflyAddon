using System.Diagnostics;
using System.IO;

namespace DragonflyAddon
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

    }
}
