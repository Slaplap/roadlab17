using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Interon.Roadlab.App.Droid.Core;
using Interon.Roadlab.Core.Interfaces;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(NotificationHub))]
namespace Interon.Roadlab.App.Droid.Core
{

    public class NotificationHub : INotificationHub
    {


        public async void UpdateTags(string ID, List<string> gr)
        {
            var hub = new WindowsAzure.Messaging.NotificationHub(AppConstants.NotificationHubName, AppConstants.ListenConnectionString, Application.Context);
            try
            {
                await Task.Run(() => { hub.Unregister(); });
            }
            catch (Exception e)
            {
                Debugger.Break();
            }

            var tags = new List<string>() { };

            foreach (var m in gr)
            {
                tags.Add(m);
            }

            try
            {
                await Task.Run(() =>
                {
                    hub.Register(ID, tags.ToArray());
                });
            }
            catch (Exception e)
            {
              //  Debugger.Break();
            }
        }

        public string ID { get; }
    }
}