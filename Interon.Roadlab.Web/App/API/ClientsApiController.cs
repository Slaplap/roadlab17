using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Interon.Roadlab.Core;
using Interon.Roadlab.Core.Dto;
using Interon.Roadlab.Core.Models;
using Microsoft.IdentityModel.Logging;
using Our.Umbraco.AuthU;
using Our.Umbraco.AuthU.Web.WebApi;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Persistence.Querying;
using Umbraco.Core.Services;
using Umbraco.Core.Services.Implement;
using Umbraco.Web.PublishedCache;
using Umbraco.Web.PublishedModels;
using Umbraco.Web.WebApi;
using Branch = Umbraco.Web.PublishedModels.Branch;
using Company = Umbraco.Web.PublishedModels.Company;

namespace Interon.Roadlab.Web.App.API
{

    
    public class ClientsApiController : UmbracoApiController
    {
        [HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public bool TestGet()
        {
            return true;
        }
        [HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public async Task<LIMS.DTO.Client> GetClientByAccountNumber(string accountNumber)
        {

            var company = await  LIMS.Services.ClientsAndContactsService.ClientByAccountNumberAsync(accountNumber);
            return company;

        }
      
    }
}