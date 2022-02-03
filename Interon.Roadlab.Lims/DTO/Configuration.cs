using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class Configuration
    {
        [JsonProperty("prefix")]
        public object Prefix { get; set; }

        [JsonProperty("suffix")]
        public object Suffix { get; set; }

        [JsonProperty("apiCall")]
        public ApiCall ApiCall { get; set; }

        [JsonProperty("addPrefix")]
        public bool AddPrefix { get; set; }

        [JsonProperty("addSuffix")]
        public bool AddSuffix { get; set; }

        [JsonProperty("maxNumber")]
        public object MaxNumber { get; set; }

        [JsonProperty("minNumber")]
        public object MinNumber { get; set; }

        [JsonProperty("isRequired")]
        public bool IsRequired { get; set; }

        [JsonProperty("dateInputType")]
        public string DateInputType { get; set; }

        [JsonProperty("allowPastDates")]
        public bool AllowPastDates { get; set; }

        [JsonProperty("allowFutureDates")]
        public bool AllowFutureDates { get; set; }

        [JsonProperty("itemTypesAllowed")]
        public List<object> ItemTypesAllowed { get; set; }

        [JsonProperty("applyQuantityLimits")]
        public bool ApplyQuantityLimits { get; set; }

        [JsonProperty("inputValidationType")]
        public object InputValidationType { get; set; }

        [JsonProperty("itemSelectionTagIds")]
        public List<object> ItemSelectionTagIds { get; set; }

        [JsonProperty("allowMultipleAnswers")]
        public bool AllowMultipleAnswers { get; set; }

        [JsonProperty("locationTypesAllowed")]
        public List<object> LocationTypesAllowed { get; set; }

        [JsonProperty("displayYesNoOptionsAs")]
        public string DisplayYesNoOptionsAs { get; set; }

        [JsonProperty("sectionsToShowSummary")]
        public List<string> SectionsToShowSummary { get; set; }

        [JsonProperty("locationSelectionTagIds")]
        public List<object> LocationSelectionTagIds { get; set; }

        [JsonProperty("allowedTakeMediaFileTypes")]
        public List<string> AllowedTakeMediaFileTypes { get; set; }

        [JsonProperty("allowedAttachmentFileTypes")]
        public List<string> AllowedAttachmentFileTypes { get; set; }

        [JsonProperty("subtractQuantityFromLocation")]
        public bool SubtractQuantityFromLocation { get; set; }

        [JsonProperty("displayMultipleChoiceOptionsAs")]
        public string DisplayMultipleChoiceOptionsAs { get; set; }

        [JsonProperty("displayYesNoOptionsAsCustomTextNegative")]
        public string DisplayYesNoOptionsAsCustomTextNegative { get; set; }

        [JsonProperty("displayYesNoOptionsAsCustomTextPositive")]
        public string DisplayYesNoOptionsAsCustomTextPositive { get; set; }

        [JsonProperty("showCost")]
        public bool ShowCost { get; set; }

        [JsonProperty("showPrice")]
        public bool ShowPrice { get; set; }

        [JsonProperty("showTotal")]
        public bool ShowTotal { get; set; }

        [JsonProperty("showAvailableQuantities")]
        public bool ShowAvailableQuantities { get; set; }
    }
}