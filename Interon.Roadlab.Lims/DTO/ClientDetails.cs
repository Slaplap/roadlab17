using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class ClientDetails
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("trading_name")]
        public string TradingName { get; set; }

        [JsonProperty("previous_name")]
        public object PreviousName { get; set; }

        [JsonProperty("company_registration_number")]
        public object CompanyRegistrationNumber { get; set; }

        [JsonProperty("type_of_business_id")]
        public int TypeOfBusinessId { get; set; }

        [JsonProperty("postal_address")]
        public string PostalAddress { get; set; }

        [JsonProperty("physical_address")]
        public string PhysicalAddress { get; set; }

        [JsonProperty("delivery_address")]
        public object DeliveryAddress { get; set; }

        [JsonProperty("web_address")]
        public object WebAddress { get; set; }

        [JsonProperty("vat_registration_number")]
        public string VatRegistrationNumber { get; set; }

        [JsonProperty("tax_office")]
        public object TaxOffice { get; set; }

        [JsonProperty("account_accepted")]
        public int AccountAccepted { get; set; }

        [JsonProperty("physical_location_id")]
        public object PhysicalLocationId { get; set; }

        [JsonProperty("delivery_location_id")]
        public object DeliveryLocationId { get; set; }

        [JsonProperty("postal_location_id")]
        public object PostalLocationId { get; set; }

        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("account_type")]
        public string AccountType { get; set; }

        [JsonProperty("credit_limit")]
        public int CreditLimit { get; set; }

        [JsonProperty("credit_balance")]
        public string CreditBalance { get; set; }
    }
}