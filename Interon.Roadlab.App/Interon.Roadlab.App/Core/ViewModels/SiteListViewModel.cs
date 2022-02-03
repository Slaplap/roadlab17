using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Interon.Roadlab.App.Core.Const;
using Interon.Roadlab.LIMS.DTO;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core.ViewModels
{
    [QueryProperty(nameof(SearchString), "SearchString")]
    class SiteListViewModel : ViewModelBase
    {
        public SiteListViewModel()
        {
            Commands.Add("SiteSearchCommand", new AsyncCommand(SiteSearchCommand, allowsMultipleExecutions: false));
            Commands.Add("SiteSelectedCommand", new AsyncCommand<int>(SiteSelectedCommand, allowsMultipleExecutions: false));
        }

        private  async Task SiteSelectedCommand(int arg)
        {
            Site site = Sites.Where(x => x.Id == arg).FirstOrDefault();

            MessagingCenter.Send(site,MessagingCenterValues.SiteChange);
            await Shell.Current.Navigation.PopAsync();

        }

        private async Task SiteSearchCommand()
        {
            await SearchSites(SearchString);
        }

        private string _SearchString;
        private bool _isSubmitEnabled;
        private string _SiteSearch;
        public ObservableCollection<Site> Sites { get; set; } = new ObservableCollection<Site>();
        public bool IsSearchButtonEnabled
        {
            get => _isSubmitEnabled;
            set
            {
                SetProperty(ref _isSubmitEnabled, value);

            }
        }

        protected override async  void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "SearchString")
            {
                try
                {

                    await SearchSites(SearchString);
                }
                catch
                {
                    throw;
                }
            }

        }

        public string SearchString
        {
            get => _SearchString;
            set
            {
                SetProperty(ref _SearchString, value);
                SiteSearch = value;
            }
        }
        public string SiteSearch
        {
            get => _SiteSearch;
            set
            {
                SetProperty(ref _SiteSearch, value);

            }
        }
        public async Task SearchSites(string searchString)
        {
            var sites = await LIMS.Services.ServicesService.SiteByClientOrSearchAsync(searchString);
            foreach (var sitesSite in sites.Sites)
            {
                Sites.Add(sitesSite);
            }
        }
    }
}