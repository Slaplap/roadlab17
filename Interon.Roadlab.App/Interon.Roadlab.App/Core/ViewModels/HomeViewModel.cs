using Interon.Roadlab.App.Core.Models;
using Interon.Roadlab.App.Core.Services;
using Interon.Roadlab.App.Views.Branches;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Interon.Roadlab.App.Core.Const;
using Interon.Roadlab.App.Views.Registration;
using Interon.Roadlab.Core;
using Interon.Roadlab.Core.Enums;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core.ViewModels
{
    internal class HomeViewModel : ViewModelBase
    {
        private bool _isRefreshing;
        private string _quoteActionCountString;
        private string _bookingActionCountString;
        private string _notificationsCount;
        private int _notificationHeight = 92;

        public HomeViewModel()
        {

            if (!SecureStorageService.HasAllLocalLoginCredentials())
            {
                App.Current.MainPage = new RegisterPage();
            }
            Commands.Add("Refresh", new AsyncCommand(Refresh));

            Commands.Add("ClientRequestList", new AsyncCommand<object>(ClientRequestList,allowsMultipleExecutions:false));
            Commands.Add("ClientRequestRequest", new AsyncCommand<object>(ClientRequestRequest, allowsMultipleExecutions: false));

            Commands.Add("Branches", new AsyncCommand(Branches, allowsMultipleExecutions: false));
            Commands.Add("ClearNotification", new AsyncCommand<object>(ClearNotification, allowsMultipleExecutions: false));
            Commands.Add("GotoClientRequest", new AsyncCommand<object>(GotoClientRequest, allowsMultipleExecutions: false));
            Commands.Add("RequestCallback", new AsyncCommand(RequestCallback, allowsMultipleExecutions: false));
            Commands.Add("Feedback", new AsyncCommand(Feedback, allowsMultipleExecutions: false));
            Commands.Add("Test", new AsyncCommand(TestCommand, allowsMultipleExecutions: false));
            NotificationHubService.RegisterUserWithNotificationHub();
            LoadNotifications();
            LoadClientRequests();
            MessagingCenter.Subscribe<object>(this, MessagingCenterValues.NotificationsChange, (sender) =>
            {
                LoadNotifications();

            });

            MessagingCenter.Subscribe<object>(this, MessagingCenterValues.RequestChange, (sender) =>
            {

                LoadClientRequests();
            });
           



        }



        private async Task Feedback()
        {

            Shell.Current.GoToAsync($"Message?MessageType=feedback");
        }


        private async Task TestCommand()
        {
            Shell.Current.GoToAsync($"ClientRequestComments?ClientRequestId=test");
        }

        private async Task GotoClientRequest(object obj)
        {
            await Busy();
            if (obj == null) return;
            await Shell.Current.GoToAsync(obj.ToString());
        }


        private async Task ClearNotification(object obj)
        {
            await Busy();
            NotificationService notificationService = new NotificationService();
            var notification = notificationService.GetNotificationByKey(Guid.Parse(obj.ToString()));
            if (notification != null)
            {
                notification.ReadDate = DateTime.Now;

                notificationService.CreateOrUpdateNotification(notification, false, true);
                LoadNotifications();
            }

            await NotBusy();
        }

        public void LoadClientRequests()
        {
            try
            {
                RequestService requestService = new RequestService();
                QuoteActionCountString = $" Ready ({requestService.GelAllClientRequestsByStatus(ClientRequestStatus.QuoteReady).Count})";
                BookingActionCountString = $" Ready ({requestService.GelAllClientRequestsByStatus(ClientRequestStatus.BookingReady).Count})";
            }
            catch (Exception ex)
            {
                Debugger.Break();

            }

        }
        public void LoadNotifications()
        {
            try
            {

                NotificationService notificationService = new NotificationService();
               
                var member = MemberService.GetMember();
                var allUnreadNotifications = notificationService.GetAllNotifications().Where(x => x.ReadDate == null);

                Notifications.Clear();
                foreach (var item in allUnreadNotifications)
                {
                    Notifications.Add(item);
                }


                NotificationsCount = $"Notifications ({Notifications.Count})";


                NotificationHeight = 92;
                if (Notifications.Count != 0)
                {
                    if (Notifications.Count < 4)
                    {
                        NotificationHeight = Notifications.Count * 170;
                    }
                    else
                    {
                        NotificationHeight = 3 * 170;
                    }
                }
                else
                {
                    NotificationHeight = 92;
                }


            }
            catch (Exception ex)
            {
                Debugger.Break();

            }

        }


        public string QuoteActionCountString
        {
            get => _quoteActionCountString;
            set
            {
                if (value == _quoteActionCountString) return;

                SetProperty(ref _quoteActionCountString, value);
            }
        }

        public string BookingActionCountString
        {
            get => _bookingActionCountString;
            set
            {
                if (value == _bookingActionCountString) return;

                SetProperty(ref _bookingActionCountString, value);
            }
        }

        public int NotificationHeight
        {
            get => _notificationHeight;
            set
            {
                if (value == _notificationHeight) return;
                SetProperty(ref _notificationHeight, value);
            }
        }

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

        public ObservableCollection<Notification> Notifications { get; set; } = new ObservableCollection<Notification>();

        public string NotificationsCount
        {
            get => _notificationsCount;
            set
            {
                if (value == _notificationsCount) return;

                SetProperty(ref _notificationsCount, value);
            }
        }

        private async Task Branches()
        {
            await Busy();
            var stack = Shell.Current.Navigation.NavigationStack;
            // Shell.Current.GoToAsync($"BranchListPage") ;
           await Shell.Current.Navigation.PushAsync(new BranchListPage());
           await NotBusy();

        }

        private async Task Refresh()

        {
            await Busy();
            QuoteActionCountString = $" Ready (...)";
            BookingActionCountString = $" Ready (...)";
            Globals.StoppedBackgroundServices.Clear();
            IsRefreshing = true;
            LoadNotifications();
            IsRefreshing = false;
            await NotBusy();

        }

        private async Task RequestCallback()
        {
            await Busy();
           await Shell.Current.GoToAsync($"Message?MessageType=callback");
           await NotBusy();
        }

        private async Task ClientRequestList(object o)

        {
            await Busy();
            string type = o.ToString().URLDecode();
            IsRefreshing = false;
            //Shell.Current.GoToAsync($"//ClientRequestListPage?type={o.ToString()}");
            try
            {
              await  Shell.Current.GoToAsync($"ClientRequestListPage?ClientRequestStatus={type}");
            }
            catch (Exception e)
            {
                Debugger.Break();
            }

            await NotBusy();
        }

        private async Task ClientRequestRequest(object transactionType)
        {
            IsRefreshing = false;
            await Busy();
            try
            {
              await  Shell.Current.GoToAsync($"ClientRequestRequestPage?requesttype={transactionType.ToString()}");
            }
            catch (Exception e)
            {
                Debugger.Break();
            }

            await NotBusy();

        }
        //private void ClientRequestList()

        //{
        //    IsRefreshing = false;
        //    Shell.Current.GoToAsync($"//ClientRequestListPage?type=test");
        //}
    }
}