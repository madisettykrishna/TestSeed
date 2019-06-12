using CoreGraphics;
using UIKit;

namespace SeedApp.IOS.Views
{
    public class TouchTransparentView : UIView
    {
        public TouchTransparentView(CGRect frame) : base(frame)
        {
        }

        public override bool PointInside(CGPoint point, UIEvent uievent)
        {
            base.PointInside(point, uievent);
            return false;
        }
    }
}