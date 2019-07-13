using System;
using UnitsNet;

namespace FlushTheToiletWebServer.Services
{
    public interface IManDetectionService
    {
        event Action<bool> SomeoneDetectedChanged;
        event Action<bool> SomeoneIsPeeingChanged;

        void StartManDetection();
        void StopManDetection();
        Length Distance { get; }
        bool SomeoneDetected { get; set; }
        bool SomeoneIsPeeing { get; set; }
        
    }
}