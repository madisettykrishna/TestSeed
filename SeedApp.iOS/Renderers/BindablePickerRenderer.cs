using System;
using System.Collections.Specialized;
using System.ComponentModel;
using CoreGraphics;
using SeedApp.Controls;
using SeedApp.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BindablePicker), typeof(BindablePickerRenderer))]

namespace SeedApp.iOS.Renderers
{
    public class BindablePickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            InitializeNativeControl(e.NewElement as BindablePicker);
        }

        private void InitializeNativeControl(BindablePicker entry)
        {
            if (entry != null && !string.IsNullOrEmpty(entry.LeftImageSource))
            {
                Control.BorderStyle = UITextBorderStyle.None;

                UIView controlLeftView = new UIView();
                controlLeftView.Frame = new CGRect(0, 0, 50, 45f);
                UIView leftView = new UIView(new CGRect(0, 0, controlLeftView.Frame.Height, controlLeftView.Frame.Height));
                leftView.BackgroundColor = UIColor.FromRGB(238, 238, 238);

                UIImageView imageView = new UIImageView();
                imageView.Frame = new CGRect(10, 10, 25, 25);
                imageView.Image = UIImage.FromFile(entry.LeftImageSource);
                imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
                leftView.Add(imageView);

                UIView leftLineView = new UIView(new CGRect(leftView.Frame.Width - 1, 0, 1, leftView.Frame.Height));
                leftLineView.BackgroundColor = UIColor.FromRGB(230, 230, 230);
                leftView.Add(leftLineView);

                Control.Add(leftView);

                UIView rightView = new UIView(new CGRect(0, UIScreen.MainScreen.Bounds.Width - 8, 8, controlLeftView.Frame.Height));

                Control.LeftView = controlLeftView;
                Control.LeftViewMode = UITextFieldViewMode.Always;
                Control.RightView = rightView;
                Control.RightViewMode = UITextFieldViewMode.UnlessEditing;
                Control.TextColor = UIColor.Black;
                Control.Layer.BorderWidth = 1f;
                Control.Layer.BorderColor = UIColor.FromRGB(230, 230, 230).CGColor;
            }
            else
            {
                Control.Layer.BorderColor = UIKit.UIColor.Black.CGColor;
            }
        }
    }

    public class PickerRenderer : ViewRenderer<Picker, UITextField>
    {
        private UIPickerView _picker;
        private UIColor _defaultTextColor;
        private bool _disposed;

        private IElementController ElementController => Element as IElementController;

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            if (e.OldElement != null)
                ((INotifyCollectionChanged)e.OldElement.Items).CollectionChanged -= RowsCollectionChanged;

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var entry = new XLabs.Forms.Controls.NoCaretField { BorderStyle = UITextBorderStyle.RoundedRect };

                    entry.EditingDidBegin += OnStarted;
                    entry.EditingDidEnd += OnEnded;

                    _picker = new UIPickerView();

                    var width = UIScreen.MainScreen.Bounds.Width;
                    var _internalView = new UIView();
                    _internalView.Frame = new CGRect(0, 0, width, 44);
                    _internalView.BackgroundColor = UIColor.Clear;

                    var _headerLabel = new UILabel(new CGRect(50, 0, width - 100, _internalView.Frame.Height));
                    _headerLabel.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
                    _headerLabel.BackgroundColor = UIColor.Clear;
                    _headerLabel.TextColor = UIColor.Black;
                    _headerLabel.Text = (e.NewElement as BindablePicker).Title;
                    _headerLabel.TextAlignment = UITextAlignment.Center;
                    _internalView.AddSubview(_headerLabel);

                    var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
                    var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) =>
                    {
                        var s = (PickerSource)_picker.Model;
                        if (s.SelectedIndex == -1 && Element.Items != null && Element.Items.Count > 0)
                            UpdatePickerSelectedIndex(0);
                        UpdatePickerFromModel(s);
                        entry.ResignFirstResponder();
                    });

                    var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (o, a) =>
                    {
                        var s = (PickerSource)_picker.Model;
                        if (s.SelectedIndex == -1 && Element.Items != null && Element.Items.Count > 0)
                            UpdatePickerSelectedIndex(0);
                        Element.SelectedItem = null;
                        Control.Text = null;
                        entry.ResignFirstResponder();
                    });

                    var toolbar = new UIToolbar(new CGRect(0, 0, width, 44)) { BarStyle = UIBarStyle.Default, Translucent = true };
                    toolbar.Add(_internalView);

                    toolbar.SetItems(new[] { cancelButton, spacer, doneButton }, false);

                    entry.InputView = _picker;
                    entry.InputAccessoryView = toolbar;

                    _defaultTextColor = entry.TextColor;

                    SetNativeControl(entry);
                }

                _picker.Model = new PickerSource(this);

                UpdatePicker();
                UpdateTextColor();

                ((INotifyCollectionChanged)e.NewElement.Items).CollectionChanged += RowsCollectionChanged;
            }

            base.OnElementChanged(e);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == Picker.TitleProperty.PropertyName)
                UpdatePicker();
            if (e.PropertyName == Picker.SelectedIndexProperty.PropertyName)
                UpdatePicker();
            if (e.PropertyName == Picker.TextColorProperty.PropertyName || e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
                UpdateTextColor();
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;

            if (disposing)
            {
                _defaultTextColor = null;

                if (_picker != null)
                {
                    if (_picker.Model != null)
                    {
                        _picker.Model.Dispose();
                        _picker.Model = null;
                    }

                    _picker.RemoveFromSuperview();
                    _picker.Dispose();
                    _picker = null;
                }

                if (Control != null)
                {
                    Control.EditingDidBegin -= OnStarted;
                    Control.EditingDidEnd -= OnEnded;
                }

                if (Element != null)
                    ((INotifyCollectionChanged)Element.Items).CollectionChanged -= RowsCollectionChanged;
            }

            base.Dispose(disposing);
        }

        private void OnEnded(object sender, EventArgs eventArgs)
        {
            ElementController.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
        }

        private void OnStarted(object sender, EventArgs eventArgs)
        {
            ElementController.SetValueFromRenderer(VisualElement.IsFocusedProperty, true);
        }

        private void RowsCollectionChanged(object sender, EventArgs e)
        {
            UpdatePicker();
        }

        private void UpdatePicker()
        {
            var selectedIndex = Element.SelectedIndex;
            var items = Element.Items;
            Control.Placeholder = Element.Title;
            var oldText = Control.Text;
            Control.Text = selectedIndex == -1 || items == null ? string.Empty : items[selectedIndex];
            UpdatePickerNativeSize(oldText);
            _picker.ReloadAllComponents();
            if (items == null || items.Count == 0)
                return;

            UpdatePickerSelectedIndex(selectedIndex);
        }

        private void UpdatePickerFromModel(PickerSource s)
        {
            if (Element != null)
            {
                var oldText = Control.Text;
                Control.Text = s.SelectedItem;
                UpdatePickerNativeSize(oldText);
                ElementController.SetValueFromRenderer(Picker.SelectedIndexProperty, s.SelectedIndex);
            }
        }

        private void UpdatePickerNativeSize(string oldText)
        {
            if (oldText != Control.Text)
                ((IVisualElementController)Element).NativeSizeChanged();
        }

        private void UpdatePickerSelectedIndex(int formsIndex)
        {
            var source = (PickerSource)_picker.Model;
            source.SelectedIndex = formsIndex;
            source.SelectedItem = formsIndex >= 0 ? Element.Items[formsIndex] : null;
            _picker.Select(Math.Max(formsIndex, 0), 0, true);
        }

        private void UpdateTextColor()
        {
            if (Element != null)
            {
                var textColor = Element.TextColor;

                if (!Element.IsEnabled)
                    Control.TextColor = _defaultTextColor;
                else
                    Control.TextColor = textColor.ToUIColor();
            }
        }

        private class PickerSource : UIPickerViewModel
        {
            private PickerRenderer _renderer;
            private bool _disposed;

            public PickerSource(PickerRenderer renderer)
            {
                _renderer = renderer;
            }

            public int SelectedIndex { get; internal set; }

            public string SelectedItem { get; internal set; }

            public override nint GetComponentCount(UIPickerView picker)
            {
                return 1;
            }

            public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
            {
                return _renderer.Element.Items != null ? _renderer.Element.Items.Count : 0;
            }

            public override string GetTitle(UIPickerView picker, nint row, nint component)
            {
                return _renderer.Element.Items[(int)row];
            }

            public override void Selected(UIPickerView picker, nint row, nint component)
            {
                if (_renderer.Element.Items.Count == 0)
                {
                    SelectedItem = null;
                    SelectedIndex = -1;
                }
                else
                {
                    SelectedItem = _renderer.Element.Items[(int)row];
                    SelectedIndex = (int)row;
                }

                _renderer.UpdatePickerFromModel(this);
            }

            protected override void Dispose(bool disposing)
            {
                if (_disposed)
                    return;

                _disposed = true;

                if (disposing)
                    _renderer = null;

                base.Dispose(disposing);
            }
        }
    }
}
