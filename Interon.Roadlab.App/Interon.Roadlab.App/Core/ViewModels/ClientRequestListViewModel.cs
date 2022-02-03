using Interon.Roadlab.App.Core.Models;
using Interon.Roadlab.App.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Interon.Roadlab.App.Core.Const;
using Interon.Roadlab.Core;
using Interon.Roadlab.Core.Enums;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core.ViewModels
{
   
    internal class ClientRequestListViewModel : ViewModelBase
    {

        private string _heading;
        private bool _isMyOrder;

        private List<PickerKeyValue> _PickerKeyValue = new List<PickerKeyValue>();
        private PickerKeyValue _SetFilter;
        private string _requestStatus;
        public ClientRequestListViewModel( string requestStatus)
        {
            RequestStatus = requestStatus;
            Commands.Add("GotoClientRequest", new Command(GotoClientRequest));

            MessagingCenter.Subscribe<object>(this, MessagingCenterValues.RequestChange, (sender) =>
            {
               
                 LoadClientRequestsByType();
             });
           
           // Init();
          }
       
        
        private void Init()
        { 
            if (RequestStatus == null) return;
            RequestStatus = _requestStatus.URLDecode();
            Heading            = _requestStatus + " List";

            FilterList.Add(
                new PickerKeyValue()
                {
                    Key   = "0",
                    Value = "My " + _requestStatus + " List"
                });
            if (SecureStorageService.ClientId != 0)
            {
                FilterList.Add(
                    new PickerKeyValue()
                    {
                        Key = "1",
                        Value = "Account " + _requestStatus + " List"

                    });
            }

            if (SecureStorageService.SiteId != 0)
            {
                FilterList.Add(
                    new PickerKeyValue()
                    {
                        Key = "2",
                        Value = "Site " + _requestStatus + " List"
                    });
            }

            SetFilter = new PickerKeyValue()
            {
                Key   = "0",
                Value = "My " + _requestStatus + " List"
            };
        }
      //  public ObservableCollection<PickerKeyValue> FilterList = new ObservableCollection<PickerKeyValue>();
        public List<PickerKeyValue> FilterList
        {
            get => _PickerKeyValue;
            set => SetProperty(ref _PickerKeyValue, value);
        }

        public string Heading
        {
            get => _heading;
            set => SetProperty(ref _heading, value);
        }
        public string RequestStatus
        {
            get => _requestStatus;
            set => SetProperty(ref _requestStatus, value);
        }
        public bool IsMyOrder
        {
            get => _isMyOrder;
            set => SetProperty(ref _isMyOrder, value);
        }

        public PickerKeyValue SetFilter
        {
            get => _SetFilter;
            set
            {
                SetProperty(ref _SetFilter, value);
                if (value.Value.Contains("My "))
                {
                    IsMyOrder = true;
                }
                else
                {
                    IsMyOrder = false;
                }
               
            }
        }

        public ObservableCollection<ClientRequest> ClientRequests { get; set; } = new ObservableCollection<ClientRequest>();
        protected override void OnPropertyChanged(string propertyName = null)
        {
            
            base.OnPropertyChanged(propertyName);
            if (propertyName == "SetFilter")
            {
                LoadClientRequestsByType();
            }
            if (propertyName == "RequestStatus")
            {
                Init();
            }
        }

        private void GotoClientRequest(object obj)
        {
            GotoClientRequestAsync(obj);
        }

        private async Task GotoClientRequestAsync(object obj)
        {
            IsBusy = true;

            await Shell.Current.GoToAsync("ClientRequestPage?ClientRequestId=" + obj.ToString()).ConfigureAwait(true);
            IsBusy = false;
        }

        private void LoadClientRequests()
        {
            RequestService requestService = new RequestService();
            List<ClientRequest> _allClientRequests;

            _allClientRequests = requestService.GelAllClientRequests();

            ClientRequests.Clear();
            foreach (var oq in _allClientRequests)
            {
                ClientRequests.Add(oq);
            }
        }

        private void LoadClientRequestsByType()
        {
            try
            {
                RequestService requestService = new RequestService();
                
                //TODO remove just for debugging

                var allClientRequests = requestService.GelAllClientRequests();
                ClientRequests.Clear();
                if (SetFilter.Key == "0")
                {

                    foreach (var transaction in allClientRequests)
                    {
                        if (transaction.Token.Contact.Id != SecureStorageService.ContactId)
                        {
                            
                            continue;
                        }
                        if (string.IsNullOrWhiteSpace(_requestStatus))
                        {
                            ClientRequests.Add(transaction);
                        }
                        else
                        {
                            if (transaction.Status.FirstOrDefault().StatusContverter().Contains(_requestStatus))
                            {
                                ClientRequests.Add(transaction);
                            }
                        }
                    }
                }
                else if (SetFilter.Key == "1")
                {
                    foreach (var transaction in allClientRequests)
                    {
                       
                            if (string.IsNullOrWhiteSpace(_requestStatus))
                            {
                                ClientRequests.Add(transaction);
                            }
                            else
                            {
                                if (transaction.Status.FirstOrDefault().StatusContverter().Contains(_requestStatus))
                                {
                                    ClientRequests.Add(transaction);
                                }
                            }
                       
                    }
                }
                else if (SetFilter.Key == "2")
                {
                    foreach (var transaction in allClientRequests)
                    {
                        if (transaction.Site.Id != SecureStorageService.SiteId)
                        {

                            continue;
                        }
                        if (string.IsNullOrWhiteSpace(_requestStatus))
                        {
                            ClientRequests.Add(transaction);
                        }
                        else
                        {
                            if (transaction.Status.FirstOrDefault().StatusContverter().Contains(_requestStatus))
                            {
                                ClientRequests.Add(transaction);
                            }
                        }
                    }
                    Debugger.Break();
                }
            }
            catch (Exception e)
            {
                Debugger.Break();
            }
        }
    }
}