using System;
using System.Threading.Tasks;

namespace SeedApp.Common.Interfaces
{
    public interface INavigationService
    {
        PageKey CurrentPageKey { get; }

        bool HasBackPage { get; }

        Task LoginStatusChanged();

        Task NavigateTo(PageKey pageKey, bool animated = false);

        Task PopToRoot();

        Task GoBack(int numberOfPagesToSkip = 0);
    }
}