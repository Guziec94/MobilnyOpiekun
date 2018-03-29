using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using Windows.UI.Xaml.Controls;
using NotificationsExtensions.Toasts;
using NotificationsExtensions;
using Windows.Devices.Sensors;
using Windows.UI.Popups;
using Windows.Storage;
using Windows.Media.Playback;
using Windows.Media;
using System.IO;
using System.Threading;
using Windows.Devices.Sms;
using Windows.Devices.Geolocation;

namespace MobilnyOpiekun.Background
{
    public sealed class MojTrigger : IBackgroundTask
    {
        private AutoResetEvent autoResetEvent;

        private void wyswietlPowiadomienieLubWyslijSms(int ktorePowiadomienie = 1)
        {
            string trescPowiadomienia = "Dzień dobry, obudziłeś/aś się już? ";
            switch (ktorePowiadomienie)
            {
                case 1:
                    trescPowiadomienia += "Odpowiedz na to powiadomienie w ciągu 5 minut.";
                    break;
                case 2:
                    trescPowiadomienia += "Na odpowiedź masz tylko 3 minuty.";
                    break;
                case 3:
                    trescPowiadomienia += "Po 2 minutach zostanie wysłana wiadomość SMS do wszystkich opiekunów.";
                    break;
                case 4:
                    // UŻYTKOWNIK NIE BYŁ AKTYWNY I NIE ODPOWIEDZIAŁ NA POWIADOMIENIA, NALEŻY ROZESŁAĆ WIADOMOŚCI
                    SmsDevice2 smsDevice2 = SmsDevice2.GetDefault();
                    if(smsDevice2 != null)
                    {
                        KonfiguracjaBackground.AktualnaPozycja = null;
                        Task pobieranieLokalizacji = new Task(async () =>
                        {
                            Geolocator geolocator =  new Geolocator();
                            KonfiguracjaBackground.AktualnaPozycja = await geolocator.GetGeopositionAsync();
                        });
                        pobieranieLokalizacji.Start();
                        pobieranieLokalizacji.Wait(int.Parse(KonfiguracjaBackground.MaksymalnyCzasOczekiwaniaGps));
                        string trescWiadomosci = "Hej, potrzebuję Twojej pomocy. Skontaktuj się ze mną jak najszybciej. ";
                        if (KonfiguracjaBackground.AktualnaPozycja != null)
                        {
                            string szerokoscGeograficzna = KonfiguracjaBackground.AktualnaPozycja.Coordinate.Latitude.ToString().Replace(',', '.');// North/South
                            string dlugoscGeograficzna = KonfiguracjaBackground.AktualnaPozycja.Coordinate.Longitude.ToString().Replace(',', '.');// East/West
                            string link = $"http://bing.com/maps/default.aspx?rtp=~pos.{szerokoscGeograficzna}_{dlugoscGeograficzna}";
                            trescWiadomosci += "Moja lokalizacja:\n" + link;
                        }
                        trescWiadomosci += $"\n{KonfiguracjaBackground.Imie} {KonfiguracjaBackground.Nazwisko}";

                        foreach (var kontakt in KonfiguracjaBackground.Opiekunowie)
                        {
                            SmsTextMessage2 msg = new SmsTextMessage2();
                            msg.To = kontakt.NumerTelefonu;
                            msg.Body = trescWiadomosci;
                            SmsSendMessageResult result = smsDevice2.SendMessageAndGetResultAsync(msg).GetResults();
                        }
                    }
                    autoResetEvent.Set();
                    return;
            }

            var toastActions = new ToastActionsCustom()
            {
                Buttons =
                {
                    new ToastButton("OK, już wstałem/am.", "check")
                    {
                        ActivationType = ToastActivationType.Background
                    }
                }
            };
            var toastAudio = new ToastAudio()
            {
                Src = new Uri("ms-appx:///Assets/alarmSound.mp3")
            };
            var toastVisual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = "Pobudka!"
                        },
                        new AdaptiveText()
                        {
                            Text = trescPowiadomienia
                        }
                    }
                }
            };
            var toastContent = new ToastContent()
            {
                Actions = toastActions,
                Audio = toastAudio,
                Launch = "NIEMAMPOJECIA",//Chourouk
                Visual = toastVisual
            };

            ShowToast(toastContent, ktorePowiadomienie);
        }

        private void ShowToast(ToastContent content, int ktorePowiadomienie)
        {
            ToastNotification toast = new ToastNotification(content.GetXml());
            //toast.Dismissed += Toast_Dismissed;
            toast.Activated += Toast_Activated;
            toast.Priority = ToastNotificationPriority.High;

            var toastData = new NotificationData();
            toastData.Values.Add("numerPowiadomienia", ktorePowiadomienie.ToString());
            toast.Data = toastData;
            
            switch (ktorePowiadomienie)
            {
                case 1:
                    toast.ExpirationTime = DateTime.Now.AddMinutes(5);
                    break;
                case 2:
                    toast.ExpirationTime = DateTime.Now.AddMinutes(3);
                    break;
                case 3:
                    toast.ExpirationTime = DateTime.Now.AddMinutes(2);
                    break;
            }
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private void Toast_Activated(ToastNotification sender, object args)
        {
            if((args as ToastActivatedEventArgs).Arguments == "check")
            {
                autoResetEvent.Set();
            }
        }

        //private void Toast_Dismissed(ToastNotification sender, ToastDismissedEventArgs args)
        //{
        //    if(sender.ExpirationTime.HasValue && sender.ExpirationTime.Value <= DateTime.Now)
        //    {
        //        var numerPowiadomieniaString = sender.Data.Values["numerPowiadomienia"];
        //        int numerPowiadomienia = int.Parse(numerPowiadomieniaString);
        //    }
        //}

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            try
            {
                bool czyAgentWykonujePrace = bool.Parse(ApplicationData.Current.LocalSettings.Values["czyAgentWykonujePrace"].ToString() ?? "false");
                if (czyAgentWykonujePrace)
                {
                    return;
                }
                ApplicationData.Current.LocalSettings.Values["czyAgentWykonujePrace"] = true;
                var sensor = ActivitySensor.GetDefaultAsync();
                if (sensor == null)
                {
                    MessageDialog dialog = new MessageDialog("Nie udało się uzyskać dostępu do sensora.");
                }
                else
                {
                    KonfiguracjaBackground.WczytajKonfiguracje();

                    // Czas pobudki w tym przypadku zostaje uzyty jako okres czasu 
                    TimeSpan koniec = TimeSpan.Parse(KonfiguracjaBackground.PoczatekAktywnosci);
                    // Sprawdzamy 2h przed czasem pobudki
                    DateTimeOffset poczatek = new DateTimeOffset(DateTime.Today.ToLocalTime() + koniec - new TimeSpan(2, 0, 0));

                    var odczyty = await ActivitySensor.GetSystemHistoryAsync(poczatek, koniec);

                    int odczytyNiepewne = 0;
                    foreach (var odczyt in odczyty)
                    {
                        if (odczyt.Activity != ActivityType.Idle && odczyt.Activity != ActivityType.Stationary && odczyt.Activity != ActivityType.Unknown && odczyt.Activity != ActivityType.InVehicle)
                        {
                            if (odczyt.Confidence == ActivitySensorReadingConfidence.High)
                            {
                                return;
                            }
                            else
                            {
                                odczytyNiepewne++;
                                if (odczytyNiepewne > 3)
                                {
                                    return;
                                }
                            }
                        }
                    }
                    wyswietlPowiadomienieLubWyslijSms();
                    autoResetEvent = new AutoResetEvent(false);
                    autoResetEvent.WaitOne();
                    ApplicationData.Current.LocalSettings.Values["czyAgentWykonujePrace"] = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationData.Current.LocalSettings.Values["czyAgentWykonujePrace"] = false;
            }
        }
    }
}