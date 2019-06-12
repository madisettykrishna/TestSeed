using System;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Logging;
using Xamarin.Forms;

namespace SeedApp.Services
{
    public class MessagingService : IMessagingService
    {
        private readonly ILogger _logger;

        public MessagingService(ILogger logger)
        {
            _logger = logger;
        }

        public string OnAppResumeMsg { get; } = "OnAppResume";

        public string OnAppSleepMsg { get; } = "OnAppSleep";

        public void Send<TMessage>(TMessage message, object sender = null) where TMessage : IMessage
        {
            if (sender == null)
                sender = new object();

            _logger.Verbose($"Broadcasting message: {message.GetType().Name}", message, new[] { LoggerConstants.Messaging });

            MessagingCenter.Send(sender, typeof(TMessage).FullName, message);
        }

        public void Subscribe<TMessage>(object subscriber, Action<object, TMessage> callback) where TMessage : IMessage
        {
            MessagingCenter.Subscribe(subscriber, typeof(TMessage).FullName, callback, null);
        }

        public void Unsubscribe<TMessage>(object subscriber) where TMessage : IMessage
        {
            MessagingCenter.Unsubscribe<object, TMessage>(subscriber, typeof(TMessage).FullName);
        }
    }
}
