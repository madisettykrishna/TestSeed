using System;
using System.Net;
using Xamarin.Forms;

namespace SeedApp.Controls
{
    public class CookieWebView : WebView
    {
        public static readonly BindableProperty ConfirmProperty = BindableProperty.Create(propertyName: "Confirm", returnType: typeof(Action), declaringType: typeof(CookieWebView), defaultValue: default(Action));

        public static readonly BindableProperty CookiesProperty = BindableProperty.Create(propertyName: "Cookies", returnType: typeof(CookieContainer), declaringType: typeof(CookieWebView), defaultValue: default(string));

        public CookieWebView()
        {
            Cookies = new CookieContainer();
        }

        public CookieContainer Cookies
        {
            get { return (CookieContainer)GetValue(CookiesProperty); }
            set { SetValue(CookiesProperty, value); }
        }

        public string AccessToken { get; set; }

        public string RegistrationUrl { get; set; }

        public string Domain { get; set; }

        public Action Confirm
        {
            get { return (Action)GetValue(ConfirmProperty); }
            set { SetValue(ConfirmProperty, value); }
        }

        public void ConfirmPage()
        {
        }
    }
}
