using FlushTheToiletWebServer.CF01.Devices;
using System.Threading;

namespace FlushTheToiletWebServer.CF01
{
    public class FlusherMotor : IFlusherMotor
    {
        private static byte FLUSH_OUTPUT = PinAssignment.FLUSH_OUTPUT;
        private static byte STOP_FLUSH_OUTPUT = PinAssignment.STOP_FLUSH_OUTPUT;

        private Motor mFlushMotor;
        
        public FlusherMotor()
        {
            mFlushMotor = new Motor(FLUSH_OUTPUT, STOP_FLUSH_OUTPUT);
        }

        public void Forward()
        {
            mFlushMotor.Forward();
        }

        public void Backward()
        {
            mFlushMotor.Backward();
        }

        public void Stop()
        {
            mFlushMotor.Stop();
        }
    }
}
