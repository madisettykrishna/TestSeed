﻿using System;

namespace SeedApp.Data.Dtos
{
    public class ClientBranchAddEditModelDto
    {
        public int ClientBranchId { get; set; }

        public int ClientId { get; set; }

        public string BranchName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Zip { get; set; }

        public string PAN { get; set; }

        public string GST { get; set; }

        public string ConcernEmails { get; set; }
    }
}
