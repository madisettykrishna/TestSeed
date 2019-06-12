using System;
using SeedApp.Common.Enums;

namespace SeedApp.Common.Interfaces
{
    public interface IPlatformService
    {
        Size ScreenSize { get; }

        void InvokeOnMainThread(Action action);

        void InvokeOnBackgroundThread(Action action);

        string AppVersion { get; }

        string OsVersion { get; }

        string DeviceName { get; }

        DeviceSize DeviceSize { get; }
    }
}