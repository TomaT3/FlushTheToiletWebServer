using System;
using System.Threading;
using System.Device.Gpio;

namespace FlushTheToiletWebServer.CF01.Devices
{
    public class Led:  IDisposable
    {
        private readonly IGpioController _gpio;

        LedState _ts = new LedState();

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
        public Led(IGpioController gpio, int pinNumber) 
        {
            _gpio = gpio;
            InitLed(pinNumber);
        }

        private void InitLed(int pinNumber) {
            _ts.pinNumber = pinNumber;
            _gpio.OpenPin(_ts.pinNumber, PinMode.Output);
            _gpio.Write(_ts.pinNumber, PinValue.Low);

            _ts.tmr = new Timer(BlinkTime_Tick, _ts, Timeout.Infinite, Timeout.Infinite);
        }


        public void On() {
            if (_ts.running) { return; }
            _gpio.Write(_ts.pinNumber, PinValue.High);
        }

        public void Off() {
            if (_ts.running) { return; }
            _gpio.Write(_ts.pinNumber, PinValue.Low);
        }

        public void BlinkOn(uint Milliseconds, BlinkRate blinkRate) {

            if (_ts.running) { return; }
            _ts.running = true;

            _ts.blinkMilliseconds = Milliseconds;
            _ts.BlinkMillisecondsToDate = 0;
            _ts.blinkRateMilliseconds = (int)blinkRate;
            _ts.tmr.Change(0, _ts.blinkRateMilliseconds);
        }

        void BlinkTime_Tick(object state) {
            var ts = (LedState)state;

            if (!ts.ledOn)
                _gpio.Write(ts.pinNumber, PinValue.High);
            else
                _gpio.Write(ts.pinNumber, PinValue.Low);

            ts.ledOn = !ts.ledOn;

            ts.BlinkMillisecondsToDate += ts.blinkRateMilliseconds;
            if (ts.BlinkMillisecondsToDate >= ts.blinkMilliseconds) {
                // turn off blink
                ts.tmr.Change(Timeout.Infinite, Timeout.Infinite);
                _gpio.Write(ts.pinNumber, PinValue.Low);
                ts.running = false;
            }
        }

        public void Dispose() {
            _gpio.Dispose();
        }
    }
}
