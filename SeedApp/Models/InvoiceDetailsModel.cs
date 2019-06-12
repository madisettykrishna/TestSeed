using System;

namespace SeedApp.Models
{
    public class InvoiceDetailsModel
    {
        public string CompanyName { get; set; }

        public string BranchName { get; set; }

        public string InvoiceDueDate { get; set; }

        public string Amount { get; set; }

        public string Status { get; set; }

        public string InvoiceNumber { get; set; }

        public string TotalPaidAmount { get; set; }

        public string TotalPendingAmount { get; set; }

        public string GSTAmount { get; set; }
    }
}
