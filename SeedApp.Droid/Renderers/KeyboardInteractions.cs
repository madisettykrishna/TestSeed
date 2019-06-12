using Android.App;
using Android.Content;
using Android.Views.InputMethods;
using SeedApp.Droid.Renderers;
using Xamarin.Forms;

[assembly: Dependency(typeof(KeyboardInteractions))]

namespace SeedApp.Droid.Renderers
{
    public class KeyboardInteractions : IKeyboardInteractions
    {
        public void HideKeyboard()
        {
            try
            {
                var context = MainActivity.Instance;
                var inputMethodManager = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
                if (inputMethodManager != null && context is Activity)
                {
                    var activity = context as Activity;
                    var token = activity.CurrentFocus?.WindowToken;
                    inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);

                    activity.Window.DecorView.ClearFocus();
                }
            }
            catch
            {
            }
        }
    }
}