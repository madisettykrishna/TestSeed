using System;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Foundation;
using HockeyApp;
using ImageCircle.Forms.Plugin.iOS;
using LabelHtml.Forms.Plugin.iOS;
using SeedApp.Common;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Logging;
using SeedApp.Data;
using SeedApp.Data.Helpers;
using Newtonsoft.Json;
using Plugin.Toasts;
using UIKit;
using Xamarin.Forms;

namespace SeedApp.IOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IBITHockeyManagerDelegate, IBITCrashManagerDelegate
    {
        public static new AppDelegate Self { get; private set; }

        #region Hockey App SDK

        [Export("viewControllerForHockeyManager:componentManager:")]
        public UIViewController ViewControllerForHockeyManager(
            BITHockeyManager hockeyManager,
            BITHockeyBaseManager componentManager)
        {
            return UIApplication.SharedApplication.Windows[0].RootViewController;
        }

        #endregion Hockey App SDK

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            UIDevice.CurrentDevice.BatteryMonitoringEnabled = true;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            DependencyService.Register<ToastNotification>();
            Rg.Plugins.Popup.Popup.Init();
            ToastNotification.Init();
            HtmlLabelRenderer.Initialize();
            global::Xamarin.Forms.Forms.Init();
            ImageCircleRenderer.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            global::Xamarin.FormsMaps.Init();
            Self = this;

            app.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
            app.SetStatusBarHidden(false, UIStatusBarAnimation.None);
            SetGlobaliOsAppearance();
            WireupDependencies();

            if (!NSUserDefaults.StandardUserDefaults.BoolForKey("FirstTimeLoadingKey"))
            {
                NSUserDefaults.StandardUserDefaults.SetBool(true, "FirstTimeLoadingKey");
                ContainerManager.Container.Resolve<ISecureStorageProvider>().Delete("CurrentAccessToken");
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, new NSSet());
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }

#if !DEBUG
            ContainerManager.Container.Resolve<IAppAnalyticsProvider>().Initialize();
#endif

            ////var x = typeof(Plugin.GridViewControl.iOS.Renderers.GridViewRenderer); Need to figure out why its not working with Release configuraion
            var application = ContainerManager.Container.Resolve<App>();

            LoadApplication(application);

            return base.FinishedLaunching(app, options);
        }

        public override void DidEnterBackground(UIApplication application)
        {
            UIDevice.CurrentDevice.BatteryMonitoringEnabled = false;
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
            application.ApplicationIconBadgeNumber = 0;
        }

        public override void WillEnterForeground(UIApplication application)
        {
            UIDevice.CurrentDevice.BatteryMonitoringEnabled = true;
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            PresentNotification(userInfo);
            completionHandler(UIBackgroundFetchResult.NoData);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            ////Modify device token for compatibility Azure
            var token = deviceToken.Description;
            token = token.Trim('<', '>').Replace(" ", string.Empty);

            Console.WriteLine("deviceToken " + deviceToken);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            var alert = new UIAlertView("MemberPlus", "Notification registration failed! Try again!", null, "OK", null);
            alert.Show();
        }

        private static void SetGlobaliOsAppearance()
        {
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes
            {
                Font = UIFont.FromName("Roboto", 20f)
            });
        }

        private static void WireupDependencies()
        {
            ContainerManager.InitializeDependencyRegistrar();
            DataBootstrapper.Initialize(ContainerManager.DependencyRegistrar);
            Bootstrapper.Initialize(ContainerManager.DependencyRegistrar);
            IosBootstrapper.Initialize(ContainerManager.DependencyRegistrar);

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<CommonAutoMapperProfile>();
                cfg.AddProfile<DataAutoMapperProfile>();
                cfg.AddProfile<AutoMapperProfile>();
                cfg.AddProfile<IosAutoMapperProfile>();
            });

            ContainerManager.DependencyRegistrar.RegisterInstance(Mapper.Instance);
        }

        private async void PresentNotification(NSDictionary dict)
        {
            try
            {
                //// Extract some data from the notifiation and display it using an alert view.
                NSDictionary aps = dict.ObjectForKey(new NSString("aps")) as NSDictionary;
                string msg = string.Empty;

                if (aps.ContainsKey(new NSString("alert")))
                {
                    msg = aps.ValueForKey(new NSString("alert")).ToString();

                    var options = new NotificationOptions()
                    {
                        Description = msg,
                        IsClickable = true //// Set to true if you want the result Clicked to come back (if the user clicks it)
                    };

                    var notification = DependencyService.Get<IToastNotificator>();
                    await notification.Notify(options);
                }
            }
            catch
            {
            }
        }
    }
}