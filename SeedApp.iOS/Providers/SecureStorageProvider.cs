using System.Text;
using SeedApp.Common.Interfaces;
using XLabs.Platform.Services;

namespace SeedApp.IOS.Providers
{
    public class SecureStorageProvider : ISecureStorageProvider
    {
        private readonly ISecureStorage _secureStorage;

        public SecureStorageProvider(ISecureStorage secureStorage)
        {
            _secureStorage = secureStorage;
        }

        public void Delete(string key)
        {
            _secureStorage.Delete(key);
        }

        public string GetValue(string key)
        {
            try
            {
                if (!_secureStorage.Contains(key))
                    return string.Empty;
                var data = _secureStorage.Retrieve(key);

                return Encoding.UTF8.GetString(data);
            }
            catch
            {
                return string.Empty;
            }
        }

        public void SetValue(string key, string value)
        {
            _secureStorage.Store(key, Encoding.UTF8.GetBytes(value));
        }
    }
}