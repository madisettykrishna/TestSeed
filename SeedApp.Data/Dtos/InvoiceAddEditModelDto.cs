using System;
using System.Collections.Generic;

namespace SeedApp.Data.Dtos
{
    public class InvoiceAddEditModelDto
    {
        public int InvoiceId { get; set; }

        public DateTime InvoiceDueDate { get; set; }

        public DateTime FromTime { get; set; }

        public DateTime ToTime { get; set; }

        public string InvoiceNumber { get; set; }

        public string PoNO { get; set; }

        public string GST { get; set; }

        public string SACNo { get; set; }

        public int ClientBranchId { get; set; }

        public int ClientId { get; set; }

        public ClientBranchAddEditModelDto InvoiceTo { get; set; }

        public decimal CGSTRate { get; set; }

        public decimal CGSTAmount { get; set; }

        public decimal SGSTRate { get; set; }

        public decimal SGSTAmount { get; set; }

        public decimal TotalWithoutTax { get; set; }

        public decimal TotalWithTax { get; set; }

        public int UserId { get; set; }

        public int OrgId { get; set; }

        public List<InvoiceItemAddEditModelDto> InvoiceItems { get; set; }

        public string CGST { get; set; }

        public string SGST { get; set; }

        public string ClientName { get; set; }

        public decimal TotalPenalties { get; set; }

        public decimal TotalPendingAmount { get; set; }

        public int Status { get; set; }
    }
}
