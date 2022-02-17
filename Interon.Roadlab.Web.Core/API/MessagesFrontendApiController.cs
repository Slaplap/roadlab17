using System.Web.Http;
using Interon.Roadlab.Core.Dto;
using Interon.Roadlab.Web.Core.Services;
using Umbraco.Web.WebApi;

namespace Interon.Roadlab.Web.Core.API
{
    public class MessagesFrontendApiController : UmbracoApiController
    {
        private MessageService _MessageService;

        public MessagesFrontendApiController(MessageService MessageService)
        {
            this._MessageService = MessageService;
        }

        public MessagesFrontendApiController()
        {
        }

      

        [System.Web.Mvc.HttpPost]
        [System.Web.Http.AcceptVerbs("POST")]
        public bool SaveMessageJsonDto([FromBody] MessageJsonDto MessageDto)
        {
            _MessageService.CreateOrUpdateMessage(_MessageService.DtoToMessage(MessageDto),false,false);

            return true;
        }
         
    }
}