using Interon.Roadlab.App.Core;
using Interon.Roadlab.App.Views;
using Interon.Roadlab.App.Views.Branches;
using Interon.Roadlab.App.Views.Profile;
using Interon.Roadlab.App.Views.Registration;
using Interon.Roadlab.App.Views.Settings;
using Interon.Roadlab.App.Views.ClientRequests;
using System;
using System.Collections.Generic;
using Interon.Roadlab.App.Core.Const;
using Interon.Roadlab.App.Core.Services;
 
using Xamarin.Forms;
using DeveloperPage = Interon.Roadlab.App.Views.Developer.DeveloperPage;
using NewRegisterPage = Interon.Roadlab.App.Views.Registration.NewRegisterPage;
using RegisterPage = Interon.Roadlab.App.Views.Registration.RegisterPage;

namespace Interon.Roadlab.App
{
    public partial class AppShell : Shell
    {
        private Dictionary<string, Type> routes = new Dictionary<string, Type>();
        public Dictionary<string, Type> Routes { get { return routes; } }

        public AppShell()
        {
            LoadEvents();
            InitializeComponent();
            RegisterRoutes();
            BackgroundColor = Color.Red;
            Context.Instance.IsRunning = true;
            MessagingCenter.Send(new object(), MessagingCenterValues.PeriodicServiceRestart);
        }

        private void LoadEvents()
        {
            this.Navigating += AppShell_Navigating;
            this.Navigated += AppShell_Navigated;
             
        }

        private void AppShell_Navigated(object sender, ShellNavigatedEventArgs e)
        {
         SecureStorageService.FromUrl = e.Current.Location.ToString();
        }

        private void AppShell_Navigating(object sender, ShellNavigatingEventArgs e)
        {
          
        }

        protected override bool OnBackButtonPressed()
        {
           
            return base.OnBackButtonPressed();
        }

        private void RegisterRoutes()
        {
            routes.Add("RegisterPage", typeof(RegisterPage));
            routes.Add("BranchListPage", typeof(BranchListPage));
            routes.Add("SettingsPage", typeof(SettingsPage));
            routes.Add("ProfilePage", typeof(ProfilePage));
            routes.Add("ReRegisterPage", typeof(ReRegisterPage));
            routes.Add("NewRegisterPage", typeof(NewRegisterPage));
            routes.Add("ErrorPage", typeof(ErrorPage));
            routes.Add("DeveloperPage", typeof(DeveloperPage));
            routes.Add("ClientRequestListPage", typeof(ClientRequestListPage));
            routes.Add("ClientRequestSitePage", typeof(ClientRequestSitePage));
            routes.Add("ClientRequestRequestPage", typeof(ClientRequestRequestPage));
            routes.Add("ClientRequestPage", typeof(ClientRequestPage));
            routes.Add("ClientRequestComments", typeof(ClientRequestComments));
            routes.Add("SiteListPage", typeof(SiteListPage));
            routes.Add("Test", typeof(Test));
            routes.Add("Message", typeof(MessagePage));

            routes.Add("LogOut", typeof(LogOut)); ;
            routes.Add("HomePage", typeof(HomePage)); ;

            foreach (var item in routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }
    }
}