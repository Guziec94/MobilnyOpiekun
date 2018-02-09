using System;
using Windows.UI;
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
            guid = new Guid();
        }
        public Opiekun(string nazwa, string numerTelefonu)
        {
            guid = new Guid();
            this.nazwa = nazwa;
            this.numerTelefonu = numerTelefonu;
        }

        public string OpiekunToString()
        {
            return $"{nazwa};{numerTelefonu}";
        }

        public Opiekun(string opiekunString)
        { 
            var parametry = opiekunString.Split(';');
            guid = new Guid();
            nazwa = parametry[0];
            numerTelefonu = parametry[1];
        }

        public void AktualizujOpiekuna(string nazwa, string numerTelefonu)
        {
            this.nazwa = numerTelefonu;
            this.numerTelefonu = numerTelefonu;
        }

        public StackPanel GenerujStackPanel()
        {
            StackPanel rezultat = new StackPanel()
            {
                Name = "stpaOpiekun",
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch,
                Background = new SolidColorBrush(Colors.Red)
            };

            StackPanel stpaInformacjeOgolne = new StackPanel()
            {
                Name = "stpaInformacjeOgolne"
            };
            rezultat.Children.Add(stpaInformacjeOgolne);

            TextBlock nazwa = new TextBlock
            {
                Name = "txtNazwaOpiekuna",
                Text = this.nazwa
            };
            stpaInformacjeOgolne.Children.Add(nazwa);

            TextBlock numerTelefonu = new TextBlock()
            {
                Name = "txtNumerTelefonuOpiekuna",
                Text = this.numerTelefonu,
                Width = 200
            };
            stpaInformacjeOgolne.Children.Add(numerTelefonu);

            Button btnEdytuj = new Button()
            {
                Name = "btnEdytuj",
                Width = 50,
                Height = 50,
                Margin = new Windows.UI.Xaml.Thickness(5,0,5,0),
                FontFamily = new FontFamily("Segoe MDL2 Assets"),
                Content = "\xE70F"
            };
            rezultat.Children.Add(btnEdytuj);

            Button btnUsun = new Button()
            {
                Name = "btnUsun",
                Width = 50,
                Height = 50,
                Margin = new Windows.UI.Xaml.Thickness(0),
                FontFamily = new FontFamily("Segoe MDL2 Assets"),
                Content = "\xE74D"
            };
            rezultat.Children.Add(btnUsun);

            return rezultat;
        }
    }
}
