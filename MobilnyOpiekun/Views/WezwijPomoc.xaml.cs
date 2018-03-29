using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MobilnyOpiekun.Classes;
using Windows.UI.Popups;
using Windows.Devices.Geolocation;
using System.Threading.Tasks;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MobilnyOpiekun.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WezwijPomoc : Page
    {
        public WezwijPomoc()
        {
            this.InitializeComponent();
            if (!WiadomoscSMS.czyZainicjalizowane)
            {
                scrlZawartosc.Visibility = Visibility.Collapsed;
                txtMaskaBlokujaca.Text = "Nie znaleziono urządzenia, które może wysyłać SMS. Więcej informacji znajdziesz na stronie Pomoc.";
                ZablokujZawartosc("Nie znaleziono urządzenia, które może wysyłać SMS. Funkcja wzywania pomocy będzie niedostępna. Chcesz przejść do ustawień aby jeszcze raz zainicjalizować wysyłanie SMS?");
            }
            else
            if (!Konfiguracja.opiekunowie.Any())
            {
                scrlZawartosc.Visibility = Visibility.Collapsed;
                txtMaskaBlokujaca.Text = "Nie znaleziono ani jednego opiekuna.";
                ZablokujZawartosc("Nie znaleziono żadnego opiekuna. Czy chcesz przejść do ustawień aby go dodać?");
            }
            else
            {
                KlasaPomocniczna.opiekunowieWybraniDoPomocy = new List<Opiekun>();
                foreach (Opiekun opiekun in Konfiguracja.opiekunowie)
                {
                    stpaOpiekunowie.Children.Add(opiekun.GenerujStackPanelDoWyboru());
                }
            }
        }

        async void ZablokujZawartosc(string wiadomosc)
        {
            ContentDialog md = new ContentDialog();
            md.Title = "Funkcja wzywania pomocy jest niedostępna";
            md.Content = wiadomosc;
            md.PrimaryButtonText = "Tak";
            md.SecondaryButtonText = "Nie";
            if (await md.ShowAsync() == ContentDialogResult.Primary)
            {
                KlasaPomocniczna.PrzejdzDo("Ustawienia", "Ustawienia");
            }
            else
            {
                maskaBlokujaca.Visibility = Visibility.Visible;
            }
        }

        private void btnWezwijPomoc_Click(object sender, RoutedEventArgs e)
        {
            if (KlasaPomocniczna.opiekunowieWybraniDoPomocy.Any())
            {
                txtOpiekunowieBlad.Visibility = Visibility.Collapsed;
                KlasaPomocniczna.aktualnaPozycja = null;
                Task pobieranieLokalizacji = new Task(() =>
                {
                    KlasaPomocniczna.aktualnaPozycja = Lokalizacja.PobierzLokalizacje().Result;
                });
                pobieranieLokalizacji.Start();
                pobieranieLokalizacji.Wait(int.Parse(Konfiguracja.maksymalnyCzasOczekiwaniaGps));
                string trescWiadomosci = "Hej, potrzebuję Twojej pomocy. Skontaktuj się ze mną jak najszybciej. ";
                if (KlasaPomocniczna.aktualnaPozycja != null)
                {
                    string szerokoscGeograficzna = KlasaPomocniczna.aktualnaPozycja.Coordinate.Latitude.ToString().Replace(',', '.');// North/South
                    string dlugoscGeograficzna = KlasaPomocniczna.aktualnaPozycja.Coordinate.Longitude.ToString().Replace(',', '.');// East/West
                    string link = $"http://bing.com/maps/default.aspx?rtp=~pos.{szerokoscGeograficzna}_{dlugoscGeograficzna}";
                    trescWiadomosci += "Moja lokalizacja:\n" + link;
                }
                trescWiadomosci += $"\n{Konfiguracja.imie} {Konfiguracja.nazwisko}";

                // Tak dla pewności
                if (WiadomoscSMS.czyZainicjalizowane)
                {
                    WiadomoscSMS.InicjalizujSMS();
                }

                foreach (Opiekun opiekun in KlasaPomocniczna.opiekunowieWybraniDoPomocy)
                {
                    bool czyWyslano = false;
                    try
                    {
                        czyWyslano = WiadomoscSMS.WyslijSMS(opiekun.numerTelefonu, trescWiadomosci);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else
            {
                txtOpiekunowieBlad.Visibility = Visibility.Visible;
            }
        }
    }
}
