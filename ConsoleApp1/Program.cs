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


        static void Main(string[] args)
        {
            Console.WriteLine(Admin.IsAdmin);
            if(!Admin.IsAdmin)
            {
                Admin.RestartAsAdmin();
            }
            Environment.GetCommandLineArgs();
            DragonflyOrder order = new DragonflyOrder();
            foreach (var check in order.Order)
            {
                if(!check.Check())
                {
                    Console.WriteLine(check.Solve());
                }
                if(!check.Check())
                {
                    Console.WriteLine(check.Error);
                    Console.WriteLine(check.AvailableAction);
                    break;
                }
            }
            Console.Read();

        }
    }
}
