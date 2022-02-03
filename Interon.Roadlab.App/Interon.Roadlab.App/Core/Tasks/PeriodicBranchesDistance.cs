using Interon.Roadlab.App.Core.Services;
using Matcha.BackgroundService;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Interon.Roadlab.App.Core.Const;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core.Tasks
{
    public class PeriodicBranchesDistance : IPeriodicTask
    {


        public PeriodicBranchesDistance(int seconds)
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

            try
            {
                Globals.RunningServices.Add(this.GetType().Name);
                Location location;
                BranchesService branches = new BranchesService();
                try
                {

                    location = await Geolocation.GetLastKnownLocationAsync().ConfigureAwait(true);
                    if (location == null)
                    {
                        return true;
                    }

                    foreach (var branch in branches.GetBranches())
                    {
                        try
                        {
                            var locationEnd = new Location(double.Parse(branch.Latitude, CultureInfo.InvariantCulture),
                                double.Parse(branch.Longitude, CultureInfo.InvariantCulture));
                            var distance = Location.CalculateDistance(location, locationEnd, DistanceUnits.Kilometers);
                            if (distance > 0)
                            {
                                branch.Distance = distance;
                                branches.CreateOrUpdateBranch(branch);
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



                MessagingCenter.Send<PeriodicBranchesDistance>(this, MessagingCenterValues.BranchesChange);


                return StopThisTask();
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
