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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MobilnyOpiekun
{
    /// <summary>
    /// Główny widok aplikacji, to tutaj zawarte jest hamburger menu oraz logika odpowiadająca za przechodzenie pomiędzy podstr.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        bool czyWiadomosciAktywne;
        public MainPage()
        {
            InitializeComponent();
            KlasaPomocniczna.PokazPasekStanuAsync();
            czyWiadomosciAktywne = WiadomoscSMS.InicjalizujSMS();
            KlasaPomocniczna.mainPageInstance = this;
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

        public void przejdzDo(string strona, string tytulStrony)
        {
            // Ustawienie tytułu wyświetlanej strony
            txtTytulStrony.Text = tytulStrony;
            // Załadowanie strony do obiektu Frame
            var stronaDoWyswietlenia = "MobilnyOpiekun.Views." + strona;
            zawartosc.Navigate(Type.GetType(stronaDoWyswietlenia), this);

            if (HamburgerMenu.IsPaneOpen)
            {
                HamburgerMenu.IsPaneOpen = false;
            }
        }

        public void przejdzDoStronyGlownej()
        {
            mnuItem_Tapped(mnuStronaGlowna, new TappedRoutedEventArgs());
        }

        public void przejdzDoUstawien()
        {
            mnuItem_Tapped(mnuUstawienia, new TappedRoutedEventArgs());
        }
    }
}
