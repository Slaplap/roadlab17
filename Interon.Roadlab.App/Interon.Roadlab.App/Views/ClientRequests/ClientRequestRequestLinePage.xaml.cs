using System.Collections.ObjectModel;
using Interon.Roadlab.App.Core.Models;
using Interon.Roadlab.App.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Views.ClientRequests
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty("requestType", "requestType")]
    public partial class ClientRequestRequestLinePage : ContentPage
    {
        public string requestType { get; set; }
        public ClientRequestRequestLinePage(ObservableCollection<ClientRequest> clientRequest)
        {
            this.BindingContext = new ClientRequestRequestLineViewModel(clientRequest);
            InitializeComponent();
         //  Shell.Current.Navigation.PopModalAsync(true);
           
        }
    }
}