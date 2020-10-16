
using Python.Runtime;
using Python.Included;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DragonflyAddon
{
    public class Person
    {
        public Person(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    public class Test
    {
        public void TestFunc()
        {
            //Installer.SetupPython().Wait();
            //var installed = Installer.TryInstallPip();
            //Installer.LogMessage += Installer_LogMessage;
            //Installer.PipInstallModule("numpy");
            //Installer.PipInstallModule("dragonfly-opt -v");
            //PythonEngine.Initialize();
            //dynamic sys = PythonEngine.ImportModule("dragonfly");

            var pat=PythonEngine.PythonPath;
            PythonEngine.Initialize();

            // create a person object
            Person person = new Person("John", "Smith");


            
            // acquire the GIL before using the Python interpreter
            using (Py.GIL())
            {
                var path = @"C:/Users/laise/Documents/VoiceCoding/Natlink/_example1.py";
                //Py.Import();
                // create a Python scope



                using (PyScope scope = Py.CreateScope())
                {
                    scope.Import("os");
                    scope.Exec("path = os.getcwd()");
                    var s = scope.Get<string>("path");
                    scope.Exec("bridge = open(\"bridge.py\").read()");
                    var b = scope.Get<string>("bridge");
                    scope.Exec(b);
                    //var tset = scope.Get("MyRule");
                    scope.Exec("my_function(\""+path+"\")");
                    // convert the Person object to a PyObject
                    PyObject pyPerson = person.ToPython();

                    // create a Python variable "person"
                    scope.Set("person", pyPerson);

                    // the person object may now be used in Python
                    string code = "fullName = person.FirstName + ' ' + person.LastName";
                    scope.Exec(code);
                }
            }
        }

        private void Installer_LogMessage(string obj)
        {
            Console.WriteLine(obj);
        }
    }


}
