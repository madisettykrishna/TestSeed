namespace SeedApp.Common.Interfaces
{
    public interface ISecureStorageProvider
    {
        string GetValue(string key);

        void SetValue(string key, string value);

        void Delete(string key);
    }
}