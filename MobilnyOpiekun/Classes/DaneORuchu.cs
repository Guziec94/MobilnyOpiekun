using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Sensors;

namespace MobilnyOpiekun.Classes
{

    public static class DaneORuchu
    {
        static Guid ActivitySensorClassId = new Guid("9D9E0118-1807-4F2E-96E4-2CE57142E196");
        static IReadOnlyList<ActivitySensorReading> odczyty;

        private static ActivitySensor activitySensor;
        public static async Task<bool> InicjalizujDaneORuchu()
        {
            activitySensor = await ActivitySensor.GetDefaultAsync();
            return activitySensor != null;
        }

        public static bool SprawdzDostep()
        {
            var deviceAccessInfo = DeviceAccessInformation.CreateFromDeviceClassId(ActivitySensorClassId);
            if(deviceAccessInfo.CurrentStatus == DeviceAccessStatus.Allowed)
            {
                if(ActivitySensor.GetDefaultAsync() != null)
                {
                    return true;
                }
            }
            return false;
        }

        public async static void PobierzAktualnaAktywnosc()
        {
            // Get the current activity reading
            var aktywnosc = await activitySensor.GetCurrentReadingAsync();
            if (aktywnosc != null)
            {
                var ScenarioOutput_Activity = aktywnosc.Activity.ToString();
                var ScenarioOutput_Confidence = aktywnosc.Confidence.ToString();
                var ScenarioOutput_Timestamp = aktywnosc.Timestamp.ToString("u");
            }
        }

        public async static Task<IReadOnlyList<ActivitySensorReading>> PobierzHisotrycznaAktywnosc(DateTimeOffset poczatekAktywnosci, TimeSpan dlugosc)
        {
            DaneORuchu.odczyty = await ActivitySensor.GetSystemHistoryAsync(poczatekAktywnosci, dlugosc);
            return odczyty;
        }

        public static async Task<bool> CzyUzytkownikJuzWstal(bool kolejneSprawdzenie = false)
        {
            // FUNKCJA PRZENIESIONA DO BACKGROUND TASKA
            return true;
        }
    }
}
