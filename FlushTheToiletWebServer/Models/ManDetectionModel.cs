using FlushTheToiletWebServer.CF01;
using FlushTheToiletWebServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;

namespace FlushTheToiletWebServer.Models
{
    public class ManDetectionModel : IManDetectionModel
    {
        private readonly IManDetectionService mManDetectionService;
        private readonly IManDetectionDistanceSensor mManDetectionDistanceSensor;

        public ManDetectionModel(
            IManDetectionService manDetectionService,
            IManDetectionDistanceSensor manDetectionDistanceSensor)
        {
            mManDetectionService = manDetectionService;
            mManDetectionDistanceSensor = manDetectionDistanceSensor;
        }

        public double GetDistance()
        {
            return mManDetectionDistanceSensor.GetDistance().Meters;
        }

        public void StartManDetection()
        {
            mManDetectionService.StartManDetection();
        }

        public void StopManDetection()
        {
            mManDetectionService.StopManDetection();
        }

        public double Distance
        {
            get => mManDetectionService.Distance.Meters;
        }
    }
}
