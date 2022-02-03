using System;
using System.Threading.Tasks;
using Interon.Roadlab.App.Core.Models;
using Interon.Roadlab.App.Core.Services;
using Interon.Roadlab.App.Views.Registration;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core.ViewModels
{
    class ResetPasswordViewModel:ViewModelBase
    {
        private string _name;
        private string _password;
        private string _otpMessage;
        private string _cellnumber;
        private bool _noregistereduser;
        private string _otp;
        private string _email;

        public ResetPasswordViewModel()
        {
            SecureStorageService.FromUrl = "OTP";
             
            NoRegisteredUser = !MemberService.IsRegisteredOnLocalDevice();
            ErrorHandler eh = new ErrorHandler();
            this.IsBusy = false;
            Commands.Add("CommandCancel", new Command(CommandCancel));
            Commands.Add("CommandSubmit", new AsyncCommand(CommandSubmit, allowsMultipleExecutions: false));
            Commands.Add("CommandOTP", new AsyncCommand(CommandOTP, allowsMultipleExecutions: false));
            
            Name = "Welcome Back " + MemberService.GetMember().Name;
            if (NoRegisteredUser)
            {
                OTPMessage = "Enter One Time Pin (Click on request OTP)";
            }
            else
            {

              OTPMessage = "Please enter OTP(One Time Pin) sent to your mobile device with number ending with : " + MemberService.GetMember().CellNo.Substring(MemberService.GetMember().CellNo.Length - 4, 4);
            }
        }


        private async Task CommandOTP()
        {
            IsBusy = true;
            try
            {
                if (string.IsNullOrWhiteSpace(Email))
                {
                    DependencyServiceService.LongMessage("Email is required to request OTP");
                    return;
                }

                if (NoRegisteredUser)
                {
                    await MemberOnlineService.RequestOTP(Email.Trim());
                }
                else
                {
                     
                    await MemberOnlineService.RequestOTP(MemberService.GetMember().Email.Trim());
                } 
                await Application.Current.MainPage.DisplayAlert("OTP Request", "Your request has been sent and you should be receiving an sms shortly.", "Ok");
            }
            catch (Exception ex)
            {
               ErrorService.ErrorExceptionAndAnalyticsAndModal("Error Requesting OTP","CommandOTP",ex);
            }

            IsBusy = false;
        }

        private async Task CommandSubmit()
        {
            
            IsBusy = true;
            //give back to the main thread so IsBusy can change
            await Task.Delay(10);
            if (string.IsNullOrWhiteSpace(Password))
            {
                DependencyServiceService.LongMessage("Password cannot be blank");
                IsBusy = false;
                //give back to the main thread so IsBusy can change
                await Task.Delay(10);
                return;
            }
            if (string.IsNullOrWhiteSpace(OTP.ToString()))
            {
                DependencyServiceService.LongMessage("OTP cannot be blank");
                IsBusy = false;
                //give back to the main thread so IsBusy can change
                await Task.Delay(10);
                return;
            }
            
            var success = await MemberOnlineService.SetMemberPassword(Email, Password, OTP);
            if (!success)
            {
                ErrorService.EventAndAnalyticsAndModal("Error Resetting Password",AnalyticsService.EventCategory.Info);
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
                        IsBusy = false;
                        await Task.Delay(10);
                        return;
                    }
                    else
                    {
                        hasValidCredentials =
                            await AuthenticationService.HasValidCredentialsAsync().ConfigureAwait(true);
                        if (hasValidCredentials)
                        {
                            IsBusy = false;
                            Globals.StartAppShell();
                            return;
                        }
                    }

                }
                catch (Exception er)
                {
                    IsBusy = false;
                    Application.Current.MainPage = new RegisterPage();
                    ErrorService.ErrorExceptionAndAnalyticsAndModal("Error logging in", "ReRegisterPage - BtnContinue_OnClickedAsync", er);
                    return;
                }

            }

            IsBusy = false;
            Globals.StartAppShell();
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

        public string CellNumber
        {
            get => _cellnumber;
            set => SetProperty(ref _cellnumber, value);

        }
        public bool NoRegisteredUser
        {
            get => _noregistereduser;
            set => SetProperty(ref _noregistereduser, value);

        }
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        public string OTPMessage
        {
            get => _otpMessage;
            set => SetProperty(ref _otpMessage, value);
        }
        public string OTP
        {
            get => _otp;
            set => SetProperty(ref _otp, value);
        }
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

       
 
    }
}
