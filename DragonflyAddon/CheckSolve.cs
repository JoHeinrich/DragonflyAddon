using System;
using System.Collections.Generic;
using System.Text;
using VoiceControl;

namespace DragonflyAddon
{
    public class CheckSolve
    {
        public static string Execute<T>() where T : ICheckSolve, new()
        {
            T installation = new T();
            if (!installation.Check())
            {
                var v= installation.Solve();
                if (!string.IsNullOrEmpty(v)) return v;
            }
            if(!installation.Check())
            {
                return installation.Error;
            }
            return "";
        }
    }
}
