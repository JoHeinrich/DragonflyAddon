using Python.Runtime;
using System;

namespace DragonflyAddon
{
    public class CheckPythonInPath : CheckSolve
    {
        public CheckPythonInPath() : base("Python not in path", "Adding Python to Path")
        {

        }

        public override bool InternalCheck()
        {
            try
            {
                var test = PythonEngine.PythonPath;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override void InternalSolve()
        {
            var installations = PathFinder.GetPythonInstallations();
            var currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
            foreach (var installation in installations)
            {
                string path = installation + ";" + installation + "\\Scripts;" + currentPath;
                Environment.SetEnvironmentVariable("PATH", path, EnvironmentVariableTarget.Process);
                if (Check())
                {
                    return;
                }
            }
        }

    }
}

