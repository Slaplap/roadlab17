using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Views.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResetPasswordPage : ContentPage
    {
        public ResetPasswordPage()
        {
            InitializeComponent();
        }
        private void Focus()
        {

            img1.IsVisible = !img1.IsVisible;
            img2.IsVisible = !img2.IsVisible;

        }
        protected override bool OnBackButtonPressed()
        {
            Application.Current.NavigationProxy.PopToRootAsync();
            return base.OnBackButtonPressed();
        }

        private void FocusEvent(object sender, FocusEventArgs e)
        {
           Focus();
        }
    }
}