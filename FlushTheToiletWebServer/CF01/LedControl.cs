using FlushTheToiletWebServer.CF01.Devices;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlushTheToiletWebServer.CF01
{
    public class LedControl : ILedControl
    {
        readonly Led mRedLed;
        readonly Led mYellowLed;
        readonly Led mGreenLed;

        public LedControl(IGpioController gpio)
        {
            mRedLed = new Led(gpio, PinAssignment.RED_LED);
            mYellowLed = new Led(gpio, PinAssignment.YELLOW_LED);
            mGreenLed = new Led(gpio, PinAssignment.GREEN_LED);
            TurnAllLedsOn();
            Thread.Sleep(500);
            TurnAllLedsOff();
        }

        public void TurnAllLedsOff()
        {
            RedLedOff();
            YellowLedOff();
            GreenLedOff();
        }

        public void TurnAllLedsOn()
        {
            RedLedOn();
            YellowLedOn();
            GreenLedOn();
        }

        public void RedLedOn()
        {
            mRedLed.On();
        }

        public void RedLedOff()
        {
            mRedLed.Off();
        }

        public void YellowLedOn()
        {
            mYellowLed.On();
        }

        public void YellowLedOff()
        {
            mYellowLed.Off();
        }

        public void GreenLedOn()
        {
            mGreenLed.On();
        }

        public void GreenLedOff()
        {
            mGreenLed.Off();
        }

    }
}
