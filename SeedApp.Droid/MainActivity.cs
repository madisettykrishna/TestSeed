using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Autofac;
using AutoMapper;
using Firebase.Iid;
using Firebase.Messaging;
using HockeyApp.Android;
using HockeyApp.Android.Metrics;
using ImageCircle.Forms.Plugin.Droid;
using LabelHtml.Forms.Plugin.Droid;
using SeedApp.Common;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Logging;
using SeedApp.Data;
using Mindscape.Raygun4Net;
using Newtonsoft.Json;
using Plugin.CurrentActivity;
using Plugin.Media;
using Plugin.Permissions;
using Xamarin;

namespace SeedApp.Droid
{
    [Activity(Label = "SeedApp", MainLauncher = true, NoHistory = false, Theme = "@style/Theme.Splash", LaunchMode = LaunchMode.SingleInstance,
          ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize,
          ScreenOrientation = ScreenOrientation.Portrait)]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private View layout;

        private readonly string[] permissionsLocation =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation,
            Manifest.Permission.Camera
        };

        public static MainActivity Instance { get; set; }

        public static System.Drawing.Size ScreenSize { get; set; }

        private static Context MainApplicationContext { get; set; }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Instance = this;
            ScreenSize = new System.Drawing.Size(ConvertPixelsToDp(Resources.DisplayMetrics.WidthPixels), ConvertPixelsToDp(Resources.DisplayMetrics.HeightPixels));
            AndroidEnvironment.UnhandledExceptionRaiser += OnException_Occured;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            Rg.Plugins.Popup.Popup.Init(this, bundle);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            await CrossMedia.Current.Initialize();
            HtmlLabelRenderer.Initialize();
            Xamarin.Forms.Forms.Init(this, bundle);
            ImageCircleRenderer.Init();
            FormsMaps.Init(this, bundle);
            CrossCurrentActivity.Current.Init(this, bundle);

            /////RaygunClient.Attach("c9bjTQYyDzHMNXETOE6A");

            ApplicationInfoProvider.MainApplicationContext = ApplicationContext;
            WireupDependencies();
#if !DEBUG
            ContainerManager.Container.Resolve<IAppAnalyticsProvider>().Initialize();
#endif
            var application = ContainerManager.Container.Resolve<App>();

            LoadApplication(application);

            GetLocationPermissionAsync();
        }

        protected virtual void OnException_Occured(object sender, RaiseThrowableEventArgs e)
        {
#if !DEBUG
            ContainerManager.Container.Resolve<IAppAnalyticsProvider>()
                            .ReportException(e.Exception, new[] { LoggerConstants.UnhandeledException });
            ContainerManager.Container.Resolve<ILogger>()
                            .Exception(e.Exception, new[] { LoggerConstants.UnhandeledException });
#endif
            e.Handled = true;
        }

        private static void WireupDependencies()
        {
            ContainerManager.InitializeDependencyRegistrar();
            DataBootstrapper.Initialize(ContainerManager.DependencyRegistrar);
            Bootstrapper.Initialize(ContainerManager.DependencyRegistrar);
            DroidBootstrapper.Initialize(ContainerManager.DependencyRegistrar);

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<CommonAutoMapperProfile>();
                cfg.AddProfile<DataAutoMapperProfile>();
                cfg.AddProfile<AutoMapperProfile>();
                cfg.AddProfile<DroidAutoMapperProfile>();
            });

            ContainerManager.DependencyRegistrar.RegisterInstance(Mapper.Instance);
        }

        private int ConvertPixelsToDp(float pixelValue)
        {
            return (int)(pixelValue / Resources.DisplayMetrics.Density);
        }

        private void GetLocationPermissionAsync()
        {
            const string UserPermission = Manifest.Permission.AccessFineLocation;

            if (CheckSelfPermission(UserPermission) == (int)Permission.Granted)
            {
                return;
            }

            if (ShouldShowRequestPermissionRationale(UserPermission))
            {
                ////Explain to the user why we need to read the contacts
                Snackbar.Make(layout, "Location access is required to track your trip.",
                    Snackbar.LengthIndefinite)
                    .SetAction("OK", v => RequestPermissions(permissionsLocation, 0))
                    .Show();

                return;
            }

            RequestPermissions(permissionsLocation, 0);
        }
    }
}