using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlushTheToiletWebServer.ApiModels
{
    public class FlusherStateMachienStatusDTO
    {
        [JsonProperty]
        public bool IsInAutomaticMode { get; set; }

        [JsonProperty]
        public double Distance { get; set; }

        [JsonProperty]
        public string CurrentState { get; set; }
    }
}
