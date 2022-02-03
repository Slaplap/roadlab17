using System;
using System.Collections.Generic;
using Interon.Roadlab.Core.Enums;
using Interon.Roadlab.Core.Interfaces;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core
{
    public static class DependencyServiceService
    {
        public static void ShortMessage(string message, bool OnlyShowInDebug = false)
        {
            if (OnlyShowInDebug)
            {
#if (DEBUG)
                DependencyService.Get<IToastService>().CookIt(message, MyToastLength.Short);
#endif

            }
            else
            {
                DependencyService.Get<IToastService>().CookIt(message, MyToastLength.Short);
            }

        }

        public static void LongMessage(string message, bool OnlyShowInDebug = false)
        {
            if (OnlyShowInDebug)
            {
#if (DEBUG)
                DependencyService.Get<IToastService>().CookIt(message, MyToastLength.Long);
#endif

            }
            else
            {
                DependencyService.Get<IToastService>().CookIt(message, MyToastLength.Long);
            }
        }

        public static void UpdateNotificationHubTags(String ID, List<string> gr)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                //TODO IOS
                //var hubios = DependencyService.Get();
                //hubios.UpdateTagsIOS(hubios.DeviceToken, Groups);
            }
            else
            {
                try
                {
                    DependencyService.Get<INotificationHub>().UpdateTags(ID, gr);
                }
                catch (Exception e)
                {

                }
                
            }
        }
    }
}
