using System;
using System.Threading;
using System.Device.Gpio;

namespace FlushTheToiletWebServer.CF01.Devices
{
    public class Led:  IDisposable  {

        GpioController gpio = new GpioController();

        LedState ts = new LedState();

        class LedState {
            public uint blinkMilliseconds = 0;
            public int BlinkMillisecondsToDate;
            public bool ledOn = false;
            public int pinNumber;
            public Timer tmr;
            public int blinkRateMilliseconds;
            public bool running = false;
        }

        public enum BlinkRate {
            Slow = 1000,
            Medium = 500,
            Fast = 75,
            VeryFast = 25,
        }

        /// <summary>
        /// Simnple Led control
        /// </summary>
        /// <param name="pin">From the SecretLabs.NETMF.Hardware.NetduinoPlus.Pins namespace</param>
        /// <param name="name">Unique identifying name for command and control</param>
        public Led(int pinNumber) {
            InitLed(pinNumber);
        }

        private void InitLed(int pinNumber) {
            ts.pinNumber = pinNumber;
            gpio.OpenPin(ts.pinNumber, PinMode.Output);
            gpio.Write(ts.pinNumber, PinValue.Low);

            ts.tmr = new Timer(BlinkTime_Tick, ts, Timeout.Infinite, Timeout.Infinite);
        }


        public void On() {
            if (ts.running) { return; }
            gpio.Write(ts.pinNumber, PinValue.High);
        }

        public void Off() {
            if (ts.running) { return; }
            gpio.Write(ts.pinNumber, PinValue.Low);
        }

        public void BlinkOn(uint Milliseconds, BlinkRate blinkRate) {

            if (ts.running) { return; }
            ts.running = true;

            ts.blinkMilliseconds = Milliseconds;
            ts.BlinkMillisecondsToDate = 0;
            ts.blinkRateMilliseconds = (int)blinkRate;
            ts.tmr.Change(0, ts.blinkRateMilliseconds);
        }

        void BlinkTime_Tick(object state) {
            var ts = (LedState)state;

            if (!ts.ledOn)
                gpio.Write(ts.pinNumber, PinValue.High);
            else
                gpio.Write(ts.pinNumber, PinValue.Low);

            ts.ledOn = !ts.ledOn;

            ts.BlinkMillisecondsToDate += ts.blinkRateMilliseconds;
            if (ts.BlinkMillisecondsToDate >= ts.blinkMilliseconds) {
                // turn off blink
                ts.tmr.Change(Timeout.Infinite, Timeout.Infinite);
                gpio.Write(ts.pinNumber, PinValue.Low);
                ts.running = false;
            }
        }

        public void Dispose() {
            gpio.Dispose();
        }
    }
}
