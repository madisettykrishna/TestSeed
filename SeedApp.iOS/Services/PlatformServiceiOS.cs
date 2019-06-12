using System;
using System.Collections.Generic;
using System.Threading;
using Foundation;
using SeedApp.Common;
using SeedApp.Common.Constants;
using SeedApp.Common.Enums;
using SeedApp.Common.Interfaces;
using SeedApp.IOS.Providers;
using UIKit;

namespace SeedApp.IOS.Services
{
    public class PlatformServiceIos : IPlatformService
    {
        private readonly Lazy<string> _lazyAppVersion;
        private readonly Lazy<Version> _lazyOsVersion;
        private readonly Lazy<string> _lazyBundleId;
        private readonly Dictionary<Size, DeviceSize> _types = new Dictionary<Size, DeviceSize>();

        public PlatformServiceIos()
        {
            _lazyAppVersion = new Lazy<string>(() =>
            {
                if (NSBundle.MainBundle.InfoDictionary == null)
                    return string.Empty;

                NSObject buildObject;
                var build = NSBundle.MainBundle.InfoDictionary.TryGetValue((NSString)"CFBundleVersion",
                    out buildObject)
                    ? buildObject.ToString()
                    : string.Empty;

                return $"{build}";
            });

            _lazyOsVersion = new Lazy<Version>(() => new Version(UIDevice.CurrentDevice.SystemVersion));

            //// This will obtain the BundleId of the app.
            _lazyBundleId = new Lazy<string>(() =>
            {
                NSObject bundleId;
                return NSBundle.MainBundle.InfoDictionary.TryGetValue((NSString)"CFBundleIdentifier", out bundleId) ? bundleId.ToString() : string.Empty;
            });

            _types.Add(new Size(DeviceConstants.ScreenWidth.IPhone4, DeviceConstants.ScreenHeight.IPhone4), DeviceSize.XSmall);
            _types.Add(new Size(DeviceConstants.ScreenWidth.IPhone5, DeviceConstants.ScreenHeight.IPhone5), DeviceSize.Small);
            _types.Add(new Size(DeviceConstants.ScreenWidth.IPhone6And7, DeviceConstants.ScreenHeight.IPhone6And7), DeviceSize.Medium);
            _types.Add(new Size(DeviceConstants.ScreenWidth.IPhone6And7Plus, DeviceConstants.ScreenHeight.IPhone6And7Plus), DeviceSize.Large);
            _types.Add(new Size(DeviceConstants.ScreenWidth.IPadAir, DeviceConstants.ScreenHeight.IPadAir), DeviceSize.XLarge);
            _types.Add(new Size(DeviceConstants.ScreenWidth.IPadPro, DeviceConstants.ScreenHeight.IPadPro), DeviceSize.XXLarge);
        }

        public bool IsConnected => Reachability.InternetConnectionStatus() != NetworkStatus.NotReachable;

        public Size ScreenSize => new Size(UIScreen.MainScreen.Bounds.Size.Width, UIScreen.MainScreen.Bounds.Size.Height);

        ////public string AppVersion { get; } = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion").ToString();

        public string AppVersion => _lazyAppVersion.Value;

        public string OsVersion => UIDevice.CurrentDevice.SystemVersion;

        public string DeviceName => UIDevice.CurrentDevice.Name;

        public DeviceSize DeviceSize
        {
            get
            {
                var size = ScreenSize;

                var isPortrait = size.Height > size.Width;

                foreach (var keyValue in _types)
                {
                    var isSizeEqual = false;

                    if (isPortrait)
                    {
                        isSizeEqual = keyValue.Key.Width == size.Width && keyValue.Key.Height == size.Height;
                    }
                    else
                    {
                        isSizeEqual = keyValue.Key.Width == size.Height && keyValue.Key.Height == size.Width;
                    }

                    if (isSizeEqual)
                    {
                        return keyValue.Value;
                    }
                }

                return DeviceSize.XXLarge;
            }
        }

        public void InvokeOnMainThread(Action action)
        {
            AppDelegate.Self.InvokeOnMainThread(action);
        }

        public void InvokeOnBackgroundThread(Action action)
        {
            ThreadPool.QueueUserWorkItem(data => action());
        }
    }
}