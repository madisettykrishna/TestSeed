using System;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Logging;
using Xamarin.Forms;

namespace SeedApp
{
	public class MessagingService : IMessagingService
	{
		private readonly ILogger _logger;

		public MessagingService(ILogger logger)
		{
			_logger = logger;
		}

		public void Send<TMessage>(TMessage message, object sender = null) where TMessage : IMessage
		{
			if (sender == null)
				sender = new object();

			_logger.Info($"Broadcasting message: {message.GetType().Name}", message, new[] { LoggerConstants.Messaging });

			MessagingCenter.Send(sender, message.GetType().FullName, message);
		}

		public void Subscribe<TMessage>(object subscriber, Action<object, TMessage> callback) where TMessage : IMessage
		{
			MessagingCenter.Subscribe(subscriber, typeof(TMessage).FullName, callback);
		}

		public void Unsubscribe<TMessage>(object subscriber) where TMessage : IMessage
		{
			MessagingCenter.Unsubscribe<object, TMessage>(subscriber, typeof(TMessage).FullName);
		}
	}
}
