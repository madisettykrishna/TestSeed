using System.Threading.Tasks;
using Xamarin.Forms;

namespace SeedApp.Common.Interfaces
{
    public interface IDialogService
    {
        Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons);

        Task DisplayAlert(string title, string message, string cancel);

        Task<bool> DisplayAlert(string title, string message, string ok, string cancel);

        void ShowProgress(string message);

        void HideProgress();

        void ShowModalPopup(View view);

        void HideModalPopup();
    }
}