using Interon.Roadlab.App.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Views.Branches
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BranchListPage : ContentPage
    {

        public BranchListPage()
        {
            BindingContext = new  BranchViewModel();
            
           
            InitializeComponent();
            
        }
        
    }
}