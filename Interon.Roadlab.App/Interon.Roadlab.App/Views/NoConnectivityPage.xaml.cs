using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoConnectivityPage : ContentPage
    {
        public NoConnectivityPage()
        {
            InitializeComponent();
        }

        private void Back_OnClicked(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PopModalAsync();
        }
    }
}