using Interon.Roadlab.App.Core;
using Interon.Roadlab.App.Core.Services;
using Interon.Roadlab.Core;
using Interon.Roadlab.Core.Dto;
using System;
using System.Linq;
using System.Threading.Tasks;
using Interon.Roadlab.App.Core.Tasks;
 
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Views.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public string Email { get; set; } = "anton@interon.co.za";
        private int count = 0;
        
        public RegisterPage()
        {
            InitializeComponent();
            this.BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CheckConnection();
           
            this.IsBusy = false;
            MyVersion.Text = $"{Xamarin.Essentials.AppInfo.VersionString} {Xamarin.Essentials.AppInfo.BuildString}";

            url.Text = Interon.Roadlab.Core.Environment.CMSBaseUrl;
            url2.Text = Interon.Roadlab.Core.Environment.LIMSBaseUrl;


            url.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(OnLabelClicked),
            });
        }

        public async Task CheckConnection()
        {
            await PeriodicAppSettings.Job();
        }
        private void OnLabelClicked()
        {
            count++;
            if (count > 10)
            {
                try
                {
                    if ((int)Interon.Roadlab.Core.Environment.Active == 2)
                    {
                        Interon.Roadlab.Core.Environment.Active = 0;
                    }
                    else
                    {
                        Interon.Roadlab.Core.Environment.Active++;
                    }
                }
                catch
                {
                    Interon.Roadlab.Core.Environment.Active = Interon.Roadlab.Core.Environment.Default;
                }


                url.Text = Interon.Roadlab.Core.Environment.CMSBaseUrl;
                url2.Text = Interon.Roadlab.Core.Environment.LIMSBaseUrl;
                count = 0;

            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void BtnSubmit_OnClicked(object sender, EventArgs e)
        {
            this.IsBusy = true;

            if (!OnlineService.HasInternet())
            {
                IsBusy = false;
                ErrorService.Error("ER000 No Internet");
                return;
            }

            var hasConnectionToServer =  OnlineService.HasConnectionToServerAsync() ;
            if (!hasConnectionToServer)
            {
                IsBusy = false;
                ErrorService.EventAndAnalyticsAndModal("ER001 - No Connection to server", AnalyticsService.EventCategory.Warning);
                return;
            }

            if (!txtEmail.Text.IsValidEmail())
            {
                IsBusy = false;
                DependencyServiceService.LongMessage("Invalid Email Address");
                return;
            }

            MemberDto member;
            try
            {
                member = await MemberOnlineService.GetMemberByEmail(txtEmail.Text).ConfigureAwait(true);
            }
            catch (Exception exx)
            {
                ErrorService.Error("Error getting member form server : GetMemberByEmail");

                IsBusy = false;
                return;
            }
            if (member == null)
            {
                await App.Current.MainPage.Navigation.PushModalAsync(new NewRegisterPage(txtEmail.Text), true).ConfigureAwait(true);

                IsBusy = false;
            }
            else
            {
                SecureStorageService.MemberId = member.Id;
              
                ClientsService cs = new ClientsService();
                MemberService.CreateOrUpdateMember(MemberService.DtoToMember(member));
                Interon.Roadlab.LIMS.DTO.Client clientDto;
                try
                {
                    var clientByAccountNumber = await Roadlab.LIMS.Services.ClientsAndContactsService.ClientByAccountNumberAsync(member.Clients.FirstOrDefault().AccountNumber);
                    if (clientByAccountNumber != null)
                    {
                        cs.CreateOrUpdateCompany(cs.DtoToCompany(clientByAccountNumber));
                        SecureStorageService.ClientAccountNumber = member.Clients.FirstOrDefault().AccountNumber;
                        SecureStorageService.ClientId = member.Clients.FirstOrDefault().ClientId;
                        SecureStorageService.ClientName = member.Clients.FirstOrDefault().Name;
                        SecureStorageService.ContactId = member.Clients.FirstOrDefault().ContactId;


                    }
                    else
                    {
                        ErrorService.ErrorExceptionAndAnalyticsAndModal("Error getting client form server : ", "BtnSubmit_OnClicked",new Exception("Client Account number registered on member in cms not found in LIMS"));

                        IsBusy = false;
                        return;
                    }
                }
                catch
                {
                    ErrorService.Error("Error getting client form server : GetClientByAccountNumber");

                    IsBusy = false;
                    return;
                }

                var isAuthenticated = await AuthenticationService.HasValidCredentialsAsync().ConfigureAwait(false);
                var isRegisteredOnLocalDevice = MemberService.IsRegisteredOnLocalDevice();
                if (!isAuthenticated && isRegisteredOnLocalDevice)
                {
                    await App.Current.MainPage.Navigation.PushModalAsync(new ReRegisterPage(), true).ConfigureAwait(true);

                    IsBusy = false;
                    return;
                }
                isAuthenticated = await AuthenticationService.HasValidCredentialsAsync().ConfigureAwait(false);
                if (isAuthenticated && MemberService.IsRegisteredOnLocalDevice())
                {
                    Globals.StartAppShell();
                }
                if (!isAuthenticated && !MemberService.IsRegisteredOnLocalDevice())
                {
                    
                   
                    await App.Current.MainPage.Navigation.PushModalAsync(new ReRegisterPage(), true);

                    IsBusy = false;
                    return;
                }
                if (isAuthenticated && !MemberService.IsRegisteredOnLocalDevice())
                {
                    throw new NotImplementedException();
                }
                
            }

            this.IsBusy = false;
        }

       

        private void TxtEmail_OnFocused(object sender, FocusEventArgs e)
        {
            fucus();
        }

        private void fucus()
        {
            url.IsVisible = !url.IsVisible;
            url2.IsVisible = !url2.IsVisible;
            title.IsVisible = !title.IsVisible;
            img1.IsVisible = !img1.IsVisible;
            img2.IsVisible = !img2.IsVisible;
            MyVersion.IsVisible = !MyVersion.IsVisible;
        }

        private void TxtEmail_OnUnfocused(object sender, FocusEventArgs e)
        {
            fucus();
        }
    }
}