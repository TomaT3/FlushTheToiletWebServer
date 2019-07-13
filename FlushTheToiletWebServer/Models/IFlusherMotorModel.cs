namespace FlushTheToiletWebServer.Models
{
    public interface IFlusherMotorModel
    {
        void Flush();
        void MotorBackward();
        void MotorForward();
        void MotorStop();
    }
}
