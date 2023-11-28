using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Interon.Roadlab.Web.Extensions
{
    public static class PublishedElementExtensionMethod
    {
        public static string AltText(this IPublishedElement content)
        {
            try
            {

                if (content.HasProperty("image"))
                {
                    var img = content.Value<IPublishedContent>("image");
                    var fd = img.GetProperty("fileDescription").Value().ToString();
                    return fd;
                }

                return "";
            }
            catch
            {
                return "";
            }
        }
    }

    public static class ExtensionMethod
    {
        public static string ImageUrl(this IPublishedContent content)
        {
            if (content != null)
            {
                return content.Url();
            }
            else
            {
                return "";
            }
        }
        public static string AltText(this IPublishedContent content)
        {
            try
            {

                if (content.HasProperty("fileDescription"))
                {
                    var str = content.GetProperty("fileDescription").Value().ToString();
                    return str;

                }

                return "";
            }
            catch
            {
                return "";
            }
        }


        

       
    }
}