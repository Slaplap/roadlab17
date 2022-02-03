using System;
using System.Diagnostics;
using System.Text;
using Interon.Roadlab.App.Core.Tasks;
using Matcha.BackgroundService;

namespace Interon.Roadlab.App.Core.Services
{
    public static class TaskService
    {
        public static void Init()
        {
            try
            {

                 
                BackgroundAggregatorService.Instance.Clear();

                BackgroundAggregatorService.Add(() => new PeriodicServerAvailability(Globals.PeriodicServerAvailabilityInterval));
                BackgroundAggregatorService.Add(() => new PeriodicAppSettings(Globals.PeriodicAppSettingsInterval));
                BackgroundAggregatorService.Add(() => new PeriodicBranches(Globals.PeriodicBranchesInterval));
                BackgroundAggregatorService.Add(() => new PeriodicBranchesDistance(Globals.PeriodicBranchDisctanceInterval));
                BackgroundAggregatorService.Add(() => new PeriodicBackgroundServiceAnalytics(Globals.PeriodicBackgroundServiceInterval));
                BackgroundAggregatorService.Add(() => new PeriodicRequestsByClientAndContact(Globals.PeriodicClientRequestsInterval));
                BackgroundAggregatorService.Add(() => new PeriodicNotifications(Globals.PeriodicNotificationsInterval));
                BackgroundAggregatorService.Add(() => new PeriodicSites(Globals.SitesInterval));
                BackgroundAggregatorService.Add(() => new PeriodicCategories(Globals.CategoriesInterval));

                BackgroundAggregatorService.StopBackgroundService();
                BackgroundAggregatorService.StartBackgroundService();
            }
            catch (Exception e)
            {
                Debugger.Break();
            }
        }

       

        public static void Stop()
        {
         
            BackgroundAggregatorService.StopBackgroundService();
            
        }
        public static void Start()
        {

            BackgroundAggregatorService.StartBackgroundService();

        }

       
    }
}
