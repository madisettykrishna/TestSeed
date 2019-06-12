using System;
using System.Collections.Generic;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Messages;
using SeedApp.Common.Models;
using Mindscape.Raygun4Net;
using Mindscape.Raygun4Net.Messages;

namespace SeedApp.SharedCode.Raygun
{
    public class RaygunAppAnalyticsProvider : IAppAnalyticsProvider
    {
        private readonly IMemberPlusAppConfig _appConfig;
        private readonly IPlatformService _platformService;

        public RaygunAppAnalyticsProvider(IMemberPlusAppConfig appConfig, IPlatformService platformService,
            IMessagingService messagingService)
        {
            _appConfig = appConfig;
            _platformService = platformService;

            ////messagingService.Subscribe<UserDetailsRefreshedMessage>(this, (o, e) => SetUser(e.User));
        }

        public void Initialize()
        {
            RaygunClient.Current.ApplicationVersion = _platformService.AppVersion;
        }

        public void ReportException(Exception ex, string[] tags = null, Dictionary<string, object> data = null)
        {
            RaygunClient.Current.SendInBackground(ex, tags, data);
        }

        public void ReportApiTiming(string api, long timeInMilliSecs)
        {
            ////try
            ////{
            ////    RaygunClient.Current.SendPulseTimingEvent(RaygunPulseEventType.NetworkCall, api, timeInMilliSecs);
            ////}
            ////catch (Exception)
            ////{
            //// KB: THis is not super important call so we swallow exception if it fails
            ////}
        }

        private void SetUser(CurrentUser user)
        {
            RaygunClient.Current.UserInfo = new RaygunIdentifierMessage(user.ContactId.ToString())
            {
                FirstName = user.FirstName,
                FullName = user.Name,
            };
        }
    }
}