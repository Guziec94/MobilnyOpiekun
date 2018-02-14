using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MobilnyOpiekun.Classes;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MobilnyOpiekun.Views
{
    /// <summary>
    /// Strona umożliwiająca konfigurację parametrów aplikacji.
    /// </summary>
    public sealed partial class Ustawienia : Page
    {
        public Ustawienia()
        {
            InitializeComponent();

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
            ZaladujListeOpiekunow();
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
            DodajOpiekuna cD = new DodajOpiekuna();
            if (await cD.ShowAsync() == ContentDialogResult.Primary)
            {
                Opiekun doDodania = cD.utworzonyOpiekun;
                Konfiguracja.opiekunowie.Add(doDodania);
                stpaOpiekunowie.Children.Clear();
                ZaladujListeOpiekunow();
            }
        }

        private void ZaladujListeOpiekunow()
        {
            if (Konfiguracja.opiekunowie != null)
            {
                foreach (Opiekun opiekun in Konfiguracja.opiekunowie)
                {
                    stpaOpiekunowie.Children.Add(opiekun.GenerujStackPanel());
                }
            }
        }

        private async Task<string> InputTextDialogAsync(string tytul, string btnZatwierdz, bool dwaPrzyciski = false, string btnOdrzuc = "Odrzuć")
        {
            TextBox inputTextBox = new TextBox
            {
                AcceptsReturn = false,
                Height = 32
            };
            ContentDialog dialog = new ContentDialog
            {
                Content = inputTextBox,
                Title = tytul,
                IsSecondaryButtonEnabled = dwaPrzyciski,
                PrimaryButtonText = btnZatwierdz,
                SecondaryButtonText = btnOdrzuc,
            };
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                return inputTextBox.Text;
            }
            else
            {
                return "";
            }
        }

        private void stpaOpiekunowie_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            StackPanel wybranyElement = sender as StackPanel;
            UIElementCollection cos = wybranyElement.Children;
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
    }
}
