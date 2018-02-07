using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace MobilnyOpiekun.Classes
{
    public static class KlasaPomocniczna
    {
        public static MainPage mainPageInstance;

        public static async void PokazPasekStanuAsync()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                statusbar.BackgroundOpacity = 1;
                await statusbar.ShowAsync();
            }
        }

        public static void PrzejdzDo(string strona, string tytulStrony)
        {
            mainPageInstance.przejdzDo(strona, tytulStrony);
        }

        public static void PrzejdzDoStronyGlownej()
        {
            mainPageInstance.PrzejdzDoStronyGlownej();
        }
    }
}
