using Interon.Roadlab.App.Core;
using Interon.Roadlab.App.Core.Services;
using Interon.Roadlab.App.Core.Tasks;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Threading.Tasks;
using Interon.Roadlab.App.Core.Const;
using Interon.Roadlab.App.Core.ViewModels;
using Interon.Roadlab.App.Views;
using Interon.Roadlab.App.Views.Registration;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Application = Xamarin.Forms.Application;
using RegisterPage = Interon.Roadlab.App.Views.Registration.RegisterPage;

namespace Interon.Roadlab.App
{
    public partial class App : Application
    {
        public static bool IsRootPage { get; set; }
        public static bool CanExit { get; set; }

        public App()
        {

            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            InitializeComponent();

            Context.Instance.IsLocal = true;
            MessagingCenter.Subscribe<object>(this,MessagingCenterValues.PeriodicServiceRestart, (sender) =>
            {
                TaskService.Init();
            });
            MessagingCenter.Subscribe<ErrorMessageViewModel>(this, MessagingCenterValues.Error, (sender) =>
            {
                  var error = new ErrorPage(new ErrorMessageViewModel() { Message = sender.Message });
            });
        }

        private async Task OpenPageBasedOnCredentialsAsync()
        {
            try
            {
                var hasCredentials = SecureStorageService.HasAllLocalLoginCredentials();
                // var hasCredentials = await AuthenticationService.HasValidCredentialsAsync().ConfigureAwait(true);
                if (!hasCredentials)
                {
                    MainPage = new RegisterPage();
                }
                else
                {
                    Globals.StartAppShell();
                }
            }
            catch (System.AggregateException ae)
            {
                ErrorService.ErrorExceptionAndAnalyticsAndModal("Error Starting Up", "OpenPageBasedOnCredentialsAsync()", ae);
            }
            catch (Exception e)
            {
                ErrorService.ErrorExceptionAndAnalyticsAndModal("Error Starting Up", "OpenPageBasedOnCredentialsAsync()", e);
            }
        }


        protected override async void OnStart()
        {
            await OpenPageBasedOnCredentialsAsync().ConfigureAwait(true);
            Globals.NewApp();
            await PeriodicServerAvailability.CheckServer();
            TaskService.Init();
            
            await InitAppCenterAsync();
        }


        




        private async Task InitAppCenterAsync()
        {
            await Task.Run(() => InitAppCenter());
        }

        private async Task InitAppCenter()
        {
            if (!AppCenter.Configured)
            {

            }
            try
            {
                //AppCenter.Start("android=0792e7ae-3cc3-4042-b961-2047130ba85d;" +

                //                "ios=11b35ddd-a3f7-4bbb-bef3-5e07d23b5938;",
                //    typeof(Analytics), typeof(Crashes), typeof(Distribute), typeof(Push));

                AppCenter.Start("android=0792e7ae-3cc3-4042-b961-2047130ba85d;" +

                                "ios=11b35ddd-a3f7-4bbb-bef3-5e07d23b5938;",
                    typeof(Analytics), typeof(Crashes));


                AppCenter.SetUserId(SecureStorageService.Username ?? "newuser");
                SecureStorageService.InstallId = await AppCenter.GetInstallIdAsync();
                if (!string.IsNullOrWhiteSpace(Globals.NotificationHubToken))
                {
                    SecureStorageService.NotificationHubToken = Globals.NotificationHubToken;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected override void OnSleep()
        {
            TaskService.Stop();
        }

        protected override void OnResume()
        {
            if (SecureStorageService.FromUrl == "OTP")
            {
                MainPage = new ResetPasswordPage();
            }

            if (MemberService.IsRegisteredOnLocalDevice())
            {
                TaskService.Start();
                Globals.StartAppShell();
            }
        }
    }
}