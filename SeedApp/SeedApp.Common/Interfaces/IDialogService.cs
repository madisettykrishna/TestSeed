using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SeedApp.Common.Interfaces
{
	public interface IDialogService
	{
		Task DisplayAlertAsync(string title, string message, string cancel);

		Task<bool> DisplayAlertAsync(string title, string message, string ok, string cancel);

		void ShowProgress(string message);

		void HideProgress();

		/// <summary>
		/// This will Shows the modal popup.
		/// Make sure that passed View and it's all child control must be set either width or height,
		/// Otherwise that control not display in popup.
		/// </summary>
		/// <param name="view">View, this will be shown in popup. make sure view and it's all child control has either width or height property set.</param>
		void ShowModalPopup(View view);

		void HideModalPopup();
	}
}
