using System;
using System.Linq;

namespace FlushTheToiletWebServer.CF01
{
    public interface ILedControl
    {
        void GreenLedOff();
        void GreenLedOn();
        void RedLedOff();
        void RedLedOn();
        void TurnAllLedsOff();
        void TurnAllLedsOn();
        void YellowLedOff();
        void YellowLedOn();
    }
}
