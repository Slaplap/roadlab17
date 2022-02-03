using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
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

    
    public class BranchesApiController : UmbracoApiController
    {
        [HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public bool TestGet()
        {
            return true;
        }


        [HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public DateTime GetBranchesUpdateDate()
        {

            return Umbraco.ContentAtRoot().OfType<Branches>()?.FirstOrDefault()?.UpdateDate ?? DateTime.Now;

        }

        [HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public  BranchesDto GetAllBranches()
        {
           
            BranchesDto branchesDto = new BranchesDto();
            var umbracoBranches = Umbraco.ContentAtRoot().OfType<Branches>().FirstOrDefault()?.Children;
            foreach (Branch branch in umbracoBranches)
            {
                var branchDto = new BranchDto()
                {
                    Name = branch.Name,
                    Id = branch.Id,
                    Address1 = branch.BranchAddress1,
                    Address2 = branch.BranchAddress2,
                    Address3 = branch.BranchAddress3,
                    Code = branch.BranchPostalCode,
                    TelephoneNumber = branch.BranchTelephoneNumber,
                    CellNumber = branch.BranchCellNumber,
                    Longitude = branch.BranchLongitude,
                    Latitude = branch.BranchLatitude,
                    Email = branch.BranchEmail,
                    City = branch.BranchCity,
                    Province = branch.BranchProvince,
                    Country = branch.BranchCountry


                };
                branchesDto.Ids.Add(branchDto.Id.ToString());
                branchesDto.branchDtos.Add(branchDto);
            }

            return branchesDto;
        }

    }
}