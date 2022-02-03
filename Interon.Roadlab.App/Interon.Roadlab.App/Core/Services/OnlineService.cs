using System.Threading.Tasks;

using Interon.Roadlab.App.Views;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core.Services
{
   

    public static class OnlineService
    {
        private static string _BASEURL = Interon.Roadlab.Core.Environment.CMSBaseUrl;
        
        private static readonly int ATTEMPTS = Globals.NumberOfConnectionLoops;

        public static string BASEURL
        {
            get
            {
                _BASEURL = Interon.Roadlab.Core.Environment.CMSBaseUrl;
                return _BASEURL;
            }
        }
        public static string PATH
        {
            get
            {
                return Interon.Roadlab.Core.Environment.CMSAPI;
            }
        }
        public static string TOKEN
        {
            get
            {
                return Interon.Roadlab.Core.Environment.CMSAPI_TOKEN_ENDPOINT;
            }
        }

      

        public static bool HasInternet()
        {
            var current = Connectivity.NetworkAccess;
             
            if (current != NetworkAccess.Internet)
            {
                try
                {
                    Shell.Current.Navigation.PushModalAsync(new NoConnectivityPage());
                }
                catch
                {

                }

                return false;
            }

            return true;
        }
        public static bool HasConnectionToServerAsync()
        {
           

            return Globals.IsServerAvaiable;


        }


      


    }
}
