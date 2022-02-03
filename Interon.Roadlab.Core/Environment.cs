namespace Interon.Roadlab.Core
{
    public static class Environment
    {
        public static readonly Stage Default = Stage.Local;
        static Stage _active = Default;
       


        public static Stage Active { get => _active; set => _active = value; }
        public enum Stage
        {
            Production,
            Staging,
            Development,
            Local
        }

        public static string CMSBaseUrl
        {
            get
            {
                switch (Active)
                {
                    case Stage.Production:
                        return "https://roadlab.co.za";
                    case Stage.Development:
                        return "https://roadlabdevelopment.azurewebsites.net";
                    case Stage.Staging:
                        return "https://roadlabstaging.azurewebsites.net";
                    case Stage.Local:
                        return "http://roadlab.ngrok.io";
                    default:
                        return "https://roadlab.co.za";
                }




            }

        }
        public static string CMSAPI { get;   } = "/umbraco/api";
        public static string CMSAPIURL { get; } = CMSBaseUrl + CMSAPI;
        public static string CMSAPI_TOKEN_ENDPOINT { get; } = "/oauth/token";
        public static string CMSAPI_AVAILABLE_URL_ENDPOINT { get; } =  CMSAPIURL + "/MainApi/" + "Available";
        public static string CMSAPI_APPSETTINGS_URL_ENDPOINT { get; } =  CMSAPIURL + "/AppSettingsApi/" + "GetAppSettings";
        public static string CMSAPI_MEMBERSHIPBYEMAIL_URL_ENDPOINT { get; } = CMSAPIURL + "/MemberApi/" + "GetMemberByEmail";
        public static string CMSAPI_REQUESTOTP_URL_ENDPOINT { get; } = CMSAPIURL  +"/MemberApi/" + "RequestOTP";
        public static string CMSAPI_SETPASSWORD_URL_ENDPOINT { get; } = CMSAPIURL + "/MemberApi/" + "SetPassword";
        public static string CMSAPI_SETMEMBERDEVICEID_URL_ENDPOINT { get; set; } = CMSAPIURL + "/MemberApi/" + "SetMemberDeviceId";
        public static string CMSAPI_CREATEMEMBER_URL_ENDPOINT { get; set; } = CMSAPIURL + "/MemberApi/" + "CreateMember";
        public static string CMSAPI_NOTIFICATIONS_URL_ENDPOINT { get; set; } = CMSAPIURL + "/NotificationsFrontendApi";



        public static string LIMSBaseUrl
        {
            get
            {
                switch (Active)
                {
                    case Stage.Production:
                        return "https://lims.roadlab.co.za";
                    case Stage.Development:
                        return "https://roadlabdemo.qa.comune.co.za";
                    case Stage.Staging:
                        return "https://roadlabdemo.qa.comune.co.za";
                    case Stage.Local:
                        return "http://roadlabdemo.qa.comune.co.za";
                    default:
                        return "https://lims.roadlab.co.za";
                }




            }

        }
        public static string LIMSAPIVERSION { get;   } = "api";
        public static string LIMSAPIURL { get;  } = LIMSBaseUrl + "/" ;
        public static string LIMSAPI_STATUS_URL_ENDPOINT { get; } = LIMSAPIURL + LIMSAPIVERSION +"/" + "status";
        public static string LIMSAPI_CLIENTSLIST_URL_ENDPOINT { get; } = LIMSAPIURL + "/client/ajaxgetclients";
        public static string LIMSAPI_CONTACTSLISTBYCOMPANY_URL_ENDPOINT { get; } = LIMSAPIURL + "/client/ajaxgetclientcontacts";
        public static string BEARERTOKEN { get; set; } = "8cc61965d51374b7afce2506351e2b7472a0d595";
        public static string LIMSAPI_CREATECONTACT_URL_ENDPOINT { get; set; } = LIMSAPIURL + "/clientapplication/registercontact";
        public static string LIMSAPI_REQUEST_URL_ENDPOINT { get; set; } = LIMSAPIURL + "/labmanagement/clientrequests";
        public static string LIMSAPI_LISTUSERSINGROUP_URL_ENDPOINT { get; } = LIMSAPIURL + "/groups/ajaxgetmembers/group_id";
        public static string LIMSAPI_GETGROUPANDCHILDREN_URL_ENDPOINT { get; } = LIMSAPIURL + "/groups/AjaxGetGroupChildren/group_id";
        public static string LIMSAPI_SERVICECATEGORIES_URL_ENDPOINT { get; } = LIMSAPIURL + "/service/categories";
        public static string LIMSAPI_VARIANTSBYCATEGORY_URL_ENDPOINT { get; } = LIMSAPIURL + "/service/variants";

        public static string LIMSAPI_SITESBYCLIENTORSEARCH_URL_ENDPOINT { get; }=LIMSAPIURL + "/client/ajaxgetclientsites";
        public static string LIMSAPI_GETUSER_URL_ENDPOINT { get; }=LIMSAPIURL + "/api/view/model/users";
        public static string LIMSAPI_USERUPDATE_URL_ENDPOINT { get; } = LIMSAPIURL + "/user/update";
        public static string LIMSAPI_USERCREATE_URL_ENDPOINT { get; } = LIMSAPIURL + "/user/create";
        public static string LIMSAPI_REQUESTBYID_URL_ENDPOINT { get; } = LIMSAPIURL + "labmanagement/request";
        public static string LIMSAPI_NEWREQUEST_URL_ENDPOINT { get; } = LIMSAPIURL + "labmanagement/newrequest/13";
    }
}