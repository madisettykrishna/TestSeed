using System;
using SQLite.Net.Attributes;

namespace SeedApp.Common.Models
{
    public abstract class EntityBase
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        public DateTimeOffset ClientCreatedOnUtc { get; set; }

        public DateTimeOffset ClientModifiedOnUtc { get; set; }
    }
}