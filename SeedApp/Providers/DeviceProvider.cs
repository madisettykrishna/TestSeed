using System;
using SeedApp.Common.Interfaces;
using Xamarin.Forms;

namespace SeedApp.Providers
{
    public class DeviceProvider : IDeviceProvider
    {
        public string DeviceType => Device.RuntimePlatform;

        public void InvokeOnMainThread(Action action)
        {
            Device.BeginInvokeOnMainThread(action);
        }

        public T OnPlatform<T>(T iOS, T android, T winPhone)
        {
            return Device.OnPlatform<T>(iOS, android, winPhone);
        }

        public void StartTimer(TimeSpan interval, Func<bool> callback)
        {
            Device.StartTimer(interval, callback);
        }
    }
}
