using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sensors;

namespace MobilnyOpiekun.Classes
{
    public static class DaneORuchu
    {
        private static ActivitySensor activitySensor;
        public static async Task<bool> InicjalizujDaneORuchu()
        {
            activitySensor = await ActivitySensor.GetDefaultAsync();
            return activitySensor != null;
        }
    }
}
