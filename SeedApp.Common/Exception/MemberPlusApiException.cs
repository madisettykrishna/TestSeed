using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Logging;

namespace SeedApp.Common.Exception
{
    public class MemberPlusApiException : System.Exception
    {
        public MemberPlusApiException(MmpApiErrorCodes errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
            SetUserMessage(errorCode);
        }

        public MmpApiErrorCodes ErrorCode { get; private set; }

        public string UserMessage { get; private set; }

        public static MemberPlusApiException ProcessApiException(System.Exception ex, int? id = null, string api = null, string verb = null, string url = null, object data = null)
        {
            WebException webEx = ex as WebException;

            bool isTimeout = ex is TaskCanceledException || (webEx != null && webEx.Status != WebExceptionStatus.UnknownError);
            bool isOnline = ContainerManager.Container.Resolve<IConnectivityHelper>().IsConnected;

            if (id != null)
                ContainerManager.Container.Resolve<ILogger>().Error($"InvocationId<{id}> Request to {api} failed",
                    new { ex.Message, isOnline, isTimeout, verb, url, data },
                    new[] { LoggerConstants.ApiRequest });

            Dictionary<string, object> metadata = new Dictionary<string, object>
                {
                    {"verb", verb},
                    {"url", url},
                    {"data", data},
                    {"isOnline", isOnline},
                    {"isTimedOut", isTimeout}
                };

            if (webEx != null)
                metadata.Add("WebExceptionStatus", webEx.Status.ToString());

            if (!isOnline)
                return new MemberPlusApiException(MmpApiErrorCodes.ConnectivityLost, ex.Message);

            if (!isTimeout)
                ContainerManager.Container.Resolve<IAppAnalyticsProvider>().ReportException(ex, new[] { LoggerConstants.ApiRequest }, metadata);

            return new MemberPlusApiException(MmpApiErrorCodes.ServerUnreachable, ex.Message);
        }

        private void SetUserMessage(MmpApiErrorCodes errorCode)
        {
            switch (errorCode)
            {
                case MmpApiErrorCodes.GenericError:
                    UserMessage = "We are unable to process your request at this time. Please try again.";
                    break;
                case MmpApiErrorCodes.ConnectivityLost:
                    UserMessage = "Lost connection. Reconnect to continue.";
                    break;
                case MmpApiErrorCodes.ServerUnreachable:
                    UserMessage = "Our servers are unreachable. Please check your connectivity and retry.";
                    break;
                case MmpApiErrorCodes.Unauthorized:
                    UserMessage = "Your session has expired. Please login again.";
                    break;
                case MmpApiErrorCodes.InvalidRequest:
                    UserMessage = "Invalid username and password. Please try again.";
                    break;
                case MmpApiErrorCodes.InvalidCurrentPassword:
                    UserMessage = "The current password is invalid.";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(errorCode), errorCode, null);
            }
        }
    }
}