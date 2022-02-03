using System.Collections.Generic;

namespace Interon.Roadlab.App.Core.Services
{
  public static   class NotificationHubService
    {
        public static void RegisterUserWithNotificationHub()
        {
            List<string> tags = new List<string>();
            tags.Add(AppConstants.SubscriptionTags[0]);
            tags.Add(SecureStorageService.Username);
           
            if (MemberService.IsRegisteredOnLocalDevice())
            {
                tags.Add(MemberService.GetMember().CellNo);
            }

            DependencyServiceService.UpdateNotificationHubTags(SecureStorageService.NotificationHubToken, tags);
        }
    }
}
