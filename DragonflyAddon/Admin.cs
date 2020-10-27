using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Diagnostics;
namespace DragonflyAddon
{
    public class Admin
    {
        public static bool IsAdmin
        {
            get
            {
                var platform = Environment.OSVersion.Platform;
                var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);
                var role = principal.IsInRole(WindowsBuiltInRole.Administrator);
                return (Environment.OSVersion.Platform != PlatformID.Win32NT 
                    || (Environment.OSVersion.Platform == PlatformID.Win32NT
                        && (new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator)));
            }
        }

        public static void ElevateIfNecessary()
        {
            if (!Admin.IsAdmin)
            {
                Admin.RestartAsAdmin();
            }
        }

        public static void RestartAsAdmin()
        {
            // Restart and run as admin
            var exeName = Process.GetCurrentProcess().MainModule.FileName;
            ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
            startInfo.Verb = "runas";
            startInfo.Arguments = Environment.CommandLine;
            Process.Start(startInfo);
            Environment.Exit(0);
        }
    }
}
