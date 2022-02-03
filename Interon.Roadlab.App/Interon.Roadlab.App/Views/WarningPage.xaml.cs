using Interon.Roadlab.App.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WarningPage : ContentPage
    {
       
      
        public WarningPage(ModalMessageViewModel modalMessageViewModel)
        {
            this.BindingContext = modalMessageViewModel;
            InitializeComponent();
           

        }
       // public ICommand GoBaCommand => new Command(()=> Shell.Current.Navigation.PopAsync(true));
       
    }
}