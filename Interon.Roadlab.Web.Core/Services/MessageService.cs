 

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Interon.Roadlab.Core.Dto;
using Interon.Roadlab.Core.Models;
using Interon.Roadlab.Web.App.API;
using Interon.Roadlab.Web.App.Models;
using NPoco;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using Umbraco.Core.Scoping;

namespace Interon.Roadlab.Web.App.Services
{
    public class MessageService
    {
        private readonly IScopeProvider scopeProvider;

        public MessageService(IScopeProvider scopeProvider)
        {
            this.scopeProvider = scopeProvider;
        }

        public List<Message> GetAllMessages()
        {
            using (var scope = scopeProvider.CreateScope(autoComplete: true))
            {
                var sql = scope.SqlContext.Sql().Select("*").From<Message>();

                var Messages = scope.Database.Fetch<Message>(sql);

                return Messages.ToList();
            }
        }

        public List<Message> GetMessagesByMemberId(int memberId)
        {

            var memberMessages = GetAllMessages().Where(x => x.MemberId == memberId);
            return memberMessages.ToList();

        }
        public Message GetMessagesByKey(Guid key)
        {
            using (var scope = scopeProvider.CreateScope(autoComplete: true))
            {
                var sql = scope.SqlContext.Sql().Select("*").From<Message>().Where($"key ='{key}'");

                var Message = scope.Database.Fetch<Message>(sql).FirstOrDefault();

                return Message;
            }
        }

        public Message CreateOrUpdateMessage(Message Message,bool UpdateDate,bool IsSyncStart)
        {
            try
            {
                var datetimenow = DateTime.Now;
                using (var scope = scopeProvider.CreateScope(autoComplete: true))
                {

                    var _Message = scope.Database.Fetch<Message>($"Select * from Messages where [Key]='{Message.Key}'").FirstOrDefault();
                    if (_Message == null)
                    {
                        if (IsSyncStart)
                        {
                            Message.SyncDate = datetimenow;
                        }

                        if (UpdateDate)
                        {
                            Message.UpdateDate = datetimenow;
                            Message.CreateDate = datetimenow;
                        }

                        Message.Key = Guid.NewGuid();
                        scope.Database.Insert(Message);
                    }
                    else
                    {
                        if (IsSyncStart)
                        {
                            Message.SyncDate = datetimenow;
                        }

                        if (UpdateDate)
                        {
                            Message.UpdateDate = datetimenow;
                            Message.CreateDate = datetimenow;
                        }

                        scope.Database.Update(Message);
                    }
                }

                return Message;
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

        public MessageJsonDto MessageToDto(Message Message)
        {
            var MessageDTO = new MessageJsonDto()
            {
                Key =  Message.Key,
                MemberId =  Message.MemberId,
                CreateDate = Message.CreateDate,
                UpdateDate = Message.UpdateDate,
                SyncDate = Message.SyncDate,
                MessageType = Message.MessageType,
                Payload = Message.Payload
                

            };
            return MessageDTO;
        }

        public Message DtoToMessage(MessageJsonDto MessageJsonDto)
        {

            var Message = new Message()
            {
                Key               = MessageJsonDto.Key,
                MemberId         = MessageJsonDto.MemberId,
                CreateDate       = MessageJsonDto.CreateDate,
                UpdateDate = MessageJsonDto.UpdateDate,
                SyncDate = MessageJsonDto.SyncDate,
                Payload = MessageJsonDto.Payload,
                MessageType = MessageJsonDto.MessageType
                
            };
            return Message;
        }
        public PagedUmbracoResult GetPaged(int itemsPerPage, int pageNumber, string sortColumn, string sortOrder,
       string status, string searchTerm)
        {
            var query = new Sql();
            var sql =
                "SELECT [Key],MemberId,Title,Message,Link,Data,MessageType,PushOutcome,CreateDate,UpdateDate,ReadDate,SyncDate from Messages WHERE 1=1 ";

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