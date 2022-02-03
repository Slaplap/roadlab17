using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage2 : ContentPage
    {

        public HomePage2()
        {
            InitializeComponent();

            BindingContext = this;
            MyVersion.Text = $"  {Xamarin.Essentials.AppInfo.VersionString} {Xamarin.Essentials.AppInfo.BuildString} ";



            // AnimationView.Play();
        }




        private void ProfileClick(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//profilePage");


        }

        private void BranchClick(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//branchListPage");
        }

        private void NotificationsClick(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//notificationListPage");
        }

        private void ClientRequestRequestClick(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//ClientRequestRequestPage");
        }

        private void QuoteClick(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//quotesListPage");
        }
    }
}