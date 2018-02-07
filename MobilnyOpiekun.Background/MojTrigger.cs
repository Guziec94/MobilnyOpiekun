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

namespace MobilnyOpiekun.Background
{
    public sealed class MojTrigger: IBackgroundTask
    {
        private void Show(ToastContent content)
        {
            ToastNotification toast = new ToastNotification(content.GetXml());
            toast.ExpirationTime = DateTime.Now.AddHours(3);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            try
            {
                Show(new ToastContent()
                {
                    Visual = new ToastVisual()
                    {
                        BindingGeneric = new ToastBindingGeneric()
                        {
                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = "Title text"
                                },

                                new AdaptiveText()
                                {
                                    Text = "Content text"
                                }

                                //new AdaptiveImage()
                                //{
                                //    Source = image
                                //}
                            }

                            //AppLogoOverride = new ToastGenericAppLogo()
                            //{
                            //    Source = logo,
                            //    HintCrop = ToastGenericAppLogoCrop.Circle
                            //}
                        }
                    },

                    Launch = "Chourouk",

                    Actions = new ToastActionsCustom()
                    {
                        //Inputs =
                        //{
                        //    new ToastTextBox("tbQuickReply")
                        //    {
                        //        PlaceholderContent = ""
                        //    }
                        //},

                        Buttons =
                        {
                            new ToastButton("Ok content", "true")
                            {
                                //ImageUri = "Assets/next.png",
                                //TextBoxId = "tbQuickReply",
                                ActivationType = ToastActivationType.Background
                            },
                            new ToastButton("Cancel content", "false")
                            {
                                //ImageUri = "Assets/next.png",
                                //TextBoxId = "tbQuickReply",
                                ActivationType = ToastActivationType.Background
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {

            }
        }
    }
}
