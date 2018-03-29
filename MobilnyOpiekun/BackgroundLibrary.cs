using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace MobilnyOpiekun
{
    static class BackgroundLibrary
    {
        private static IBackgroundTaskRegistration registration;

        public static bool IsWorking
        {
            get
            {
                return BackgroundTaskRegistration.AllTasks.Any();
            }
        }

        public static bool Init()
        {
            if (IsWorking)
            {
                registration = BackgroundTaskRegistration.AllTasks.Values.First();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<bool> Toggle()
        {
            if (IsWorking)
            {
                while (registration != null)
                {
                    registration.Unregister(true);
                    registration = BackgroundTaskRegistration.AllTasks.Any() ? BackgroundTaskRegistration.AllTasks.Values.First() : null;
                }
                return false;
            }
            else
            {
                if (Classes.Konfiguracja.CzyKonfiguracjaPoprawna() && Classes.Konfiguracja.CzyPrzyznanoWszystkieDostepy())
                {
                    try
                    {
                        ApplicationData.Current.LocalSettings.Values["czyAgentWykonujePrace"] = false;
                        BackgroundExecutionManager.RemoveAccess(); // This is the magic line!
                        await BackgroundExecutionManager.RequestAccessAsync();
                        BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
                        builder.Name = typeof(Background.MojTrigger).FullName;
                        builder.SetTrigger(new TimeTrigger(15, false));
                        builder.SetTrigger(new SystemTrigger(SystemTriggerType.TimeZoneChange, false));
                        builder.TaskEntryPoint = builder.Name;
                        builder.Register();
                        registration = BackgroundTaskRegistration.AllTasks.Values.First();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
