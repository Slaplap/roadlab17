using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Interon.Roadlab.App.Core.Const;
using Interon.Roadlab.App.Core.Models;
using Interon.Roadlab.App.Core.Services;
using Interon.Roadlab.LIMS.DTO;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core.ViewModels
{
    public class xSite
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
    class ClientRequestSiteViewModel : ViewModelBase
    {
        private xSite _setSiteDropdownSelectedValue;
        private bool _isSubmitEnabled;
        private string _searchString;
        private bool _IsSearcButtonEnabled;
        public ObservableCollection<xSite> Sites { get; set; } = new ObservableCollection<xSite>();
        public xSite SetSiteDropdownSelectedValue
        {
            get => _setSiteDropdownSelectedValue;
            set
            {
                SetProperty(ref _setSiteDropdownSelectedValue, value);
                MessagingCenter.Send(value,MessagingCenterValues.SiteChange);
                Shell.Current.Navigation.PopAsync();
            }
        }



        public bool IsSubmitEnabled
        {
            get => _isSubmitEnabled;
            set
            {
                SetProperty(ref _isSubmitEnabled, value);
                
            }
        }
        public bool IsSearcButtonEnabled
        {
            get => _IsSearcButtonEnabled;
            set
            {
                SetProperty(ref _IsSearcButtonEnabled, value);

            }
        }
        public string SearchString
        {
            get => _searchString;
            set
            {
                SetProperty(ref _searchString, value);
                if (value.Length > 4)
                {
                    IsSearcButtonEnabled = true;
                }
            }
        }
        public ClientRequestSiteViewModel(ObservableCollection<ClientRequest> clientRequest)
        {
            Commands.Add("SiteSearchCommand", new AsyncCommand(SiteSearchCommand, allowsMultipleExecutions: false));
            Commands.Add("SubmitCommand", new AsyncCommand(SubmitCommand, allowsMultipleExecutions: false));
            Commands.Add("CancelCommand", new AsyncCommand(CancelCommand, allowsMultipleExecutions: false));
            
            MessagingCenter.Subscribe<object>(this, MessagingCenterValues.SitesChange, (sender) =>
            {
                LoadSites();

            });
            MessagingCenter.Subscribe<Site>(this, MessagingCenterValues.SiteChange, (sender) =>
            {
                Site site = sender;
                MessagingCenter.Send(new xSite(){Key = site.Id,Value = site.SiteName}, MessagingCenterValues.SiteChange);
                Shell.Current.Navigation.PopAsync();

            });
            LoadSites();
        }

        private async Task CancelCommand()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        private async  Task SubmitCommand()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        private async Task SiteSearchCommand()
        { 
           await Busy();
           await Shell.Current.GoToAsync($"SiteListPage?SearchString={SearchString}");
            await NotBusy();
        }

        private void LoadSites()
        {
            if (SecureStorageService.Sites == null)
            {
                return;
            }
            foreach (var rootSite in SecureStorageService.Sites.Sites)
            {
                var item = new xSite();
                item.Key = rootSite.Id;
                if(string.IsNullOrWhiteSpace(rootSite.Name))
                {
                    try
                    {
                        item.Value =   rootSite.Text.ToString();
                    }
                    catch
                    {
                        item.Value = "-------------------";
                    }
                }
                else
                {
                    item.Value = rootSite.Name;
                }
                
                Sites.Add(item);
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}