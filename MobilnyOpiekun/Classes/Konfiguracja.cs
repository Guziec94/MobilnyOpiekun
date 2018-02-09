using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        static string[] parametryDoSkonfigurowania = { "imie", "nazwisko", "poczatekAktywnosci", "koniecAktywnosci" };

        public static bool CzyPierwszeUruchomienie()
        {
            if (ApplicationData.Current.LocalSettings.Values.Keys.Any())
            {
                WczytajKonfiguracje();
                return false;
            }
            WstepnieSkonfiguruj();
            return true;
        }

        public static void WstepnieSkonfiguruj()
        {
            foreach(string parametr in parametryDoSkonfigurowania)
            {
                typeof(Konfiguracja).GetField(parametr).SetValue(parametr, null);
            }
            opiekunowie = new List<Opiekun>();
        }

        public static void WczytajKonfiguracje()
        {
            ApplicationDataCompositeValue odczytaneParametry = (ApplicationDataCompositeValue)ApplicationData.Current.LocalSettings.Values["konfiguracja"];
            foreach (string parametr in odczytaneParametry.Keys)
            {
                if (parametr == "opiekunowie")
                {
                    opiekunowie = KlasaPomocniczna.StringToOpiekunowie(odczytaneParametry["opiekunowie"] as string);
                }
                else
                {
                    typeof(Konfiguracja).GetField(parametr).SetValue(parametr, odczytaneParametry[parametr]);
                }
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
            applicationDataCompositeValue["opiekunowie"] = KlasaPomocniczna.OpiekunowieToString(opiekunowie);
            ApplicationData.Current.LocalSettings.Values["konfiguracja"] = applicationDataCompositeValue;
        }
    }
}
