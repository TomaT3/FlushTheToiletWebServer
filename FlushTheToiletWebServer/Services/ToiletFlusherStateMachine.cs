using System;
using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Extensions;
using Appccelerate.StateMachine.Machine;
using FlushTheToiletWebServer.CF01;
using ioBroker.net;

namespace FlushTheToiletWebServer.Services
{
    public class ToiletFlusherStateMachine : IToiletFlusherStateMachine
    {
        private const string IOBROKER_MANPEEING_ID = "javascript.0.toilet.someonepeeing";

        private readonly IIoBrokerDotNet _ioBroker;
        private PassiveStateMachine<ToiletStates, ToiletEvents> mToiletStateMachine;
        private CurrentStateExtension mCurrentStateExtension;
        private IFlushService mFlusherMotor;
        private IManDetectionService mManDetection;
        private ILedControl mLedControl;
        private ToiletStateMachineStatus mStatus;
        private bool mStateMachineIsRunning;

        public ToiletFlusherStateMachine(
            IFlushService flusherMotor,
            IManDetectionService manDetection,
            ILedControl ledControl,
            IIoBrokerDotNet ioBroker)
        {
            _ioBroker = ioBroker;
            mFlusherMotor = flusherMotor;
            mManDetection = manDetection;
            mLedControl = ledControl;
            mStatus = new ToiletStateMachineStatus();
            mManDetection.SomeoneDetectedChanged += SomeoneDetectedChangedHandler;
            mManDetection.SomeoneIsPeeingChanged += SomeoneIsPeeingChangedHandler;
            ConfigureStateMachine();
        }

        public ToiletStateMachineStatus GetStatus()
        {
            mStatus.IsInAutomaticMode = mStateMachineIsRunning;
            mStatus.CurrentState = mCurrentStateExtension.CurrentState.ToString();
            mStatus.Distance = mManDetection.Distance;
            return mStatus;
        }

        public void StartToiletStateMachine()
        {
            if (!mStateMachineIsRunning)
            {
                ResetTempFlushCount();
                mStateMachineIsRunning = true;
                mToiletStateMachine.Fire(ToiletEvents.StartStateMachine);
                mManDetection.StartManDetection();
            }
        }

        public void StopToiletStateMachine()
        {
            mManDetection.StopManDetection();
            mToiletStateMachine.Fire(ToiletEvents.StopStateMachine);
        }

        private void SomeoneIsPeeingChangedHandler(bool manIsPeeing)
        {
            if (manIsPeeing)
                mToiletStateMachine.Fire(ToiletEvents.ManPees);
            else
                mToiletStateMachine.Fire(ToiletEvents.ManLeft);
        }

        private void SomeoneDetectedChangedHandler(bool manInfrontOfToilet)
        {
            if (manInfrontOfToilet)
                mToiletStateMachine.Fire(ToiletEvents.ManArrived);
            else
                mToiletStateMachine.Fire(ToiletEvents.ManLeft);
        }

        private void ConfigureStateMachine()
        {
            mToiletStateMachine = new PassiveStateMachine<ToiletStates, ToiletEvents>();
            mCurrentStateExtension = new CurrentStateExtension();
            mToiletStateMachine.AddExtension(mCurrentStateExtension);

            mToiletStateMachine.In(ToiletStates.WaitingForStart)
                .ExecuteOnEntry(StopStateMachine)
                .On(ToiletEvents.StartStateMachine).Goto(ToiletStates.WaitingForMan);

            mToiletStateMachine.In(ToiletStates.WaitingForMan)
                .ExecuteOnEntry(WaitForMan)
                .On(ToiletEvents.ManArrived).Goto(ToiletStates.ManInfrontOfToilet)
                .On(ToiletEvents.StopStateMachine).Goto(ToiletStates.WaitingForStart);

            mToiletStateMachine.In(ToiletStates.ManInfrontOfToilet)
                .ExecuteOnEntry(ManArrived)
                .On(ToiletEvents.ManPees).Goto(ToiletStates.ManIsPeeing)
                .On(ToiletEvents.ManLeft).Goto(ToiletStates.WaitingForMan)
                .On(ToiletEvents.StopStateMachine).Goto(ToiletStates.WaitingForStart);

            mToiletStateMachine.In(ToiletStates.ManIsPeeing).
                ExecuteOnEntry(ManIsPeeing)
                .On(ToiletEvents.ManLeft).Goto(ToiletStates.PeeingFinished)
                .On(ToiletEvents.StopStateMachine).Goto(ToiletStates.WaitingForStart);

            mToiletStateMachine.In(ToiletStates.PeeingFinished)
                .ExecuteOnEntry(Flush)
                .On(ToiletEvents.FlushFinished).Goto(ToiletStates.WaitingForMan)
                .On(ToiletEvents.StopStateMachine).Goto(ToiletStates.WaitingForStart);

            mToiletStateMachine.Initialize(ToiletStates.WaitingForStart);
            mToiletStateMachine.Start();
        }

        private void StopStateMachine()
        {
            mLedControl.TurnAllLedsOff();
            mFlusherMotor.Stop();
            mStateMachineIsRunning = false;
        }

        private void WaitForMan()
        {
            mLedControl.TurnAllLedsOff();
            mLedControl.BlueLedOn();
            _ioBroker.TrySetStateAsync<bool>(IOBROKER_MANPEEING_ID, false);
        }

        private void ManArrived()
        {
            mLedControl.BlueLedOff();
            mLedControl.GreenLedOn();
            _ioBroker.TrySetStateAsync<bool>(IOBROKER_MANPEEING_ID, true);
        }

        private void ManIsPeeing()
        {
            mLedControl.BlueLedOff();
            mLedControl.GreenLedOff();
            mLedControl.YellowLedOn();
        }

        private void Flush()
        {
            mLedControl.BlueLedOff();
            mLedControl.GreenLedOff();
            mLedControl.YellowLedOff();
            mLedControl.RedLedOn();
            mFlusherMotor.Flush();
            SetFlushCountInIoBroker();
            mToiletStateMachine.Fire(ToiletEvents.FlushFinished);
        }

        private void ResetTempFlushCount()
        {
            var tempCountId = "javascript.0.toilet.flushes.count";
            _ioBroker.TrySetStateAsync<int>(tempCountId, 0);
        }

        private void SetFlushCountInIoBroker()
        {
            try
            {
                var tempCountId = "javascript.0.toilet.flushes.count";
                var tempCountResult = _ioBroker.TryGetStateAsync<int>(tempCountId, TimeSpan.FromSeconds(5)).Result;
                if (tempCountResult.Success)
                {
                    var newValue = tempCountResult.Value + 1;
                    _ioBroker.TrySetStateAsync<int>(tempCountId, newValue);
                }

                var totalCountId = "javascript.0.toilet.flushes.totalcount";
                var totalCountResult = _ioBroker.TryGetStateAsync<int>(totalCountId, TimeSpan.FromSeconds(5)).Result;
                if (totalCountResult.Success)
                {
                    var newValue = totalCountResult.Value + 1;
                    _ioBroker.TrySetStateAsync<int>(totalCountId, newValue);
                }
            }
            catch
            {
                // just continue
            }
        }
    }

    internal class CurrentStateExtension : ExtensionBase<ToiletStates, ToiletEvents>
    {
        public ToiletStates CurrentState { get; private set; }

        public override void SwitchedState(
            IStateMachineInformation<ToiletStates, ToiletEvents> stateMachine,
            IState<ToiletStates, ToiletEvents> oldState,
            IState<ToiletStates, ToiletEvents> newState)
        {
            this.CurrentState = newState.Id;
        }
    }

    enum ToiletStates
    {
        WaitingForStart,
        WaitingForMan,
        ManInfrontOfToilet,
        ManIsPeeing,
        PeeingFinished
    }

    enum ToiletEvents
    {
        StartStateMachine,
        StopStateMachine,
        ManArrived,
        ManPees,
        ManLeft,
        FlushFinished
    }
}
