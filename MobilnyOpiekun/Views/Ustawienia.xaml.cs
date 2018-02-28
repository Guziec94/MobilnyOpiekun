using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MobilnyOpiekun.Classes;
using Windows.UI.Popups;
using Windows.UI.Core;
using System.Linq;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MobilnyOpiekun.Views
{
    /// <summary>
    /// Strona umożliwiająca konfigurację parametrów aplikacji.
    /// </summary>
    public sealed partial class Ustawienia : Page
    {
        Task odswiezanieListyOpiekunow;
        bool czyodswiezanieListyOpiekunowAktywne = true;
        public Ustawienia()
        {
            InitializeComponent();

            #region ładowanie konfiguracji i wypełnianie pól danymi
            Konfiguracja.WczytajKonfiguracje();

            txtImie.Text = Konfiguracja.imie??"";
            txtNazwisko.Text = Konfiguracja.nazwisko ?? "";
            TimeSpan poczatek, koniec;
            if(TimeSpan.TryParse(Konfiguracja.poczatekAktywnosci, out poczatek))
            {
                timePoczatek.Time = poczatek;
            }
            if (TimeSpan.TryParse(Konfiguracja.koniecAktywnosci, out koniec))
            {
                timeKoniec.Time = koniec;
            }
            #endregion

            #region odswiezanie listy opiekunow
            KlasaPomocniczna.odswiezListeOpiekunow = new System.Threading.AutoResetEvent(false);
            odswiezanieListyOpiekunow = new Task(async () => 
            {
                while (czyodswiezanieListyOpiekunowAktywne)
                {
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        ZaladujListeOpiekunow();
                    });
                    KlasaPomocniczna.odswiezListeOpiekunow.WaitOne();
                }
            });
            odswiezanieListyOpiekunow.Start();
            #endregion

            #region uruchomienie funkcji walidujących
            txtImie_TextChanged(null, null);
            txtNazwisko_TextChanged(null, null);
            timePoczatek_TimeChanged(null, null);
            timeKoniec_TimeChanged(null, null);
            #endregion
        }

        public void ZatrzymajOdswiezanie()
        {
            try
            {
                czyodswiezanieListyOpiekunowAktywne = false;
                KlasaPomocniczna.odswiezListeOpiekunow.Set();
            }
            catch { }
        }

        private void btnOdrzucUstawienia_Click(object sender, RoutedEventArgs e)
        {
            KlasaPomocniczna.PrzejdzDoStronyGlownej();
        }

        private void btnZachowajUstawienia_Click(object sender, RoutedEventArgs e)
        {
            if (Konfiguracja.CzyKonfiguracjaPoprawna())
            {
                Konfiguracja.ZapiszKonfiguracje();
                KlasaPomocniczna.PrzejdzDoStronyGlownej();
            }
            else
            {
                txtBladOgolny.Visibility = Visibility.Visible;
            }
        }

        private async void btnDodajOpiekuna_Click(object sender, RoutedEventArgs e)
        {
            if (Konfiguracja.opiekunowie.Count == 5)
            {
                MessageDialog messageDialog = new MessageDialog("Nie można dodać więcej niż 5 opiekunów.");
                messageDialog.ShowAsync();
                return;
            }
            DodawanieOpiekuna cD = new DodawanieOpiekuna();
            if (await cD.ShowAsync() == ContentDialogResult.Primary)
            {
                Opiekun doDodania = cD.utworzonyOpiekun;
                Konfiguracja.opiekunowie.Add(doDodania);
                ZaladujListeOpiekunow();
            }
        }

        private void ZaladujListeOpiekunow()
        {
            stpaOpiekunowie.Children.Clear();
            if (Konfiguracja.opiekunowie != null)
            {
                foreach (Opiekun opiekun in Konfiguracja.opiekunowie)
                {
                    stpaOpiekunowie.Children.Add(opiekun.GenerujStackPanelDoEdycji());
                }
            }
        }

        private void txtImie_TextChanged(object sender, TextChangedEventArgs e)
        {
            Konfiguracja.imie = txtImie.Text;
            txtImieBlad.Visibility = Konfiguracja.imie == "" ? Visibility.Visible : Visibility.Collapsed;
        }

        private void txtNazwisko_TextChanged(object sender, TextChangedEventArgs e)
        {
            Konfiguracja.nazwisko = txtNazwisko.Text;
            txtNazwiskoBlad.Visibility = Konfiguracja.nazwisko == "" ? Visibility.Visible : Visibility.Collapsed;
        }

        private void timePoczatek_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            Konfiguracja.poczatekAktywnosci = timePoczatek.Time.ToString();
            if(timePoczatek.Time < timeKoniec.Time)
            {
                txtTimeBlad.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtTimeBlad.Visibility = Visibility.Visible;
            }
        }

        private void timeKoniec_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            Konfiguracja.koniecAktywnosci = timeKoniec.Time.ToString();
            if (timePoczatek.Time < timeKoniec.Time)
            {
                txtTimeBlad.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtTimeBlad.Visibility = Visibility.Visible;
            }
        }

        private void stpaOpiekunowie_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Konfiguracja.opiekunowie.Any())
            {
                txtOpiekunowieBlad.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtOpiekunowieBlad.Visibility = Visibility.Visible;
            }
        }
    }
}
