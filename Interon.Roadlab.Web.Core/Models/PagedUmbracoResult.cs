using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Interon.Roadlab.Web.Core.Models
{
    [DataContract(Name = "pagedData", Namespace = "")]
    public class PagedUmbracoResult
    {
        [DataMember(Name = "data")]
        public List<object> Data { get; set; }

        [DataMember(Name = "currentPage")]
        public long CurrentPage { get; set; }

        [DataMember(Name = "itemsPerPage")]
        public long ItemsPerPage { get; set; }

        [DataMember(Name = "totalPages")]
        public long TotalPages { get; set; }

        [DataMember(Name = "totalItems")]
        public long TotalItems { get; set; }
    }
}