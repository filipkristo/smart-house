using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace SmartHouse.UWPClient.BackgroundTasks
{
    public class TileBackgroundTaskRegister
    {
        private const string taskName = "SmartHouseTileUpdateTask";
        private const string taskEntryPoint = "BackgroundTask.TileBackgroundTask";

        public async void RegisterBackgroundTask()
        {
            if (BackgroundTaskRegistration.AllTasks.Any(x => x.Value.Name == taskName))
                return;

            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            if (backgroundAccessStatus == BackgroundAccessStatus.AlwaysAllowed ||
                backgroundAccessStatus == BackgroundAccessStatus.AllowedSubjectToSystemPolicy)
            {
                var taskBuilder = new BackgroundTaskBuilder
                {
                    Name = taskName,
                    TaskEntryPoint = taskEntryPoint
                };

                taskBuilder.SetTrigger(new TimeTrigger(120, false));
                taskBuilder.IsNetworkRequested = true;

                var internetCondition = new SystemCondition(SystemConditionType.InternetAvailable);
                taskBuilder.AddCondition(internetCondition);

                var registration = taskBuilder.Register();
            }
        }
    }
}
