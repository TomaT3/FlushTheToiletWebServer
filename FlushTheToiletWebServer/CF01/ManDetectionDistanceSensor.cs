using FlushTheToiletWebServer.CF01.Devices;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;

namespace FlushTheToiletWebServer.CF01
{
    public class ManDetectionDistanceSensor : IManDetectionDistanceSensor
    {
        private const byte TRIGGER_PIN = PinAssignment.TRIGGER_PIN;
        private const byte ECHO_PIN = PinAssignment.ECHO_PIN;

        private HCSR04 mDistanceSensor;

        public ManDetectionDistanceSensor(IGpioController gpio)
        {
            mDistanceSensor = new HCSR04(gpio, TRIGGER_PIN, ECHO_PIN, Length.FromMeters(3.0));
        }

        public Length GetDistance()
        {
            var distance = mDistanceSensor.GetDistance();
            return distance;
        }
    }
}
