using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Mindscape.Raygun4Net;
using SeedApp.Forms.iOS;
using UIKit;

namespace SeedApp.iOS
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main(string[] args)
		{
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			var iosAppConfig = new IosAppConfig();

#if (APPSTORE || PLAYSTORE)
            RaygunClient.Attach(iosAppConfig.StoreAppRaygunKey);
#elif (BETA)
            RaygunClient.Attach(iosAppConfig.BetaAppRaygunKey);
#else
			RaygunClient.Attach(iosAppConfig.TestAppRaygunKey);
#endif
			UIApplication.Main(args, null, "AppDelegate");
		}
	}
}
