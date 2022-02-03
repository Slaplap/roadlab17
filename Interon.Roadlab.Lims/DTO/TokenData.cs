using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Interon.Roadlab.LIMS.DTO
{
    public class TokenData
    {
        [JsonProperty("site")]
        public object Site { get; set; }

        [JsonProperty("group")]
        public Group Group { get; set; }

        [JsonProperty("smses")]
        public List<Sms> Smses { get; set; }

        [JsonProperty("client")]
        public ClientDetails Client { get; set; }

        [JsonProperty("emails")]
        public List<Email> Emails { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("contact")]
        public Contact Contact { get; set; }

        [JsonProperty("quotation")]
        public Quotation Quotation { get; set; }

        [JsonProperty("accessHash")]
        public string AccessHash { get; set; }

        [JsonProperty("bookedDate")]
        public string BookedDate { get; set; }

        [JsonProperty("pastelOrder")]
        public PastelOrder PastelOrder { get; set; }

        [JsonProperty("formSheetIds")]
        public FormSheetIds FormSheetIds { get; set; }

        [JsonProperty("contact_email")]
        public string ContactEmail { get; set; }

        [JsonProperty("jobIsComplete")]
        public string JobIsComplete { get; set; }

        [JsonProperty("orderAccepted")]
        public string OrderAccepted { get; set; }

        [JsonProperty("purchaseOrder")]
        public PurchaseOrder PurchaseOrder { get; set; }

        [JsonProperty("quotation_pdf")]
        public QuotationPdf QuotationPdf { get; set; }

        [JsonProperty("requestNumber")]
        public string RequestNumber { get; set; }

        [JsonProperty("workConfirmed")]
        public string WorkConfirmed { get; set; }

        [JsonProperty("date_requested")]
        public string DateRequested { get; set; }

        [JsonProperty("quotationTotal")]
        public string QuotationTotal { get; set; }

        [JsonProperty("isJobSuccessful")]
        public string IsJobSuccessful { get; set; }

        [JsonProperty("needsCollection")]
        public string NeedsCollection { get; set; }

        [JsonProperty("paymentRequired")]
        public string PaymentRequired { get; set; }

        [JsonProperty("reporting_group")]
        public ReportingGroup ReportingGroup { get; set; }

        [JsonProperty("requestAccurate")]
        public string RequestAccurate { get; set; }

        [JsonProperty("originating_user")]
        public OriginatingUser OriginatingUser { get; set; }

        [JsonProperty("quotationOutcome")]
        public string QuotationOutcome { get; set; }

        [JsonProperty("assignedTechnician")]
        public AssignedTechnician AssignedTechnician { get; set; }

        [JsonProperty("aAnswerQuestionPair")]
        public List<object> AAnswerQuestionPair { get; set; }

        [JsonProperty("bookings_controller")]
        public BookingsController BookingsController { get; set; }

        [JsonProperty("timeSensitiveSample")]
        public string TimeSensitiveSample { get; set; }

        [JsonProperty("information_adequate")]
        public bool InformationAdequate { get; set; }

        [JsonProperty("accountInGoodStanding")]
        public string AccountInGoodStanding { get; set; }

        [JsonProperty("currentAccountBalance")]
        public string CurrentAccountBalance { get; set; }

        [JsonProperty("changesCostImplications")]
        public string ChangesCostImplications { get; set; }

        [JsonProperty("equipment_is_unavailable")]
        public string EquipmentIsUnavailable { get; set; }

        [JsonProperty("pastelQuotationReference")]
        public string PastelQuotationReference { get; set; }

        [JsonProperty("requested_service_details")]
        public string RequestedServiceDetails { get; set; }

        [JsonProperty("create_field_services_form")]
        public CreateFieldServicesForm CreateFieldServicesForm { get; set; }

        [JsonProperty("is_manager_review_rejected")]
        public bool IsManagerReviewRejected { get; set; }

        [JsonProperty("is_manager_review_requested")]
        public bool IsManagerReviewRequested { get; set; }

        [JsonProperty("fetch_field_services_answers")]
        public FetchFieldServicesAnswers FetchFieldServicesAnswers { get; set; }

        [JsonProperty("answer_sheet_endpoint_response")]
        public AnswerSheetEndpointResponse AnswerSheetEndpointResponse { get; set; }

        [JsonProperty("fetch_field_services_questions")]
        public FetchFieldServicesQuestions FetchFieldServicesQuestions { get; set; }

        [JsonProperty("answer_sheet_endpoint_response_questions")]
        public AnswerSheetEndpointResponseQuestions AnswerSheetEndpointResponseQuestions { get; set; }

        [JsonProperty("contact_surname")]
        public string ContactSurname { get; set; }

        [JsonProperty("contact_firstname")]
        public string ContactFirstname { get; set; }

        [JsonProperty("overrideAuthorisationTerms")]
        public string OverrideAuthorisationTerms { get; set; }

        [JsonProperty("accountStatusOverrideRequested")]
        public string AccountStatusOverrideRequested { get; set; }

        [JsonProperty("accountStatusOverrideAuthorised")]
        public string AccountStatusOverrideAuthorised { get; set; }

        [JsonProperty("delay")]
        public Delay Delay { get; set; }

        [JsonProperty("jobOutcomeComment")]
        public string JobOutcomeComment { get; set; }

        [JsonProperty("originating_technician")]
        public OriginatingTechnician OriginatingTechnician { get; set; }

        [JsonProperty("waitForJobExecutability")]
        public string WaitForJobExecutability { get; set; }

        [JsonProperty("unavailable_equipment_list_template")]
        public string UnavailableEquipmentListTemplate { get; set; }

        [JsonProperty("testobject")]
        public TestObject TestObject { get; set; }


    }

    //test object to test if there is no such field int the token 
    public class TestObject
    {
        public string Field1 { get; set; }
        public List<string> List { get; set; }
    }
}