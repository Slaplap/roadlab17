using Interon.Roadlab.Core.Dto;
using Interon.Roadlab.Web.App.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using Interon.Roadlab.Core.Models;
using Interon.Roadlab.Web.App.Models;
using Umbraco.Web.WebApi;
using Member = Umbraco.Web.PublishedModels.Member;

namespace Interon.Roadlab.Web.App.API
{
    public class NotificationsFrontendApiController : UmbracoApiController
    {
        private NotificationService _notificationService;

        public NotificationsFrontendApiController(NotificationService notificationService)
        {
            this._notificationService = notificationService;
        }

        public NotificationsFrontendApiController()
        {
        }

        [System.Web.Mvc.HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public bool Ping()
        {
            return true;
        }

        [System.Web.Mvc.HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public DateTime GetNotificationsSyncDateByMemberId(int memberId)
        {
            try
            {

                var notifications = GetAllNotifications().NotificationJsonDtos.Where(x => x.MemberId == memberId).Max(x => x.SyncDate);
                return notifications;
            }
            catch(Exception e)
            {
                return new DateTime();
            }
        }

        [System.Web.Mvc.HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public  NotificationsDto  GetNotificationByKey(Guid Key)
        {
            var notificationsDto = new NotificationsDto();
            var notification = _notificationService.GetNotificationsByKey(Key);
            var notificationJsonDto =  _notificationService.NotificationToDto( notification);
            notificationsDto.NotificationJsonDtos.Add(notificationJsonDto);
            return notificationsDto;
        }

        [System.Web.Mvc.HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public NotificationsDto GetAllNotifications()
        {
            var notificationsDto = new NotificationsDto();
          
             
            var allNotifications = _notificationService.GetAllNotifications();
            foreach (var notification in allNotifications)
            {
                notificationsDto.NotificationJsonDtos.Add(_notificationService.NotificationToDto(notification));
            }

            return notificationsDto;
        }

        [System.Web.Mvc.HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public  NotificationsDto GetNotificationsByMemberId(int memberId,bool all= false)
        {
            var notificationsDto = new NotificationsDto();
            var notifications = _notificationService.GetNotificationsByMemberId(memberId);
            foreach (var notification in notifications)
            {
                if (!all)
                {
                    if (notification.ReadDate.Value.Year < 1)
                    {
                        continue;
                    }
                }
                notificationsDto.NotificationJsonDtos.Add(_notificationService.NotificationToDto(notification));
            }

            return notificationsDto;
        }

        [System.Web.Mvc.HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public NotificationsDto GetNewNotificationsByMemberId(int memberId,DateTime syncDateTime )
        {
            var notificationsDto = new NotificationsDto();
            var notifications    = _notificationService.GetNotificationsByMemberId(memberId);
            foreach (var notification in notifications.Where(x=>x.SyncDate > syncDateTime))
            {
                
                notificationsDto.NotificationJsonDtos.Add(_notificationService.NotificationToDto(notification));
            }

            return notificationsDto;
        }


        [System.Web.Mvc.HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public NotificationsDto GetNotificationsByMemberIdAndSyncDate(int memberId,DateTime syncDate)
        {
            var notificationsDto = new NotificationsDto();
            var notifications    = _notificationService.GetNotificationsByMemberId(memberId);
            foreach (var notification in notifications.Where(x=>x.SyncDate > syncDate))
            {
                 
                notificationsDto.NotificationJsonDtos.Add(_notificationService.NotificationToDto(notification));
            }

            return notificationsDto;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Http.AcceptVerbs("POST")]
        public bool SaveNotificationJsonDto([FromBody] NotificationsDto notificationsDto)
        {
            _notificationService.CreateOrUpdateNotification(_notificationService.DtoToNotification(notificationsDto.NotificationJsonDtos.FirstOrDefault()),false,true);
            var notification = _notificationService.GetNotificationsByKey(notificationsDto.NotificationJsonDtos.FirstOrDefault().Key);

            return true;
        }
        [System.Web.Mvc.HttpPost]
        [System.Web.Http.AcceptVerbs("POST")]
        public bool SetNotificationToRead(Guid notificationKey)
        {
            var notification = _notificationService.GetNotificationsByKey(notificationKey);
            notification.ReadDate = DateTime.Now;
            
            notification = _notificationService.CreateOrUpdateNotification(notification, false, true);


            return true;
        }
        [System.Web.Mvc.HttpPost]
        [System.Web.Http.AcceptVerbs("POST")]
        public bool SetNotificationToReadForm(Guid notificationKey)
        {
            var notification = _notificationService.GetNotificationsByKey(notificationKey);

            notification = _notificationService.CreateOrUpdateNotification(notification, false, true);


            return true;
        }
        [System.Web.Mvc.HttpPost]
        [System.Web.Http.AcceptVerbs("POST")]
        public Core.Models.NotificationJsonDto SyncLocalNotificationsToServerAsync([FromBody] Core.Models.NotificationJsonDto NotificationJsonDto)
        {
           

            //BUMP THE SYNC DATE to send back to device to conclude sync

            var Notification = _notificationService.CreateOrUpdateNotification(_notificationService.DtoToNotification(NotificationJsonDto), false, true);
            
            return _notificationService.NotificationToDto(Notification);
        }
    }
}