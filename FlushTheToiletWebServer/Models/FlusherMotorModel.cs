using FlushTheToiletWebServer.CF01;
using FlushTheToiletWebServer.Services;

namespace FlushTheToiletWebServer.Models
{
    public class FlusherMotorModel : IFlusherMotorModel
    {
        private readonly IFlushService mFlushService;
        private readonly IFlusherMotor mFlusherMotor;
        public FlusherMotorModel(
            IFlushService flushService,
            IFlusherMotor flusherMotor)
        {
            mFlushService = flushService;
            mFlusherMotor = flusherMotor;
        }

        public void MotorForward()
        {
            mFlusherMotor.Forward();
        }

        public void MotorBackward()
        {
            mFlusherMotor.Backward();
        }

        public void MotorStop()
        {
            mFlusherMotor.Stop();
        }

        public void Flush()
        {
            mFlushService.Flush();
        }
    }
}
