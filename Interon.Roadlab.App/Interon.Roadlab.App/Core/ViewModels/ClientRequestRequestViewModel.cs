using Interon.Roadlab.App.Core.Models;
using Interon.Roadlab.App.Core.Services;
using Interon.Roadlab.App.Views;
using Interon.Roadlab.App.Views.ClientRequests;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interon.Roadlab.App.Core.Const;
using Interon.Roadlab.App.Core.Tasks;
using Interon.Roadlab.Core;
using Interon.Roadlab.Core.Enums;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core.ViewModels
{
    public class xBranch
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
    [QueryProperty(nameof(RequestType), "requesttype")]
    internal class ClientRequestRequestViewModel : ViewModelBase
    {
        private string _transactionType;
        public ObservableCollection<ClientRequest> ClientRequest { get; set; } = new ObservableCollection<ClientRequest>();

        public List<xBranch> Branches { get; set; } = new List<xBranch>();
        private int _requestedById = 0;
        private string _requestedByName = "";
        public string Title { get; set; }
        public string RequestType
        {
            get => _RequestType;
            set => SetProperty(ref _RequestType, value.ToString());
        }
        public string TransactionType
        {
            get => _transactionType;
            set => SetProperty(ref _transactionType, value.ToString());
        }
        public ClientRequestRequestViewModel()
        {
            RequestService requestService = new RequestService();
           
            var member = MemberService.GetMember();

            _requestedById = member.Id;
            _requestedByName = member.Fullname;

            Commands.Add("Submit", new AsyncCommand(Submit));
            Commands.Add("Add", new AsyncCommand(Add, allowsMultipleExecutions: false));
            Commands.Add("Delete", new Command(Delete));
            Commands.Add("GeolocateCommand", new AsyncCommand(GeolocateCommand, allowsMultipleExecutions: false));
             Commands.Add("SiteSearchCommand", new AsyncCommand(SiteSearch, allowsMultipleExecutions: false));
           // SiteSearchCommand = CommandFactory.Create(SiteSearch);
            Commands.Add("ClearCommand", new Command(ClearCommand));
            if (SecureStorageService.Site != null)
            {
                SiteLocationAddress = SecureStorageService.Site.Value;
            }
            SetQuickFillList();
            MessagingCenter.Subscribe<object>(this, MessagingCenterValues.RequestChange, (sender) =>
            {
                SetQuickFillList();

            });

            MessagingCenter.Subscribe<xSite>(this, MessagingCenterValues.SiteChange, (sender) =>
            {
                SecureStorageService.Site = sender;
                SiteLocationAddress = sender.Value;


            });
            MessagingCenter.Subscribe<PeriodicBranches>(this, MessagingCenterValues.BranchesChange, (sender) =>
            {
                SetBranches();

            });
            SetBranches();
            MessagingCenter.Subscribe<ClientRequestRequestLineViewModel>(this, MessagingCenterValues.LineItemChange, (sender) =>
            {
                LineError = "";
                CVHeight = ClientRequest.Count * 165;
            });
        }
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "RequestType")
            {
                ClientRequestRequestViewModelInit(RequestType);
            }
        }
        private void ClientRequestRequestViewModelInit(string transactionType)
        {
            TransactionType = transactionType;
            Title = TransactionType.ToString().ToProperCase() + " " + "Request";
           


        }

        public IAsyncCommand SiteSearchCommand { get; set; }


        private async Task SiteSearch()
        {
            try
            {
               
                await Busy();
                await Shell.Current.Navigation.PushModalAsync(new ClientRequestSitePage(ClientRequest), true);
            }
            finally
            {
                await NotBusy();
            }
        }

        private void ClearCommand()
        {
            DeleteForm();
        }

        private void SetBranches()
        {
            BranchesService bs = new BranchesService();
            var branches = bs.GetBranches();
            Branches.Clear();
            foreach (var branch in branches)
            {
                Branches.Add(new xBranch()
                {
                    Key = branch.Id,
                    Value = branch.Name
                });
            }
        }

        private void SetQuickFillList()
        {
            RequestService requestService = new RequestService();
            if (QuickFillList == null)
            {
                QuickFillList = new List<PickerKeyValue>();
            }
            else
            {
                QuickFillList.Clear();
            }
            //QuickFillList.Add(new PickerKeyValue()
            //{
            //    Key = "Clear",
            //    Value = "Clear Form"
            //});
            //foreach (var pickerKeyValue in requestService.GetPastProjects())
            //{
            //    QuickFillList.Add(pickerKeyValue);
            //}

            _setQuickFillDropdownSelectedValue = null;
            QuickFillListShow = QuickFillList.Any();
        }

        private async Task GeolocateCommand()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);

                var placemark = placemarks?.FirstOrDefault();
                if (placemark != null)
                {
                    var geocodeAddress =
                        $"AdminArea:       {placemark.AdminArea}\n" +
                        $"CountryCode:     {placemark.CountryCode}\n" +
                        $"CountryName:     {placemark.CountryName}\n" +
                        $"FeatureName:     {placemark.FeatureName}\n" +
                        $"Locality:        {placemark.Locality}\n" +
                        $"PostalCode:      {placemark.PostalCode}\n" +
                        $"SubAdminArea:    {placemark.SubAdminArea}\n" +
                        $"SubLocality:     {placemark.SubLocality}\n" +
                        $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
                        $"Thoroughfare:    {placemark.Thoroughfare}\n";
                }
                StringBuilder sb = new StringBuilder();
                if (placemark.FeatureName != placemark.SubThoroughfare)
                {
                    sb.Append(placemark.FeatureName + "--");
                }
                sb.Append(placemark.SubThoroughfare + " ");
                sb.Append(placemark.Thoroughfare + ", ");
                sb.Append(placemark.SubLocality + ", ");
                sb.Append(placemark.Locality + ", ");
                sb.Append(placemark.CountryCode + "");
                SiteLocationAddress = sb.ToString();
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        public bool QuickFillListShow
        {
            get => _quickFillListShow;
            set => SetProperty(ref _quickFillListShow, value);
        }

        public List<PickerKeyValue> QuickFillList
        {
            get => _quickFillList;
            set
            {
                SetProperty(ref _quickFillList, value);
                if (_quickFillList.Count > 1)
                {
                    QuickFillListShow = true;
                }

                QuickFillListShow = false;
            }
        }

        private PickerKeyValue _setQuickFillDropdownSelectedValue;

        //public PickerKeyValue SetQuickFillDropdownSelectedValue
        //{
        //    get => _setQuickFillDropdownSelectedValue;
        //    set
        //    {
        //        SetProperty(ref _setQuickFillDropdownSelectedValue, value);

        //        try
        //        {
        //            var transaction = new RequestService().GetClientRequestAndClientRequestLinesByKey(Guid.Parse(value.Key));
        //            CopyToForm(transaction);
        //        }
        //        catch
        //        {
        //        }
        //    }
        //}

        //private void CopyToForm(ClientRequest transaction)
        //{
        //    Project = transaction.Project;
        //    SetSiteDropdownSelectedValue = Branches.Find(x => x.Key == transaction.BranchId);
        //    ContactPerson = transaction.ContactPerson;
        //    ContactPersonEmail = transaction.ContactPersonEmail;
        //    ContactPersonNumber = transaction.ContactPersonNumber;
        //    SiteLocationAddress = transaction.SiteLocationAddress;
        //}
         
        private void DeleteForm()
        {
            try
            {
                Project = "";

                ContactPerson = "";
                ContactPersonEmail = "";
                ContactPersonNumber = "";
                SiteLocationAddress = "";
             //   SetQuickFillDropdownSelectedValue = new PickerKeyValue();

                SetBranchDropdownSelectrdValue = new xBranch();
            }
            catch
            {
            }
        }

         

        private void Delete(object obj)
        {
            var orderline = ClientRequest.FirstOrDefault(x => x.Id.ToString() == obj.ToString());
            ClientRequest.Remove(orderline);
        }

        private double _cvHeight;

        public double CVHeight
        {
            get => _cvHeight;
            set => SetProperty(ref _cvHeight, value);
        }

        private async Task  Add()
        {
            try
            {
                 await Shell.Current.Navigation.PushModalAsync(new ClientRequestRequestLinePage(ClientRequest), true).ConfigureAwait(true);
                 IsBusy = true;
            }
            finally
            {
                IsBusy = false;
            }
        }
        

        private async Task Submit()
        {
            await Busy();
            try
            {
                IsBusy = true;
                if (!IsValidAndSetErrors())
                {
                    ScrollY = 0;
                    return;
                }



                RequestService requestService = new RequestService();
               
                ClientsService clientsService = new ClientsService();

                MessagingCenter.Send(this, MessagingCenterValues.RequestChange);

                
                await Shell.Current.Navigation.PushModalAsync(new InfoPage(new ModalMessageViewModel()
                {
                    Message = "Thank you for your request."
                }), true);


               await Shell.Current.Navigation.PopToRootAsync();
                //ResetForm();}
            }
            finally
            {
                await NotBusy();
            }

        }

        private void ResetForm()
        {
            this.ClientRequest.Clear();
            Project = "";
            ContactPerson = "";
            ContactPersonNumber = "";
            ContactPersonEmail = "";
            SiteLocationAddress = "";
        }

        public string SpecialInstructions
        {
            get => _specialInstructions;
            set => SetProperty(ref _specialInstructions, value);
        }

        private xBranch _setBranchDropdownSelectrdValue;

        public xBranch SetBranchDropdownSelectrdValue
        {
            get => _setBranchDropdownSelectrdValue;
            set => SetProperty(ref _setBranchDropdownSelectrdValue, value);
        }

        private bool IsValidAndSetErrors()
        {
            ProjectError = "";
            ContactPersonError = "";
            ContactPersonEmailError = "";
            SiteLocationAddressError = "";
            ContactPersonNumberError = "";
            LineError = "";
            BranchesError = "";
            int errorcount = 0;
            if (string.IsNullOrWhiteSpace(Project))
            {
                ProjectError = "Please enter field";
                errorcount += 1;
            }

            if (string.IsNullOrWhiteSpace(ContactPerson))
            {
                ContactPersonError = "Please enter field";
                errorcount += 1;
            }

            if (string.IsNullOrWhiteSpace(ContactPersonEmail))
            {
                ContactPersonEmailError = "Please enter field";
                errorcount += 1;
            }

            if (string.IsNullOrWhiteSpace(ContactPersonNumber))
            {
                ContactPersonNumberError = "Please enter field";
                errorcount += 1;
            }

            if (string.IsNullOrWhiteSpace(SiteLocationAddress))
            {
                SiteLocationAddressError = "Please enter field";
                errorcount += 1;
            }

            if (!ClientRequest.Any())
            {
                LineError = "Please enter line items";
                errorcount += 1;
            }
            if (SetBranchDropdownSelectrdValue == null)
            {
                BranchesError = "Please select branch";
                errorcount += 1;
            }
            if (errorcount > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string LineError
        {
            get => _lineError;
            set => SetProperty(ref _lineError, value);
        }

        private bool _buttonEnabled = true;

        public bool ButtonEnabled
        {
            get => _buttonEnabled;
            set => SetProperty(ref _buttonEnabled, value);
        }

        private double _ScrollY = 0;

        public double ScrollY
        {
            get => _ScrollY;
            set => SetProperty(ref _ScrollY, value);
        }

        private string _Project = "";

        public string Project
        {
            get => _Project;
            set => SetProperty(ref _Project, value);
        }

        public int RequestedById
        {
            get => _RequestedById;
            set => SetProperty(ref _RequestedById, value);
        }

        public string RequestedByName
        {
            get => _RequestedByName;
            set => SetProperty(ref _RequestedByName, value);
        }

        private string _ContactPerson = "";

        public string ContactPerson
        {
            get => _ContactPerson;
            set => SetProperty(ref _ContactPerson, value);
        }

        private string _ContactPersonNumber = "";

        public string ContactPersonNumber
        {
            get => _ContactPersonNumber;
            set => SetProperty(ref _ContactPersonNumber, value);
        }

        private string _ContactPersonEmail = "";

        public string ContactPersonEmail
        {
            get => _ContactPersonEmail;
            set => SetProperty(ref _ContactPersonEmail, value);
        }

        private string _SiteLocationAddress = "";
        private string _projectError = "";
        private string _requestedByError = "";
        private string _contactPersonError = "";
        private string _contactPersonNumberError = "";
        private string _contactPersonEmailError = "";
        private string _siteLocationAddressError = "";
        private string _specialInstructions;
        private string _lineError;
        private string _RequestedByName;
        private int _RequestedById;
        private string _branchesError;
        private List<PickerKeyValue> _PickerKeyValue;
        private bool _quickFillListShow;
        private List<PickerKeyValue> _quickFillList;
        private string _RequestType;

        public string SiteLocationAddress
        {
            get => _SiteLocationAddress;
            set => SetProperty(ref _SiteLocationAddress, value);
        }

        public string ProjectError
        {
            get => _projectError;
            set => SetProperty(ref _projectError, value);
        }

        public string RequestedByError
        {
            get => _requestedByError;
            set => SetProperty(ref _requestedByError, value);
        }

        public string ContactPersonError
        {
            get => _contactPersonError;
            set => SetProperty(ref _contactPersonError, value);
        }

        public string ContactPersonNumberError
        {
            get => _contactPersonNumberError;
            set => SetProperty(ref _contactPersonNumberError, value);
        }

        public string ContactPersonEmailError
        {
            get => _contactPersonEmailError;
            set => SetProperty(ref _contactPersonEmailError, value);
        }

        public string SiteLocationAddressError
        {
            get => _siteLocationAddressError;
            set => SetProperty(ref _siteLocationAddressError, value);
        }

        public string BranchesError
        {
            get => _branchesError;
            set => SetProperty(ref _branchesError, value);
        }
    }
}