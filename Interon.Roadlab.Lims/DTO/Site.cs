using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class Site
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public object Description { get; set; }

        [JsonProperty("pax")]
        public object Pax { get; set; }

        [JsonProperty("unit_number")]
        public object UnitNumber { get; set; }

        [JsonProperty("complex_name")]
        public object ComplexName { get; set; }

        [JsonProperty("street_number")]
        public object StreetNumber { get; set; }

        [JsonProperty("street_name")]
        public object StreetName { get; set; }

        [JsonProperty("suburb")]
        public object Suburb { get; set; }

        [JsonProperty("city")]
        public object City { get; set; }

        [JsonProperty("country")]
        public object Country { get; set; }

        [JsonProperty("postal_code")]
        public object PostalCode { get; set; }

        [JsonProperty("longitude")]
        public object Longitude { get; set; }

        [JsonProperty("latitude")]
        public object Latitude { get; set; }

        [JsonProperty("state")]
        public object State { get; set; }

        [JsonProperty("phone_number")]
        public object PhoneNumber { get; set; }
        [JsonProperty("text")]
        public object Text { get; set; }
        [JsonIgnore]
        public string SiteName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Name))
                {
                    return Name;
                }
                else
                {
                    if (Text != null)
                    {
                        return (string)Text;
                    }

                    return "";
                }
            }
        }
    }

}