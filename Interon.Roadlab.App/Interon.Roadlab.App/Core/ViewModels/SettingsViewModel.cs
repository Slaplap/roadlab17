using Interon.Roadlab.App.Core.Services;

namespace Interon.Roadlab.App.Core.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private bool _showNotifications = true;
        private bool _showBookings = true;
        private bool _showQuotes = true;
        private bool _showFavourites;

        public SettingsViewModel()
        {
            
         
          
          
        }

        public bool ShowNotifications
        {
            get
            {
                _showNotifications = SecureStorageService.ShowNotifications;
                return _showNotifications;
            }
            set
            {
                SetProperty(ref _showNotifications, value);
                SecureStorageService.ShowNotifications = value;
            }
        }

        public bool ShowBookings
        {
            get
            {
                _showBookings = SecureStorageService.ShowBookings;
                return _showBookings;
            }
            set
            {
                SetProperty(ref _showBookings, value);
                SecureStorageService.ShowBookings = value;
            }
        }

        public bool ShowQuotes
        {
            get
            {
                _showQuotes = SecureStorageService.ShowQuotes;
                return _showQuotes;
            }
            set
            {
                SetProperty(ref _showQuotes, value);
                SecureStorageService.ShowQuotes = value;
            }
        }

        public bool ShowFavourites
        {
            get
            {
                _showFavourites = SecureStorageService.ShowFavourites;
                return _showFavourites;
            }
            set
            {
                SetProperty(ref _showFavourites, value);
                SecureStorageService.ShowFavourites = value;
            }
        }
    }
}