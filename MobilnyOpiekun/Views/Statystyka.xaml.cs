using MobilnyOpiekun.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MobilnyOpiekun.Views
{
    public sealed partial class Statystyka : Page
    {
        public Statystyka()
        {
            this.InitializeComponent();
            if (!DaneORuchu.SprawdzDostep())
            {
                scrlZawartosc.Visibility = Visibility.Collapsed;
                txtMaskaBlokujaca.Text = "Urządzenie nie posiada odpowiednich czujników lub dostęp do nich jest zablokowany. Więcej informacji znajdziesz w sekcji Pomoc.";
                ZablokujZawartosc("Historia aktywności jest niedostępna, czy chcesz przejść do ustawień aby jeszcze raz ją zainicjalizować?");
            }
            else
            {
                WyswietlDaneStatystyczne();
            }
        }

        private async void WyswietlDaneStatystyczne()
        {
            try
            {
                var activitySensor = await ActivitySensor.GetDefaultAsync();
                if (activitySensor != null)
                {
                    var daneZOstatnich24h = await ActivitySensor.GetSystemHistoryAsync(DateTime.Now, new TimeSpan(24, 0, 0));//DaneORuchu.PobierzHisotrycznaAktywnosc(DateTime.Now, new TimeSpan(24, 0, 0));
                    int i = 0;
                    int ileWRuchu = 0;
                    int ileWMiejscu = 0;
                    List<ActivityType> aktywnosciWRuchu = new List<ActivityType>() { ActivityType.Biking, ActivityType.Running, ActivityType.Walking };
                    List<ActivityType> aktywnosciWMiejscu = new List<ActivityType>() { ActivityType.Fidgeting, ActivityType.Idle, ActivityType.InVehicle, ActivityType.Stationary };

                    if (daneZOstatnich24h.Count > 0)
                    {
                        foreach (var odczyt in daneZOstatnich24h)
                        {
                            i++;
                            if (aktywnosciWMiejscu.Contains(odczyt.Activity))
                            {
                                ileWMiejscu++;
                            }
                            else if (aktywnosciWRuchu.Contains(odczyt.Activity))
                            {
                                ileWRuchu++;
                            }
                        }
                        progWMiejscu.Value = (double)(ileWMiejscu / i) * 100;
                        progWRuchu.Value = (double)(ileWRuchu / i) * 100;
                    }
                    else
                    {
                        MessageDialog message = new MessageDialog("Aplikacja nie mogła pobrać danych.");
                        message.ShowAsync();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageDialog message = new MessageDialog("Aplikacja napotkała problem\n"+ex.Message);
                message.ShowAsync();
            }
        }

        async void ZablokujZawartosc(string wiadomosc)
        {
            ContentDialog md = new ContentDialog();
            md.Title = "Historia aktywności jest niedostępna.";
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
    }
}
