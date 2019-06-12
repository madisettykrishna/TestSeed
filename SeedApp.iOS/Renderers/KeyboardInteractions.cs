using SeedApp.IOS.Renderers;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(KeyboardInteractions))]

namespace SeedApp.IOS.Renderers
{
    public class KeyboardInteractions : IKeyboardInteractions
    {
        public void HideKeyboard()
        {
            try
            {
                UIApplication.SharedApplication.KeyWindow.EndEditing(true);
            }
            catch
            {
            }
        }
    }
}