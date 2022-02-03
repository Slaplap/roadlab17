using Interon.Roadlab.App.Core.Services;
using Matcha.BackgroundService;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Interon.Roadlab.App.Core.Const;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core.Tasks
{
    public class PeriodicNotifications : IPeriodicTask
    {
        private DateTime SYNCDATETIME;
        public TimeSpan Interval { get; set; }

        public PeriodicNotifications(int seconds)
        {
            SYNCDATETIME = SecureStorageService.SyncNotificationsDateTime;
            Interval = TimeSpan.FromSeconds(seconds);
        }

        public PeriodicNotifications()
        {
        }
        public async Task<bool> StartJob()
        {

            if (Globals.StoppedBackgroundServices.Contains(this.GetType().Name) || Globals.RunningServices.Contains(this.GetType().Name))
            {
                return true;
            }
            

            if (!SecureStorageService.HasAllLocalLoginCredentials())
            {
                return true;
            }

            var connectionToServer =  OnlineService.HasConnectionToServerAsync();
            if ( !connectionToServer)
            {
                return true;
            }

            try
            {
                Globals.RunningServices.Add(this.GetType().Name);
                var datetimeNow = DateTime.Now;
                 
            
                await SendNewLocalNotificationsToServer().ConfigureAwait(true);
                await GetAllNewNotificationsFromServer().ConfigureAwait(true);
                Globals.PeriodicNotificationsInterval = 10;
                return true;
            }
            finally
            {
                Globals.RunningServices.RemoveAll(x => x == this.GetType().Name);
            }
        }


        public  async Task<bool> SendNewLocalNotificationsToServer()
        {
            try
            {
                NotificationService NotificationService = new NotificationService();
                if (!NotificationService.GetAllNewLocalNotifications().Any())
                {
                    return false;
                }

                if (!OnlineService.HasInternet())
                {
                    return false;
                }

                var connectionToServerAsync =  OnlineService.HasConnectionToServerAsync();
                if (!connectionToServerAsync)
                {
                    return false;
                }

                var upsyncNotifications = NotificationService.GetAllNewLocalNotifications();
                foreach (var Notification in upsyncNotifications)
                {
                    var _Notification = NotificationService.GetNotificationByKey(Notification.Key);
                    var notificationDto = NotificationService.NotificationToDto(_Notification);
                    var savedOrder = await NotificationsOnlineService
                        .SyncLocalNotificationsToServerAsync(notificationDto).ConfigureAwait(true);
                    if (savedOrder.Key == Guid.Empty)
                    {
                        ErrorService.EventAndAnalytics("SyncUpFailed :" + Notification.Key, AnalyticsService.EventCategory.Error);
                        return true;
                    }

               
                }

           
               
               

                return true;
            }
            catch (Exception e)
            {
                ErrorService.ErrorExceptionAndAnalytics("ER004", "SendNewLocalNotificationsToServer()", e);
                return false;
            }

            return false;
        }


        private void DebugMethod()
        {
            //Debug class
            SecureStorageService.SyncNotificationsDateTime = new DateTime();
            NotificationService NotificationService = new NotificationService();
            var allNotifications = NotificationService.GetAllNotifications();
            var allNewNotifications = NotificationService.GetAllNewLocalNotifications();
            Debugger.Break();
        }

        private async Task GetAllNewNotificationsFromServer()
        {

            try
            {
                NotificationService NotificationService = new NotificationService();

                var ServerUpdateDate = await NotificationsOnlineService.GetLatestNotificationSyncDateByMemberId((SecureStorageService.MemberId)).ConfigureAwait(true);
                var localUpdateDate = SecureStorageService.SyncNotificationsDateTime;

                if (localUpdateDate < ServerUpdateDate)
                {
                    var newNotificationsByCompanyId = await NotificationsOnlineService.GetNewNotificationsByMemberId(SecureStorageService.MemberId, localUpdateDate).ConfigureAwait(true);
                    if (newNotificationsByCompanyId.Count == 0)
                    {
                        return;
                    }
                    NotificationService.CreateOrUpdateNotificationListFromDtoList(newNotificationsByCompanyId, false, false);
                    //THE SYNC DATE  MUST ONLY BE SET FROM THE SERVER LAST UPDATE DATE 
                    SecureStorageService.SyncNotificationsDateTime = newNotificationsByCompanyId.Max(x => x.SyncDate);
                    MessagingCenter.Send<object>(new Object(),MessagingCenterValues.NotificationsChange);

                }

            }
            catch (Exception e)
            {
                 
                ErrorService.ErrorExceptionAndAnalytics("ER005", "GetAllNewNotificationsFromServer()", e);
            }
        }
          private bool StopThisTask()
        {
            Globals.StoppedBackgroundServices.Add(this.GetType().Name);
            return false;
        }
       
    }
}