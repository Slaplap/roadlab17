using Interon.Roadlab.App.Core.Models;
using Interon.Roadlab.App.Core.Services;
using Interon.Roadlab.App.Core.Tasks;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Interon.Roadlab.App.Core.ViewModels
{
    internal class BranchViewModel : ViewModelBase

    {
        private bool _isRefreshing;
        public ObservableCollection<Branch> Branches { get; set; } = new ObservableCollection<Branch>();

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                if (value == _isRefreshing) return;
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public BranchViewModel()
        {
            var s = Shell.Current.CurrentState;
             
           
            
            IsBusy = true;
            Commands.Add("Telephone_OnClick", new Command(Telephone_OnClick));
            Commands.Add("Map_Click", new Command(Map_Click));
            Commands.Add("Refresh", new Command(Refresh));
            BranchesService branchService = new BranchesService();

            Branches.Clear();
            var branches = branchService.GetBranches();
            branches.ForEach(x => Branches.Add(x));
            if (branches == null || !branches.Any())
            {
                IsBusy = true;
            }
            else
            {
                IsBusy = false;
            }

            MessagingCenter.Subscribe<PeriodicBranches>(this, "NewBranches", (sender) =>
               {
                   Refresh();
               });
           
        }

        private void Refresh()

        {
            IsRefreshing = true;
            BranchesService branchService = new BranchesService();
            Branches.Clear();
            branchService.GetBranches().ForEach(x => Branches.Add(x));
            IsRefreshing = false;
        }

        private void Map_Click(object o)
        {
            Branch branch = (Branch)o;
            try
            {
                double longitude = double.Parse(branch.Longitude.ToString(), CultureInfo.InvariantCulture);
                double latitude = double.Parse(branch.Latitude.ToString(), CultureInfo.InvariantCulture);
                var location = new Location(latitude, longitude);
                var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
                Map.OpenAsync(location, options);
            }
            catch (ArgumentNullException anEx)
            {
                ErrorService.ErrorExceptionAndAnalyticsAndModal("Mao Null", "Map_Click", anEx);
            }
            catch (FeatureNotSupportedException ex)
            {
                ErrorService.ErrorExceptionAndAnalyticsAndModal("Direction not supported by device", "Map_Click", ex);
            }
            catch (FormatException fex)
            {
                ErrorService.ErrorExceptionAndAnalyticsAndModal("GPS Coordinates incorrect - " + branch.Name, "Map_Click", fex);
            }
            catch (Exception ex)
            {
                ErrorService.ErrorExceptionAndAnalyticsAndModal("Other Error " + ex.Message, "Map_Click", ex);
            }
        }

        private string NormalizeNumber(object telephoneNumber)
        {
            var _number = telephoneNumber.ToString();
            _number = _number.RemoveWhiteSpace();
            _number = _number.Replace("(0)", "");
            return _number;
        }

        private void Telephone_OnClick(object number)
        {
            try
            {
                PhoneDialer.Open(NormalizeNumber(number));
            }
            catch (ArgumentNullException anEx)
            {
                ErrorService.ErrorExceptionAndAnalyticsAndModal("Number Null", "Telephone_OnClick", anEx);
            }
            catch (FeatureNotSupportedException ex)
            {
                ErrorService.ErrorExceptionAndAnalyticsAndModal("Dialing not supported", "Telephone_OnClick", ex);
            }
            catch (Exception ex)
            {
                ErrorService.ErrorExceptionAndAnalyticsAndModal("Other Error", "Telephone_OnClick", ex);
            }
        }
    }
}