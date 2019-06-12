using Android.Webkit;
using SeedApp.Controls;
using SeedApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CookieWebView), typeof(CookieWebViewRenderer))]

namespace SeedApp.Droid.Renderers
{
    public class CookieWebViewRenderer : WebViewRenderer
    {
        public CookieWebViewRenderer(MainActivity context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                CookieManager.Instance.SetAcceptCookie(true);
                var cookies = CookieManager.Instance.GetCookie(CookieWebView.RegistrationUrl);
                CookieManager.Instance.SetCookie(CookieWebView.Domain, "MMP.Auth.AccessToken=" + CookieWebView.AccessToken + "; Domain=" + CookieWebView.Domain);
                cookies = CookieManager.Instance.GetCookie(CookieWebView.RegistrationUrl);
                Control.LoadUrl(CookieWebView.RegistrationUrl);
            }
        }

        public CookieWebView CookieWebView => Element as CookieWebView;
    }

    internal class CookieWebViewClient : WebViewClient
    {
        private readonly CookieWebView _cookieWebView;
        internal CookieWebViewClient(CookieWebView cookieWebView)
        {
            _cookieWebView = cookieWebView;
        }

        public override WebResourceResponse ShouldInterceptRequest(Android.Webkit.WebView view, IWebResourceRequest request)
        {
            request.RequestHeaders.Add("cookie", "MMP.Auth.AccessToken=" + _cookieWebView.AccessToken);
            return base.ShouldInterceptRequest(view, request);
        }

        public override void OnPageFinished(global::Android.Webkit.WebView view, string url)
        {
        }
    }
}
