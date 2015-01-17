using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.PushNotifications;
using Microsoft.WindowsAzure.Messaging;

namespace Wiktionary
{
    public static class Notification
    {
        static string hubName = "wiktionaryhub";
        static string endPoint = "Endpoint=sb://wiktionary.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=sWROQRh/lq7vChsrKn/6Lfb3VqRU+dyGI0tmbsvhiCI=";
 
        static NotificationHub hub = new NotificationHub(hubName, endPoint);

        public static void AbonnementNotification()
        {
            // Execution du register en asynchrone
            Task.Run(async () =>
            {
                var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                await hub.RegisterNativeAsync(channel.Uri);
            });
        }

    }
}
