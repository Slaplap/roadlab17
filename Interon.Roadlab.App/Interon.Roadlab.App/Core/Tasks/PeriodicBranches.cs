using Interon.Roadlab.App.Core.Services;
using Matcha.BackgroundService;
using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core.Tasks
{
    public class PeriodicBranches : IPeriodicTask
    {


        public PeriodicBranches(int seconds)
        {
            Interval = TimeSpan.FromSeconds(seconds);
        }
        public PeriodicBranches()
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
                var connectionToServerAsync =  OnlineService.HasConnectionToServerAsync();
                if (!connectionToServerAsync)
                {
                    return true;
                }
                var isUptoDate = await BranchesOnlineService.IsUpToDate().ConfigureAwait(true);
                if (!isUptoDate)
                {

                    var branches = await BranchesOnlineService.GetBranches();
                    BranchesService branchesService = new BranchesService();
                    if (branches.Any())
                    {

                        branchesService.DeleteBranches();
                    }

                    foreach (var branchDto in branches)
                    {
                        var branch = branchesService.DtoToBranch(branchDto);
                        branchesService.CreateOrUpdateBranch(branch);

                    }
                    Location location;

                    try
                    {

                        location = await Geolocation.GetLastKnownLocationAsync().ConfigureAwait(true);
                        if (location == null)
                        {
                            return true;
                        }

                        foreach (var branch in branchesService.GetBranches())
                        {
                            try
                            {
                                var locationEnd = new Location(double.Parse(branch.Latitude, CultureInfo.InvariantCulture),
                                    double.Parse(branch.Longitude, CultureInfo.InvariantCulture));
                                var distance = Location.CalculateDistance(location, locationEnd, DistanceUnits.Kilometers);
                                if (distance > 0)
                                {
                                    branch.Distance = distance;
                                    branchesService.CreateOrUpdateBranch(branch);
                                }

                            }
                            catch
                            {
                                continue;
                            }



                        }
                    }
                    catch (FeatureNotSupportedException fnsEx)
                    {
                        return StopThisTask();
                    }
                    catch (FeatureNotEnabledException fneEx)
                    {

                    }
                    catch (PermissionException pEx)
                    {

                    }
                    catch (Exception ex)
                    {

                    }





                    var result = await BranchesOnlineService.SetUpToDate().ConfigureAwait(true);
                    isUptoDate = await BranchesOnlineService.IsUpToDate().ConfigureAwait(true);
                    MessagingCenter.Send<PeriodicBranches>(this, "NewBranches");
                    return StopThisTask();
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
    }

}
