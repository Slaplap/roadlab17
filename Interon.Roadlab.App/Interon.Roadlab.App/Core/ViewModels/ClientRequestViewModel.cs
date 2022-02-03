using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Interon.Roadlab.App.Core.Const;
using Interon.Roadlab.App.Core.Models;
using Interon.Roadlab.App.Core.Services;
using Interon.Roadlab.App.Views;
using Interon.Roadlab.Core.Enums;
using Interon.Roadlab.LIMS.DTO;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core.ViewModels
{

    [QueryProperty("ClientRequestId", "ClientRequestId")]
    public class ClientRequestViewModel : ViewModelBase
    {



        private double _rowListHeight;
        private string _heading;
        private bool _isOrder;

        private string _clientRequestId;
        private string _transactionType;
        private string _orderNumber;
        private bool _validateOrderNumber;
        private ClientRequest _clientRequest;
        private bool _isDocumentVisible;
        private bool _isBookingAgentVisible;
        private bool _isTechnicianVisible;
        private bool _isQuoteAcceptanceVisible;
        private bool _isServicesVisible;
        private bool _isEquipmentVisible;
        private double _RowHeadingPlusListHeight;
        private bool _IsQuotationTotalVisible;

        public ClientRequestViewModel()
        {
            try
            {



                //   Commands.Add("Close", new Command(CmdClose));
                Commands.Add("QuoteAcceptReject", new AsyncCommand(QuoteAcceptReject, allowsMultipleExecutions: false));

                Commands.Add("OpenClientRequestCommand", new Command(OpenClientRequestCommand));
                MessagingCenter.Subscribe<object>(this, MessagingCenterValues.RequestChange, (sender) =>
                {
                    LoadClientRequest(ClientRequestId);
                });

            }
            catch (Exception ex)
            {
                Debugger.Break();
            }
        }
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName != "ClientRequestId")
                return;
            LoadClientRequest(ClientRequestId);
        }


        public double RowListHeight
        {
            get => _rowListHeight;
            set => SetProperty(ref _rowListHeight, value);
        }
        public double RowHeadingPlusListHeight
        {
            get => _RowHeadingPlusListHeight;
            set => SetProperty(ref _RowHeadingPlusListHeight, value);
        }
        public string Heading
        {
            get => _heading;
            set => SetProperty(ref _heading, value);
        }

        public bool IsOrder
        {
            get => _isOrder;
            set => SetProperty(ref _isOrder, value);
        }
        public string OrderNumber
        {
            get => _orderNumber;
            set => SetProperty(ref _orderNumber, value);
        }

        public bool ValidationFailedOrderNumber
        {
            get => _validateOrderNumber;
            set => SetProperty(ref _validateOrderNumber, value);
        }
        public ClientRequest ClientRequest
        {
            get => _clientRequest;
            set => SetProperty(ref _clientRequest, value);
        }
        public bool IsDocumentVisible
        {
            get => _isDocumentVisible;
            set => SetProperty(ref _isDocumentVisible, value);
        }
        public bool IsTechnicianVisible
        {
            get => _isTechnicianVisible;
            set => SetProperty(ref _isTechnicianVisible, value);
        }
        public bool IsQuotationTotalVisible
        {
            get => _IsQuotationTotalVisible;
            set => SetProperty(ref _IsQuotationTotalVisible, value);
        }
        public bool IsBookingAgentVisible
        {
            get => _isBookingAgentVisible;
            set => SetProperty(ref _isBookingAgentVisible, value);
        }
        public bool IsQuoteAcceptanceVisible
        {
            get => _isQuoteAcceptanceVisible;
            set => SetProperty(ref _isQuoteAcceptanceVisible, value);
        }

        public bool IsServicesVisible
        {
            get => _isServicesVisible;
            set => SetProperty(ref _isServicesVisible, value);
        }
        public bool IsEquipmentVisible
        {
            get => _isEquipmentVisible;
            set => SetProperty(ref _isEquipmentVisible, value);
        }
        public int ClientRequestId
        {
            get => Convert.ToInt32(_clientRequestId);
            set => SetProperty(ref _clientRequestId, value.ToString());
        }

        public string ClientRequestType
        {
            get => _transactionType;    
            set => _transactionType = value;
        }


        private async Task QuoteAcceptReject()
        {
            Device.OpenUri(new Uri(Interon.Roadlab.Core.Environment.LIMSAPIURL + "/" + ClientRequest.Token.OrderAccepted));

        }

        private void LoadClientRequest(int id)
        {
            try
            {
                RequestService requestService = new RequestService();

                var _clientRequests = requestService.GetClientRequestById(id);


                ClientRequest = _clientRequests;
                Heading = ClientRequest.Status.FirstOrDefault();

                SetVisiblility();

                int servcesPlusEquipmentCount = 1;
                try
                {
                    servcesPlusEquipmentCount += _clientRequests.Token?.Quotation?.Equipment?.Count ?? 1;


                }
                catch
                {

                }
                try
                {

                    servcesPlusEquipmentCount = +_clientRequests.Token?.Quotation?.Services?.Count ?? 1;

                }
                catch
                {

                }


                RowListHeight = (servcesPlusEquipmentCount  ) * (60 * 3 );
                RowHeadingPlusListHeight = RowListHeight/2 ;
            }
            catch (Exception e)
            {
                Debugger.Break();
            }
        }

        private void SetVisiblility()
        {
            if (!string.IsNullOrWhiteSpace(ClientRequest.Token.QuotationPdf?.Url))
            {
                IsDocumentVisible = true;
            }

            if (ClientRequest.Token.AssignedTechnician != null)
            {
                IsTechnicianVisible = true;
            }
            if (ClientRequest.Token.BookingsController != null)
            {
                IsBookingAgentVisible = true;
            }

            if (ClientRequest.Token.Quotation?.Services != null)
            {
                if ((ClientRequest.Token.Quotation?.Services).Any() )
                {
                    IsServicesVisible = true;
                }
            }
            if (ClientRequest.Token.Quotation?.Equipment != null)
            {
                if ((ClientRequest.Token.Quotation?.Equipment).Any())
                {
                    IsServicesVisible = true;
                }
            }

            if (!string.IsNullOrWhiteSpace(ClientRequest.QuotationTotal) && (IsServicesVisible || IsEquipmentVisible))
            {
                IsQuotationTotalVisible = true;
            }
        }

        private void OpenClientRequestCommand()
        {
            Device.OpenUri(new Uri(Interon.Roadlab.Core.Environment.LIMSAPIURL + "/" + ClientRequest.Token.QuotationPdf.Url));
        }

        private void RejectCommand()
        {

            try
            {
                //  Shell.Current.Navigation.PopAsync();
                Shell.Current.GoToAsync($"ClientRequestComments?ClientRequestId={ClientRequestId}");

            }
            catch (Exception e)
            {
                Debugger.Break();

            }
            // await Shell.Current.Navigation.PopAsync();
        }
        //private void RejectCommand()
        //{

        //    Shell.Current.GoToAsync($"ClientRequestComments?transactionKey={ClientRequestId}&requestType={ClientRequestType}");
        //}
    }
}
