using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MobilnyOpiekun.Classes;
using System.Linq;
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
        }

        private void btnOdrzucUstawienia_Click(object sender, RoutedEventArgs e)
        {
            //mainPageInstance.przejdzDoStronyGlownej();
            KlasaPomocniczna.PrzejdzDoStronyGlownej();
        }

        private void btnZachowajUstawienia_Click(object sender, RoutedEventArgs e)
        {
            if(timePoczatek.Time > timeKoniec.Time)
            {
                MessageDialog messageDialog = new MessageDialog("Zakres godzin aktywności w ciągu dnia jest błędny. Popraw godzinę początku i końca dnia, a następnie zapisz zmiany raz jeszcze.");
                messageDialog.ShowAsync();
                return;
            }
            Konfiguracja.ZapiszKonfiguracje();
            KlasaPomocniczna.PrzejdzDoStronyGlownej();
        }

        private async void btnDodajOpiekuna_Click(object sender, RoutedEventArgs e)
        {
            DodajOpiekuna cD = new DodajOpiekuna();
            if (await cD.ShowAsync() == ContentDialogResult.Primary)
            {
                Opiekun doDodania = cD.utworzonyOpiekun;
                stpaOpiekunowie.Children.Add(doDodania.GenerujStackPanel());
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
        }

        private void txtNazwisko_TextChanged(object sender, TextChangedEventArgs e)
        {
            Konfiguracja.nazwisko = txtNazwisko.Text;
        }

        private void timePoczatek_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            Konfiguracja.poczatekAktywnosci = timePoczatek.Time.ToString();
        }

        private void timeKoniec_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            Konfiguracja.koniecAktywnosci = timeKoniec.Time.ToString();
        }
    }
}
