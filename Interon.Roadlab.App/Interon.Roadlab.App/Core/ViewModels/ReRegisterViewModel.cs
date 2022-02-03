using System;
using System.Threading.Tasks;
using Interon.Roadlab.App.Core.Services;
using Interon.Roadlab.App.Core.ViewModels;
using Interon.Roadlab.App.Views.Registration;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core.Models
{
    
    class ReRegisterViewModel:ViewModelBase
    {
        private string _name;
        private string _password;

        public ReRegisterViewModel()
        {
             ErrorHandler eh = new ErrorHandler();
            this.IsBusy = false;
            Commands.Add("CommandCancel", new Command(CommandCancel));
            Commands.Add("CommandSubmit", new AsyncCommand(CommandSubmit, allowsMultipleExecutions: false));
            Commands.Add("CommandForget", new AsyncCommand(ForgetSubmit, allowsMultipleExecutions: false));
            //   this.txtPassword.Text = "~~!1nter0n";
            
            //if (!MemberService.IsRegisteredOnLocalDevice())
            //{

            //    ErrorService.EventAndAnalyticsAndModal("User Not Registered",AnalyticsService.EventCategory.Warning);

            //    return;
            //}

            Name = "Welcome Back " + MemberService.GetMember().Name;
        }

        private async Task ForgetSubmit()
        {
          await  App.Current.MainPage.Navigation.PushModalAsync(new ResetPasswordPage());
        }


        private async Task CommandSubmit()
        {
          
            try
            {
                await Busy();
                if (string.IsNullOrWhiteSpace(Password))
                {
                    DependencyServiceService.LongMessage("Password cannot be blank");
                    await NotBusy();
                    return;
                }

             

                var hasValidCredentials = await AuthenticationService.HasValidCredentialsAsync().ConfigureAwait(true);
                if (!hasValidCredentials)
                {

                    try
                    {
                        var loginResult = await AuthenticationService.Login(MemberService.GetMember().Email, Password).ConfigureAwait(true);
                        if (!loginResult)
                        {
                            DependencyServiceService.LongMessage("Error Logging in");
                            await NotBusy();
                            return;
                        }
                        else
                        {
                            hasValidCredentials =
                                await AuthenticationService.HasValidCredentialsAsync().ConfigureAwait(true);
                            if (hasValidCredentials)
                            {
                                await NotBusy();
                                Globals.StartAppShell();
                                return;
                            }
                        }

                    }
                    catch (Exception er)
                    {
                        await NotBusy();
                        Application.Current.MainPage = new RegisterPage();
                        ErrorService.ErrorExceptionAndAnalyticsAndModal("Error logging in", "ReRegisterPage - BtnContinue_OnClickedAsync", er);
                        return;
                    }

                }

                Globals.StartAppShell();

            }
            finally
            {
                await NotBusy();
            }
           
        }

       
        private void CommandCancel()
        {
            App.Current.MainPage.Navigation.PopModalAsync();
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

       
 
    }
}
