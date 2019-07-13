namespace FlushTheToiletWebServer.CF01
{
    public interface IFlusherMotor
    {
        void Forward();
        void Backward();
        void Stop();
    }
}
