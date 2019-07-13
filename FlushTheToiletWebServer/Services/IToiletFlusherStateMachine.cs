
namespace FlushTheToiletWebServer.Services
{
    public interface IToiletFlusherStateMachine
    {
        ToiletStateMachineStatus GetStatus();
        void StartToiletStateMachine();
        void StopToiletStateMachine();
    }
}
