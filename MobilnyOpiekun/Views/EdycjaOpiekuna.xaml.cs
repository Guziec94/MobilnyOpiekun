using MobilnyOpiekun.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MobilnyOpiekun.Views
{
    public sealed partial class EdycjaOpiekuna : ContentDialog
    {
        public Opiekun zmodyfikowanyOpiekun;
        private Guid guid;
        public EdycjaOpiekuna(Opiekun modyfikowany)
        {
            InitializeComponent();
            PrimaryButtonText = "Zapisz zmiany";
            SecondaryButtonText = "Odrzuć zmiany";
            txtNazwaOpiekuna.Text = modyfikowany.nazwa;
            txtNumerTelefonu.Text = modyfikowany.numerTelefonu;
            guid = modyfikowany.guid;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            txtBledy.Text = "";
            string nazwaKontaktu = txtNazwaOpiekuna.Text;
            string numerTelefonu = txtNumerTelefonu.Text;
            bool czySaBledy = false;
            if (nazwaKontaktu.Length == 0)
            {
                txtBledy.Text += "\nNazwa kontaktu nie może być pusta.";
                czySaBledy = true;
            }
            if (numerTelefonu.Length == 0)
            {
                txtBledy.Text += "\nWprowadź numer telefonu ręcznie lub poprzez naciśnięcie przycisku z ikoną kontaktu.";
                czySaBledy = true;
            }
            // Podczas edycji nie jest sprawdzana unikalność numeru
            if (czySaBledy)
            {
                args.Cancel = true;
                return;
            }
            else
            {
                zmodyfikowanyOpiekun = new Opiekun(nazwaKontaktu, numerTelefonu, guid);
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
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
                    txtNumerTelefonu.Text = numer;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
