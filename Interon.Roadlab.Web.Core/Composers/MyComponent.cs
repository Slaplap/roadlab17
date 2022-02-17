using System.Linq;
using Our.Umbraco.AuthU;
using Our.Umbraco.AuthU.Data;
using Our.Umbraco.AuthU.Services;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Interon.Roadlab.Web.Core.Composers
{
    public class MyComponent : IComponent
    {
        public void Initialize()
        {
            OAuth.ConfigureEndpoint("/oauth/token", new OAuthOptions
            {
                UserService = new UmbracoMembersOAuthUserService(),
                SymmetricKey = "856FECBA3B06519C8DDDBC80BB080553",
                AccessTokenLifeTime = 2, // minutes
                AllowInsecureHttp = true, // During development only
                ClientStore = new UmbracoDbOAuthClientStore(),
                RefreshTokenStore = new UmbracoDbOAuthRefreshTokenStore(),
                RefreshTokenLifeTime = 525600, //minute
                
                AllowedOrigin = "*",
                
             
            });
            // ContentService.Saving += this.ContentService_Saving;
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjUxNjU1QDMxMzgyZTMxMmUzMGR0cHpvd1lEZmxhNkZtTkdncnc1Q2lzOUtKTlVEYzZoWGdqbFdzQUFYbXM9");
        }

        public void Terminate()
        {
        }

        /// <summary>
        /// Listen for when content is being saved, check if it is a
        /// new item and fill in some default data.
        /// </summary>
        private void ContentService_Saving(IContentService sender, SaveEventArgs<IContent> e)
        {
            foreach (var content in e.SavedEntities
                //Check if the content item type has a specific alias
                .Where(c => c.ContentType.Alias.InvariantEquals("MyContentType"))
                //Check if it is a new item
                .Where(c => c.HasIdentity == false))
            {
                //check if the item has a property called 'richText'
                if (content.HasProperty("richText"))
                {
                    //get the rich text value
                    var val = content.GetValue<string>("richText");

                    //if there is a rich text value, set a default value in a
                    // field called 'excerpt' that is the first
                    // 200 characters of the rich text value
                    content.SetValue("excerpt", val == null
                        ? string.Empty
                        : string.Join(string.Empty, val.StripHtml().StripNewLines().Take(200)));
                }
            }
        }
    }
}