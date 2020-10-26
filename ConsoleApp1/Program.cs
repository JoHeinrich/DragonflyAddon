using DragonflyAddon;
using Python.Deployment;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceControl;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //ProgramInstaller.RunElevatedProcess("cmd");
            //PathFinder.GetPythonPathfromPath().Version;
            CheckSolve.Execute<CheckNatlinkConfiguration3_8>();
            //CheckSolve.Execute<CheckNatlinkConfiguration3_8>();
            Console.Read();

        }
    }
}
