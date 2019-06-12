using System;
using SQLite.Net.Attributes;

namespace SeedApp.Common.Models
{
    public class LogEntry : EntityBase
    {
        [PrimaryKey, AutoIncrement]
        public override int Id { get; set; }

        public string Message { get; set; }

        public string Level { get; set; }

        public string Tags { get; set; }

        public string CallerMemberName { get; set; }

        public string CallerFullTypeName { get; set; }

        public string Data { get; set; }

        public int TaskId { get; set; }

        public string CreatedOn { get; set; }

        public string Version { get; set; }

        public override string ToString()
        {
            return $"Message={Message},Level={Level},Tags={Tags},CallerMemberName={CallerMemberName},CallerFullTypeName={CallerFullTypeName},Data={Data},TaskId={TaskId},CreatedOn={CreatedOn},Version={Version}";
        }
    }
}
