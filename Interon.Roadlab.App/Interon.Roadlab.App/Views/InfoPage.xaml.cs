using Interon.Roadlab.App.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
       
      
        public InfoPage(ModalMessageViewModel modalMessageViewModel)
        {
            
            InitializeComponent();
            this.BindingContext = modalMessageViewModel;

        }
       // public ICommand GoBaCommand => new Command(()=> Shell.Current.Navigation.PopAsync(true));
       
    }
}