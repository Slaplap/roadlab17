using Interon.Roadlab.App.Core.Services;
using Matcha.BackgroundService;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Interon.Roadlab.App.Core.Tasks
{
    public class PeriodicAppSettings : IPeriodicTask
    {


        public PeriodicAppSettings(int seconds)
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
            if (!OnlineService.HasInternet())
            {
                return true;
            }


            try
            {
                Globals.RunningServices.Add(this.GetType().Name);
                var connectionToServerAsync =  OnlineService.HasConnectionToServerAsync();
                if (!connectionToServerAsync)
                {
                    return true;
                }
                var isUptoDate = await AppSettingsOnlineService.IsUpToDate().ConfigureAwait(true);

                if (!isUptoDate)
                {
                    await Job();
                    var result = await AppSettingsOnlineService.SetUpToDate().ConfigureAwait(true);
                    isUptoDate = await AppSettingsOnlineService.IsUpToDate().ConfigureAwait(true);
                    if (isUptoDate)
                    {
                        return StopThisTask();
                    }

                }

                return true;
            }
            finally
            {
                Globals.RunningServices.RemoveAll(x => x == this.GetType().Name);
            }

        }
        private bool StopThisTask()
        {
            Globals.StoppedBackgroundServices.Add(this.GetType().Name);
            return false;
        }
        public static async Task<bool> Job()
        {
            try
            {
                Globals.RunningServices.Add("PeriodicAppSettings");
                DataService ass = new DataService();

                var appsettings = await AppSettingsOnlineService.GetAppSettings();




                SecureStorageService.PlaystoreLink = appsettings.PlaystoreLink;
                SecureStorageService.AppleLink = appsettings.AppleLink;
                SecureStorageService.VersionString = appsettings.VersionString;
                SecureStorageService.BuildString = appsettings.BuildString;
                SecureStorageService.ForceUpdate = appsettings.ForceUpdate;
                SecureStorageService.ForceReinstall = appsettings.ForceReinstall;
            }
            catch
            {
                return false;
            }
            finally
            {
                Globals.RunningServices.RemoveAll(x => x == "PeriodicAppSettings");
            }

            return true;
        }
    }

}
