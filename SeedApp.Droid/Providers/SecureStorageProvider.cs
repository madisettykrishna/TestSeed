using Android.Content;
using SeedApp.Common.Interfaces;

namespace SeedApp.Droid.Providers
{
    public class SecureStorageProvider : ISecureStorageProvider
    {
        private IApplicationInfoProvider _applicationInfoProvider;

        public SecureStorageProvider(IApplicationInfoProvider applicationProvider)
        {
            _applicationInfoProvider = applicationProvider;
        }

        public void Delete(string key)
        {
            var editor = ApplicationInfoProvider.MainApplicationContext.GetSharedPreferences("MemberPlusPreferences", FileCreationMode.Private).Edit();
            editor.Remove(key);
            editor.Commit();
        }

        public string GetValue(string key)
        {
            var pref = ApplicationInfoProvider.MainApplicationContext.GetSharedPreferences("MemberPlusPreferences", FileCreationMode.Private);
            return pref.GetString(key, string.Empty);
        }

        public void SetValue(string key, string value)
        {
            var editor = ApplicationInfoProvider.MainApplicationContext.GetSharedPreferences("MemberPlusPreferences", FileCreationMode.Private).Edit();
            editor.PutString(key, value);
            editor.Commit();
        }
    }
}