using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace MobilnyOpiekun.Classes
{
    public static class Lokalizacja
    {
        static Geolocator geolocator = new Geolocator();
        static GeolocationAccessStatus geolocationAccessStatus;
        public static async Task<bool> InicjalizujGPS()
        {
            geolocationAccessStatus = await Geolocator.RequestAccessAsync();
            geolocator = geolocator ?? new Geolocator();
            if(geolocator == null)
            {
                return false;
            }
            return geolocationAccessStatus == GeolocationAccessStatus.Allowed;
        }

        public static async Task<Geoposition> PobierzLokalizacje()
        {
            var geoposition = await geolocator.GetGeopositionAsync();
            return geoposition;
        }
    }
}
