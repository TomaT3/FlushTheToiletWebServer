using System;
using System.Linq;
using UnitsNet;

namespace FlushTheToiletWebServer.Models
{
    public interface IManDetectionModel
    {
        /// <summary>
        /// Distance in meters
        /// </summary>
        /// <returns></returns>
        double GetDistance();
        void StartManDetection();
        void StopManDetection();

        /// <summary>
        /// Distance in meters
        /// </summary>
        double Distance { get; }
    }
}
