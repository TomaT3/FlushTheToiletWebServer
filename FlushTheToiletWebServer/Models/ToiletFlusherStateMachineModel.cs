using FlushTheToiletWebServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlushTheToiletWebServer.Models
{
    public class ToiletFlusherStateMachineModel : IToiletFlusherStateMachineModel
    {
        private readonly IToiletFlusherStateMachine mToiletFlusherStateMachine;

        public ToiletFlusherStateMachineModel(IToiletFlusherStateMachine toiletFlusherStateMachine)
        {
            mToiletFlusherStateMachine = toiletFlusherStateMachine;
        }

        public void StartToiletStateMachine()
        {
            mToiletFlusherStateMachine.StartToiletStateMachine();
        }

        public void StopToiletStateMachine()
        {
            mToiletFlusherStateMachine.StopToiletStateMachine();
        }

        public ToiletStateMachineStatus GetStatus()
        {
            return mToiletFlusherStateMachine.GetStatus();
        }
    }
}
