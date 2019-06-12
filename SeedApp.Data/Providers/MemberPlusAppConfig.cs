using SeedApp.Common.Interfaces;

namespace SeedApp.Data.Providers
{
    public class MemberPlusAppConfig : IMemberPlusAppConfig
    {
        public string ServerDataUrl
        {
            get
            {
                return "http://invoicesync.in";
            }
        }

        public string ServerLoginUrl
        {
            get
            {
                return "http://invoicesync.in/token";
            }
        }
    }
}