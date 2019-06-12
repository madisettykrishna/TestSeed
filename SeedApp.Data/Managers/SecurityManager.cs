using SeedApp.Common.Interfaces;

namespace SeedApp.Data.Managers
{
    public class SecurityManager : ISecurityManager
    {
        public const string CurrentAccessTokenKey = "CurrentAccessToken";
        private readonly ISecureStorageProvider _secureStorageProvider;

        public SecurityManager(ISecureStorageProvider secureStorageProvider)
        {
            _secureStorageProvider = secureStorageProvider;
        }

        public string CurrentAccessToken => _secureStorageProvider.GetValue(CurrentAccessTokenKey);

        public bool IsLoggedIn => !string.IsNullOrEmpty(CurrentAccessToken);

        public void SaveAccessToken(string accessToken)
        {
            _secureStorageProvider.SetValue(CurrentAccessTokenKey, accessToken);
        }

        public void Logout()
        {
            _secureStorageProvider.Delete(CurrentAccessTokenKey);
        }
    }
}