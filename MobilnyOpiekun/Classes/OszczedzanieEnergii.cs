using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.System.Power;
using Windows.UI.Xaml.Controls;

namespace MobilnyOpiekun.Classes
{
    public static class OszczedzanieEnergii
    {
        public static async void SprawdzOszczedzanieEnergii()
        {
            //Get reminder preference from LocalSettings
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            bool dontAskAgain = Convert.ToBoolean(localSettings.Values["czyPytacOOszczedzanieEnergii"]);
            
            // Check if battery saver is on and that it's okay to raise dialog
            if (PowerManager.EnergySaverStatus == EnergySaverStatus.On && dontAskAgain == false)
            {
                ContentDialog oszczedzanieEnergiiDialog = new ContentDialog();
                oszczedzanieEnergiiDialog.PrimaryButtonText = "Otwórz ustawienia\noszczędzania energii";
                oszczedzanieEnergiiDialog.SecondaryButtonText = "Ignoruj";
                oszczedzanieEnergiiDialog.Title = "Oszczędzanie energii jest włączone";
                StackPanel stackPanel = new StackPanel();
                TextBlock informacja = new TextBlock()
                {
                    Text = "Oszczędzanie energii jest włączone co może spowodować niepoprawne działanie aplikacji. Kliknij przycisk \"Otwórz ustawienia\", a następnie otwórz sekcję \"Użycie baterii przez aplikację\". Odnajdź aplikację MobilnyOpiekun, kliknij na nią i wybierz \"Zawsze dozwolone w tle\"."
                };
                CheckBox czyIgnorowac = new CheckBox()
                {
                    Content = "Zawsze ignoruj ten problem",
                    Name = "czyIgnorowac",
                    IsChecked = false
                };
                stackPanel.Children.Add(informacja);
                stackPanel.Children.Add(czyIgnorowac);
                oszczedzanieEnergiiDialog.Content = stackPanel;

                ContentDialogResult dialogResult = await oszczedzanieEnergiiDialog.ShowAsync();
                if (dialogResult == ContentDialogResult.Primary)
                {
                    // Launch battery saver settings (settings are available only when a battery is present)
                    await Launcher.LaunchUriAsync(new Uri("ms-settings:batterysaver-settings"));
                }

                CheckBox chkWynik = ((oszczedzanieEnergiiDialog.Content as StackPanel).Children.FirstOrDefault(x=>x.GetType() == czyIgnorowac.GetType()) as CheckBox);

                if (chkWynik.IsChecked == true)
                {
                    localSettings.Values["czyPytacOOszczedzanieEnergii"] = "true";
                }
            }
        }
    }
}
