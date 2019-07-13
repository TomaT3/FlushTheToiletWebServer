using System;
using System.Linq;
using UnitsNet;

namespace FlushTheToiletWebServer.CF01
{
    public interface IManDetectionDistanceSensor
    {
        Length GetDistance();
    }
}
