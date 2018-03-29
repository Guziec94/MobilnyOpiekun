using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Storage;

namespace MobilnyOpiekun.Background
{
    public static class KonfiguracjaBackground
    {
        private static string imie;
        private static string nazwisko;
        private static string poczatekAktywnosci;
        private static string koniecAktywnosci;
        private static IList<OpiekunBackground> opiekunowie;
        private static string maksymalnyCzasOczekiwaniaGps;
        private static Geoposition aktualnaPozycja;

        static string[] przechowywaneParametry = { "imie", "nazwisko", "poczatekAktywnosci", "koniecAktywnosci", "maksymalnyCzasOczekiwaniaGps" };

        public static string Imie { get => imie; set => imie = value; }
        public static string Nazwisko { get => nazwisko; set => nazwisko = value; }
        public static string PoczatekAktywnosci { get => poczatekAktywnosci; set => poczatekAktywnosci = value; }
        public static string KoniecAktywnosci { get => koniecAktywnosci; set => koniecAktywnosci = value; }
        public static IList<OpiekunBackground> Opiekunowie { get => opiekunowie; set => opiekunowie = value; }
        public static string MaksymalnyCzasOczekiwaniaGps { get => maksymalnyCzasOczekiwaniaGps; set => maksymalnyCzasOczekiwaniaGps = value; }
        public static Geoposition AktualnaPozycja { get => aktualnaPozycja; set => aktualnaPozycja = value; }

        public static void WczytajKonfiguracje()
        {
            ApplicationDataCompositeValue odczytaneParametry = (ApplicationDataCompositeValue)ApplicationData.Current.LocalSettings.Values["konfiguracja"] ?? new ApplicationDataCompositeValue();
            Imie = odczytaneParametry["imie"] as string;
            Nazwisko = odczytaneParametry["nazwisko"] as string;
            PoczatekAktywnosci = odczytaneParametry["poczatekAktywnosci"] as string;
            KoniecAktywnosci = odczytaneParametry["koniecAktywnosci"] as string;
            Opiekunowie = StringToOpiekunowie(odczytaneParametry["opiekunowie"] as string);
            MaksymalnyCzasOczekiwaniaGps = odczytaneParametry["maksymalnyCzasOczekiwaniaGps"] as string;
        }

        public static void ZapiszKonfiguracje()
        {
            ApplicationDataCompositeValue applicationDataCompositeValue = new ApplicationDataCompositeValue();
            foreach (string parametr in przechowywaneParametry)
            {
                var wartosc = typeof(KonfiguracjaBackground).GetField(parametr).GetValue(parametr);
                applicationDataCompositeValue.Add(parametr, wartosc);
            }
            applicationDataCompositeValue.Add("opiekunowie", OpiekunowieToString(Opiekunowie));
            ApplicationData.Current.LocalSettings.Values["konfiguracja"] = applicationDataCompositeValue;
        }

        public static string OpiekunowieToString(IList<OpiekunBackground> opiekunowie)
        {
            string wyjscie = "";
            foreach (OpiekunBackground opiekun in opiekunowie)
            {
                wyjscie += opiekun.OpiekunToString() + (char)0;
            }
            return wyjscie;
        }

        public static IList<OpiekunBackground> StringToOpiekunowie(string opiekunowieString)
        {
            IList<OpiekunBackground> wyjscie = new List<OpiekunBackground>();
            var opiekunowieDoDeserializacji = opiekunowieString.Split((char)0);
            foreach (string opiekunDoDeserializacji in opiekunowieDoDeserializacji)
            {
                if (opiekunDoDeserializacji != "")
                {
                    wyjscie.Add(new OpiekunBackground(opiekunDoDeserializacji));
                }
            }
            return wyjscie;
        }
    }

    public sealed class OpiekunBackground
    {
        private string nazwa;
        private string numerTelefonu;
        private Guid guid;

        public string Nazwa { get => nazwa; set => nazwa = value; }
        public string NumerTelefonu { get => numerTelefonu; set => numerTelefonu = value; }
        public Guid Guid { get => guid; set => guid = value; }

        /// <summary>
        /// Konstruktor używany podczas ładowania konfiguracji - deserializuje łańcuch znaków.
        /// </summary>
        /// <param name="opiekunString">Łańcuch znaków w postaci {nazwa};{numerTelefonu}</param>
        public OpiekunBackground(string opiekunString)
        {
            var parametry = opiekunString.Split(';');
            Guid = Guid.NewGuid();
            Nazwa = parametry[0];
            NumerTelefonu = parametry[1];
        }

        /// <summary>
        /// Serializacja opiekuna do łańcucha znaków.
        /// </summary>
        /// <returns>Łańcuch znaków w postaci "{nazwa};{numerTelefonu}".</returns>
        public string OpiekunToString()
        {
            return $"{Nazwa};{NumerTelefonu}";
        }
    }
}
