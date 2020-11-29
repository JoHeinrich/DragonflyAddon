using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceControl;

namespace DragonflyAddon
{
    class AppContext : IInformationSource
    {
        public string Name => "AppContext";
        public string Value => "executable=\"\", title=\"\"";
    }
}
