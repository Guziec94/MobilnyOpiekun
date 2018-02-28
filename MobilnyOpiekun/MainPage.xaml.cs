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
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Popups;
using Windows.UI.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MobilnyOpiekun
{
    /// <summary>
    /// Główny widok aplikacji, to tutaj zawarte jest hamburger menu, logika odpowiadająca za przechodzenie pomiędzy podstronami oraz ładowanie konfiguracji.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        bool dostepDoWiadomosci;
        bool dostepDoLokalizacja;
        bool dostepDoDaneORuchu;
        bool czyAgentAktywny;
        public MainPage()
        {
            InitializeComponent();

            ApplicationView.GetForCurrentView().VisibleBoundsChanged += MainPage_VisibleBoundsChanged;
            MainPage_VisibleBoundsChanged(null, null);

            KlasaPomocniczna.PokazPasekStanuAsync();

            Task inicjalizacja = new Task(async () =>
            {
                dostepDoWiadomosci = WiadomoscSMS.InicjalizujSMS();
                dostepDoLokalizacja = await Lokalizacja.InicjalizujGPS();
                //Lokalizacja.PobierzLokalizacje();
                dostepDoDaneORuchu = await DaneORuchu.InicjalizujDaneORuchu();
                czyAgentAktywny = BackgroundLibrary.Init();
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                    {
                        OszczedzanieEnergii.SprawdzOszczedzanieEnergii();
                    }
                );
            });
            inicjalizacja.Start();
            bool czyKonfigurowac = Konfiguracja.CzyPierwszeUruchomienie();

            PrzejdzDoStronyGlownej();

            KlasaPomocniczna.mainPageInstance = this;
        }

        /// <summary>
        /// W zależności od wysokości okna ustawia wysokość Frame'a. Wywoływane podczas zmiany wymiarów wyświetlanego widoku, np. podczas ukrycia programowego paska nawigacji.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MainPage_VisibleBoundsChanged(ApplicationView sender, object args)
        {
            zawartosc.Height = ApplicationView.GetForCurrentView().VisibleBounds.Height - 50;
            if (zawartosc.Height > this.Height - 50) 
            {
                // Frame (zawartosc) nie może być większy niż otaczające go elementy
                zawartosc.Height = this.Height - 50;
            }
        }

        private void btnHamburger_Click(object sender, RoutedEventArgs e)
        {
            HamburgerMenu.IsPaneOpen = !HamburgerMenu.IsPaneOpen;
        }

        public void mnuItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StackPanel wybranyElement = sender as StackPanel;
            // Ustawienie tytułu wyświetlanej strony
            string tytulStrony = (wybranyElement.Children.FirstOrDefault(x => x.GetType().Name == "TextBlock") as TextBlock).Text;
            txtTytulStrony.Text = tytulStrony;
            // Załadowanie strony do obiektu Frame
            var stronaDoWyswietlenia = "MobilnyOpiekun.Views." + wybranyElement.Name.Substring(3);
            zawartosc.Navigate(Type.GetType(stronaDoWyswietlenia), this);

            if (HamburgerMenu.IsPaneOpen)
            {
                HamburgerMenu.IsPaneOpen = false;
            }
        }

        public void przejdzDo(string strona, string tytulStrony = "")
        {
            // Załadowanie strony do obiektu Frame
            var stronaDoWyswietlenia = "MobilnyOpiekun.Views." + strona;
            zawartosc.Navigate(Type.GetType(stronaDoWyswietlenia), this);

            // Ustawienie tytułu wyświetlanej strony
            txtTytulStrony.Text = tytulStrony;

            if (HamburgerMenu.IsPaneOpen)
            {
                HamburgerMenu.IsPaneOpen = false;
            }
        }

        public void PrzejdzDoStronyGlownej()
        {
            mnuItem_Tapped(mnuStronaGlowna, new TappedRoutedEventArgs());
        }

        public void PrzejdzDoUstawien()
        {
            mnuItem_Tapped(mnuUstawienia, new TappedRoutedEventArgs());
        }

        private async void zawartosc_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (zawartosc.Content != null)
            {
                string stronaPrzedPrzejsciem = (zawartosc.Content as Page).BaseUri.Segments[2].Split('.')[0];
                if(stronaPrzedPrzejsciem == "Ustawienia")
                {
                    (zawartosc.Content as Views.Ustawienia).ZatrzymajOdswiezanie();
                }
            }
        }
    }
}
