using System;
using SeedApp.Common.Interfaces;

namespace SeedApp.Common.Messages
{
	public class ConnectivityChangedMessage : IMessage
	{
		public ConnectivityChangedMessage(bool isConnected)
		{
			IsConnected = isConnected;
		}

		public bool IsConnected { get; private set; }
	}
}
