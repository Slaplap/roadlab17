using Interon.Roadlab.App.Core.Models;
using Interon.Roadlab.App.Core.ViewModels;
using Interon.Roadlab.Core.Dto;
using Interon.Roadlab.Core.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Interon.Roadlab.Core.Enums;
using Interon.Roadlab.LIMS.DTO;
using Client = Interon.Roadlab.LIMS.DTO.Client;

namespace Interon.Roadlab.App.Core.Services
{
    internal class RequestService : BaseService
    {
        private SQLiteConnection _db;

        public RequestService()
        {
            try
            {
                _db = GetConnection();


                this._db.CreateTable<ClientRequest>();
            }
            catch(Exception ex)
            {

            }
        }

        public ClientRequest DtoToClientRequestTable(RootClientRequest clientRequestRoot)
        {
            try
            {
                var clientRequestTable = new ClientRequest()
                {
                    Id = clientRequestRoot.Id,
                    DateCreated = DateTime.Parse(clientRequestRoot.DateCreated),
                    DateUpdated =  DateTime.Parse(clientRequestRoot.DateUpdated),
                    Status = clientRequestRoot.Status,
                    Token =  clientRequestRoot.TokenData

                };


                
                return clientRequestTable;
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }

            return null;
        }

        public RootClientRequest ClientRequestTableToDto(ClientRequest clientRequest)
        {
            try
            {
                var clientRequestRoot = new RootClientRequest()
                {
                   Id = clientRequest.Id,
                   DateCreated = clientRequest.DateCreated.ToString(),
                   DateUpdated = clientRequest.DateUpdated.ToString(),
                   TokenData = clientRequest.Token

                };
               
                return clientRequestRoot;
            }
            catch (Exception err)
            {
            }
            return new RootClientRequest();
        }

        public bool IsClientRequestSavedOnLocalDevice(int id)
        {
            return _db.Query<Branch>($"SELECT * FROM ClientRequest where Id = '{id}' ").Any();
        }

       

        public List<ClientRequest> GelAllClientRequests()
        {
            var transactions = _db.Query<ClientRequest>("SELECT * FROM ClientRequest");
            return transactions;
        }


        public ClientRequest GetClientRequestById(int id)
        {
            var transactions = _db.Query<ClientRequest>($"SELECT * FROM ClientRequest where Id='{id}'");
            return transactions.FirstOrDefault();
        }
        public bool CreateOrUpdateClientRequest(RootClientRequest clientRequest )
        { 
           
            try
            {
                _db.BeginTransaction();
                if ( IsClientRequestSavedOnLocalDevice(clientRequest.Id))
                {
                   
                    var c = DtoToClientRequestTable(clientRequest);
                    _db.Update(c);
                }
                else
                {
                    var c = DtoToClientRequestTable(clientRequest);

                    _db.Insert(c);
                }

        
                _db.Commit();

            }
            catch (SQLite.SQLiteException se)
            {
                Debugger.Break();
                _db.Rollback();
                ErrorService.ErrorExceptionAndAnalytics("Error inserting clientRequest", "CreateOrUpdateClientRequest()", se);
                return false;
            }
            catch (Exception ex)
            {
                Debugger.Break();
                ErrorService.ErrorExceptionAndAnalytics("Error inserting clientRequest", "CreateOrUpdateClientRequest()", ex);
                return false;
            }

            return true;
        }









        public void DeleteClientRequests()
        {
            _db.Execute("Delete from ClientRequest where 1=1");
            
        }

        public void DeleteClientRequest(ClientRequest clientRequest)
        {
             

            _db.Delete<ClientRequest>(clientRequest);
        }


        public List<ClientRequest> GelAllClientRequestsByStatus(string quoteReady)
        {
            List<ClientRequest> clientRequests = new List<ClientRequest>();
            var _clientRequests = GelAllClientRequests();
            foreach (var clientRequest in _clientRequests)
            {
                if (clientRequest.Status.FirstOrDefault().StatusContverter() == quoteReady)
                {
                    clientRequests.Add(clientRequest);
                }

            }

            return clientRequests;
        }
    }
}