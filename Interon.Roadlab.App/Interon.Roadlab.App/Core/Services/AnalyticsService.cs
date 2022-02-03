using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace Interon.Roadlab.App.Core.Services
{
    public static class AnalyticsService
    {
        public enum EventCategory
        {
             
             
            Error,
            Info,
            Warning
        }
        public static void TrackEvent(EventCategory eventCategory, string message ="")
            {
                       Analytics.TrackEvent(eventCategory.ToString(), new Dictionary<string, string> {
                        {"Category" , eventCategory.ToString() },
                        { "User", SecureStorageService.Username},
                        { "Date", DateTime.Now.Date.ToString("MM/dd/yyyy HH:mm tt")},
                        { "Device", DeviceInfo.Model },
                        { "OS", DeviceInfo.VersionString},
                        { "Message",message}

                       });
        }
        public static void TrackError(Exception ex,string methodName)
        {
            Crashes.TrackError(ex, new Dictionary<string, string> {
                          { "User", SecureStorageService.Username},
                          { "Date", DateTime.Now.Date.ToString("MM/dd/yyyy HH:mm tt")},
                          { "Device", DeviceInfo.Model },
                          { "OS", DeviceInfo.VersionString},
                          { "methodName", methodName }
                         });
        }
    }
}
