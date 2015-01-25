using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.PushNotifications;
using Microsoft.WindowsAzure.Messaging;

namespace Wiktionary
{
    public static class Notification
    {
        private static string _mot;
        public static string Mot
        {
            get { return _mot; }
            set
            {
                _mot = value;
                OnPropertyChanged("Mot");
            }
        }

        private static string hubName = "wiktionaryhub";
        private static string endPoint = "Endpoint=sb://wiktionary.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=sWROQRh/lq7vChsrKn/6Lfb3VqRU+dyGI0tmbsvhiCI=";

        static NotificationHub hub = new NotificationHub(hubName, endPoint);

        private static void Initialize()
        {
            if (String.IsNullOrEmpty(Mot))
                Mot = "";
        }

        public static void AbonnementNotification()
        {
            Initialize();

            // Execution du register en asynchrone
            Task.Run(async () =>
            {
                var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                await hub.RegisterNativeAsync(channel.Uri);
            });
        }

        public static event PropertyChangedEventHandler GlobalPropertyChanged = delegate { };
        static void OnPropertyChanged(string propertyName)
        {
            GlobalPropertyChanged(
                typeof(Notification),
                new PropertyChangedEventArgs(propertyName));
        }

    }
}
