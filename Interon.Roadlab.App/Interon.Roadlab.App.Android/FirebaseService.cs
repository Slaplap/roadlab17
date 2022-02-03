using System;
using System.Diagnostics;
using System.Linq;
using WindowsAzure.Messaging;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Firebase.Messaging;

namespace Interon.Roadlab.App.Droid
{
    [Service(Name = "com.Interon.Roadlab.App.FirebaseService")]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    [Android.Runtime.Preserve(AllMembers = true)]
    public class FirebaseService : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            string messageBody = string.Empty;

            if (message.GetNotification() != null)
            {
                messageBody = message.GetNotification().Body;
            }

            // NOTE: test messages sent via the Azure portal will be received here
            else
            {
                messageBody = message.Data.Values.First();
            }

            // convert the incoming message to a local notification
            SendLocalNotification(messageBody);

            // send the incoming message directly to the MainPage
            SendMessageToMainPage(messageBody);
        }

        public override void OnNewToken(string token)
        {
            // TODO: save token instance locally, or log if desired


            try
            {
                Interon.Roadlab.App.Core.Services.SecureStorageService.NotificationHubToken = token;
                Interon.Roadlab.App.Core.Globals.NotificationHubToken                       = token;
                SendRegistrationToServer(token);
              
            }
            catch (Exception e)
            {
                Debugger.Break();
                throw;
            }
        }

        void SendLocalNotification(string body)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.PutExtra("message", body);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.UpdateCurrent);

            var notificationBuilder = new NotificationCompat.Builder(this, AppConstants.NotificationChannelName)
                .SetContentTitle("Roadlab Notification Message")
                .SetSmallIcon(Resource.Drawable.icon)
                .SetContentText(body)
                .SetAutoCancel(true)
                .SetShowWhen(false)
                .SetContentIntent(pendingIntent);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                notificationBuilder.SetChannelId(AppConstants.NotificationChannelName);
            }

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(0, notificationBuilder.Build());
        }

        void SendMessageToMainPage(string body)
        {
           // (App.Current.MainPage as MainPage)?.AddMessage(body);
        }

        void SendRegistrationToServer(string token)
        {
            try
            {
                NotificationHub hub = new NotificationHub(AppConstants.NotificationHubName, AppConstants.ListenConnectionString, this);

                // register device with Azure Notification Hub using the token from FCM
                Registration registration = hub.Register(token, AppConstants.SubscriptionTags);
                

                // subscribe to the SubscriptionTags list with a simple template.
                string pnsHandle = registration.PNSHandle;
                TemplateRegistration templateReg = hub.RegisterTemplate(pnsHandle, "defaultTemplate", AppConstants.FCMTemplateBody, AppConstants.SubscriptionTags);
            }
            catch (Exception e)
            {
                Log.Error(AppConstants.DebugTag, $"Error registering device: {e.Message}");
            }
        }
    }
}