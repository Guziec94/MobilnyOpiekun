using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MobilnyOpiekun.Classes
{
    static class Konfiguracja
    {
        public static string imie;
        public static string nazwisko;
        public static string poczatekAktywnosci;
        public static string koniecAktywnosci;
        public static List<Opiekun> opiekunowie;

        static string[] parametryDoSkonfigurowania = { "imie", "nazwisko", "poczatekAktywnosci", "koniecAktywnosci", "opiekunowie" };

        public static bool CzyPierwszeUruchomienie()
        {
            if (ApplicationData.Current.LocalSettings.Values.Keys.Any())
            {
                //imie = "Tomasz";
                //nazwisko = "Braczynski";
                //poczatekAktywnosci = "08:00:00";
                //koniecAktywnosci = "20:00:00";
                //ZapiszKonfiguracje();
                WczytajKonfiguracje();
                return false;
            }
            WstepnieSkonfiguruj();
            return true;
        }

        public static void WstepnieSkonfiguruj()
        {
            ApplicationDataCompositeValue applicationDataCompositeValue = new ApplicationDataCompositeValue();
            foreach(string parametr in parametryDoSkonfigurowania)
            {
                applicationDataCompositeValue.Add(parametr, null);
            }
            ApplicationData.Current.LocalSettings.Values["konfiguracja"] = applicationDataCompositeValue;
        }

        public static void WczytajKonfiguracje()
        {
            ApplicationDataCompositeValue odczytaneParametry = (ApplicationDataCompositeValue)ApplicationData.Current.LocalSettings.Values["konfiguracja"];
            foreach (string parametr in odczytaneParametry.Keys)
            {
                typeof(Konfiguracja).GetField(parametr).SetValue(parametr, odczytaneParametry[parametr]);
            }
        }

        public static void ZapiszKonfiguracje()
        {
            ApplicationDataCompositeValue applicationDataCompositeValue = new ApplicationDataCompositeValue();
            foreach (string parametr in parametryDoSkonfigurowania)
            {
                var wartosc = typeof(Konfiguracja).GetField(parametr).GetValue(parametr);
                applicationDataCompositeValue.Add(parametr, wartosc);
            }
            ApplicationData.Current.LocalSettings.Values["konfiguracja"] = applicationDataCompositeValue;
        }
    }
}
