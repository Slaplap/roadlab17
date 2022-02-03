using Interon.Roadlab.Core.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Interon.Roadlab.Core.Models
{
    public class RequestJsonDto

    {
        [JsonProperty("key")]
        public Guid Key { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("companyAccountNumber")]
        public string CompanyAccountNumber { get; set; }

        [JsonProperty("companyName")]
        public int BranchId { get; set; }

        [JsonProperty("branchId")]
        public string BranchName { get; set; }

        [JsonProperty("branchName")]
        public string CompanyName { get; set; }

        [JsonProperty("project")]
        public string Project { get; set; }

        [JsonProperty("requestBy")]
        public int RequestedBy { get; set; }

        [JsonProperty("requestByName")]
        public string RequestedByName { get; set; }

        [JsonProperty("contactPerson")]
        public string ContactPerson { get; set; }

        [JsonProperty("contactPersonNumber")]
        public string ContactPersonNumber { get; set; }

        [JsonProperty("contactPersonEmail")]
        public string ContactPersonEmail { get; set; }

        [JsonProperty("siteLocationAddress")]
        public string SiteLocationAddress { get; set; }

        [JsonProperty("specialInstructions")]
        public string SpecialInstructions { get; set; }

        [JsonProperty("createDate")]
        public DateTime CreateDate { get; set; }

        [JsonProperty("updateDate")]
        public DateTime UpdateDate { get; set; }
        [JsonProperty("syncDate")]
        public DateTime SyncDate { get; set; }
        [JsonProperty("actionDate")]
        public DateTime? ActionDate { get; set; }

        [JsonProperty("closedDate")]
        public DateTime? ClosedDate { get; set; }
        
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("comments")]
        public string comments { get; set; }
        [JsonProperty("orderNumber")]
        public string OrderNumber { get; set; }

        public List<ClientRequestLineJsonDto> ClientRequestLineJsonDtos { get; set; } = new List<ClientRequestLineJsonDto>();
        public List<ClientRequestFileDto> ClientRequestFileDtos { get; set; } = new List<ClientRequestFileDto>();
    }
}