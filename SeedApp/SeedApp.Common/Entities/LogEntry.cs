using System;
using Newtonsoft.Json;
using SQLite.Net.Attributes;

namespace SeedApp.Common.Entities
{
	public class LogEntry
	{
		public LogEntry()
		{
			DateTimeUtc = DateTime.UtcNow;
			DateTimeUtcPretty = DateTimeUtc.Value.ToString("O");
		}

		[PrimaryKey, AutoIncrement]
		[JsonIgnore]
		public long LogEntryId { get; set; }

		[JsonIgnore]
		public string Tags { get; set; }

		public string Message { get; set; }

		public string Level { get; set; }

		[JsonIgnore]
		public string Data { get; set; }

		[JsonProperty("Data")]
		[Ignore]
		public object DataObject { get; set; }

		[JsonProperty("Timestamp")]
		public DateTime? DateTimeUtc { get; set; }

		[JsonIgnore]
		public string DateTimeUtcPretty { get; set; }

		[JsonIgnore]
		public DateTime? SentToServerAtUtc { get; set; }

		[JsonIgnore]
		public bool? IsSkipped { get; set; }

		public string CallerMemberName { get; set; }

		public string CallerFullTypeName { get; set; }

		public long? TimeInMilliseconds { get; set; }

		public int ThreadId { get; set; }

		public int TaskId { get; set; }

		public string ClientDeviceRegistrationKey { get; set; }

		public string UserName { get; set; }

		public string Version { get; set; }

		public string Platform { get; set; }

		public string Build { get; set; }

		public string App { get; set; }

		public string SourceIp { get; set; }

		public override string ToString()
		{
			return
				$"[LogEntry: Tags={Tags}, Message={Message}, Level={Level}, Data={Data}, DateTimeUtc={DateTimeUtc}, SentToServerAtUtc={SentToServerAtUtc}, CallerMemberName={CallerMemberName}, TimeInMilliseconds={TimeInMilliseconds}, ThreadId={ThreadId},TaskId={TaskId},UserName={UserName},Version={Version},Platform={Platform},Build={Build},App={App},SourceIp={SourceIp}]";
		}
	}
}
