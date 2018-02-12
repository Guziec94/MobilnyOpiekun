using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using MobilnyOpiekun.Classes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MobilnyOpiekun.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StronaGlowna : Page
    {
        bool zmianaPodczasLadowania;
        public StronaGlowna()
        {
            InitializeComponent();
            if (BackgroundLibrary.IsWorking)
            {
                zmianaPodczasLadowania = true;
                tglBackgroundTask.IsOn = true;
            }
            zmianaPodczasLadowania = false;

            txtWersjaAplikacji.Text = "v" + KlasaPomocniczna.PobierzWersjeAplikacji();
        }

        private async void tglBackgroundTask_Toggled(object sender, RoutedEventArgs e)
        {
            if (!zmianaPodczasLadowania)
            {
                bool poprzedniStan = BackgroundLibrary.IsWorking;
                bool aktualnyStan = await BackgroundLibrary.Toggle();
                if(poprzedniStan == aktualnyStan)
                {
                    var dialog = new MessageDialog("Coś poszło nie tak, agent nie został poprawnie " + (poprzedniStan ? "wyłączony." : "włączony."));
                    await dialog.ShowAsync();
                }
            }
        }
    }
}
