using Foundation;
using SeedApp.Controls;
using SeedApp.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CookieWebView), typeof(CookieWebViewRenderer))]

namespace SeedApp.iOS.Renderers
{
    public class CookieWebViewRenderer : ViewRenderer
    {
        public CookieWebView CookieWebView => Element as CookieWebView;
        private UIWebView _webView;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || Element == null)
                return;

            var cookieJar = NSHttpCookieStorage.SharedStorage;
            cookieJar.AcceptPolicy = NSHttpCookieAcceptPolicy.Always;
            NSDictionary properties = NSDictionary.FromObjectsAndKeys(
                new[] { CookieWebView.Domain, "/", "MMP.Auth.AccessToken", CookieWebView.AccessToken },
                new[] { NSHttpCookie.KeyDomain, NSHttpCookie.KeyPath, NSHttpCookie.KeyName, NSHttpCookie.KeyValue
            });

            cookieJar.SetCookie(NSHttpCookie.CookieFromProperties(properties));
            _webView = new UIWebView();
            SetNativeControl(_webView);
            _webView.ShouldStartLoad += _webView_ShouldStartLoad;
            _webView.LoadRequest(NSUrlRequest.FromUrl(NSUrl.FromString(CookieWebView.RegistrationUrl)));
        }

        private bool _webView_ShouldStartLoad(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
        {
            if (request.Url.AbsoluteString.Contains("Confirm"))
            {
                CookieWebView.Confirm();
                return true;
            }

            return true;
        }
    }
}
