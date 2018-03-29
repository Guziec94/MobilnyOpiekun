using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sms;

namespace MobilnyOpiekun.Classes
{
    public static class WiadomoscSMS
    {
        private static SmsDevice2 smsDevice2;
        public static bool czyZainicjalizowane;

        public static bool InicjalizujSMS()
        {
            try
            {
                smsDevice2 = SmsDevice2.GetDefault();
            }
            catch (Exception ex)
            {

            }
            return czyZainicjalizowane = (smsDevice2 != null);
        }

        public static bool WyslijSMS(string numerOdbiorcy, string tekstWiadomosci)
        {
            try
            {
                if (numerOdbiorcy == "")
                {
                    throw new Exception("Numer odbiorcy wiadomości był pusty.");
                }

                if (tekstWiadomosci == "")
                {
                    throw new Exception("Treść wiadomości była pusta.");
                }

                // Create a text message - set the entered destination number and message text.
                SmsTextMessage2 msg = new SmsTextMessage2();
                msg.To = numerOdbiorcy;
                msg.Body = tekstWiadomosci;

                // Send the message asynchronously
                //rootPage.NotifyUser("Sending message ...", NotifyType.StatusMessage);
                SmsSendMessageResult result = smsDevice2.SendMessageAndGetResultAsync(msg).GetResults();

                if (result.IsSuccessful)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
