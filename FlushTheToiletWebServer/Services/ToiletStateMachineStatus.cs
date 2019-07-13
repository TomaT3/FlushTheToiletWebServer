using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlushTheToiletWebServer.Services
{
    public class ToiletStateMachineStatus
    {
        public bool IsInAutomaticMode { get; set; }
        public UnitsNet.Length Distance { get; set; }
        public string CurrentState { get; set; }
    }
}
