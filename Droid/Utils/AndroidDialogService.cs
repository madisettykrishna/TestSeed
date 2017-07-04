using System;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics.Drawables;
using Android.Views;
using SeedApp.Common.Interfaces;
using SeedApp.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace SeedApp.Forms.Droid.Utils
{
	public class AndroidDialogService : IDialogService
	{
		private static readonly object Locker = new object();
		private AlertDialog _progressAlertDialog, _modalAlertDialog;

		public Task DisplayAlertAsync(string title, string message, string cancel)
		{
			TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

			Dialog dialog;
			AlertDialog.Builder alert = new AlertDialog.Builder(Xamarin.Forms.Forms.Context);
			alert.SetTitle(title);
			alert.SetMessage(message);
			alert.SetNegativeButton(cancel, (sender, e) =>
			{
				((Dialog)sender).Dismiss();
				taskCompletionSource.SetResult(false);
			});
			dialog = alert.Create();

			Device.BeginInvokeOnMainThread(() => { dialog.Show(); });

			return taskCompletionSource.Task;
		}

		public Task<bool> DisplayAlertAsync(string title, string message, string ok, string cancel)
		{
			TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

			Dialog dialog;
			AlertDialog.Builder alert = new AlertDialog.Builder(Xamarin.Forms.Forms.Context);
			alert.SetTitle(title);
			alert.SetMessage(message);
			alert.SetNegativeButton(cancel, (sender, e) =>
			{
				((Dialog)sender).Hide();
				taskCompletionSource.SetResult(false);
			});

			alert.SetPositiveButton(ok, (sender, e) =>
			{
				((Dialog)sender).Hide();
				taskCompletionSource.SetResult(true);
			});
			dialog = alert.Create();

			Device.BeginInvokeOnMainThread(() => { dialog.Show(); });

			return taskCompletionSource.Task;
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

		public void HideProgress()
		{
			lock (Locker)
			{
				if (_progressAlertDialog != null)
				{
					Device.BeginInvokeOnMainThread(() =>
					{
						_progressAlertDialog.Dismiss();
						_progressAlertDialog.Dispose();
						_progressAlertDialog = null;
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
					InputTransparent = true,
					WidthRequest = bound.Width,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					Children = { view }
				};

				var nativeView = ConvertFormsToNative(containerLayout, new Rectangle(0, 0, bound.Width, bound.Height));

				_modalAlertDialog = new AlertDialog.Builder(Xamarin.Forms.Forms.Context).Create();
				_modalAlertDialog.SetView(nativeView);

				_modalAlertDialog.SetCanceledOnTouchOutside(false);

				Device.BeginInvokeOnMainThread(() =>
				{
					_modalAlertDialog.Show();
					ColorDrawable transparentColor = new ColorDrawable(Android.Graphics.Color.Transparent);
					_modalAlertDialog.Window.SetBackgroundDrawable(transparentColor);
				});
			}
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
				var bound = MainActivity.ScreenSize;
				var size = Math.Max(bound.Width / 5, 120);
				_progressAlertDialog = new AlertDialog.Builder(Xamarin.Forms.Forms.Context).Create();

				var progressView = CreateProgressView(message, size, bound.Width, bound.Height);
				var nativeProgressView = ConvertFormsToNative(progressView, new Rectangle(0, 0, bound.Width, bound.Height));

				_progressAlertDialog.SetView(nativeProgressView);
				_progressAlertDialog.SetCanceledOnTouchOutside(false);
				_progressAlertDialog.SetInverseBackgroundForced(true);

				Device.BeginInvokeOnMainThread(() =>
				{
					_progressAlertDialog.Show();
					ColorDrawable transparentColor = new ColorDrawable(Android.Graphics.Color.Transparent);
					_progressAlertDialog.Window.SetBackgroundDrawable(transparentColor);
				});
			}
		}

		private Android.Views.View ConvertFormsToNative(Xamarin.Forms.View view, Rectangle size)
		{
			var renderer = Platform.CreateRenderer(view);
			var viewGroup = renderer.ViewGroup;
			renderer.Tracker.UpdateLayout();

			var layoutParams = new ViewGroup.LayoutParams((int)size.Width, (int)size.Height);
			viewGroup.LayoutParameters = layoutParams;
			view.Layout(size);
			view.BackgroundColor = Color.Transparent;
			viewGroup.Layout((int)size.X, (int)size.Y, (int)size.Width, (int)size.Height);

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
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Content = frame
			};

			return containerFrame;
		}
	}
}
