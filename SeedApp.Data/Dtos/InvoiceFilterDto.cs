using System;
namespace SeedApp.Data.Dtos
{
    public class InvoiceFilterDto
    {
        public DateTime? InvoiceDueDate { get; set; }

        public DateTime? FromTime { get; set; }

        public DateTime? ToTime { get; set; }

        public DateTime? GenerationFromTime { get; set; }

        public DateTime? GenerationToTime { get; set; }

        public string InvoiceNumber { get; set; }

        public int? SelectedClientBranch { get; set; }

        public int? SelectedClient { get; set; }

        public decimal? TotalWithTax { get; set; }

        public int? PaymentOption { get; set; }
    }
}
