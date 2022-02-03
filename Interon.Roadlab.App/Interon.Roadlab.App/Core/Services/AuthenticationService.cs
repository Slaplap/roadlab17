using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Microsoft.AppCenter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Interon.Roadlab.App.Core.Services
{
    public static class AuthenticationService
    {
        public static async Task<bool> HasValidCredentialsAsync()
        {
            try
            {
                if (!SecureStorageService.HasAllLocalLoginCredentials())
                {
                    return false;
                }
                else
                {
                    if (SecureStorageService.ExpireDate < DateTime.Now)
                    {
                        var refreshtoken = SecureStorageService.RefreshToken;
                        if ( OnlineService.HasInternet() && OnlineService.HasConnectionToServerAsync())
                        {
                          
                            var refreshed = await AuthenticationService.Refresh(refreshtoken).ConfigureAwait(true);
                            if (refreshed)
                            {
                                return true;
                            }
                            else
                            {
                                AnalyticsService.TrackEvent(AnalyticsService.EventCategory
                                    .Info, "Access token has expired");
                                var login = await AuthenticationService
                                    .Login(SecureStorageService.Username, SecureStorageService.Password)
                                    .ConfigureAwait(true);
                                if (login)
                                {
                                    return true;
                                }
                                else
                                {
                                    
                                    AnalyticsService.TrackEvent(AnalyticsService.EventCategory.Warning, "Login after token expired false");
                                    return false;
                                }
                            }

                        }

                        return true;
                    }

                    return true;
                }

            }
            catch (Exception ex)
            {
                Debugger.Break();
            }

            return false;
        }

        public static bool LogOut()
        {
            SecureStorageService.LogOut();

            return true;
        }

        public static async Task<bool> ValidateCredentials()
        {
            var fUrl = new Url(OnlineService.BASEURL + OnlineService.PATH + "MemberApi/" + "IsAuthorized");
            int attempt = 0;

            while (attempt < 3)
            {
                attempt++;
                try
                {
                    var authenticated = await fUrl.WithOAuthBearerToken(SecureStorageService.AccessToken)
                        .WithTimeout(30).PostJsonAsync(new object()).ReceiveJson<bool>().ConfigureAwait(true);

                    return authenticated;
                }
                catch (FlurlHttpTimeoutException)
                {
                    // FlurlHttpTimeoutException derives from FlurlHttpException; catch here only
                    // if you want to handle timeouts as a special case
                    ErrorService.Error("HasValidCredentials" + "Timeout");

                }
                catch (FlurlHttpException ex)
                {


                    if (ex.Message.Contains("401"))
                    {
                        var result = await Refresh(SecureStorageService.RefreshToken).ConfigureAwait(true);
                        if (result)
                        {

                            return true;


                        }
                        else
                        {
                            continue;
                        }


                    }
                    else
                    {
                        AnalyticsService.TrackError(ex, "ValidateCredentials");

                    }


                }

            }
            AnalyticsService.TrackEvent( AnalyticsService.EventCategory.Warning, "Could not Validate with token");
            return false;
        }
        public static async Task<bool> Login(string username, string password)
        {




            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", "RoadlabClient"),
                new KeyValuePair<string, string>("client_secret", "roadlab"),
                new KeyValuePair<string, string>("device_id",  AppCenter.GetInstallIdAsync().Result.ToString())



            };
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, OnlineService.BASEURL + OnlineService.TOKEN);

                request.Content = new FormUrlEncodedContent(keyValues);

                var client = new HttpClient();

                var response = client.SendAsync(request).Result;
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        throw new Exception($"400 Invalid login details or possible member lock.");
                    }
                    ErrorService.EventAndAnalytics($"Error Logging in for User:{username} Password:{password} Status Code :{response.StatusCode.ToString()} Message : {response.ReasonPhrase}",AnalyticsService.EventCategory.Error);
                   
                }
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);


                JObject jdynamic = JsonConvert.DeserializeObject<dynamic>(content);
                var accessToken = jdynamic.Value<string>("access_token");
                var refershToken = jdynamic.Value<string>("refresh_token");
                var date = DateTime.Now.ToString();
                var accessTokenExpiration = jdynamic.Value<int>("expires_in");
                Debug.WriteLine(content);
                try
                {
                    SecureStorageService.AccessToken = accessToken;
                    SecureStorageService.RefreshToken = refershToken;
                    SecureStorageService.ExpireDate = DateTime.Now.AddSeconds(accessTokenExpiration);
                    SecureStorageService.Username = username;
                    SecureStorageService.Password = password;


                }
                catch (Exception ex)
                {
                    // Possible that device doesn't support secure storage on device.
                }
                //register the username with Notification hub.
            
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                
                throw e;
            }
            


         
          

        }
        public static async Task<bool> Refresh(string refreshToken)
        {


            string deviceId = null;
            try
            {
                deviceId = AppCenter.GetInstallIdAsync().Result.ToString();
            }
            catch
            {

            }

            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("client_id", "RoadlabClient"),
                new KeyValuePair<string, string>("client_secret", "roadlab"),
                new KeyValuePair<string, string>("device_id", deviceId)

            };
            var request = new HttpRequestMessage(HttpMethod.Post, OnlineService.BASEURL + OnlineService.TOKEN);

            request.Content = new FormUrlEncodedContent(keyValues);

            var client = new HttpClient();
            HttpResponseMessage response;
            try
            {
                response =  client.SendAsync(request).Result;

                if (!response.IsSuccessStatusCode)
                {
                   
                    return false;
                }
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);


                JObject jdynamic = JsonConvert.DeserializeObject<dynamic>(content);
                var accessToken = jdynamic.Value<string>("access_token");
                var refershToken = jdynamic.Value<string>("refresh_token");
                var date = DateTime.Now.ToString();
                var accessTokenExpiration = jdynamic.Value<int>("expires_in");


                //await Application.Current.MainPage.DisplayAlert("Alert", content, "OK");
                Debug.WriteLine(content);
                try
                {
                    SecureStorageService.AccessToken = accessToken;
                    SecureStorageService.RefreshToken = refershToken;
                    SecureStorageService.ExpireDate = DateTime.Now.AddSeconds(accessTokenExpiration);



                }
                catch (Exception ex)
                {
                    // Possible that device doesn't support secure storage on device.
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception exx)
            {

            }

            return false;
        }
    }
}