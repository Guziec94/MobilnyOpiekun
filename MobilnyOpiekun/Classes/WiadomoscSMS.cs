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
        private static SmsDevice2 _SmsDevice2;
        public static bool czyZainicjalizowane;
        public static bool InicjalizujSMS()
        {
            // If this is the first request, get the default SMS device.
            // If this is the first SMS device access, the user will be prompted to grant access permission for this application.
            try
            {
                //rootPage.NotifyUser("Getting default SMS device ...", NotifyType.StatusMessage);
                _SmsDevice2 = SmsDevice2.GetDefault();
            }
            catch (Exception ex)
            {
                //rootPage.NotifyUser(ex.Message, NotifyType.ErrorMessage);
            }
            return czyZainicjalizowane = (_SmsDevice2 != null);
        }

        public static void WyslijSMS(string numerOdbiorcy, string tekstWiadomosci)
        {
            if (numerOdbiorcy == "")
            {
                //rootPage.NotifyUser("Please enter sender number", NotifyType.ErrorMessage);
                return;
            }

            if (tekstWiadomosci == "")
            {
                //rootPage.NotifyUser("Please enter message", NotifyType.ErrorMessage);
                return;
            }

            string messageSendResult = "";
            try
            {
                // Create a text message - set the entered destination number and message text.
                SmsTextMessage2 msg = new SmsTextMessage2();
                msg.To = numerOdbiorcy;
                msg.Body = tekstWiadomosci;

                // Send the message asynchronously
                //rootPage.NotifyUser("Sending message ...", NotifyType.StatusMessage);
                SmsSendMessageResult result = _SmsDevice2.SendMessageAndGetResultAsync(msg).GetResults();

                if (result.IsSuccessful)
                {
                    messageSendResult += "Text message sent, cellularClass: " + result.CellularClass.ToString();
                    IReadOnlyList<Int32> messageReferenceNumbers = result.MessageReferenceNumbers;

                    for (int i = 0; i < messageReferenceNumbers.Count; i++)
                    {
                        messageSendResult += "\n\t\tMessageReferenceNumber[" + i.ToString() + "]: " + messageReferenceNumbers[i].ToString();
                    }
                    //rootPage.NotifyUser(messageSendResult, NotifyType.StatusMessage);
                }
                else
                {
                    messageSendResult += "ModemErrorCode: " + result.ModemErrorCode.ToString();
                    messageSendResult += "\nIsErrorTransient: " + result.IsErrorTransient.ToString();
                    if (result.ModemErrorCode == SmsModemErrorCode.MessagingNetworkError)
                    {
                        messageSendResult += "\n\tNetworkCauseCode: " + result.NetworkCauseCode.ToString();

                        if (result.CellularClass == CellularClass.Cdma)
                        {
                            messageSendResult += "\n\tTransportFailureCause: " + result.TransportFailureCause.ToString();
                        }
                        //rootPage.NotifyUser(messageSendResult, NotifyType.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                //rootPage.NotifyUser(ex.Message, NotifyType.ErrorMessage);
            }
        }
    }
}
