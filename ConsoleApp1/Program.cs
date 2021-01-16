using DragonflyAddon;
using Python.Deployment;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceControl;
using System.Security.Principal;
using System.Diagnostics;
namespace ConsoleApp1
{
    class Program
    {

        class Path : IPaths
        {
            public string GetPath(string directory)
            {
                return @"C:\Users\laise\Documents\EasyVoiceCodeTest3\"+directory;
            }
        }

        static void Main(string[] args)
        {
            PythonEngine.Initialize();
            HasCommands();
            Console.Read();
        }

        static string path = @"C:\Users\laise\Documents\EasyVoiceCodeTest2\Addons\DragonflyAddon\code\DragonflyAddon\UnitTestProject1\Example1.py";



        public static void HasCommands()
        {
            DragonflyAnalyzer analyzer = new DragonflyAnalyzer(path);
            var commands = analyzer.Commands;
            Console.WriteLine(commands.Count());
        }

        public static void ExecuteCommand()
        {
            DragonflyAnalyzer analyzer = new DragonflyAnalyzer(path);
            analyzer.Execute("test");
        }

        //private void CheckSolveAll()
        //{
        //    Console.WriteLine(Admin.IsAdmin);
        //    if (!Admin.IsAdmin)
        //    {
        //        Admin.RestartAsAdmin();
        //    }
        //    Environment.GetCommandLineArgs();
        //    DragonflyOrder order = new DragonflyOrder(new Path());
        //    foreach (var check in order.Order)
        //    {
        //        Console.WriteLine(check.GetType().Name);
        //        if (!check.Check())
        //        {
        //            Console.WriteLine(check.Solve());
        //        }
        //        if (!check.Check())
        //        {
        //            Console.WriteLine(check.Error);
        //            Console.WriteLine(check.AvailableAction);
        //            Console.WriteLine("Failed");
        //            Console.Read();
        //            return;
        //        }
        //    }
        //    Console.WriteLine("Completed");
        //    Console.Read();
        //}
    }
}
