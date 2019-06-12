using Mindscape.Raygun4Net;
using UIKit;

namespace SeedApp.IOS
{
    public class Application
    {
        // This is the main entry point of the application.
        private static void Main(string[] args)
        {
            ////RaygunClient.Attach("add raygunclientid here");

            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}