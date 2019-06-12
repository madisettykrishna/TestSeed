using System;

namespace SeedApp.Data.Dtos
{
    public class PaymentAddEditModelDto
    {
        public int InvoicePaymentId { get; set; }

        public DateTime OnDate { get; set; }

        public decimal Amount { get; set; }

        public string BankName { get; set; }

        public string BankTransaction { get; set; }

        public string MadeBy { get; set; }

        public string PaymentMode { get; set; }

        public bool IsItPartPayment { get; set; }

        public DateTime? RemainingPaymentExpectedDate { get; set; }

        public string PaymentNote { get; set; }

        public int InvoiceId { get; set; }
    }
}
