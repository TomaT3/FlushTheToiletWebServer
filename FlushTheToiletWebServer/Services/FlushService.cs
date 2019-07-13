using FlushTheToiletWebServer.CF01;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlushTheToiletWebServer.Services
{
    public class FlushService : IFlushService
    {
        private static int MOTOR_DRIVE_TIME = 1000; // milliseconds
        private static int MOTOR_DRIVE_TO_FLUSH_TIME = MOTOR_DRIVE_TIME + 500; // milliseconds
        private static double FLUSH_TIME = 4.0; // seconds

        private IFlusherMotor mFlusherMotor;

        public bool IsFlushing { get; private set; }

        public FlushService(IFlusherMotor flusherMotor)
        {
            mFlusherMotor = flusherMotor;
        }

        public void Stop()
        {
            mFlusherMotor.Stop();
        }

        public void Flush(double flushTime)
        {
            FlushOn();
            Thread.Sleep((int)TimeSpan.FromSeconds(flushTime).TotalMilliseconds);
            FlushOff();
        }

        public void Flush()
        {
            Flush(FLUSH_TIME);
        }

        private void FlushOn()
        {
            IsFlushing = true;
            mFlusherMotor.Forward();
            Thread.Sleep(MOTOR_DRIVE_TO_FLUSH_TIME);
            mFlusherMotor.Stop();
        }

        private void FlushOff()
        {
            mFlusherMotor.Backward();
            Thread.Sleep(MOTOR_DRIVE_TIME);
            mFlusherMotor.Stop();
            IsFlushing = false;
        }
    }
}
