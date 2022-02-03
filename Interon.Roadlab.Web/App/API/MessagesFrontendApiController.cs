using Interon.Roadlab.Core.Dto;
using Interon.Roadlab.Web.App.Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Interon.Roadlab.Core.Models;
using Interon.Roadlab.Web.App.Models;
using Umbraco.Web.WebApi;
using Member = Umbraco.Web.PublishedModels.Member;

namespace Interon.Roadlab.Web.App.API
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