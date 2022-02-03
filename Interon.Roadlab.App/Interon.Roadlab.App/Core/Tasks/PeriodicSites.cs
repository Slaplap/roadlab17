using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interon.Roadlab.App.Core.Const;
using Interon.Roadlab.App.Core.Services;
using Interon.Roadlab.LIMS.Services;
using Matcha.BackgroundService;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core.Tasks
{
    public class PeriodicSites : IPeriodicTask
    {
        public PeriodicSites(int seconds)
        {
            Interval = TimeSpan.FromSeconds(seconds);
        }

        public TimeSpan Interval { get; set; }

        public async Task<bool> StartJob()
        {
            if (Globals.StoppedBackgroundServices.Contains(this.GetType().Name) || Globals.RunningServices.Contains(this.GetType().Name))
            {
                return true;
            }
            if (!SecureStorageService.HasAllLocalLoginCredentials())
            {
                return true;
            }
            if (!OnlineService.HasInternet())
            {
                return true;
            }


            try
            {
                Globals.RunningServices.Add(this.GetType().Name);

                await GetSites();
                MessagingCenter.Send<object>(new Object(), MessagingCenterValues.SitesChange);
                return StopThisTask();
            }
            finally
            {
                Globals.RunningServices.RemoveAll(x => x == this.GetType().Name);
            }

        }

        private async Task  GetSites()
        {
            SecureStorageService.Sites = await LIMSOnlineService.SitesByClientOrSearch(SecureStorageService.ClientId);
        }

        private bool StopThisTask()
        {
            Globals.StoppedBackgroundServices.Add(this.GetType().Name);
            return false;
        }
    }
}