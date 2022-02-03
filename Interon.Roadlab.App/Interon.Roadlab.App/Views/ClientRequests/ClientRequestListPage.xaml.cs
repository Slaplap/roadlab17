using Interon.Roadlab.App.Core.ViewModels;
using Interon.Roadlab.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Views.ClientRequests
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty("ClientRequestStatus", "ClientRequestStatus")]
    public partial class ClientRequestListPage : ContentPage
    {
         
        private string _clientRequestStatus;


        public ClientRequestListPage()
        {
            InitializeComponent();
           
        }

        public string ClientRequestStatus
        {
            get => _clientRequestStatus;
            set => _clientRequestStatus = value;
        }

        protected override void OnAppearing()
        {
            BindingContext = new ClientRequestListViewModel(ClientRequestStatus.URLDecode());
        }
    }
}