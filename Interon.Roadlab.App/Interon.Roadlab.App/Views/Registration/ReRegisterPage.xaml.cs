using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Views.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReRegisterPage : ContentPage
    {
        public ReRegisterPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.NavigationProxy.PopToRootAsync();
            return base.OnBackButtonPressed();
        }
        private void Focus()
        {

            img1.IsVisible = !img1.IsVisible;
            img2.IsVisible = !img2.IsVisible;

        }
        private void FocusEvent(object sender, FocusEventArgs e)
        {
            Focus();

        }
    }
}