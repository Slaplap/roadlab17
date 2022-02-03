using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Interon.Roadlab.App.Core;
using Interon.Roadlab.App.Core.Services;

using Interon.Roadlab.App.Views.Registration;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Views.Developer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class DeveloperPage : ContentPage
    {

        private ICommand _forgotPasswordCommand;
        private int taps = 0;
        private string baseUrl = "BaseUrl";


        public DeveloperPage()
        {


            InitializeComponent();
            developerStack.IsVisible = SecureStorageService.IsDeveloperMode;
            infoStack.IsVisible = !SecureStorageService.IsDeveloperMode; ;
            var test = Xamarin.Essentials.DeviceInfo.Platform;
            url.Text = SecureStorageService.BaseUrl;
            url.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(OnLabelClicked),
            });
            SetSecureDetails();
            SetMemberDetails();
            LoadVersionData();
        }
        public void LoadVersionData()
        {
            VersionTracking.Track();

            //verfirstLaunch        = VersionTracking.IsFirstLaunchEver.ToString();
            //verfirstLaunchCurrent = VersionTracking.IsFirstLaunchForCurrentVersion.ToString();
            //verfirstLaunchBuild   = VersionTracking.IsFirstLaunchForCurrentBuild.ToString();
            entVersion.Text = "Version : " + Xamarin.Essentials.AppInfo.VersionString;
            entBuild.Text = "Build : " + Xamarin.Essentials.AppInfo.BuildString;

            //if (VersionTracking.PreviousVersion != null) verpreviousVersion = VersionTracking.PreviousVersion.ToString();
            //if (VersionTracking.PreviousBuild != null) verpreviousBuild     = VersionTracking.PreviousBuild.ToString();
            //verfirstVersion = VersionTracking.FirstInstalledVersion.ToString();
            //verfirstBuild   = VersionTracking.FirstInstalledBuild.ToString();
            //foreach (string item in VersionTracking.VersionHistory)
            //    verversionHistory = item.ToString();
            //foreach (string item in VersionTracking.VersionHistory)
            //    verbuildHistory = item.ToString();
        }

        private void SetMemberDetails()
        {
            try
            {
                
                var member = MemberService.GetMember();
                entMemberName.Text = member.Name;
                entMemberSurname.Text = member.Surname;
                entMemberCellNo.Text = member.CellNo;
                entMemberEmail.Text = member.Email;
                entMemberCompanies.Text = JsonConvert.DeserializeObject<Dictionary<string, string>>(MemberService.GetMember().Clients).FirstOrDefault().Value; ;
                entMemberId.Text = member.Id.ToString();
            }
            catch
            {
                entMemberName.Text = "Error in SetMemberDetails()";
            }
        }
        private void SetSecureDetails()
        {
            try
            {
                entAccessToken.Text = SecureStorageService.AccessToken;
                entRefreshToken.Text = SecureStorageService.RefreshToken;
                entUsername.Text = SecureStorageService.Username;
                entExpireDate.Text = SecureStorageService.ExpireDate.ToString();
            }
            catch
            {
                entAccessToken.Text = "Error in SetSecureDetails";
            }
        }
        private void OnLabelClicked()
        {
            taps++;
            if (taps > 10)
            {
                SecureStorageService.IsDeveloperMode = true;
                developerStack.IsVisible = SecureStorageService.IsDeveloperMode;
                infoStack.IsVisible = !SecureStorageService.IsDeveloperMode; ;
                if (baseUrl == "BaseUrl")
                {
                    baseUrl = "BaseUrl1";
                }
                else
                {
                    baseUrl = "BaseUrl";
                }

                url.Text = Interon.Roadlab.Core.Environment.CMSBaseUrl;
                taps = 0;

            }
        }



        private void BtnDeleteSecureStorage_OnClicked(object sender, EventArgs e)
        {
            SecureStorageService.ClearStorage();
        }

        private void BtnDeleteMember_OnClicked(object sender, EventArgs e)
        {

            MemberService.DeleteMember();
        }

        private void BtnRegister_OnClicked(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PushModalAsync(new Registration.RegisterPage(), true);
            Shell.Current.FlyoutIsPresented = false;
        }

        private void BtnReRegister_OnClicked(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PushModalAsync(new ReRegisterPage(), true);
            Shell.Current.FlyoutIsPresented = false;
        }

        private void BtnNewRegister_OnClicked(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PushModalAsync(new Registration.NewRegisterPage("anton@nteron.co.za"), true);
            Shell.Current.FlyoutIsPresented = false;
        }

        private void BtnDisableDev_OnClicked(object sender, EventArgs e)
        {
            SecureStorageService.IsDeveloperMode = !SecureStorageService.IsDeveloperMode;
            developerStack.IsVisible = SecureStorageService.IsDeveloperMode;
            infoStack.IsVisible = SecureStorageService.IsDeveloperMode; ;
        }
    }
}