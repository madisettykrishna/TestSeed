using System;
using SeedApp.Controls;
using SeedApp.IOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ToolBarItemPage), typeof(ToolBarItemPageRenderer))]

namespace SeedApp.IOS.Renderers
{
    public class ToolBarItemPageRenderer : PageRenderer
    {
        public ToolBarItemPageRenderer()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            MoveCancelButtonToLeft();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        protected void MoveCancelButtonToLeft()
        {
            try
            {
                if (NavigationController.TopViewController.NavigationItem.RightBarButtonItems.Length == 2)
                {
                    NavigationController.TopViewController.NavigationItem.RightBarButtonItems[1].ImageInsets = new UIEdgeInsets(0, 20, 0, -20);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var ToolBarItem = (ToolBarItemPage)e.NewElement;
                ToolBarItem.OnToolBarItemAdded += (sender, e1) =>
                {
                    MoveCancelButtonToLeft();
                };
            }
        }
    }
}