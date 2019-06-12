using System;
using System.Collections.Generic;
using SQLite.Net.Attributes;

namespace SeedApp.Common.Models
{
    public class CurrentUser : EntityBase
    {
        [PrimaryKey, AutoIncrement]
        public override int Id { get; set; }

        public string FirstName { get; set; }

        public string Name { get; set; }

        public int ContactId { get; set; }

        public short TenantId { get; set; }

        public string TenantName { get; set; }

        public Guid TenantKey { get; set; }

        public int CurrentOrganizationId { get; set; }

        public string CurrentOrganizationName { get; set; }

        public string LastName { get; set; }

        public string TermsContent { get; set; }

        public bool HasAgreedOnTerms { get; set; }

        public int? TermsOfUseId { get; set; }

        public string TenantLogoUrl { get; set; }

        public string ContactLogoUrl { get; set; }
    }
}