using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace SeedApp.Common
{
    public class MultiSelectListItem<T> : INotifyPropertyChanged
    {
        private bool isMultipleChecked = false;

        private bool isSingleChecked = false;

        public MultiSelectListItem()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public T Item { get; set; }

        public bool IsMultipleChecked
        {
            get
            {
                return isMultipleChecked;
            }

            set
            {
                if (isMultipleChecked != value)
                {
                    isMultipleChecked = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("IsMultipleChecked"));
                    PropertyChanged(this, new PropertyChangedEventArgs("ActiveColor"));
                    PropertyChanged(this, new PropertyChangedEventArgs("CheckedStatus"));
                }
            }
        }

        public bool IsSingleChecked
        {
            get
            {
                return isSingleChecked;
            }

            set
            {
                if (isSingleChecked != value)
                {
                    isSingleChecked = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("IsSingleChecked"));
                    PropertyChanged(this, new PropertyChangedEventArgs("ActiveColorSingle"));
                    PropertyChanged(this, new PropertyChangedEventArgs("CheckedStatusSingle"));
                }
            }
        }

        public Color ActiveColor
        {
            get
            {
                if (IsMultipleChecked)
                {
                    return Color.FromHex("#53297F");
                }
                else
                {
                    return Color.FromHex("#8c8c8c");
                }
            }
        }

        public ImageSource CheckedStatus
        {
            get
            {
                if (IsMultipleChecked)
                {
                    return ImageSource.FromFile("Checked.png");
                }
                else
                {
                    return ImageSource.FromFile("Unchecked.png");
                }
            }
        }

        public Color ActiveColorSingle
        {
            get
            {
                if (IsSingleChecked)
                {
                    return Color.FromHex("#53297F");
                }
                else
                {
                    return Color.FromHex("#8c8c8c");
                }
            }
        }

        public ImageSource CheckedStatusSingle
        {
            get
            {
                if (IsSingleChecked)
                {
                    return ImageSource.FromFile("radiobuttonSelected.png");
                }
                else
                {
                    return ImageSource.FromFile("radiobutton.png");
                }
            }
        }
    }
}
