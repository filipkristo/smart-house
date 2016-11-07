﻿using System;
using System.Threading;
using System.Timers;

namespace SmartHouse.Lib
{
	public class AlarmClock
	{
		private System.Timers.Timer timer;
		private DateTime alarmTime;
		private bool enabled;

		public event EventHandler Alarm;

		public AlarmClock(DateTime alarmTime)
		{
			this.alarmTime = alarmTime;

			timer = new System.Timers.Timer();
			timer.Elapsed += timer_Elapsed;
			timer.Interval = 1000;
			timer.Start();

			enabled = true;
		}

		void timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (enabled && DateTime.Now > alarmTime)
			{
				enabled = false;
				OnAlarm();
				timer.Stop();
			}
		}

		protected virtual void OnAlarm()
		{
			Alarm?.Invoke(this, EventArgs.Empty);
		}

	}
}
