using Interon.Roadlab.App.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core.Services
{
    public static class  NavigationService
    {
    public static ContentPage GetPageByConnectivity(ContentPage contentPage)
    {
        var current = Connectivity.NetworkAccess;

        if (current == NetworkAccess.Internet)
        {
            return contentPage;
        }
        else
        {
            return new NoConnectivityPage();
        }
    }
}
}
