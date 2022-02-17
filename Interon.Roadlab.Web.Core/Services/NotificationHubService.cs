using System.Net;
using System.Threading.Tasks;
using Interon.Roadlab.Web.Core.Models;
using Microsoft.Azure.NotificationHubs;

namespace Interon.Roadlab.Web.Core.Services
{
    public class NotificationHubService
    {
        private string _token;

        public NotificationHubService()
        {
            _token = "AAAArFvBu1k:APA91bHOhlSCfNuTZWw9_ZgNDg_Ea6v4jERO_uYhEMOO5fAdHtyOseyO4J3f16livbvIsiiEfg5KiMCzE_wbL2ycNuksHDQDQU7S63_7p2Yg6kDj8Y0ATk6rSX-CRx8UMf-P5331IgsO";


        }

        public static NotificationHubService Instance { get; set; } = new NotificationHubService();

        public async Task <NotificationOutcome> SendAndroidNotification( string to_tag, string message, string pns,string user = "Roadlab", string hubName = "RoadlabNotificationHub")
        {
            
            var userTag = new string[]
            {
                to_tag
                
            };

            Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;
            var ret = HttpStatusCode.InternalServerError;

            switch (pns.ToLower())
            {
                case "wns":
                    // Windows 8.1 / Windows Phone 8.1
                    var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">" +
                                "From " + user + ": " + message + "</text></binding></visual></toast>";
                    outcome = await NotificationHubModel.Instance.Hub.SendWindowsNativeNotificationAsync(toast, userTag);
                    break;
                case "apns":
                    // iOS
                    var alert = "{\"aps\":{\"alert\":\"" + "From " + user + ": " + message + "\"}}";
                    outcome = await NotificationHubModel.Instance.Hub.SendAppleNativeNotificationAsync(alert, userTag);
                    break;
                case "fcm":
                    // Android
                    var notif = "{ \"data\" : {\"message\":\"" + "From " + user + ": " + message + "\"}}";
                    outcome = await NotificationHubModel.Instance.Hub.SendFcmNativeNotificationAsync(notif, userTag);
                    break;
            }

            if (outcome != null)
            {
                return outcome;
            }

            return null;
        }
    }
}