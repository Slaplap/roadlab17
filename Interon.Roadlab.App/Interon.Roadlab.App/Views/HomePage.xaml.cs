using Interon.Roadlab.App.Core;
using Interon.Roadlab.App.Core.Services;
using Interon.Roadlab.App.Core.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Interon.Roadlab.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class HomePage : ContentPage
    {
        private HomeViewModel _homeViewModel;
        public HomePage()
        {
            _homeViewModel = new HomeViewModel();
            BindingContext = _homeViewModel;
            InitializeComponent();



        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var a = Xamarin.Essentials.AppInfo.Version;
            var b = Xamarin.Essentials.AppInfo.BuildString;
            var c = Xamarin.Essentials.AppInfo.VersionString;
            var d = Xamarin.Essentials.AppInfo.PackageName;
            

            if ( !string.IsNullOrWhiteSpace(SecureStorageService.VersionString) && !string.IsNullOrWhiteSpace(SecureStorageService.BuildString))
            {

                if (Xamarin.Essentials.AppInfo.VersionString != SecureStorageService.VersionString || Xamarin.Essentials.AppInfo.BuildString != SecureStorageService.BuildString)
                {

                    if (!Globals.AskedForUpdate)
                    {
                        bool answer = await DisplayAlert("New Version", "A new version is available do you wish to download?", "Yes", "No");
                        if (answer)
                        {
                            AppInfo.ShowSettingsUI();
                            await Browser.OpenAsync(SecureStorageService.PlaystoreLink, BrowserLaunchMode.SystemPreferred);
                        }
                        else
                        {
                            Globals.AskedForUpdate = true;
                        }
                    }
                    if (SecureStorageService.ForceUpdate)
                    {
                        await DisplayAlert("New Version", "This is a critical update.Please Update app", "Update");
                       
                        await Browser.OpenAsync(SecureStorageService.PlaystoreLink, BrowserLaunchMode.SystemPreferred);
                    }
                    if (SecureStorageService.ForceReinstall )
                    {
                          await DisplayAlert("New Version", "Breaking Change, please uninstall the app and reinstall from the store","Close");
                          await Browser.OpenAsync(SecureStorageService.PlaystoreLink, BrowserLaunchMode.SystemPreferred);
                          Globals.CloseApp();
                    }
                }

            }
        }
    }
}