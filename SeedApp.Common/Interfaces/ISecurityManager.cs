namespace SeedApp.Common.Interfaces
{
    public interface ISecurityManager
    {
        string CurrentAccessToken { get; }

        bool IsLoggedIn { get; }

        void SaveAccessToken(string accessToken);

        void Logout();
    }
}
