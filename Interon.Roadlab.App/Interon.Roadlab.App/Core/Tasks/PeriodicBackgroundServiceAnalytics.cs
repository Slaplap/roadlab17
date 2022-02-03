using Interon.Roadlab.App.Core.Services;
using Matcha.BackgroundService;
using System;
using System.Threading.Tasks;

namespace Interon.Roadlab.App.Core.Tasks
{
    public class PeriodicBackgroundServiceAnalytics : IPeriodicTask
    {


        public PeriodicBackgroundServiceAnalytics(int seconds)
        {
            Interval = TimeSpan.FromSeconds(seconds);
            SecureStorageService.PoleStartDate = new DateTime();
        }
        public PeriodicBackgroundServiceAnalytics()
        {

        }
        public TimeSpan Interval { get; set; }

        public async Task<bool> StartJob()
        {
            if (Globals.StoppedBackgroundServices.Contains(this.GetType().Name) || Globals.RunningServices.Contains(this.GetType().Name))
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
                

                if (SecureStorageService.HasAllLocalLoginCredentials())
                {
                    MemberOnlineService.UpdateDeviceId(SecureStorageService.Username, SecureStorageService.InstallId.ToString());
                }

               
            }
            finally
            {
                Globals.RunningServices.RemoveAll(x => x == this.GetType().Name);
            }

            return true;
             

        }
        private bool StopThisTask()
        {
            Globals.StoppedBackgroundServices.Add(this.GetType().Name);
            return false;
        }
    }

}