using System;

namespace SeedApp.Common.Interfaces
{
    public interface IDeviceProvider
    {
        void StartTimer(TimeSpan interval, Func<bool> callback);

        T OnPlatform<T>(T iOS, T android, T winPhone);

        void InvokeOnMainThread(Action action);

        string DeviceType { get; }
    }
}