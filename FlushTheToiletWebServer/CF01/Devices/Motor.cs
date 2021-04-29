using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Gpio;

namespace FlushTheToiletWebServer.CF01.Devices
{
    public class Motor
    {

        private readonly IGpioController _gpio;
        private readonly int _in1;
        private readonly int _in2;


        public Motor(IGpioController gpio, byte in1, byte in2) 
        {
            _gpio = gpio;
            _in1 = in1;
            _in2 = in2;

            _gpio.OpenPin(in1, PinMode.Output);
            _gpio.OpenPin(in2, PinMode.Output);

            _gpio.Write(in1, PinValue.Low);
            _gpio.Write(in2, PinValue.Low);
        }

        public void Forward() {
            _gpio.Write(_in1, PinValue.Low);
            _gpio.Write(_in2, PinValue.High);
        }

        public void Backward() {
            _gpio.Write(_in1, PinValue.High);
            _gpio.Write(_in2, PinValue.Low);
        }

        public void Stop() {
            _gpio.Write(_in1, PinValue.Low);
            _gpio.Write(_in2, PinValue.Low);
        }
    }
}
