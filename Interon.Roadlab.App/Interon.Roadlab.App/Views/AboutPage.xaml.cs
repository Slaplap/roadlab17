using System.Threading.Tasks;
using Microsoft.AppCenter;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial  class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
           
        }

        protected override async void OnAppearing()
        {
            await Init().ConfigureAwait(true);
        }

        private async Task Init()
        {
            var Id = await AppCenter.GetInstallIdAsync();
            lbl1.Text = Id.ToString();
            txt1.Text = Id.ToString();
        }
    }
}