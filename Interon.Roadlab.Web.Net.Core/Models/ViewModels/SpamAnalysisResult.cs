using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Interon.Roadlab.Web.Net.Core.Models.ViewModels
{
    public class SpamAnalysisResult
    {
        [JsonPropertyName("classification")]
        public string Classification { get; set; } = "LEGITIMATE";
        
        [JsonPropertyName("confidence_score")]
        public int ConfidenceScore { get; set; }
        
        [JsonPropertyName("risk_level")]
        public string RiskLevel { get; set; } = "LOW";
        
        [JsonPropertyName("key_indicators")]
        public List<string> KeyIndicators { get; set; } = new List<string>();
        
        [JsonPropertyName("reasoning")]
        public string Reasoning { get; set; } = string.Empty;
        
        [JsonIgnore]
        public bool IsSpam => Classification.Equals("SPAM", StringComparison.OrdinalIgnoreCase);
        
        [JsonIgnore]
        public DateTime AnalyzedAt { get; set; } = DateTime.UtcNow;
    }
}