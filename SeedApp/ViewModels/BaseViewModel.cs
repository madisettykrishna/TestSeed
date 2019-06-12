using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;

namespace SeedApp.ViewModels
{
    public class BaseViewModel : ViewModel
    {
        private Color _backgroundColor;
        private string _title;

        public BaseViewModel()
        {
        }

        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public virtual async Task OnViewAppear()
        {
        }

        public virtual async Task OnViewDisappear()
        {
        }
    }
}