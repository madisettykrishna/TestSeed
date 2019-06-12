using System;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics.Drawables;
using Android.Views;
using SeedApp.Common.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace SeedApp.Droid.Services
{
    public class AndroidDialogService : IDialogService
    {
        private static readonly object Locker = new object();
        private AlertDialog _progressAlertDialog, _modalAlertDialog;

        public AndroidDialogService()
        {
        }

        Task<string> IDialogService.DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons)
        {
            return App.Current.MainPage.DisplayActionSheet(title, cancel, destruction, buttons);
        }

        public Task DisplayAlert(string title, string message, string cancel)
        {
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

            Dialog dialog;
            AlertDialog.Builder alert = new AlertDialog.Builder(MainActivity.Instance);
            alert.SetTitle(title);
            alert.SetMessage(message);
            alert.SetNegativeButton(cancel, (sender, e) =>
            {
                ((Dialog)sender).Dismiss();
                taskCompletionSource.SetResult(false);
            });

            dialog = alert.Create();
            dialog.SetCanceledOnTouchOutside(false);

            Device.BeginInvokeOnMainThread(() => { dialog.Show(); });

            return taskCompletionSource.Task;
        }

        public Task<bool> DisplayAlert(string title, string message, string cancel, string ok)
        {
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

            Dialog dialog;
            AlertDialog.Builder alert = new AlertDialog.Builder(MainActivity.Instance);
            alert.SetTitle(title);
            alert.SetMessage(message);
            alert.SetNegativeButton(cancel, (sender, e) =>
            {
                ((Dialog)sender).Hide();
                taskCompletionSource.SetResult(true);
            });

            alert.SetPositiveButton(ok, (sender, e) =>
            {
                ((Dialog)sender).Hide();
                taskCompletionSource.SetResult(false);
            });

            dialog = alert.Create();
            dialog.SetCanceledOnTouchOutside(false);

            Device.BeginInvokeOnMainThread(() => 
            {
                try
                {
                    dialog.Show();
                }
                catch
                {
                }
            });

            return taskCompletionSource.Task;
        }

        public void HideProgress()
        {
            lock (Locker)
            {
                if (_progressAlertDialog != null)
                {
                    _progressAlertDialog.Dismiss();
                    _progressAlertDialog.Dispose();
                    _progressAlertDialog = null;
                }
            }
        }

        public void HideModalPopup()
        {
            lock (Locker)
            {
                if (_modalAlertDialog != null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        _modalAlertDialog.Dismiss();
                        _modalAlertDialog.Dispose();
                        _modalAlertDialog = null;
                    });
                }
            }
        }

        /// <summary>
        /// This will Shows the modal popup.
        /// Make sure that passed View and it's all child control must be set either width or height,
        /// Otherwise that control not display in popup.
        /// </summary>
        /// <param name="view">View, this will be shown in popup. make sure view and it's all child control has either width or height property set.</param>
        public void ShowModalPopup(Xamarin.Forms.View view)
        {
            try
            {
                HideModalPopup();

                lock (Locker)
                {
                    var bound = MainActivity.ScreenSize;

                    StackLayout containerLayout = new StackLayout()
                    {
                        Padding = 0,
                        BackgroundColor = Color.Transparent,
                        IsClippedToBounds = true,
                        HeightRequest = bound.Height,
                        WidthRequest = bound.Width,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Children = { view }
                    };

                    var nativeView = ConvertFormsToNative(containerLayout, new Rectangle(0, 0, bound.Width, bound.Height));

                    _modalAlertDialog = new AlertDialog.Builder(MainActivity.Instance).Create();
                    _modalAlertDialog.SetView(nativeView);

                    _modalAlertDialog.SetCanceledOnTouchOutside(false);

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        try
                        {
                            _modalAlertDialog.Show();
                            ColorDrawable transparentColor = new ColorDrawable(Android.Graphics.Color.Transparent);
                            _modalAlertDialog.Window.SetBackgroundDrawable(transparentColor);
                        }
                        catch
                        {
                        }
                    });
                }
            }
            catch
            {
            }
        }

        public void ShowProgress(string message)
        {
            try
            {
                if (string.IsNullOrEmpty(message))
                {
                    message = "Loading";
                }

                HideProgress();

                lock (Locker)
                {
                    var bound = MainActivity.ScreenSize;
                    var size = Math.Max(bound.Width / 5, 120);
                    _progressAlertDialog = new AlertDialog.Builder(MainActivity.Instance).Create();

                    var progressView = CreateProgressView(message, size, bound.Width, bound.Height);
                    var nativeProgressView = ConvertFormsToNative(progressView, new Rectangle(0, 0, bound.Width, bound.Height));

                    _progressAlertDialog.SetView(nativeProgressView);
                    _progressAlertDialog.SetCanceledOnTouchOutside(false);
                    _progressAlertDialog.SetInverseBackgroundForced(true);
                    _progressAlertDialog.Show();
                    ColorDrawable transparentColor = new ColorDrawable(Android.Graphics.Color.Transparent);
                    _progressAlertDialog.Window.SetBackgroundDrawable(transparentColor);
                }
            }
            catch
            {
            }
        }

        private Android.Views.View ConvertFormsToNative(Xamarin.Forms.View view, Rectangle size)
        {
            view.Layout(size);
            view.BackgroundColor = Color.Transparent;
            var renderer = Platform.CreateRendererWithContext(view, MainActivity.Instance);
            var viewGroup = renderer.View;
            renderer.Tracker.UpdateLayout();

            return viewGroup;
        }

        private Xamarin.Forms.View CreateProgressView(string message, double size, double containerWidth, double containerHeight)
        {
            var activity = new ActivityIndicator
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                IsVisible = true,
                IsRunning = true,
                Color = Color.White,
                HeightRequest = 30,
                WidthRequest = 30
            };

            var lblMessage = new Label
            {
                VerticalTextAlignment = Xamarin.Forms.TextAlignment.Center,
                HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.None,
                HeightRequest = 50,
                WidthRequest = size,
                FontSize = Device.Idiom == TargetIdiom.Tablet ? 18 : 15,
                TextColor = Color.White,
                LineBreakMode = LineBreakMode.WordWrap,
                Text = message
            };

            var topSpacer = new BoxView
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                HeightRequest = 10
            };
            var bottomSpacer = new BoxView
            {
                VerticalOptions = LayoutOptions.EndAndExpand,
                HeightRequest = 10
            };

            var stackLayout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Vertical,
                Spacing = 4,
                Padding = 10,
                HeightRequest = size,
                WidthRequest = size,
                Children =
                {
                    topSpacer,
                    activity,
                    lblMessage,
                    bottomSpacer
                }
            };

            var frame = new Frame
            {
                BorderColor = Color.White.MultiplyAlpha(0.2),
                Padding = 5,
                HasShadow = false,
                BackgroundColor = Color.Black.MultiplyAlpha(0.8),
                IsClippedToBounds = true,
                HeightRequest = size,
                InputTransparent = true,
                WidthRequest = size,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Content = stackLayout,
            };

            Frame containerFrame = new Frame
            {
                BorderColor = Color.Transparent,
                Padding = 0,
                HasShadow = false,
                BackgroundColor = Color.Transparent,
                IsClippedToBounds = true,
                HeightRequest = containerHeight,
                InputTransparent = true,
                WidthRequest = containerWidth,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Content = frame
            };

            return containerFrame;
        }
    }
}