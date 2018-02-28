using System;
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

        static string[] przechowywaneParametry = { "imie", "nazwisko", "poczatekAktywnosci", "koniecAktywnosci" };

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
            foreach(string parametr in przechowywaneParametry)
            {
                typeof(Konfiguracja).GetField(parametr).SetValue(parametr, "");
            }
            opiekunowie = new List<Opiekun>();
        }

        public static void WczytajKonfiguracje()
        {
            ApplicationDataCompositeValue odczytaneParametry = (ApplicationDataCompositeValue)ApplicationData.Current.LocalSettings.Values["konfiguracja"]?? new ApplicationDataCompositeValue();
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
            foreach (string parametr in przechowywaneParametry)
            {
                var wartosc = typeof(Konfiguracja).GetField(parametr).GetValue(parametr);
                applicationDataCompositeValue.Add(parametr, wartosc);
            }
            applicationDataCompositeValue.Add("opiekunowie", KlasaPomocniczna.OpiekunowieToString(opiekunowie));
            ApplicationData.Current.LocalSettings.Values["konfiguracja"] = applicationDataCompositeValue;
        }

        public static bool CzyKonfiguracjaPoprawna()
        {
            if (imie == "" || nazwisko == "" || poczatekAktywnosci == "" || koniecAktywnosci == "") 
            {
                return false;
            }
            else if (TimeSpan.Parse(poczatekAktywnosci) > TimeSpan.Parse(koniecAktywnosci))
            {
                return false;
            }
            if (!opiekunowie.Any())
            {
                return false;
            }
            return true;
        }

        public static string ZwrocBledyKonfiguracji()
        {
            string wyjscie = "";
            if (imie == "" || nazwisko == "")
            {
                wyjscie += "Imię i nazwisko jest wymagane.\n";
            }
            if (poczatekAktywnosci == "" || koniecAktywnosci == "")
            {
                wyjscie += "Początkowa i końcowa godzina aktywności jest wymagana.\n";
            }
            else if (TimeSpan.Parse(poczatekAktywnosci) > TimeSpan.Parse(koniecAktywnosci))
            {
                wyjscie += "Podany zakres aktywności jest niepoprawny.\n";
            }
            if (!opiekunowie.Any())
            {
                wyjscie += "Co najmniej jeden opiekun jest wymagany.";
            }
            return wyjscie;
        }

        public static bool CzyPrzyznanoWszystkieDostepy()
        {
            return false;
        }

        public static string SprawdzDostepy()
        {
            string wyjscie = "";

            return wyjscie;
        }
    }
}
