using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading.Tasks;
using UnitsNet;

namespace FlushTheToiletWebServer.CF01.Devices
{
    public class HCSR04
    {
        private readonly IGpioController _gpio;

        private int _trig_Pin;
        private int _echo_Pin;


        static Object deviceLock = new object();

        Stopwatch sw = new Stopwatch();

        public int TimeoutMilliseconds { get; set; } = 20;

        /// <summary>
        /// Create an HCSR04 Sensor
        /// </summary>
        /// <param name="trig_Pin"></param>
        /// <param name="echo_Pin"></param>
        /// <param name="timeoutMilliseconds">defaults to 20</param>
        public HCSR04(IGpioController gpio, byte trig_Pin, byte echo_Pin, int timeoutMilliseconds=20)
        {
            _gpio = gpio;
            Initialise(trig_Pin, echo_Pin, timeoutMilliseconds);
        }

        /// <summary>
        /// Initialise ultra sonic distance sensor
        /// </summary>
        /// <param name="trig_Pin"></param>
        /// <param name="echo_Pin"></param>
        /// <param name="maxDistance">Set Ultra Sonic maximum distance to detect.  This is approximate only.  Based on 34.3 cm per millisecond, 20 degrees C at sea level</param>
        public HCSR04(IGpioController gpio, byte trig_Pin, byte echo_Pin, Length maxDistance)
        {
            _gpio = gpio;
            int milliSeconds = (int)(maxDistance.Centimeters / 34.3 * 2);
            Initialise(trig_Pin, echo_Pin, milliSeconds);
        }

        private void Initialise(byte trig_Pin, byte echo_Pin, int timeoutMilliseconds)
        {
            _trig_Pin = trig_Pin;
            _echo_Pin = echo_Pin;

            TimeoutMilliseconds = timeoutMilliseconds;

            _gpio.OpenPin(trig_Pin, PinMode.Output);
            _gpio.OpenPin(echo_Pin, PinMode.Input);
            
            _gpio.Write(trig_Pin, PinValue.Low);
        }

        /// <summary>
        /// Set Ultra Sonic maximum distance to detect.  This is approximate only.  Based on 34.3 cm per millisecond, 20 degrees C at sea level
        /// </summary>
        /// <param name="maxDistance"></param>
        /// <returns></returns>
        public Length GetDistance(Length maxDistance)
        {
            int milliSeconds = (int)(maxDistance.Centimeters / 34.3 * 2);
            return GetDistance(milliSeconds);
        }

        public Length GetDistance()
        {
            return GetDistance(TimeoutMilliseconds);
        }

        /// <summary>
        /// Measures distance in centimeters
        /// </summary>
        /// <param name="timeoutMilliseconds">20 milliseconds is enough time to measure up to 3 meters</param>
        /// <returns></returns>
        public Length GetDistance(int timeoutMilliseconds=20)
        {
            lock (deviceLock)
            {

                //http://www.c-sharpcorner.com/UploadFile/167ad2/how-to-use-ultrasonic-sensor-hc-sr04-in-arduino/
                //http://www.modmypi.com/blog/hc-sr04-ultrasonic-range-sensor-on-the-raspberry-pi


                _gpio.Write(_trig_Pin, PinValue.Low);                   // ensure the trigger is off
                Task.Delay(TimeSpan.FromMilliseconds(1)).Wait();  // wait for the sensor to settle

                _gpio.Write(_trig_Pin, PinValue.High);                       // turn on the pulse
                Task.Delay(TimeSpan.FromMilliseconds(.01)).Wait();      // let the pulse run for 10 microseconds
                _gpio.Write(_trig_Pin, PinValue.Low);                        // turn off the pulse

                var time = PulseIn(_echo_Pin, PinValue.High, timeoutMilliseconds);

                // https://en.wikipedia.org/wiki/Speed_of_sound
                // speed of sound is 34300 cm per second or 34.3 cm per millisecond
                // since the sound waves traveled to the obstacle and back to the sensor
                // I am dividing by 2 to represent travel time to the obstacle                

                return Length.FromCentimeters(time * 34.3 / 2.0); // at 20 degrees at sea level
            }
        }

        private double PulseIn(int pin_number, PinValue value, int timeout)
        {
            sw.Restart();

            // Wait for pulse
            while (sw.ElapsedMilliseconds < timeout && _gpio.Read(pin_number) != value) { }

            if (sw.ElapsedMilliseconds >= timeout)
            {
                sw.Stop();
                return 0;
            }
            sw.Restart();

            // Wait for pulse end
            while (sw.ElapsedMilliseconds < timeout && _gpio.Read(pin_number) == value) { }

            sw.Stop();

            return sw.ElapsedMilliseconds < timeout ? sw.Elapsed.TotalMilliseconds : 0;
        }
    }
}
