using FlushTheToiletWebServer.CF01.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlushTheToiletWebServer.CF01
{
    public class LedControl : ILedControl
    {
        Led mRedLed = new Led(PinAssignment.RED_LED);
        Led mYellowLed = new Led(PinAssignment.YELLOW_LED);
        Led mGreenLed = new Led(PinAssignment.GREEN_LED);

        public LedControl()
        {
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
