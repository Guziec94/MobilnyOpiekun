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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MobilnyOpiekun
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            ShowStatusBar();
        }

        private void btnHamburger_Click(object sender, RoutedEventArgs e)
        {
            HamburgerMenu.IsPaneOpen = !HamburgerMenu.IsPaneOpen;
        }

        private void mnuItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            switch ((sender as StackPanel).Name)
            {
                case "mnuStronaGlowna":
                    txtTytulStrony.Text = "Strona główna";
                    zawartosc.Navigate(typeof(Views.StronaGlowna));
                    break;
                case "mnuStatystyka":
                    txtTytulStrony.Text = "Statystyka ruchu";
                    zawartosc.Navigate(typeof(Views.Statystyka));
                    break;
                case "mnuWezwijPomoc":
                    txtTytulStrony.Text = "Wzywanie pomocy";
                    zawartosc.Navigate(typeof(Views.WezwijPomoc));
                    break;
                case "mnuUstawienia":
                    txtTytulStrony.Text = "Ustawienia";
                    zawartosc.Navigate(typeof(Views.Ustawienia));
                    break;
                case "mnuPomoc":
                    txtTytulStrony.Text = "Pomoc";
                    zawartosc.Navigate(typeof(Views.Pomoc));
                    break;
            }
            if (HamburgerMenu.IsPaneOpen)
            {
                HamburgerMenu.IsPaneOpen = false;
            }
        }

        // show the StatusBar
        private async void ShowStatusBar()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                await statusbar.ShowAsync();
                statusbar.BackgroundOpacity = 1;
            }
        }
    }
}
