namespace SeedApp.Common.Logging
{
    public interface ILoggingProvider
    {
        void Log(
            string message,
            LogLevel level,
            object data,
            string[] tags,
            string callerFullTypeName,
            string callerMemberName);
    }
}