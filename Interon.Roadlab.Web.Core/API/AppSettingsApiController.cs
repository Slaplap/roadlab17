using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Interon.Roadlab.Core.Dto;
using Our.Umbraco.AuthU.Web.WebApi;
using Umbraco.Web;
using Umbraco.Web.PublishedModels;
using Umbraco.Web.WebApi;

namespace Interon.Roadlab.Web.App.API
{
    
    public class AppSettingsApiController:UmbracoApiController
    {

        [HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public bool TestGet()
        {
            return true;
        }


        [HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public DateTime GetAppSettingsUpdateDate()
        {

            return Umbraco.ContentAtRoot().OfType<AppSettings>()?.FirstOrDefault()?.UpdateDate ?? new DateTime();

        }

        [HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public AppSettingsDto GetAppSettings()
        {
            var appSettingsDto = new AppSettingsDto();
           
            try
            {


                var umbracoAppSettings = Umbraco.ContentAtRoot().OfType<AppSettings>()?.FirstOrDefault();

                appSettingsDto.VersionString = umbracoAppSettings.AndroidVersionNumber;
                appSettingsDto.BuildString = umbracoAppSettings.AndroidBuildNumber;
                appSettingsDto.PlaystoreLink = umbracoAppSettings.PlaystoreLink;
                appSettingsDto.AppleLink = umbracoAppSettings.AppleLink;
                appSettingsDto.ForceReinstall = umbracoAppSettings.ForceReinstall;
                appSettingsDto.ForceUpdate = umbracoAppSettings.ForceUpgrade;



               
            }
            catch
            {
                Debugger.Break();
            }
            try
            {


                var umbracoAppSettings = Umbraco.ContentAtRoot().OfType<AppSettings>()?.FirstOrDefault();


                
            }
            catch
            {
                Debugger.Break();
            }
            return appSettingsDto;
        }
    }
}