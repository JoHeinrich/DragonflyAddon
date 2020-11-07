using System;
using System.Collections.Generic;
using System.Text;
using VoiceControl;

namespace DragonflyAddon
{
    public enum SupportedPythonVerions { v2_7, v3_8 }
    public class DragonflyOrder : ICheckSolveOrder
    {
        SupportedPythonVerions Version;

        public DragonflyOrder(IPaths paths)
        {
            Version = SupportedPythonVerions.v3_8;
            this.paths = paths;
        }

        public IEnumerable<ICheckSolve> Order
        {
            get
            {
                switch (Version)
                {
                    case SupportedPythonVerions.v2_7:
                        return Order2_7;
                    case SupportedPythonVerions.v3_8:
                        return Order3_8;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
        public IEnumerable<ICheckSolve> Order2_7 => new List<ICheckSolve>
        {
            new CheckPythonInstallation2_7x32(),
            new CheckPythonInPath2_7(),
            new CheckPipPackage2_7(),
            new CheckNatlinkInstallationFor2_7(),
            new CheckNatlinkConfiguration2_7(paths),
        };
        private readonly IPaths paths;

        public IEnumerable<ICheckSolve> Order3_8 => new List<ICheckSolve>
        {
            new CheckPythonInstallation3_8x32(),
            new CheckPythonInPath3_8(),
            new CheckPipPackage3_8(),
            new CheckNatlinkInstallationFor3_8(),
            new CheckNatlinkConfiguration3_8(paths),
        };
    }

}
