using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Interon.Roadlab.Core.Dto;


namespace Interon.Roadlab.App.Core.Services
{
    public static class NotificationsOnlineService
    {


        public static async Task<DateTime> GetLatestNotificationSyncDateByMemberId(int memberId)
        {
            try
            {
                var fUrl = new Url(Interon.Roadlab.Core.Environment.CMSAPI_NOTIFICATIONS_URL_ENDPOINT +   "/GetNotificationsSyncDateByMemberId?memberId=" + memberId.ToString());

                var result = await fUrl.WithOAuthBearerToken(SecureStorageService.RefreshToken).WithTimeout(Globals.Timeout).GetJsonAsync<DateTime>().ConfigureAwait(true);

                return result;
            }
            catch (FlurlHttpTimeoutException te)
            {
              
                ErrorService.ErrorExceptionAndAnalytics("Timeout Getting Notifications", "GetNotificationsSyncDateByMemberId()", te);
            }
            catch (Exception err)
            {
                Debugger.Break();
                ErrorService.ErrorExceptionAndAnalytics("Error Getting Notifications", "GetNotificationsSyncDateByMemberId()", err);
                return new DateTime();
            }
            return new DateTime();
        }
        public static async Task<List<Roadlab.Core.Models.NotificationJsonDto>>  GetNewNotificationsByMemberId(int memberId, DateTime syncDateTime)
        {
            try
            {
                var fUrl = new Url(Interon.Roadlab.Core.Environment.CMSAPI_NOTIFICATIONS_URL_ENDPOINT + $"/GetNewNotificationsByMemberId?memberId={memberId}&syncDateTime={syncDateTime}");
                List<Roadlab.Core.Models.NotificationJsonDto> Notifications = new List<Roadlab.Core.Models.NotificationJsonDto>();
                var receiveJsonList = await fUrl.WithOAuthBearerToken(SecureStorageService.RefreshToken)
                    .WithTimeout(10).GetJsonAsync<NotificationsDto>().ConfigureAwait(true);
                return receiveJsonList.NotificationJsonDtos;
            }
            catch (FlurlHttpTimeoutException te)
            {
              
                ErrorService.ErrorExceptionAndAnalytics("Timeout Getting Notifications", "GetNewNotificationsByMemberId()", te);
            }
            catch (Exception err)
            {
                Debugger.Break();
                ErrorService.ErrorExceptionAndAnalytics("Error Getting Notifications", "GetNewNotificationsByMemberId()", err);
                return new List<Roadlab.Core.Models.NotificationJsonDto>();
            }
            return new List<Roadlab.Core.Models.NotificationJsonDto>();

        }


        public static async Task<bool> SetNotificationToReadAsync(int notificationId)
        {
            try
            {
                var fUrl = new Url(Interon.Roadlab.Core.Environment.CMSAPI_NOTIFICATIONS_URL_ENDPOINT + $"/SetNotificationToRead{notificationId}");
                List<Roadlab.Core.Models.NotificationJsonDto> Orders = new List<Roadlab.Core.Models.NotificationJsonDto>();
                var receiveOrderDto = await fUrl.WithOAuthBearerToken(SecureStorageService.RefreshToken)
                    .WithTimeout(Globals.Timeout).PostJsonAsync(notificationId).ReceiveJson<bool>().ConfigureAwait(true);


                return receiveOrderDto;
            }
            catch (FlurlHttpTimeoutException te)
            {
                ErrorService.ErrorExceptionAndAnalytics("Timeout Setting Notifiation", "SetNotificationToReadAsync()", te);
            }
            catch (Exception err)
            {
                ErrorService.ErrorExceptionAndAnalytics("Error Setting Notification", "SetNotificationToReadAsync()", err);
                return false;
            }

            return false;
        }
        public static async Task<Roadlab.Core.Models.NotificationJsonDto> SyncLocalNotificationsToServerAsync(Roadlab.Core.Models.NotificationJsonDto NotificationJson)
        {
            try
            {
                var fUrl = new Url(Interon.Roadlab.Core.Environment.CMSAPI_NOTIFICATIONS_URL_ENDPOINT + "/SyncLocalNotificationsToServerAsync");
                List<Roadlab.Core.Models.NotificationJsonDto> Orders = new List<Roadlab.Core.Models.NotificationJsonDto>();
                var receiveNotificationDto = await fUrl.WithOAuthBearerToken(SecureStorageService.RefreshToken)
                    .WithTimeout(Globals.Timeout).PostJsonAsync(NotificationJson).ReceiveJson<Roadlab.Core.Models.NotificationJsonDto>().ConfigureAwait(true);

                return receiveNotificationDto;
            }
            catch (FlurlHttpTimeoutException te)
            {
                 ErrorService.ErrorExceptionAndAnalytics("Timeout Getting Notifications", "SyncLocalNotificationsToServerAsync()", te);
            }
            catch (Exception err)
            {
                Debugger.Break();
                ErrorService.ErrorExceptionAndAnalytics("Error Getting Notifiations", "SyncLocalNotificationsToServerAsync()", err);
                return new Roadlab.Core.Models.NotificationJsonDto();
            }
            return new Roadlab.Core.Models.NotificationJsonDto();
        }

    }
}
