using System.Collections.ObjectModel;
using Interon.Roadlab.App.Core.Models;
using Interon.Roadlab.App.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Views.ClientRequests
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class SiteListPage : ContentPage
    {
       
      
        
        public SiteListPage()
        {
            this.BindingContext = new SiteListViewModel();
            InitializeComponent();
           


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