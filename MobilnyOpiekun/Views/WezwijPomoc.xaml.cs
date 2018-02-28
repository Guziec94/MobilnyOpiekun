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
                ZablokujZawartosc();
            }
            else
            {
                foreach (Opiekun opiekun in Konfiguracja.opiekunowie)
                {
                    stpaOpiekunowie.Children.Add(opiekun.GenerujStackPanelDoWyboru());
                }
            }
        }

        async void ZablokujZawartosc()
        {
            ContentDialog md = new ContentDialog();
            md.Title = "Funkcja wzywania pomocy jest niedostępna";
            md.Content = "Nie znaleziono urządzenia, które może wysyłać SMS. Funkcja wzywania pomocy będzie niedostępna. Chcesz przejść do ustawień aby jeszcze raz zainicjalizować wysyłanie SMS?";
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
