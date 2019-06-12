using System;
using SQLite.Net.Attributes;

namespace SeedApp.Common.Models
{
    public class SyncMetadata : EntityBase
    {
        [PrimaryKey, AutoIncrement]
        public override int Id { get; set; }

        [Unique]
        public string EndpointName { get; set; }

        public DateTime? MaxModifiedDate { get; set; }

        public bool IsInitialRecordsSyncCompleted { get; set; }

        public int NumberOfRecordsSynced { get; set; }
    }
}