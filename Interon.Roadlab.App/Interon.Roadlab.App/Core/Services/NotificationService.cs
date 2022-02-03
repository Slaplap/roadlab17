using Interon.Roadlab.App.Core.Models;
using Interon.Roadlab.Core.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Interon.Roadlab.App.Core.Services
{
    internal class NotificationService : BaseService
    {
        private SQLiteConnection _db;

        public NotificationService()
        {
            _db = GetConnection();
            this._db.CreateTable<Notification>();

        }

        public  Notification DtoToNotification(NotificationJsonDto notificationJsonDto)
        {
            try
            {



                Notification notificaitons = new Notification()
                {
                    CreateDate = notificationJsonDto.CreateDate,
                    Key = notificationJsonDto.Key,
                    Message = notificationJsonDto.Message,
                    ReadDate = notificationJsonDto.ReadDate,
                    SyncDate = notificationJsonDto.SyncDate,
                    Title = notificationJsonDto.Title,
                    Data = notificationJsonDto.Data,
                    Link = notificationJsonDto.Link,
                    ForeignKey = notificationJsonDto.ForeignKey,
                    MemberId = notificationJsonDto.MemberId,
                    NotifiacionType = notificationJsonDto.NotificationType,
                    PushOutcome = notificationJsonDto.PushOutcome,
                    UpdateDate = notificationJsonDto.UpdateDate 

                };


                return notificaitons;
            }
            catch (Exception e)
            {
                Debugger.Break();
                return null;
            }
        }

        public  NotificationJsonDto NotificationToDto(Notification notifications)
        {

            try
            {
                var notificationDto = new NotificationJsonDto()
                {
                    CreateDate = notifications.CreateDate,
                    Data = notifications.Data,
                    Key = notifications.Key,
                    Title = notifications.Title,
                    SyncDate = notifications.SyncDate,
                    Message = notifications.Message,
                    ReadDate = notifications.ReadDate,
                    Link = notifications.Link,
                    ForeignKey = notifications.ForeignKey,
                    MemberId = notifications.MemberId,
                    NotificationType = notifications.NotifiacionType,
                    PushOutcome = notifications.PushOutcome,
                    UpdateDate = notifications.UpdateDate

                };
                return notificationDto;
            }
            catch (Exception e)
            {
                Debugger.Break();
                return null;
            }
        }

        public bool IsNotificationSavedOnLocalDevice(Guid Key)
        {
            return _db.Query<Branch>($"SELECT * FROM Notifications where Key = '{Key}' ").Any();
        }

        

        public List<Notification> GetAllNotifications()
        {
            var Notifications = _db.Query<Notification>("SELECT * FROM Notifications");
            return Notifications;
        }


        public Notification GetNotificationByKey(Guid key)
        {
            var Notifications = _db.Query<Notification>($"SELECT * FROM Notifications where Key='{key}'");
            return Notifications.FirstOrDefault();
        }

        
        


        // only set change date to true if the record is being created from user input
        public bool CreateOrUpdateNotification(Notification Notification, bool ChangeDate, bool IsStartOfTheSyncProcess)
        {
            var datetimenow = DateTime.Now;
            try
            {
                _db.BeginTransaction();
                if (Notification.Key != Guid.Empty && IsNotificationSavedOnLocalDevice(Notification.Key))
                {
                     

                    if (IsStartOfTheSyncProcess)
                    {
                        Notification.SyncDate = datetimenow;
                    }
                    _db.Update(Notification);
                }
                else
                {
                    if (Notification.Key == Guid.Empty)
                    {
                        Notification.Key = Guid.NewGuid();
                    }
                    if (IsStartOfTheSyncProcess)
                    {
                        Notification.SyncDate = datetimenow;
                    }
                    if (ChangeDate)
                    {
                        Notification.CreateDate = datetimenow;

                       
                    }

                    _db.Insert(Notification);
                }

                _db.Commit();

            }
            catch (SQLite.SQLiteException se)
            {
                Debugger.Break();
                _db.Rollback();
                ErrorService.ErrorExceptionAndAnalytics("Error inserting Notification", "CreateOrUpdateNotification()", se);
                return false;
            }
            catch (Exception ex)
            {
                Debugger.Break();
                ErrorService.ErrorExceptionAndAnalytics("Error inserting Notification", "CreateOrUpdateNotification()", ex);
                return false;
            }

            return true;
        }

        public void DeleteNotifications()
        {
            _db.Execute("Delete from Notifications where 1=1");
          
        }

        public void DeleteNotification(Notification Notification)
        {
            

            _db.Delete<Notification>(Notification);
        }

        
        public List<Notification> GetAllNewLocalNotifications()
        {
            //TODO delete 
            //    Debugger.Break();//check dates here 
            var synUp = GetAllNotifications().Where(x => x.SyncDate > SecureStorageService.SyncNotificationsDateTime).ToList();
            return synUp.ToList();
        }

        public List<Notification> GetAllByType(string type)
        {
            List<Notification> Notifications = new List<Notification>();
            var allNotifications = GetAllNotifications();
            foreach (var Notification in allNotifications.OrderByDescending(x => x.SyncDate))
            {
                foreach (var s in type.Split(','))
                {
                    if (Notification.NotifiacionType.Contains(s))
                    {
                        Notifications.Add(GetNotificationByKey(Notification.Key));
                    }
                }
            }

            return Notifications;
        }

        public void CreateOrUpdateNotificationListFromDtoList(List<NotificationJsonDto> NotificationDtos, bool ChangeDate, bool IsStartOfTheSyncProcess)
        {
            foreach (var NotificationDto in NotificationDtos)
            {
                CreateOrUpdateNotification(DtoToNotification(NotificationDto), ChangeDate, IsStartOfTheSyncProcess);
            }
        }

     

        
    }
}