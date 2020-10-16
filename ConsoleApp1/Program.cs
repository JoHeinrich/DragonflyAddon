using Python.Deployment;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Installer.SetupPython().Wait();
            PythonEngine.Initialize();
            dynamic sys = PythonEngine.ImportModule("sys");
            Console.WriteLine("Python version: " + sys.version);

        }
    }
}
