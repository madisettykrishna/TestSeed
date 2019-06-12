using System.Runtime.CompilerServices;

namespace SeedApp.Common.Logging
{
    public interface ILogger
    {
        void Error(
            string message,
            object data = null,
            string[] tags = null,
            [CallerMemberName] string callerName = null, string callerFullTypeName = null);

        void Exception(System.Exception exception, string[] tags = null, [CallerMemberName] string callerName = null,
            string callerFullTypeName = null);

        void Info(string message, object data = null, string[] tags = null, [CallerMemberName] string callerName = null,
            string callerFullTypeName = null);

        void Verbose(
            string message,
            object data = null,
            string[] tags = null,
            [CallerMemberName] string callerName = null, string callerFullTypeName = null);

        void Warning(
            string message,
            object data = null,
            string[] tags = null,
            [CallerMemberName] string callerName = null, string callerFullTypeName = null);
    }
}