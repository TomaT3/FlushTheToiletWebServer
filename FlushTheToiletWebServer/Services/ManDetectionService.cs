using FlushTheToiletWebServer.CF01;
using System;
using System.Threading;
using UnitsNet;
using UnitsNet.Units;

namespace FlushTheToiletWebServer.Services
{
    public class ManDetectionService : IManDetectionService
    {
        private const int MEASURE_INTERVAL_TIME_WHEN_NO_MAN = 2000;
        private const int MEASURE_INTERVAL_TIME_WHEN_MAN_DETECTED = 50;
        private const int PEEING_DETECTION_TIME = 5000;
        private const int NO_ONE_THERE_TIME = 2000;

        private bool boolforTest = false;

        private bool mStopManDetection;
        private Timer mTimer;
        private IManDetectionDistanceSensor mDistanceSensor;
        private Length mMinDistance = new Length(0.02, LengthUnit.Meter);
        private Length mMaxDistance = new Length(0.7, LengthUnit.Meter);

        public event Action<bool> SomeoneDetectedChanged;
        public event Action<bool> SomeoneIsPeeingChanged;

        private int mSomeoneIsPeeingCounter;
        private int mNoOneThereCounter;

        private bool mSomeoneDetected;
        private bool mSomeoneIsPeeing;

        public Length Distance { get; private set; }
        public bool SomeoneDetected
        {
            get => mSomeoneDetected;
            set
            {
                if (mSomeoneDetected)
                {
                    if (value)
                    {
                        mSomeoneIsPeeingCounter += MEASURE_INTERVAL_TIME_WHEN_MAN_DETECTED;
                        if (mSomeoneIsPeeingCounter > PEEING_DETECTION_TIME)
                        {
                            SomeoneIsPeeing = true;
                            mNoOneThereCounter = 0;
                        }
                    }
                    else
                    {
                        mNoOneThereCounter += MEASURE_INTERVAL_TIME_WHEN_MAN_DETECTED;
                        if (mNoOneThereCounter > NO_ONE_THERE_TIME)
                        {
                            ResetValues();
                            SomeoneDetectedChanged?.Invoke(mSomeoneDetected);
                        }
                    }
                }
                else
                {
                    if (value)
                    {
                        mSomeoneDetected = value;
                        SomeoneDetectedChanged?.Invoke(mSomeoneDetected);
                    }
                }
            }
        }

        public bool SomeoneIsPeeing
        {
            get => mSomeoneIsPeeing;
            set
            {
                if (mSomeoneIsPeeing != value)
                {
                    mSomeoneIsPeeing = value;
                    SomeoneIsPeeingChanged?.Invoke(mSomeoneIsPeeing);
                }
            }
        }

        public ManDetectionService(IManDetectionDistanceSensor manDetectionDistanceSensor)
        {
            mDistanceSensor = manDetectionDistanceSensor;
            mTimer = new Timer(TimerTickHandler, null, TimeSpan.FromMilliseconds(Timeout.Infinite), Timeout.InfiniteTimeSpan);
        }

        public void StartManDetection()
        {
            mTimer.Change(TimeSpan.FromMilliseconds(MEASURE_INTERVAL_TIME_WHEN_NO_MAN), Timeout.InfiniteTimeSpan);
            Distance = Length.FromMeters(0);
        }

        public void StopManDetection()
        {
            mStopManDetection = true;
        }

        private void ResetValues()
        {
            mSomeoneDetected = false;
            SomeoneIsPeeing = false;
            mSomeoneIsPeeingCounter = 0;
            mNoOneThereCounter = 0;
        }

        private void TimerTickHandler(object state)
        {
            if (mStopManDetection)
            {
                ResetValues();
                mStopManDetection = false;
                mTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
                Distance = Length.FromMeters(-1);
                return;
            }

            Distance = mDistanceSensor.GetDistance();
            

            if ((Distance > mMinDistance && Distance < mMaxDistance) 
                || boolforTest)
            {
                SomeoneDetected = true;
                mTimer.Change(TimeSpan.FromMilliseconds(MEASURE_INTERVAL_TIME_WHEN_MAN_DETECTED), Timeout.InfiniteTimeSpan);
            }
            else
            {
                SomeoneDetected = false;
                if (SomeoneDetected)
                {
                    mTimer.Change(TimeSpan.FromMilliseconds(MEASURE_INTERVAL_TIME_WHEN_MAN_DETECTED), Timeout.InfiniteTimeSpan);
                }
                else
                {
                    mTimer.Change(TimeSpan.FromMilliseconds(MEASURE_INTERVAL_TIME_WHEN_NO_MAN), Timeout.InfiniteTimeSpan);
                }
            }
        }
    }
}