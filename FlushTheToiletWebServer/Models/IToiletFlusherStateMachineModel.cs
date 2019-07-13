using FlushTheToiletWebServer.Services;
using System;
using System.Linq;

namespace FlushTheToiletWebServer.Models
{
    public interface IToiletFlusherStateMachineModel
    {
        ToiletStateMachineStatus GetStatus();
        void StartToiletStateMachine();
        void StopToiletStateMachine();
    }
}
