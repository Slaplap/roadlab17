// Use whatever namespacing works for your project.

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace Interon.Roadlab.Web.Core.API
{
    // If you want this endpoint to only be accessible when the user is logged in, 
    // then use UmbracoAuthorizedApiController instead of UmbracoApiController
    public class FileUploadApiController : UmbracoApiController
    {
        public async Task<HttpResponseMessage> UploadFileToServer()
        {
            try
            {
                var root = HttpContext.Current.Server.MapPath("~/App_Data/FileUploads");
                // Save File
                if (!Request.Content.IsMimeMultipartContent())
                {
                    
                    return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType, new HttpResponseException(HttpStatusCode.UnsupportedMediaType));
                }

                //Directory.CreateDirectory(afclpMediaPath);
                var provider = new MultipartFormDataStreamProvider(root);
                var result   = await Request.Content.ReadAsMultipartAsync(provider);

                if (result.FormData["model"] == null)
                {
                    
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new HttpResponseException(HttpStatusCode.BadRequest));
                }

                var model = result.FormData["model"];

                if (result.FileData.Count() > 0)
                {
                    //Get the files
                    foreach (var file in result.FileData)
                    {
                        //Save each uploaded file

                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, "Success!");

                //LogHelper.Error<HttpResponseException>("Error! (" + HttpStatusCode.InternalServerError + ") FileData Empty", new HttpResponseException(HttpStatusCode.InternalServerError));
                //throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, "Error! FileData Empty"));
            }
            catch (HttpResponseException ex)
            {
              
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }
    }

}
