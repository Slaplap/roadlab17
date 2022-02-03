using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interon.Roadlab.App.Core.Models;
using Interon.Roadlab.App.Core.ViewModels;
using Interon.Roadlab.LIMS.DTO;
using Interon.Roadlab.LIMS.DTO.Quicktype;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace Interon.Roadlab.App.Core.Services
{
    public static class SecureStorageService
    {
        


        public static void ClearStorage()
        {
            SecureStorage.RemoveAll();
            CleanInstall = false;

        }
        public static void  LogOut()
        {
            AccessToken = "";
            RefreshToken = "";
            Username = "";
            Password = "";
            MemberId = 0;
            ClientAccountNumber = "";
            ClearStorage();
           
        }
        public static bool HasAllLocalLoginCredentials()
        {
            if (!string.IsNullOrWhiteSpace(AccessToken) && !string.IsNullOrWhiteSpace(RefreshToken) &&
                !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password)&&!string.IsNullOrWhiteSpace(ClientAccountNumber))
            {
                return true;
            }

            return false;
        }
        public static string Secret
        {
            get => SecureStorage.GetAsync("Secret").GetAwaiter().GetResult();
            set => SecureStorage.SetAsync("Secret", value);
        }
        public static string NotificationHubToken
        {
            get => SecureStorage.GetAsync("NotficationHubToken").GetAwaiter().GetResult();
            set => SecureStorage.SetAsync("NotficationHubToken", value);
        }

        public static Member Member
        {
            get
            {
                try
                {
                   return JsonConvert.DeserializeObject<Member>(SecureStorage.GetAsync("Member").GetAwaiter().GetResult());
                }
                catch
                {
                    return null;
                }
              

            }
            set => SecureStorage.SetAsync("Member", JsonConvert.SerializeObject(value));
        }
        public static string AccessToken
        {
            get => SecureStorage.GetAsync("AccessToken").GetAwaiter().GetResult();
            set => SecureStorage.SetAsync("AccessToken", value);
        }
        public static Guid? InstallId
        {
            get => Guid.Parse(SecureStorage.GetAsync("InstallId").Result);
            set => SecureStorage.SetAsync("InstallId",  value.ToString());
        }
        public static string RefreshToken
        {
            get => SecureStorage.GetAsync("RefreshToken").GetAwaiter().GetResult();
            set => SecureStorage.SetAsync("RefreshToken", value);
        }
        public static DateTime ExpireDate
        {
            get
            {
                if (SecureStorage.GetAsync("ExpiryDate") == null)
                {
                    return new DateTime();
                }

                try
                {

                    return DateTime.Parse(SecureStorage.GetAsync("ExpiryDate").Result);
                }
                catch
                {
                    return  new DateTime();
                }
            }
            set => SecureStorage.SetAsync("ExpiryDate", value.ToString());
        }

        
        public static DateTime SyncNotificationsDateTime
        {
            get
            {
                if (SecureStorage.GetAsync("SyncNotificationsDateTime") == null)
                {
                    return new DateTime();
                }

                try
                {
                    string sbinarytime = SecureStorage.GetAsync("SyncNotificationsDateTime").GetAwaiter().GetResult();
                    var    dDate       = DateTime.FromBinary(long.Parse(sbinarytime));
                    return dDate;
                }
                catch
                {
                    return new DateTime();
                }
            }
            set
            {
                var dDateTime = value;
                SecureStorage.SetAsync("SyncNotificationsDateTime", dDateTime.ToBinary().ToString());
            }
        }
        public static string Username
        {
            get => SecureStorage.GetAsync("Email").GetAwaiter().GetResult();
            set => SecureStorage.SetAsync("Email", value);
        }
        public static string ClientAccountNumber
        {
            get => SecureStorage.GetAsync("ClientAccountNumber").GetAwaiter().GetResult();
            set => SecureStorage.SetAsync("ClientAccountNumber", value);
        }
        public static int ClientId
        {
            get => Convert.ToInt32(SecureStorage.GetAsync("ClientId").Result);
            set => SecureStorage.SetAsync("ClientId", value.ToString());
        }
        public static int ContactId
        {
            get => Convert.ToInt32(SecureStorage.GetAsync("ContactId").Result);
            set => SecureStorage.SetAsync("ContactId", value.ToString());
        }

        public static int SiteId
        {
            get => Convert.ToInt32(SecureStorage.GetAsync("SiteId").Result);
            set => SecureStorage.SetAsync("SiteId", value.ToString());
        }
        public static string Password
        {
            get => SecureStorage.GetAsync("Password").GetAwaiter().GetResult();
            set => SecureStorage.SetAsync("Password", value);
        }

        public static bool IsDeveloperMode
        {
            get
            {
                if (SecureStorage.GetAsync("IsDeveloperMode").Result == null)
                {
                    return false;
                }
                return bool.Parse(SecureStorage.GetAsync("IsDeveloperMode").Result);
            }
            set => SecureStorage.SetAsync("IsDeveloperMode", value.ToString());
        }

        public static bool IsNewVersionAvailable
        {
            get
            {
                if (SecureStorage.GetAsync("IsNewVersionAvailable").Result == null)
                {
                    return false;
                }
                return bool.Parse(SecureStorage.GetAsync("IsNewVersionAvailable").Result);
            }
            set => SecureStorage.SetAsync("IsNewVersionAvailable", value.ToString());
        }
        public static DateTime AppSettingsDate
        {
            get
            {
                if (string.IsNullOrWhiteSpace( SecureStorage.GetAsync("AppSettingsDate").Result))
                {
                    return  new DateTime();
                }
                return DateTime.Parse(SecureStorage.GetAsync("AppSettingsDate").Result);
            }
            set => SecureStorage.SetAsync("AppSettingsDate", value.ToString());
        }
        public static DateTime BranchesUpdateDate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SecureStorage.GetAsync("BranchesUpdateDate").Result))
                {
                    return new DateTime();
                }
                return DateTime.Parse(SecureStorage.GetAsync("BranchesUpdateDate").Result);
            }
            set => SecureStorage.SetAsync("BranchesUpdateDate", value.ToString());
        }

        public static DateTime PoleStartDate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SecureStorage.GetAsync("PollStartDate").Result))
                {
                    return new DateTime();
                }
                return DateTime.Parse(SecureStorage.GetAsync("PollStartDate").Result);
            }
            set => SecureStorage.SetAsync("PollStartDate", value.ToString());

        }

        public static bool ExecuteOrderTimer
        {
            get
            {
                if (SecureStorage.GetAsync("ExecuteOrderTimer").Result == null)
                {
                    return false;
                }
                return bool.Parse(SecureStorage.GetAsync("ExecuteOrderTimer").Result);
            }
            set => SecureStorage.SetAsync("ExecuteOrderTimer", value.ToString());
        }
        public static string FromUrl
        {
            get => SecureStorage.GetAsync("FromUrl").GetAwaiter().GetResult();
            set => SecureStorage.SetAsync("FromUrl", value);
        }
        public static string ClientName
        {
            get => SecureStorage.GetAsync("ClientName").GetAwaiter().GetResult();
            set => SecureStorage.SetAsync("ClientName", value);
        }
        public static int MemberId
        {
            get => int.Parse(SecureStorage.GetAsync("MemberId").Result);
            set => SecureStorage.SetAsync("MemberId", value.ToString());
        }

        public static string VersionString
        {
            get => SecureStorage.GetAsync("VersionString").GetAwaiter().GetResult();
            set => SecureStorage.SetAsync("VersionString", value);
        }

       

        public static string AppleLink
        {
            get => SecureStorage.GetAsync("AppleLink").GetAwaiter().GetResult();
            set => SecureStorage.SetAsync("AppleLink", value);
        }

        public static string PlaystoreLink
        {
            get => SecureStorage.GetAsync("PlaystoreLink").GetAwaiter().GetResult();
            set => SecureStorage.SetAsync("PlaystoreLink", value);
        }

        public static bool ShowFavourites
        {
            get
            {
                if (SecureStorage.GetAsync("ShowFavourites").Result == null)
                {
                    return false;
                }
                return bool.Parse(SecureStorage.GetAsync("ShowFavourites").Result);
            }
            set => SecureStorage.SetAsync("ShowFavourites", value.ToString());
        }
        public static bool ShowQuotes
        {
            get
            {
                if (SecureStorage.GetAsync("ShowQuotes").Result == null)
                {
                    return false;
                }
                return bool.Parse(SecureStorage.GetAsync("ShowQuotes").Result);
            }
            set => SecureStorage.SetAsync("ShowQuotes", value.ToString());
        }
        public static bool ShowBookings
        {
            get
            {
                if (SecureStorage.GetAsync("ShowBookings").Result == null)
                {
                    return false;
                }
                return bool.Parse(SecureStorage.GetAsync("ShowBookings").Result);
            }
            set => SecureStorage.SetAsync("ShowBookings", value.ToString());
        }
        public static bool ShowNotifications
        {
            get
            {
                if (SecureStorage.GetAsync("ShowNotifications").Result == null)
                {
                    return false;
                }
                return bool.Parse(SecureStorage.GetAsync("ShowNotifications").Result);
            }
            set => SecureStorage.SetAsync("ShowNotifications", value.ToString());
        }

       
        public static string BaseUrl
        {
            get => SecureStorage.GetAsync("BaseUrl").GetAwaiter().GetResult();
            set => SecureStorage.SetAsync("BaseUrl", value);
        }

        public static string BuildString
        {
            get => SecureStorage.GetAsync("BuildString").GetAwaiter().GetResult();
            set => SecureStorage.SetAsync("BuildString", value);
        }

        public static bool ForceUpdate
        {
            get
            {
                if (SecureStorage.GetAsync("ForceUpdate").Result == null)
                {
                    return false;
                }
                return bool.Parse(SecureStorage.GetAsync("ForceUpdate").Result);
            }
            set => SecureStorage.SetAsync("ForceUpdate", value.ToString());
        }
        public static bool ForceReinstall
        {
            get
            {
                if (SecureStorage.GetAsync("ForceReinstall").Result == null)
                {
                    return false;
                }
                return bool.Parse(SecureStorage.GetAsync("ForceReinstall").Result);
            }
            set => SecureStorage.SetAsync("ForceReinstall", value.ToString());
        }
        public static bool CleanInstall
        {
            get
            {
                if (SecureStorage.GetAsync("CleanInstall").Result == null)
                {
                    return false;
                }
                return bool.Parse(SecureStorage.GetAsync("CleanInstall").Result);
            }
            set => SecureStorage.SetAsync("CleanInstall", value.ToString());
        }

        public static xSite Site
    {
        get
        {
            try
            {
                return JsonConvert.DeserializeObject<xSite>(SecureStorage.GetAsync("Site").Result);
            }
            catch
            {
                return null;
            }
        }

        set => SecureStorage.SetAsync("Site", JsonConvert.SerializeObject( value));
        }

        public static RootSites Sites
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<RootSites>(SecureStorage.GetAsync("Sites").Result);
                }
                catch
                {
                    return null;
                }
            }

            set => SecureStorage.SetAsync("Sites", JsonConvert.SerializeObject(value));
        }

        public static Dictionary<string, SageCategory> Categories
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<Dictionary<string, SageCategory>>(SecureStorage.GetAsync("Categories").Result);
                }
                catch
                {
                    return null;
                }
            }

            set => SecureStorage.SetAsync("Categories", JsonConvert.SerializeObject(value));
        }

         
    }
}
//await SecureStorage.SetAsync("AccessToken", accessToken);
//await SecureStorage.SetAsync("RefreshToken", refershToken);
//await SecureStorage.SetAsync("ExpiryDate", accessTokenExpiration");
//await SecureStorage.SetAsync("Email", username);
//await SecureStorage.SetAsync("Password", password);