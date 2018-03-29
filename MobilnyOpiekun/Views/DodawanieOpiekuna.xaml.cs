using System;
using System.Linq;
using Windows.ApplicationModel.Contacts;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MobilnyOpiekun.Classes;
using Windows.System;
using System.Collections.Generic;
// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MobilnyOpiekun.Views
{
    public sealed partial class DodawanieOpiekuna : ContentDialog
    {
        public Opiekun utworzonyOpiekun;

        public DodawanieOpiekuna()
        {
            InitializeComponent();
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
            if(Konfiguracja.opiekunowie.Any(x=>x.numerTelefonu == numerTelefonu))
            {
                txtBledy.Text += "\nOpiekun o takim numerze telefonu już istnieje.";
                czySaBledy = true;
            }
            if (czySaBledy)
            {
                args.Cancel = true;
                return;
            }
            else
            {
                numerTelefonu = numerTelefonu.Replace(" ", "");
                utworzonyOpiekun = new Opiekun(nazwaKontaktu, numerTelefonu);
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
