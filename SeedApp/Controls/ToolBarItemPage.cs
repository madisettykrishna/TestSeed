using System;
using Xamarin.Forms;

namespace SeedApp.Controls
{
    public class ToolBarItemPage : ContentPage
    {
        public event EventHandler OnToolBarItemAdded;

        public void AddToolBarItem()
        {
            if (OnToolBarItemAdded != null)
            {
                OnToolBarItemAdded(this, new EventArgs());
            }
        }
    }
}