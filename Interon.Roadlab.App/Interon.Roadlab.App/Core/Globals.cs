using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Interon.Roadlab.App.Core.Services;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core
{
    public static class Globals
    {
        private static bool _isServerAvaiable = false;
        public static bool IsCMSAvailable;
        public static int Timeout { get; set; } = 120;
        public static int NumberOfConnectionLoops { get; set; } = 2;
        public static string NotificationHubToken { get; set; }

        public static bool IsServerAvaiable
        {
            get => _isServerAvaiable;
            set
            {
                if (value == true)
                {
                    Globals.StoppedBackgroundServices.Clear();
                }
                _isServerAvaiable = value;
            }
        }

        public static bool IsLIMSAvaiable { get;set; } = false;
        public static void StartAppShell()
        {

            Application.Current.MainPage = new AppShell();

        }

        public static List<string> StoppedBackgroundServices { get; set; } = new List<string>();
        public static List<string> RunningServices { get; set; } = new List<string>();
        public static int PeriodicServerAvailabilityInterval { get; set; } = 5;
        public static int PeriodicAppSettingsInterval { get; set; } = 5;
        public static int PeriodicBranchesInterval { get; set; } = 5;
        public static int PeriodicBranchDisctanceInterval { get; set; } = 5;
        public static int PeriodicBackgroundServiceInterval { get; set; } = 5;
        public static int PeriodicClientRequestsInterval { get; set; } = 5;
        public static int PeriodicNotificationsInterval { get; set; } = 5;
        public static bool AskedForUpdate { get; set; }
        public static int SitesInterval { get; set; } = 5;
        public static int CategoriesInterval { get; set; } = 5;

        public static void NewApp()
        {
            
            ClientsService   cs  = new ClientsService();
            DataService        ass = new DataService();
            RequestService ts  = new RequestService();

            if (string.IsNullOrWhiteSpace(SecureStorageService.Username))
            {
                DependencyServiceService.ShortMessage("New Installation", true);
                MemberService.DeleteMember();
                SecureStorageService.ClearStorage();
                cs.DeleteAll();
                ass.DeleteTestCategories();
                ass.DeleteTests();
                ts.DeleteClientRequests();
                SecureStorageService.ShowFavourites    = true;
                SecureStorageService.ShowBookings      = true;
                SecureStorageService.ShowQuotes        = true;
                SecureStorageService.ShowNotifications = true;

            }
        }
        public static void CloseApp()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {

#if __ANDROID__
                    var activity = (Android.App.Activity)Forms.Context;
                    activity.FinishAffinity();
#endif
#if __IOS__
                    Thread.CurrentThread.Abort();
#endif
            });


        }
    }

    public enum BackgroundServices
    {
        PeriodicAppSettings,
        PeriodicBackgroundServiceAnalytics,
        PeriodicBranches,
        PeriodicClients,
        PeriodicServerAvailability,
        PeriodicBranchesDistance,
        PeriodicRequestsByClient,
        PeriodicRequestsByClientAndContact,
        PeriodicSites,
        PeriodicCategories

    }


}
