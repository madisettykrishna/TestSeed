using System;
using System.Threading.Tasks;
using CoreGraphics;
using SeedApp.Common.Interfaces;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace SeedApp.Forms.iOS.Utils
{
	public class IosDialogService : IDialogService
	{
		private static readonly object Locker = new object();
		private UIView _overlayView, _modalPopupOverlay;

		public void HideProgress()
		{
			lock (Locker)
			{
				if (_overlayView != null)
				{
					_overlayView.RemoveFromSuperview();
					_overlayView.Dispose();
					_overlayView = null;
				}
			}
		}

		public Task DisplayAlertAsync(string title, string message, string cancel)
		{
			TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

			UIAlertController alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

			// set up button event handlers
			alert.AddAction(UIAlertAction.Create(cancel, UIAlertActionStyle.Default, a => taskCompletionSource.SetResult(false)));

			// show it
			Device.BeginInvokeOnMainThread(() =>
			{
				UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
			});

			return taskCompletionSource.Task;
		}

		public Task<bool> DisplayAlertAsync(string title, string message, string ok, string cancel)
		{
			TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

			UIAlertController alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

			// set up button event handlers
			alert.AddAction(UIAlertAction.Create(ok, UIAlertActionStyle.Default, a => taskCompletionSource.SetResult(true)));
			alert.AddAction(UIAlertAction.Create(cancel, UIAlertActionStyle.Default, a => taskCompletionSource.SetResult(false)));

			// show it
			Device.BeginInvokeOnMainThread(() =>
			{
				UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
			});

			return taskCompletionSource.Task;
		}

		public void ShowProgress(string message)
		{
			if (string.IsNullOrEmpty(message))
			{
				message = "Loading";
			}

			HideProgress();

			lock (Locker)
			{
				var bounds = UIScreen.MainScreen.Bounds;
				_overlayView = new UIView(bounds);
				_overlayView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
				_overlayView.BackgroundColor = UIColor.Clear;
				_overlayView.UserInteractionEnabled = true;

				var size = Math.Max(bounds.Width / 5, 120);
				var activityIndicator = CreateProgressView(message, size, bounds.Width, bounds.Height);
				var nativeView = ConvertFormsToNative(activityIndicator,
															 new CGRect(0,
																		0,
																		bounds.Width,
																		bounds.Height));
				_overlayView.AddSubview(nativeView);

				var windows = UIApplication.SharedApplication.Windows;
				Array.Reverse(windows);
				foreach (UIWindow window in windows)
				{
					if (window.WindowLevel == UIWindowLevel.Normal && !window.Hidden)
					{
						window.AddSubview(_overlayView);
						window.BringSubviewToFront(_overlayView);

						break;
					}
				}
			}
		}

		public void HideModalPopup()
		{
			lock (Locker)
			{
				if (_modalPopupOverlay != null)
				{
					_modalPopupOverlay.RemoveFromSuperview();
					_modalPopupOverlay.Dispose();
					_modalPopupOverlay = null;
				}
			}
		}

		/// <summary>
		/// This will Shows the modal popup.
		/// Make sure that passed View and it's all child control must be set either width or height,
		/// Otherwise that control not display in popup.
		/// </summary>
		/// <param name="view">View, this will be shown in popup. make sure view and it's all child control has either width or height property set.</param>
		public void ShowModalPopup(View view)
		{
			HideModalPopup();

			lock (Locker)
			{
				var bounds = UIScreen.MainScreen.Bounds;
				_modalPopupOverlay = new UIView(bounds);
				_modalPopupOverlay.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
				_modalPopupOverlay.BackgroundColor = UIColor.Clear;
				_modalPopupOverlay.UserInteractionEnabled = true;

				StackLayout containerFrame = new StackLayout
				{
					Padding = 0,
					BackgroundColor = Color.Transparent,
					IsClippedToBounds = true,
					HeightRequest = bounds.Height,
					InputTransparent = true,
					WidthRequest = bounds.Width,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					Children = { view }
				};

				var nativeView = ConvertFormsToNative(containerFrame,
															 new CGRect(0,
																		0,
																		bounds.Width,
																		bounds.Height));
				nativeView.UserInteractionEnabled = true;
				_modalPopupOverlay.AddSubview(nativeView);

				_modalPopupOverlay.Frame = new CGRect(0, bounds.Height, bounds.Width, bounds.Height);

				var windows = UIApplication.SharedApplication.Windows;
				Array.Reverse(windows);
				foreach (UIWindow window in windows)
				{
					if (window.WindowLevel == UIWindowLevel.Normal && !window.Hidden)
					{
						Device.BeginInvokeOnMainThread(() =>
						{
							window.MultipleTouchEnabled = true;
							window.AddSubview(_modalPopupOverlay);
							window.BringSubviewToFront(_modalPopupOverlay);

							UIView.Transition(_modalPopupOverlay, 0.22, UIViewAnimationOptions.CurveEaseIn, () =>
							{
								_modalPopupOverlay.Frame = new CGRect(0, 0, bounds.Width, bounds.Height);
							}, null);
						});
						break;
					}
				}
			}
		}

		private UIView ConvertFormsToNative(View view, CGRect size)
		{
			var renderer = Platform.CreateRenderer(view);

			renderer.NativeView.Frame = size;

			renderer.NativeView.AutoresizingMask = UIViewAutoresizing.All;
			renderer.NativeView.ContentMode = UIViewContentMode.ScaleToFill;

			renderer.Element.Layout(size.ToRectangle());

			var nativeView = renderer.NativeView;

			nativeView.SetNeedsLayout();

			return nativeView;
		}

		private View CreateProgressView(string message, double size, nfloat containerWidth, nfloat containerHeight)
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
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
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
				OutlineColor = Color.White.MultiplyAlpha(0.2),
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
				OutlineColor = Color.Transparent,
				Padding = 0,
				HasShadow = false,
				BackgroundColor = Color.Transparent,
				IsClippedToBounds = true,
				HeightRequest = containerHeight,
				InputTransparent = true,
				WidthRequest = containerWidth,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Content = frame
			};

			return containerFrame;
		}
	}
}
