using System;
using Android.Content;

namespace SeedApp.Forms.Droid.BroadcastReceiver
{
	[BroadcastReceiver()]
	public class NetworkStatusBroadcastReceiver : Android.Content.BroadcastReceiver
	{
		public event EventHandler ConnectionStatusChanged;

		public override void OnReceive(Context context, Intent intent)
		{
			if (ConnectionStatusChanged != null)
				ConnectionStatusChanged(this, EventArgs.Empty);
		}
	}
}
