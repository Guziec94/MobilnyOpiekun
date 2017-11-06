using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MobilnyOpiekun.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Ustawienia : Page
    {
        ApplicationDataContainer localSettings;
        ApplicationDataCompositeValue composite;

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

        private void btnOdrzucUstawienia_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void btnZachowajUstawienia_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values["exampleCompositeSetting"] = composite;
            Frame.Navigate(typeof(MainPage));
        }

        private async void btnWybierzKontakt_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                ContactPicker contactPicker = new ContactPicker();
                contactPicker.CommitButtonText = "Select";
                contactPicker.SelectionMode = ContactSelectionMode.Fields;
                contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);

                Contact contact = await contactPicker.PickContactAsync();
                if (contact != null)
                {
                    string numer = contact.Phones.FirstOrDefault().Number;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
