using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class FetchFieldServicesQuestions
    {
        [JsonProperty("requestUrl")]
        public string RequestUrl { get; set; }

        [JsonProperty("requestType")]
        public string RequestType { get; set; }

        [JsonProperty("headerConfig")]
        public List<string> HeaderConfig { get; set; }

        [JsonProperty("responseName")]
        public string ResponseName { get; set; }

        [JsonProperty("templateVariables")]
        public List<object> TemplateVariables { get; set; }

        [JsonProperty("parentSaveVariableName")]
        public string ParentSaveVariableName { get; set; }

        [JsonProperty("parent_save_json_paths")]
        public ParentSaveJsonPaths ParentSaveJsonPaths { get; set; }
    }
}