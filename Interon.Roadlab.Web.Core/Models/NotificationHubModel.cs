using Microsoft.Azure.NotificationHubs;

namespace Interon.Roadlab.Web.Core.Models
{
    public class NotificationHubModel
    {
        public static NotificationHubModel Instance = new NotificationHubModel();

        public NotificationHubClient Hub { get; set; }

        private NotificationHubModel()
        {
            // Please update the following connection string with your hub's DefaultFullSharedAccessSignature with the full access such as Listen, Manage, Send
            Hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://roadlab.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=QsajhGFJT80AxN9jDTTBtOMMHQRYy2hVF8qXgH2FvKg=",
                "RoadlabNotificationHub");
        }
    }
}