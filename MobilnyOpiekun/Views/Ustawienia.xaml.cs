using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MobilnyOpiekun.Classes;
using System.Linq;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MobilnyOpiekun.Views
{
    /// <summary>
    /// Strona umożliwiająca konfigurację parametrów aplikacji.
    /// </summary>
    public sealed partial class Ustawienia : Page
    {
        ApplicationDataContainer localSettings;
        ApplicationDataCompositeValue composite;
        MainPage mainPageInstance;

        public Ustawienia()
        {
            InitializeComponent();

            localSettings = ApplicationData.Current.LocalSettings;

            // Composite setting
            composite = (ApplicationDataCompositeValue)localSettings.Values["exampleCompositeSetting"];

            if (composite == null)
            {
                // No data - create empty compositeValue
                composite = new ApplicationDataCompositeValue();
            }
            else
            {
                // Access data in composite["intVal"] and composite["strVal"]
            }
        }

        // Funkcja przechwytująca parametr przekazany podczas nawigacji pomiędzy stronami.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            mainPageInstance = e.Parameter as MainPage;
        }

        private void btnOdrzucUstawienia_Click(object sender, RoutedEventArgs e)
        {
            mainPageInstance.przejdzDoStronyGlownej();
        }

        private void btnZachowajUstawienia_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values["exampleCompositeSetting"] = composite;
            mainPageInstance.przejdzDoStronyGlownej();
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
    }
}
