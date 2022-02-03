using System.Collections.ObjectModel;
using Interon.Roadlab.App.Core.Models;
using Interon.Roadlab.App.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Views.ClientRequests
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class ClientRequestSitePage : ContentPage
    {
        private bool _quickFillListShow;
        public string requestType { get; set; }
        public ClientRequestSitePage(ObservableCollection<ClientRequest> clientRequest)
        {
            this.BindingContext = new ClientRequestSiteViewModel(clientRequest);
            InitializeComponent();
            SearchButton.IsEnabled = false;


        }


       
        private void Select_OnCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            SitePicker.IsEnabled = Select.IsChecked;
            SiteSearch.IsEnabled = Search.IsChecked;
            SiteSearch.Focus();

        }


        private void SiteSearch_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (SiteSearch.Text.Length < 4)
            {
                SearchButton.IsEnabled = false;
                
            }
            else
            {

                SearchButton.IsEnabled = true;
            }
        }
    }
}