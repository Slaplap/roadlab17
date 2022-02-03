using Interon.Roadlab.App.Core;
using Interon.Roadlab.App.Core.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Views.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogOut : ContentPage
    {
        public LogOut()
        {
            InitializeComponent();
            AuthenticationService.LogOut();
            Globals.NewApp();
            Application.Current.MainPage = new RegisterPage();
           


        }
    }
}