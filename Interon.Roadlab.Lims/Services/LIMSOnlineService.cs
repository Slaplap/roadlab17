using Flurl;
using Flurl.Http;
using Interon.Roadlab.LIMS.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Interon.Roadlab.Core.Dto;
using Interon.Roadlab.LIMS.DTO.Quicktype;
using MethodTimer;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SpatialFocus.MethodCache;


namespace Interon.Roadlab.LIMS.Services
{



    public static class LIMSOnlineService
    {


        public static async Task<bool> IsServerOnlineAsync()
        {
            var fUrl = new Url(Interon.Roadlab.Core.Environment.LIMSAPI_STATUS_URL_ENDPOINT);

            try
            {
                var available = await fUrl.WithTimeout(60).GetJsonAsync<Status>().ConfigureAwait(true);

                return !available.Error;

            }
            catch (FlurlHttpTimeoutException fex)
            {


                throw fex;


            }
            catch (FlurlHttpException ex)
            {

                throw ex;


            }
            catch (Exception e)
            {
                throw e;

            }


        }
        public static async Task<RootClients> ClientListBySearch(string q, int page = 1, int pagesize = 10000)
        {
            var fUrl = new Url(Interon.Roadlab.Core.Environment.LIMSAPI_CLIENTSLIST_URL_ENDPOINT + $"?q={q}&page={page}&pagesize={pagesize}");

            try
            {
                var dynamicObject = await fUrl.WithOAuthBearerToken(Interon.Roadlab.Core.Environment.BEARERTOKEN).WithTimeout(60).GetJsonAsync().ConfigureAwait(true);
                var json = JsonConvert.SerializeObject(dynamicObject);

                RootClients rootClients = new RootClients();

                try
                {
                    rootClients = JsonConvert.DeserializeObject<RootClients>(json, new JsonSerializerSettings()
                    {
                        Error = HandleDeSerializationError,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    });
                }
                catch (JsonException je)
                {
                    
                }

                return rootClients;

            }
            catch (FlurlHttpTimeoutException fex)
            {

                throw fex;


            }
            catch (FlurlHttpException ex)
            {
                
                throw ex;

            }
            catch (Exception e)
            {
                      throw e;

            }


        }

       

        public static async Task<RootContacts> ContactListByCompany(int companyId, int page = 1, int pagesize = 10000)
        {
            var fUrl = new Url(Interon.Roadlab.Core.Environment.LIMSAPI_CONTACTSLISTBYCOMPANY_URL_ENDPOINT + $"?id={companyId}&page={page}&pagesize={pagesize}");

            try
            {
                var contacts = await fUrl.WithOAuthBearerToken(Interon.Roadlab.Core.Environment.BEARERTOKEN).WithTimeout(60).GetJsonAsync<RootContacts>().ConfigureAwait(true);

                return contacts;

            }
            catch (FlurlHttpTimeoutException fex)
            {


                throw fex;


            }
            catch (FlurlHttpException ex)
            {

                throw ex;


            }
            catch (Exception e)
            {
                throw e;

            }


        }


        public static async Task<RootMessage> CreateContact(InputByClientIdAndContact inputByClientIdAndContact)
        {
            var fUrl = new Url(Interon.Roadlab.Core.Environment.LIMSAPI_CREATECONTACT_URL_ENDPOINT);

            try
            {


                PostHeaders(out var headers);
                var response = await fUrl.WithOAuthBearerToken(Interon.Roadlab.Core.Environment.BEARERTOKEN).WithTimeout(60).WithCookie("PHPSESSID", "32b86sqhme2ctpdtrkse6paos8; Path=/;").WithHeaders(headers).PostJsonAsync(inputByClientIdAndContact).ReceiveJson().ConfigureAwait(true);
                Debugger.Break();
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                RootMessage rm = JsonConvert.DeserializeObject<RootMessage>(json);
                return rm;
            }
            //TODO trap all LIMS and log errors
            catch (FlurlHttpTimeoutException fex)
            {

                
                throw fex;


            }
            catch (FlurlHttpException ex)
            {

                throw ex;


            }
            catch (Exception e)
            {
                throw e;

            }
        }
        private static void PostHeadersNewRequest(out object headers)
        {
            headers = new
            {
                
                Cookie = "PHPSESSID=v7qdpdro520oo7i36sm1hkftp7",
                Authorization = Interon.Roadlab.Core.Environment.BEARERTOKEN,
                Content_Type = "application/json"

            };
        }
        private static void PostHeaders(out object headers)
        {
            headers = new
            {
                Cache_Control = "no-store, no-cache, must-revalidate",
                Pragma = "no-cache",
                Vary = "Accept-Encoding",
                Content_Encoding = "gzip",
                Access_Control_Allow_Origin = "*",
                Set_Cookie = "PHPSESSID=32b86sqhme2ctpdtrkse6paos8; path=/",
                X_UA_Compatible = "IE=edge",
                X_Content_Type_Options_Type_Options = "nosniff",
                Content_Length = "26938",
                Keep_Alive = "timeout=5, max=100",
                Connection = "Keep-Alive",
                Content_Type = "application/json",
                Authorization = Interon.Roadlab.Core.Environment.BEARERTOKEN
            };
        }
        private static void GetHeaders(out object headers)
        {
            headers = new
            {    
                 Cookie = "PHPSESSID=n575bepl5el112drjtretfpt8t",
                 Authorization = Interon.Roadlab.Core.Environment.BEARERTOKEN,
                 Connection = "keep-alive",
                 Accept_Encoding = "gzip, deflate, br",
                 Accept = "*/*"
            };
        }
        public static void HandleSerializationError(object sender, ErrorEventArgs errorArgs)
        {
            Debugger.Break();
            var currentError = errorArgs.ErrorContext.Error.Message;
            errorArgs.ErrorContext.Handled = true;
        }
        public static async Task<List<RootClientRequest>> RequestListByClientOrContactOrSite(InputByClientIdOrContactIdOrSiteId inputByClientIdOrContactIdOrSiteId)
        {
            var fUrl = new Url(Interon.Roadlab.Core.Environment.LIMSAPI_REQUEST_URL_ENDPOINT);

            try
            {




                PostHeaders(out var headers);
                var response = await fUrl.WithOAuthBearerToken(Interon.Roadlab.Core.Environment.BEARERTOKEN).WithTimeout(60).WithCookie("PHPSESSID", "32b86sqhme2ctpdtrkse6paos8; Path=/;").WithHeaders(headers).PostJsonAsync(inputByClientIdOrContactIdOrSiteId).ReceiveJsonList().ConfigureAwait(true);
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(response, new JsonSerializerSettings()
                {
                    Error = HandleSerializationError,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                });

                List<RootClientRequest> rootClientRequest = new List<RootClientRequest>();
                try
                {

                 
                    rootClientRequest = JsonConvert.DeserializeObject<List<RootClientRequest>>(json , new JsonSerializerSettings(){
                        Error   = HandleDeSerializationError,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    });

                }
                catch (JsonSerializationException ex)
                {
                    Debugger.Break();
                    Console.WriteLine(ex.Message);
                    // Could not find member 'DeletedDate' on object of type 'Account'. Path 'DeletedDate', line 4, position 23.
                }
                return rootClientRequest;
            }
            catch

      

        (FlurlHttpTimeoutException fex)
            {


                throw fex;


            }
            catch (FlurlHttpException ex)
            {
                Debugger.Break();
                throw ex;


            }
            catch (Exception e)
            {
                throw e;

            }
        }
         
        private static void HandleDeSerializationError(object sender, ErrorEventArgs e)
        {
            var currentError = e.ErrorContext.Error.Message;
            Debugger.Break();
            e.ErrorContext.Handled = true;
        }


        public static async Task<RootUsersInGroup> UserListInGroup(int groupId, string q)
        {
            //TODO only returns 10 items talk to Commune.
            var fUrl = new Url(Interon.Roadlab.Core.Environment.LIMSAPI_LISTUSERSINGROUP_URL_ENDPOINT + $"/{groupId}/?q={q}&include_user_details=1");

            try
            {




                PostHeaders(out var headers);
                var response = await fUrl.WithOAuthBearerToken(Interon.Roadlab.Core.Environment.BEARERTOKEN).WithTimeout(60).WithCookie("PHPSESSID", "32b86sqhme2ctpdtrkse6paos8; Path=/;").WithHeaders(headers).PostAsync().ReceiveJson<RootUsersInGroup>().ConfigureAwait(true);


                return response;
            }
            catch (FlurlHttpTimeoutException fex)
            {


                throw fex;


            }
            catch (FlurlHttpException ex)
            {

                throw ex;


            }
            catch (Exception e)
            {
                throw e;

            }
        }
        public static async Task<RootGetGroupAndChildren> GetGroupsAndChildren(int groupId)
        {
            var fUrl = new Url(Interon.Roadlab.Core.Environment.LIMSAPI_GETGROUPANDCHILDREN_URL_ENDPOINT + $"/{groupId}/include_parent/1/enabled_only/1/include_categories/1/sortField/name/sortOrder/asc");

            try
            {
                var result = await fUrl.WithOAuthBearerToken(Interon.Roadlab.Core.Environment.BEARERTOKEN).WithTimeout(60).GetJsonAsync<RootGetGroupAndChildren>().ConfigureAwait(true);

                return result;

            }
            catch (FlurlHttpTimeoutException fex)
            {


                throw fex;


            }
            catch (FlurlHttpException ex)
            {

                throw ex;


            }
            catch (Exception e)
            {
                throw e;

            }


        }

        public static async Task<Dictionary<string, SageCategory>> ServiceCategories(InputServiceCategories inputServiceCategories)
        {
            var fUrl = new Url(Interon.Roadlab.Core.Environment.LIMSAPI_SERVICECATEGORIES_URL_ENDPOINT);

            try
            {


                PostHeaders(out var headers);
                var response = await fUrl.WithOAuthBearerToken(Interon.Roadlab.Core.Environment.BEARERTOKEN).WithTimeout(60).WithCookie("PHPSESSID", "32b86sqhme2ctpdtrkse6paos8; Path=/;").WithHeaders(headers).PostJsonAsync(inputServiceCategories).ReceiveJson().ConfigureAwait(true);


                string json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                var categories = SageCategory.FromJson(json);

                return categories;
            }
            catch (FlurlHttpTimeoutException fex)
            {


                throw fex;


            }
            catch (FlurlHttpException ex)
            {

                throw ex;


            }
            catch (Exception e)
            {
                throw e;

            }
        }

        public static async Task<Dictionary<string, Variants>> VariantsByCategory(InputVariantByCategory input)
        {
            var fUrl = new Url(Interon.Roadlab.Core.Environment.LIMSAPI_VARIANTSBYCATEGORY_URL_ENDPOINT);

            try
            {
                var result = await fUrl.WithOAuthBearerToken(Interon.Roadlab.Core.Environment.BEARERTOKEN).WithTimeout(60).SendJsonAsync(HttpMethod.Get, input).ReceiveJson().ConfigureAwait(true);
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                var variants = Variants.FromJson(json);
                return variants;

            }
            catch (FlurlHttpTimeoutException fex)
            {


                throw fex;


            }
            catch (FlurlHttpException ex)
            {

                throw ex;


            }
            catch (Exception e)
            {
                throw e;

            }
        }

        public static async Task<RootSites> SitesByClientOrSearch(int clientId, string s = "", int page = 1, int pageSize = 10000)
        {
            var fUrl = new Url(Interon.Roadlab.Core.Environment.LIMSAPI_SITESBYCLIENTORSEARCH_URL_ENDPOINT + $"?q={s}&Client={clientId}&page={page}&pagesize{pageSize}");

            try
            {
                var result = await fUrl.WithOAuthBearerToken(Interon.Roadlab.Core.Environment.BEARERTOKEN).WithTimeout(60).GetJsonAsync<RootSites>().ConfigureAwait(true);

                return result;

            }
            catch (FlurlHttpTimeoutException fex)
            {


                throw fex;


            }
            catch (FlurlHttpException ex)
            {

                throw ex;


            }
            catch (Exception e)
            {
                throw e;

            }
        }

        public static async Task<RootStatusModel> UpdateUser(InputUsers inputUsers,int id)
        {
            var fUrl = new Url(Interon.Roadlab.Core.Environment.LIMSAPI_USERUPDATE_URL_ENDPOINT + $"/{id}");

            try
            {


                PostHeaders(out var headers);
                var response = await fUrl.WithOAuthBearerToken(Interon.Roadlab.Core.Environment.BEARERTOKEN).WithTimeout(60).WithCookie("PHPSESSID", "32b86sqhme2ctpdtrkse6paos8; Path=/;").WithHeaders(headers).PostJsonAsync(inputUsers).ReceiveJson<RootStatusModel>().ConfigureAwait(true);


             

                return response;
            }
            catch (FlurlHttpTimeoutException fex)
            {


                throw fex;


            }
            catch (FlurlHttpException ex)
            {

                throw ex;


            }
            catch (Exception e)
            {
                throw e;

            }
        }

        public static async Task<object> GetUsrById(int id)
        {
            var fUrl = new Url(Interon.Roadlab.Core.Environment.LIMSAPI_GETUSER_URL_ENDPOINT + $"/id/{id}");

            try
            {
                var result = await fUrl.WithOAuthBearerToken(Interon.Roadlab.Core.Environment.BEARERTOKEN).WithTimeout(60).GetJsonAsync<RootSites>().ConfigureAwait(true);

                return result;

            }
            catch (FlurlHttpTimeoutException fex)
            {


                throw fex;


            }
            catch (FlurlHttpException ex)
            {

                throw ex;


            }
            catch (Exception e)
            {
                throw e;

            }
        }

        public static async Task<RootClientRequest> GetRequestById(int requestId)
        {
            var fUrl = new Url(Interon.Roadlab.Core.Environment.LIMSAPI_REQUESTBYID_URL_ENDPOINT + $"/{requestId}");

            try
            {



                GetHeaders(out var headers);

                var response = await fUrl.WithTimeout(60).WithHeaders(headers).GetJsonAsync().ConfigureAwait(true);

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(response, new JsonSerializerSettings()
                {
                    Error = HandleSerializationError,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                });

                RootClientRequest rootClientRequest = new RootClientRequest();
                try
                {


                    rootClientRequest =  JsonConvert.DeserializeObject<RootClientRequest>(json, new JsonSerializerSettings()
                    {
                        Error = HandleDeSerializationError,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    });

                }
                catch (JsonSerializationException ex)
                {
                    Debugger.Break();
                    Console.WriteLine(ex.Message);
                    // Could not find member 'DeletedDate' on object of type 'Account'. Path 'DeletedDate', line 4, position 23.
                }
                return rootClientRequest;
            }
            catch



                (FlurlHttpTimeoutException fex)
            {


                throw fex;


            }
            catch (FlurlHttpException ex)
            {
                Debugger.Break();
                throw ex;


            }
            catch (Exception e)
            {
                throw e;

            }
        }

        public static async Task<RootClientRequest> NewRequest(InputNewRequest cc)
        {
            var fUrl = new Url(Interon.Roadlab.Core.Environment.LIMSAPI_NEWREQUEST_URL_ENDPOINT );

            try
            {


                PostHeadersNewRequest( out var headers);
                var varinJson = JsonConvert.SerializeObject(cc);
                var response = await fUrl.WithOAuthBearerToken(Interon.Roadlab.Core.Environment.BEARERTOKEN).WithTimeout(60).WithCookie("PHPSESSID", "v7qdpdro520oo7i36sm1hkftp7; Path=/;").WithHeaders(headers).PostJsonAsync(cc).ReceiveJson().ConfigureAwait(true);
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(response, new JsonSerializerSettings()
                {
                    Error = HandleSerializationError,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                });

                RootClientRequest  rootClientRequest = new  RootClientRequest();
                try
                {

                 
                    rootClientRequest = JsonConvert.DeserializeObject<RootClientRequest>(json , new JsonSerializerSettings(){
                        Error   = HandleDeSerializationError,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    });

                }
                catch (JsonSerializationException ex)
                {
                    Debugger.Break();
                    Console.WriteLine(ex.Message);
                    // Could not find member 'DeletedDate' on object of type 'Account'. Path 'DeletedDate', line 4, position 23.
                }
                return rootClientRequest;



                return response;
            }
            catch (FlurlHttpTimeoutException fex)
            {

               
                throw fex;


            }
            catch (FlurlHttpException ex)
            {

                throw ex;


            }
            catch (Exception e)
            {
                throw e;

            }
        }
    }
}
