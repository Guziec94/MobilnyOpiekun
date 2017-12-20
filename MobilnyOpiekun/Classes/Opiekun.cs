using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MobilnyOpiekun.Classes
{
    public class Opiekun
    {
        public string nazwa;
        public string numerTelefonu;
        public Opiekun() { }
        public Opiekun(string nazwa, string numerTelefonu)
        {
            this.nazwa = nazwa;
            this.numerTelefonu = numerTelefonu;
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
                Content = "\xE74D",
            };
            rezultat.Children.Add(btnUsun);

            return rezultat;
        }
    }
}
