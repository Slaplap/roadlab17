using Interon.Roadlab.App.Core;
using Interon.Roadlab.App.Core.Services;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Views.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewRegisterPage : ContentPage
    {
        private string _email = "";
        public bool Running { get; set; }

        public NewRegisterPage(string email)
        {
            _email = email;

            InitializeComponent();
            this.BindingContext = this;
            this.IsBusy = false;
            lblHeading.Text = "Registration for : " + email;

            //txtName.Text =  "Piet";
            //txtSurname.Text = "Pompies";
            //txtCellNo.Text = "0833267925";
            //txtAccountNumber.Text = "0001";
        }

        private void BtnCancel_OnClicked(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PopModalAsync();
        }

        private bool ValidateSetError()
        {
            txtAccountNumberError.Text = "";
            txtCellNoError.Text = "";
            txtNameError.Text = "";
            txtPasswordError.Text = "";
            txtSurnameError.Text = "";
            var count = 0;
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                txtNameError.Text = "Field Required";
                count += 1;
            }
            if (string.IsNullOrWhiteSpace(txtSurname.Text))
            {
                txtSurnameError.Text = "Field Required";
                count += 1;
            }
            if (string.IsNullOrWhiteSpace(txtAccountNumber.Text))
            {
                txtAccountNumberError.Text = "Field Required";
                count += 1;
            }
            if (string.IsNullOrWhiteSpace(txtCellNo.Text))
            {
                txtCellNoError.Text = "Field Required";
                count += 1;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPasswordError.Text = "Field Required";
                count += 1;
            }

            if (!string.IsNullOrWhiteSpace(txtCellNo.Text))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtCellNo.Text, "^[0-9]*$"))
                {
                    txtCellNoError.Text = "Field Required Format 0831234567";
                    count += 1;
                }
                if (txtCellNo.Text.Substring(0, 1) != "0")
                {
                    txtCellNoError.Text = "Field Required Format 0831234567";
                    count += 1;
                }
            }

            if (count > 0)
            {
                return false;
            }

            return true;
        }
        private void Focus()
        {
             
            img1.IsVisible = !img1.IsVisible;
            img2.IsVisible = !img2.IsVisible;
            
        }
        private async void BtnContinue_OnClicked(object sender, EventArgs e)
        {
            this.IsBusy = true;

            

            var hasValidCredentials = await AuthenticationService.HasValidCredentialsAsync().ConfigureAwait(false);
            if (hasValidCredentials)
            {
                Globals.StartAppShell();
            }

            if (!ValidateSetError())
            {
                this.IsBusy = false;
                return;
            }
            if (!hasValidCredentials)
            {
                try
                {
                    var memberResult = await MemberOnlineService.CreateMember(_email, txtName.Text, txtSurname.Text,
                        txtAccountNumber.Text, txtCellNo.Text, txtPassword.Text).ConfigureAwait(true);
                    if (memberResult == null)
                    {
                        Core.ErrorService.EventAndAnalyticsAndModal("Error Creating Member", AnalyticsService.EventCategory.Error);
                    }
                    else
                    {
                        var loginResult = await AuthenticationService.Login(_email, txtPassword.Text).ConfigureAwait(true);
                        if (!loginResult)
                        {
                            Core.ErrorService.EventAndAnalytics("Error Logging in from registration", AnalyticsService.EventCategory.Error);
                            this.IsBusy = false;
                            return;
                        }
                        else
                        {
                            
                            ClientsService clientsService = new ClientsService();
                            MemberService.CreateOrUpdateMember(MemberService.DtoToMember(memberResult));
                            var companyDto = await LIMS.Services.ClientsAndContactsService.ClientByAccountNumberAsync(memberResult.Clients.FirstOrDefault().AccountNumber);
                            clientsService.CreateOrUpdateCompany(clientsService.DtoToCompany(companyDto));
                            SecureStorageService.ClientAccountNumber = companyDto.AccountNumber;
                            SecureStorageService.ClientId = companyDto.Id;
                            Globals.StartAppShell();
                        }
                    }
                }
                catch (Exception er)
                {
                    this.IsBusy = false;
                    Core.ErrorService.ErrorExceptionAndAnalyticsAndModal("Error Registering", "NewReregisterPage BtnContinue_OnClicked", er);
                    return;
                }
            }

            this.IsBusy = false;
        }

        private void FocusEvent(object sender, FocusEventArgs e)
        {
            Focus();
        }
    }
}