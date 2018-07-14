using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace SmartHouse.Lib
{
	public class AlarmClock
	{
        private DateTime TargetTime;
        private Action MyAction;
        private const int MinSleepMilliseconds = 250;

        public AlarmClock(DateTime targetTime, Action myAction)
        {
            TargetTime = targetTime;
            MyAction = myAction;
        }

        public Task Start()
        {
            return Task.Run(() => ProcessTimer());
        }

        private void ProcessTimer()
        {
            var now = DateTime.Now;

            Logger.LogInfoMessage($"Timer started to execute - {now}");
            Logger.LogInfoMessage($"Timer will be executed on: {TargetTime}");

            while (now < TargetTime)
            {
                var SleepMilliseconds = (int)Math.Round((TargetTime - now).TotalMilliseconds / 2);
                Thread.Sleep(SleepMilliseconds > MinSleepMilliseconds ? SleepMilliseconds : MinSleepMilliseconds);
                now = DateTime.Now;
            }

            Logger.LogInfoMessage("Starting to execute action");
            MyAction();

            TargetTime = TargetTime.AddDays(1);
            ProcessTimer();
        }
    }
}
