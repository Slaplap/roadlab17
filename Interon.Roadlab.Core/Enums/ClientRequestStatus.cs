using System.Linq;

namespace Interon.Roadlab.Core.Enums
{
    public static class ClientRequestStatus
    {
        public static readonly string Local = "Local"; 
        public static readonly string Open = "Open"; 
        public static readonly string Ready = "Ready"; 
        public static readonly string Accepted = "Accepted"; 
        public static readonly string Rejected = "Rejected"; 
        public static readonly string Cancelled = "Cancelled"; 
        public static readonly string Closed = "Closed"; 
        public static readonly string Pending = "Pending"; 
        public static readonly string Test = "Test"; 
        public static readonly string Quote = "Quote"; 
        public static readonly string Booking = "Booking"; 
        public static readonly string Order = "Order";
        public static readonly string Error = "Errors";

        public static readonly string QuoteLocal = string.Join(" ", Quote, Local);
        public static readonly string QuoteOpen = string.Join(" ", Quote, Open);
        public static readonly string QuotePending = string.Join(" ", Quote, Pending);
        public static readonly string QuoteReady = string.Join(" ", Quote, Ready);
        public static readonly string QuoteAccepted = string.Join(" ", Quote, Accepted);
        public static readonly string QuoteRejected = string.Join(" ", Quote, Rejected);
        public static readonly string QuoteCanceled = string.Join(" ", Quote, Cancelled);
        public static readonly string QuoteClosed = string.Join(" ", Quote, Closed);

        public static readonly string OrderClosed = string.Join(" ", Order, Closed);
        public static readonly string OrderOpen = string.Join(" ", Order, Open);
        public static readonly string OrderPending = string.Join(" ", Order, Pending);
        public static readonly string OrderCancelled = string.Join(" ", Order, Cancelled);


        public static readonly string BookingLocal = string.Join(" ", Booking, Local);
        public static readonly string BookingOpen = string.Join(" ", Booking, Open);
        public static readonly string BookingPending = string.Join(" ", Booking, Pending);
        public static readonly string BookingReady = string.Join(" ", Booking, Ready);
        public static readonly string BookingAccepted = string.Join(" ", Booking, Accepted);
        public static readonly string BookingRejected = string.Join(" ", Booking, Rejected);
        public static readonly string BookingCanceled = string.Join(" ", Booking, Cancelled);
        public static readonly string BookingClosed = string.Join(" ", Booking, Closed);

        public static readonly string TestOpen = string.Join(" ", Test, Open);
        public static readonly string TestPending = string.Join(" ", Test, Pending);
        public static readonly string TestReady = string.Join(" ", Test, Ready);
        public static readonly string TestCancelled = string.Join(" ", Test, Cancelled);
        public static readonly string TestClosed = string.Join(" ",Test,Closed);
      

        //Use ClientRequestStatus Readonly Fields
        public static string GetValue(string part1,string part2)
        {
            var o     = typeof(ClientRequestStatus).GetFields().FirstOrDefault(x => x.Name.Contains(part1.ToString()) && x.Name.Contains(part2.ToString()));
            var value = o.GetValue(o);
            return value.ToString();
        }
        public  static string StatusContverter(this string limsStatus)
        {
            switch (limsStatus)
            {
                case "Archived": return ClientRequestStatus.QuoteCanceled;
                case "Change request": return ClientRequestStatus.BookingPending;
                case "EBG Proceed with request?": return ClientRequestStatus.BookingPending;
                case "Request for service": return ClientRequestStatus.QuoteOpen;
                case "Review request": return ClientRequestStatus.QuotePending;
                case "EBG Adequate information review": return ClientRequestStatus.QuotePending;
                case "Update Request": return ClientRequestStatus.QuotePending;
                case "EMG Adequate information review": return ClientRequestStatus.QuotePending;
                case "EMG Reviewed and Rejected By Manager": return ClientRequestStatus.QuoteCanceled;
                case "Create Quotation": return ClientRequestStatus.QuotePending;
                case "EBG Requested Manager Review": return ClientRequestStatus.QuotePending;
                case "Review Quotation": return ClientRequestStatus.QuotePending;
                case "EMG Requested Manager Review": return ClientRequestStatus.QuotePending;
                case "EBG Manager Rejected Review": return ClientRequestStatus.QuotePending;
                case "End": return ClientRequestStatus.BookingClosed;
                case "Quotation Outcome": return ClientRequestStatus.QuoteReady;
                case "EBG Quotation Outcome": return ClientRequestStatus.QuoteReady;
                case "EMG Quotation Outcome": return ClientRequestStatus.QuoteReady;
                case "Follow-up details": return ClientRequestStatus.QuoteReady;
                case "Follow up": return ClientRequestStatus.QuoteReady;
                case "Rejection Reason": return ClientRequestStatus.QuoteRejected;
                case "EBG Existing Client": return ClientRequestStatus.QuoteAccepted;
                case "EMG Existing Client": return ClientRequestStatus.QuoteAccepted;
                case "SubProcess: Client Application": return ClientRequestStatus.QuoteAccepted;
                case "Payment Reminders Sent Sequence": return ClientRequestStatus.OrderPending;
                case "EBG Application Accepted": return ClientRequestStatus.OrderPending;
                case "Request Number Sequence": return ClientRequestStatus.QuotePending;
                case "Record job cancellation": return ClientRequestStatus.BookingCanceled;
                case "Job cancelled": return ClientRequestStatus.BookingCanceled;
                case "Record Job Outcome": return ClientRequestStatus.BookingPending;
                case "Raise sales order": return ClientRequestStatus.OrderPending;
                case "EBG Account In Good Standing": return ClientRequestStatus.OrderPending;
                case "Request Account Status Override": return ClientRequestStatus.OrderPending;
                case "EBG Account Status Override Requested": return ClientRequestStatus.OrderPending;
                case "Authorise Account Status Override": return ClientRequestStatus.OrderPending;
                case "EMG Account Status Override Requested": return ClientRequestStatus.OrderPending;
                case "EMG Account In Good Standing": return ClientRequestStatus.OrderPending;
                case "Payment Required": return ClientRequestStatus.OrderPending;
                case "EBG Payment Required": return ClientRequestStatus.OrderPending;
                case "EMG Payment Required": return ClientRequestStatus.OrderPending;
                case "Payment Reminders": return ClientRequestStatus.OrderPending;
                case "Receive payment": return ClientRequestStatus.OrderPending;
                case "EMG Payment Reminders": return ClientRequestStatus.OrderPending;
                case "EMG Payment Reminders Sent": return ClientRequestStatus.OrderPending;
                case "EBG Payment Reminders Sent": return ClientRequestStatus.OrderPending;
                case "Email Send Payment Request": return ClientRequestStatus.OrderPending;
                case "Payment Reminder Delay": return ClientRequestStatus.OrderPending;
                case "EBG Job Complete": return ClientRequestStatus.TestOpen;
                case "Record Delay": return ClientRequestStatus.TestPending;
                case "Wait For Job Executability?": return ClientRequestStatus.TestPending;
                case "EBG Waiting For Job Executability": return ClientRequestStatus.TestPending;
                case "Email Payment Reminder": return ClientRequestStatus.OrderPending;
                case "Email: Delay Notification": return ClientRequestStatus.OrderPending;
                case "Generate Access Hash": return ClientRequestStatus.QuotePending;
                case "Email Order acceptance": return ClientRequestStatus.OrderPending;
                case "SMS Order Acceptance": return ClientRequestStatus.OrderPending;
                case "Record Acceptance": return ClientRequestStatus.OrderPending;
                case "EBG Order Accepted": return ClientRequestStatus.OrderPending;
                case "Return Equipment": return ClientRequestStatus.TestPending;
                case "IBG Work Confirmed": return ClientRequestStatus.TestPending;
                case "EMG Work Confirmed": return ClientRequestStatus.TestPending;
                case "Delay Node: Wait for work to be confirmed": return ClientRequestStatus.TestPending;
                case "No confirmation": return ClientRequestStatus.TestPending;
                case "Send sales order": return ClientRequestStatus.OrderPending;
                case "EBG Payment Reference": return ClientRequestStatus.OrderPending;
                case "EMG Changes Cost Implications": return ClientRequestStatus.OrderPending;
                case "Commit resources to booking": return ClientRequestStatus.BookingPending;
                case "Validate request accurate": return ClientRequestStatus.BookingPending;
                case "EBG Request Accurate": return ClientRequestStatus.BookingPending;
                case "Dispatch technician": return ClientRequestStatus.BookingAccepted;
                case "EBG Changes Cost Implications": return ClientRequestStatus.BookingPending;
                case "Update Quote": return ClientRequestStatus.QuotePending;
                case "Add 5% to account limit": return ClientRequestStatus.QuotePending;
                case "Cancel sales order": return ClientRequestStatus.OrderCancelled;
                case "EMG Cost Implications": return ClientRequestStatus.OrderPending;
                case "SMS Payment Reminder": return ClientRequestStatus.OrderPending;
                case "Review Equipment Required": return ClientRequestStatus.BookingReady;
                case "Equipment Received In Working Order": return ClientRequestStatus.TestPending;
                case "Confirm Work With Client": return ClientRequestStatus.TestPending;
                case "EBG Work Confirmed With Client?": return ClientRequestStatus.TestPending;
                case "EMG Waiting For Job To Execute": return ClientRequestStatus.TestPending;
                case "Job Complete?": return ClientRequestStatus.TestPending;
                case "Subprocess: Field Services Create Form": return ClientRequestStatus.TestPending;
                case "EBG Equipment Unavailable": return ClientRequestStatus.TestPending;
                case "EMG Equipment Unavailable": return ClientRequestStatus.TestPending;
                case "Email Equipment Unavailable": return ClientRequestStatus.TestPending;
                case "EBG Collection Required": return ClientRequestStatus.TestPending;
                case "EMG Collection Required": return ClientRequestStatus.TestPending;
                case "Email Collection Required": return ClientRequestStatus.TestPending;
                case "Subprocess: Field Services Fetch Form Answers": return ClientRequestStatus.TestPending;
                case "Subprocess: Field Services Fetch Form Questions": return ClientRequestStatus.TestPending;
                case "Migration: Save Question Answer Pair": return ClientRequestStatus.TestPending;
                case "SubProcess: Sample": return ClientRequestStatus.TestPending;
                case "Distribute Quotation Documents": return ClientRequestStatus.QuotePending;
                case "Generate Quotation Documents": return ClientRequestStatus.QuotePending;
                default: return ClientRequestStatus.Error;


            }
        }
    }
}