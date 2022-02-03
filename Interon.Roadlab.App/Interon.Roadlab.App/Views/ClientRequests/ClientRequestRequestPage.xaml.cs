using Interon.Roadlab.App.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Views.ClientRequests
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
  
    public partial class ClientRequestRequestPage : ContentPage
    {
        //private string _requestType;

        //public string RequestType
        //{
        //    get
        //    {
        //        return _requestType;
        //    }
        //    set
        //    {
        //        _requestType = value;
               
        //    }
        //}
        public ClientRequestRequestPage()
        {
            InitializeComponent(); 
             
            
        }
        //private void SetBindingContext(string transactionType)
        //{
        //    BindingContext = new ClientRequestRequestViewModel(transactionType);
        //}

        //protected override void OnAppearing()
        //{
        //   SetBindingContext(_requestType);
        //}
    }
}