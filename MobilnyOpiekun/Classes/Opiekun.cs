using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MobilnyOpiekun.Classes
{
    public class Opiekun
    {
        public string nazwa;
        public string numerTelefonu;
        public Guid guid;

        public Opiekun()
        {
            guid = Guid.NewGuid();
        }

        /// <summary>
        /// Konstruktor używany podczas dodawania nowego opiekuna poprzez GUI.
        /// </summary>
        /// <param name="nazwa"></param>
        /// <param name="numerTelefonu"></param>
        /// <param name="guid"></param>
        public Opiekun(string nazwa, string numerTelefonu)
        {
            this.guid = Guid.NewGuid();
            this.nazwa = nazwa;
            this.numerTelefonu = numerTelefonu;
        }

        /// <summary>
        /// Konstruktor używany podczas edycji danych lub dodawania nowego opiekuna poprzez GUI.
        /// </summary>
        /// <param name="nazwa"></param>
        /// <param name="numerTelefonu"></param>
        /// <param name="guid"></param>
        public Opiekun(string nazwa, string numerTelefonu, Guid guid)
        {
            this.guid = guid;
            this.nazwa = nazwa;
            this.numerTelefonu = numerTelefonu;
        }

        /// <summary>
        /// Konstruktor używany podczas ładowania konfiguracji - deserializuje łańcuch znaków.
        /// </summary>
        /// <param name="opiekunString">Łańcuch znaków w postaci {nazwa};{numerTelefonu}</param>
        public Opiekun(string opiekunString)
        {
            var parametry = opiekunString.Split(';');
            guid = Guid.NewGuid();
            nazwa = parametry[0];
            numerTelefonu = parametry[1];
        }

        /// <summary>
        /// Serializacja opiekuna do łańcucha znaków.
        /// </summary>
        /// <returns>Łańcuch znaków w postaci "{nazwa};{numerTelefonu}".</returns>
        public string OpiekunToString()
        {
            return $"{nazwa};{numerTelefonu}";
        }

        public void AktualizujOpiekuna(string nazwa, string numerTelefonu)
        {
            this.nazwa = numerTelefonu;
            this.numerTelefonu = numerTelefonu;
        }

        public StackPanel GenerujStackPanelDoEdycji()
        {
            StackPanel rezultat = new StackPanel()
            {
                Name = "stpaOpiekun",
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = new SolidColorBrush(Colors.WhiteSmoke),
                Margin = new Thickness(5)
            };

            StackPanel stpaInformacjeOgolne = new StackPanel()
            {
                Name = "stpaInformacjeOgolne"
            };
            rezultat.Children.Add(stpaInformacjeOgolne);

            TextBlock nazwa = new TextBlock
            {
                Name = "txtNazwaOpiekuna",
                Text = this.nazwa,
                Width = 160,
                Margin = new Thickness(5),
                TextWrapping = TextWrapping.Wrap
            };
            stpaInformacjeOgolne.Children.Add(nazwa);

            TextBlock numerTelefonu = new TextBlock()
            {
                Name = "txtNumerTelefonuOpiekuna",
                Text = this.numerTelefonu,
                FontStyle = Windows.UI.Text.FontStyle.Italic,
                Width = 160,
                Margin = new Thickness(5, 0, 5, 5),
                TextWrapping = TextWrapping.Wrap
            };
            stpaInformacjeOgolne.Children.Add(numerTelefonu);

            Button btnEdytuj = new Button()
            {
                Name = "btnEdytuj",
                Width = 50,
                Height = 50,
                Margin = new Thickness(5),
                FontFamily = new FontFamily("Segoe MDL2 Assets"),
                FontSize = 25,
                Content = "\xE70F",
                HorizontalAlignment = HorizontalAlignment.Right
            };
            btnEdytuj.Click += BtnEdytuj_ClickAsync;
            rezultat.Children.Add(btnEdytuj);

            Button btnUsun = new Button()
            {
                Name = "btnUsun",
                Width = 50,
                Height = 50,
                Margin = new Thickness(5),
                FontFamily = new FontFamily("Segoe MDL2 Assets"),
                FontSize = 25,
                Content = "\xE74D",
                HorizontalAlignment = HorizontalAlignment.Right
            };
            btnUsun.Click += BtnUsun_Click;
            rezultat.Children.Add(btnUsun);

            return rezultat;
        }

        private async void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog oknoPotwierdzeniaUsuniecia = new ContentDialog
            {
                Title = "Czy na pewno usunąć?",
                Content = $"Czy na pewno chcesz usunąć opiekuna o nazwie \"{nazwa}\" i numerze telefonu {numerTelefonu}?",
                PrimaryButtonText = "Tak",
                SecondaryButtonText = "Nie",
                IsSecondaryButtonEnabled = true
            };
            if (await oknoPotwierdzeniaUsuniecia.ShowAsync() == ContentDialogResult.Primary)
            {
                Konfiguracja.opiekunowie.Remove(this);
                KlasaPomocniczna.odswiezListeOpiekunow.Set();
            }
        }

        private async void BtnEdytuj_ClickAsync(object sender, RoutedEventArgs e)
        {
            Views.EdycjaOpiekuna oknoEdycjiOpiekuna = new Views.EdycjaOpiekuna(this);
            if (await oknoEdycjiOpiekuna.ShowAsync() == ContentDialogResult.Primary)
            {
                Opiekun zmodyfikowany = oknoEdycjiOpiekuna.zmodyfikowanyOpiekun;
                nazwa = zmodyfikowany.nazwa;
                numerTelefonu = zmodyfikowany.numerTelefonu;
                KlasaPomocniczna.odswiezListeOpiekunow.Set();
            }
        }

        public StackPanel GenerujStackPanelDoWyboru()
        {
            StackPanel rezultat = new StackPanel()
            {
                Name = "stpaOpiekun",
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = new SolidColorBrush(Colors.WhiteSmoke),
                Margin = new Thickness(5)
            };
            rezultat.Tapped += Rezultat_Tapped;

            StackPanel stpaInformacjeOgolne = new StackPanel()
            {
                Name = "stpaInformacjeOgolne"
            };
            rezultat.Children.Add(stpaInformacjeOgolne);

            TextBlock nazwa = new TextBlock
            {
                Name = "txtNazwaOpiekuna",
                Text = this.nazwa,
                Width = 290,
                Margin = new Thickness(5),
                TextWrapping = TextWrapping.Wrap
            };
            stpaInformacjeOgolne.Children.Add(nazwa);

            TextBlock numerTelefonu = new TextBlock()
            {
                Name = "txtNumerTelefonuOpiekuna",
                Text = this.numerTelefonu,
                FontStyle = Windows.UI.Text.FontStyle.Italic,
                Width = 290,
                Margin = new Thickness(5),
                TextWrapping = TextWrapping.Wrap
            };
            stpaInformacjeOgolne.Children.Add(numerTelefonu);

            CheckBox btnEdytuj = new CheckBox()
            {
                Name = "chkCzyWybrane",
                Width = 50,
                Height = 50,
                Margin = new Thickness(5),
                FontSize = 25,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            rezultat.Children.Add(btnEdytuj);

            return rezultat;
        }

        private void Rezultat_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            SolidColorBrush aktualnyKolor = (sender as StackPanel).Background as SolidColorBrush;
            if (aktualnyKolor.Color == Colors.Lime)
            {
                (sender as StackPanel).Background = new SolidColorBrush(Colors.WhiteSmoke);
                KlasaPomocniczna.opiekunowieWybraniDoPomocy.Remove(this);
            }
            else
            {
                (sender as StackPanel).Background = new SolidColorBrush(Colors.Lime);
                KlasaPomocniczna.opiekunowieWybraniDoPomocy.Add(this);
            }
        }
    }
}
