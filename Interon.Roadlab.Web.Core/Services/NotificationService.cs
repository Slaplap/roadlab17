using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Interon.Roadlab.Core.Models;
using Interon.Roadlab.Web.Core.Models;
using NPoco;
using Umbraco.Core.Persistence;
using Umbraco.Core.Scoping;

namespace Interon.Roadlab.Web.Core.Services
{
    public class NotificationService
    {
        private readonly IScopeProvider scopeProvider;

        public NotificationService(IScopeProvider scopeProvider)
        {
            this.scopeProvider = scopeProvider;
        }

        public List<Notification> GetAllNotifications()
        {
            using (var scope = scopeProvider.CreateScope(autoComplete: true))
            {
                var sql = scope.SqlContext.Sql().Select("*").From<Notification>();

                var notifications = scope.Database.Fetch<Notification>(sql);

                return notifications.ToList();
            }
        }

        public List<Notification> GetNotificationsByMemberId(int memberId,bool all=false)
        {
            var memberNotifications = all ? GetAllNotifications().Where(x => x.MemberId == memberId) : GetAllNotifications().Where(x => x.MemberId == memberId && x.ReadDate ==null);

            return memberNotifications.ToList();
        }
        public Notification GetNotificationsByKey(Guid key)
        {
            
            using (var scope = scopeProvider.CreateScope(autoComplete: true))
            {

                try
                {
                    var sql = scope.SqlContext.Sql().Select("*").From<Notification>().Where($"[key] ='{key}'");

                    var notification = scope.Database.Fetch<Notification>(sql).FirstOrDefault();

                    return notification;
                }
                catch (Exception e)
                {
                    
                    throw;
                }
            }
        }

        public Notification CreateOrUpdateNotification(Notification notification,bool UpdateDate,bool IsSyncStart)
        {
            try
            {
                var datetimenow = DateTime.Now;
                using (var scope = scopeProvider.CreateScope(autoComplete: true))
                {

                    var _notification = scope.Database.Fetch<Notification>($"Select * from Notifications where [Key]='{notification.Key}'").FirstOrDefault();
                    if (_notification == null)
                    {
                        if (IsSyncStart)
                        {
                            notification.SyncDate = datetimenow;
                        }

                        if (UpdateDate)
                        {
                            notification.UpdateDate = datetimenow;
                            notification.CreateDate = datetimenow;
                        }

                        notification.Key = Guid.NewGuid();
                        scope.Database.Insert(notification);
                    }
                    else
                    {
                        if (IsSyncStart)
                        {
                            notification.SyncDate = datetimenow;
                        }

                        if (UpdateDate)
                        {
                            notification.UpdateDate = datetimenow;
                            notification.CreateDate = datetimenow;
                        }

                        scope.Database.Update(notification);
                    }
                }

                return notification;
            }
            catch (SqlException e)
            {
                Debugger.Break();
                return null;
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }

            return null;
        }

        public NotificationJsonDto NotificationToDto(Notification notification)
        {
            var notificationDTO = new NotificationJsonDto()
            {
                Key =  notification.Key,
                MemberId =  notification.MemberId,
                CreateDate = notification.CreateDate,
                Data = notification.Data,
                Link = notification.Link,
                ForeignKey = notification.ForeignKey,
                Message = notification.Message,
                NotificationType = notification.NotificationType,
                ReadDate = notification.ReadDate,
                Title = notification.Title,
                SyncDate = notification.SyncDate,
                PushOutcome = notification.PushOutcome,
                UpdateDate = notification.UpdateDate
                

            };
            return notificationDTO;
        }

        public Notification DtoToNotification(NotificationJsonDto notificationJsonDto)
        {

            var notification = new Notification()
            {
                Key               = notificationJsonDto.Key,
                MemberId         = notificationJsonDto.MemberId,
                CreateDate       = notificationJsonDto.CreateDate,
                Data             = notificationJsonDto.Data,
                Link             = notificationJsonDto.Link,
                Message          = notificationJsonDto.Message,
                NotificationType = notificationJsonDto.NotificationType,
                ReadDate         = notificationJsonDto.ReadDate,
                Title            = notificationJsonDto.Title,
                SyncDate = notificationJsonDto.SyncDate,
                PushOutcome = notificationJsonDto.PushOutcome,
                UpdateDate = notificationJsonDto.UpdateDate
                
            };
            return notification;
        }
        public PagedUmbracoResult GetPaged(int itemsPerPage, int pageNumber, string sortColumn, string sortOrder,
       string status, string searchTerm)
        {
            var query = new Sql();
            var sql =
                "SELECT [Key],MemberId,Title,Message,Link,Data,NotificationType,PushOutcome,CreateDate,UpdateDate,ReadDate,SyncDate from notifications WHERE 1=1 ";

            query.Append(sql);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var searchList = searchTerm.Split(Char.Parse(" "));

                foreach (string searchString in searchList)
                {
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        query.Append(" AND (");
                        query.Append(" [MemberId] like @0", "%" + searchString + "%");
                        query.Append(" )");
                    }
                }
            }

            if (!string.IsNullOrEmpty(status))
            {
                query.Append($" AND [Status] = '{status}'");
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortOrder))
                query.OrderBy(sortColumn + " " + sortOrder);
            else
            {
                query.OrderBy("id asc");
            }

            using (var scope = scopeProvider.CreateScope(autoComplete: true))
            {
                var p = scope.Database.Page<object>(pageNumber, itemsPerPage, query);
                var result = new PagedUmbracoResult
                {
                    TotalPages = p.TotalPages,
                    TotalItems = p.TotalItems,
                    ItemsPerPage = p.ItemsPerPage,
                    CurrentPage = p.CurrentPage,
                    Data = p.Items.ToList<object>()
                };
                return result;
            }
        }
    }

    
}