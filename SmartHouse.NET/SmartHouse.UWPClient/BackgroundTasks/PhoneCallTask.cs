using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Calls.Background;

namespace SmartHouse.UWPClient.BackgroundTasks
{
    public class PhoneCallTask
    {
        private const string BackgroundTaskName = "SmartHousePhonecallBackgroundTask";
        private const string BackgroundTaskEntryPoint = "BackgroundPhoneTask.PhoneBackgroundTask";

        public IBackgroundTaskRegistration RegisterBackgroundTask()
        {
            if (BackgroundTaskRegistration.AllTasks.Any(x => x.Value.Name == BackgroundTaskName))
                return null;

            var phoneTaskBuilder = new BackgroundTaskBuilder()
            {
                Name = BackgroundTaskName,
                TaskEntryPoint = BackgroundTaskEntryPoint
            };

            var trigger = new PhoneTrigger(PhoneTriggerType.CallHistoryChanged, false);
            phoneTaskBuilder.SetTrigger(trigger);

            var backgroundTask = phoneTaskBuilder.Register();
            return backgroundTask;
        }

        public void UnRegisterBackgroundTask()
        {
            var task = BackgroundTaskRegistration.AllTasks.Values.FirstOrDefault(x => x.Name == BackgroundTaskName);
            task?.Unregister(true);
        }
    }
}
