using System;
using System.Diagnostics;

namespace DragonflyAddon
{
    public class CheckDragonflyPackage : CheckSolve
    {
        public CheckDragonflyPackage(string error, string action) : base("Dragonfly package not installed", "Runs pip install dragonfly2")
        {

        }

        public override bool InternalCheck()
        {
            throw new NotImplementedException();
        }

        public override void InternalSolve()
        {
            var pythonDir = PathFinder.GetPythonPathfromPath();
            Process.Start(new ProcessStartInfo(pythonDir + "/Scripts/pip.exe", "install numpy"));
            var p = Process.Start(new ProcessStartInfo(pythonDir + "/Scripts/pip.exe", "install dragonfly2") { RedirectStandardOutput = true });
            var resut = p.StandardOutput.ReadToEnd();
        }

    }
}

