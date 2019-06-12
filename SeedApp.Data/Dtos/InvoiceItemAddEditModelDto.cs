using System;

namespace SeedApp.Data.Dtos
{
    public class InvoiceItemAddEditModelDto
    {
        public int InvoiceItemId { get; set; }

        public int SerialNo { get; set; }

        public string ItemName { get; set; }

        public decimal ItemQuantity { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TotalTrips { get; set; }

        public decimal TotalGPSCost { get; set; }

        public decimal TotalPenalties { get; set; }

        public decimal ItemUnitPrice { get; set; }

        public int TotalVehicles { get; set; }
    }
}
