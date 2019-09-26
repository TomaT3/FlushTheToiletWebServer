using System.Device.Gpio;

namespace FlushTheToiletWebServer.CF01.Devices
{
    public class Motor {

        private GpioController gpio = new GpioController();
        private readonly int in1;
        private readonly int in2;


        public Motor(byte in1, byte in2) {
            this.in1 = in1;
            this.in2 = in2;            

            gpio.OpenPin(in1, PinMode.Output);
            gpio.OpenPin(in2, PinMode.Output);

            gpio.Write(in1, PinValue.Low);
            gpio.Write(in2, PinValue.Low);
        }

        public void Forward() {
            gpio.Write(in1, PinValue.Low);
            gpio.Write(in2, PinValue.High);
        }

        public void Backward() {
            gpio.Write(in1, PinValue.High);
            gpio.Write(in2, PinValue.Low);
        }

        public void Stop() {
            gpio.Write(in1, PinValue.Low);
            gpio.Write(in2, PinValue.Low);
        }
    }
}
