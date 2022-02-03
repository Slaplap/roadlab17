using Interon.Roadlab.App.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Views.ClientRequests
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty("ClientRequestId","ClientRequestId")]
    [QueryProperty("ClientRequestType", "ClientRequestType")]
    public partial class ClientRequestComments : ContentPage
    {
        private int _requestId;
        private string _transactionType;

        public ClientRequestComments()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = new ClientRequestCommentsViewModel(RequestId,ClientRequestType);
        }

        public int RequestId
        {
            get => _requestId;
            set => _requestId = value;
        }

        public string ClientRequestType
        {
            get => _transactionType;
            set => _transactionType = value;
        }
    }
}