﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.ApplicationModel;

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

        public static string OpiekunowieToString(List<Opiekun> opiekunowie)
        {
            string wyjscie = "";
            foreach (Opiekun opiekun in opiekunowie)
            {
                wyjscie += opiekun.OpiekunToString() + (char)0;
            }
            return wyjscie;
        }

        public static string PobierzWersjeAplikacji()
        {
            Version version = typeof(App).GetTypeInfo().Assembly.GetName().Version;
            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }

        public static List<Opiekun> StringToOpiekunowie(string opiekunowieString)
        {
            List<Opiekun> wyjscie = new List<Opiekun>();
            var opiekunowieDoDeserializacji = opiekunowieString.Split((char)0);
            foreach (string opiekunDoDeserializacji in opiekunowieDoDeserializacji)
            {
                if (opiekunDoDeserializacji != "")
                {
                    wyjscie.Add(new Opiekun(opiekunDoDeserializacji));
                }
            }
            return wyjscie;
        }
    }
}
