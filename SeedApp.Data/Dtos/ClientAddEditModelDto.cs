using System;
using System.Collections.Generic;

namespace SeedApp.Data.Dtos
{
    public class ClientAddEditModelDto
    {
        public int ClientId { get; set; }

        public string Name { get; set; }

        public string PAN { get; set; }

        public string GST { get; set; }

        public string ConcernEmails { get; set; }

        public List<ClientBranchAddEditModelDto> Branches { get; set; }
    }
}
