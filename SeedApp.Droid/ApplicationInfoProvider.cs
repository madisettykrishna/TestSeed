using System;
using Android.Content;

namespace SeedApp.Droid
{
    public class ApplicationInfoProvider : IApplicationInfoProvider
    {
        public static Context MainApplicationContext { get; set; }
    }
}
