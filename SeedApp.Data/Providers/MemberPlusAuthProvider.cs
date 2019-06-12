using System.Collections.Generic;
using System.Threading.Tasks;
using SeedApp.Common.Interfaces;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using SeedApp.Common.Exception;
using SeedApp.Data.Dtos;

namespace SeedApp.Data.Providers
{
    public class MemberPlusAuthProvider : IMemberPlusAuthProvider
    {
        private readonly IMemberPlusAppConfig _memberPlusAppConfig;

        public MemberPlusAuthProvider(IMemberPlusAppConfig memberPlusAppConfig)
        {
            _memberPlusAppConfig = memberPlusAppConfig;
        }

        public async Task<string> LoginAsyc(string username, string password)
        {
            var result = await InternalLoginAsync(new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "username", username },
                { "password", password }
            });

            if (!result.IsSuccess)
            {
                var exception = new MemberPlusApiException(MmpApiErrorCodes.Unauthorized, "Incorrect email address or pin");
                exception.Data.Add("Response", result.Content);
                exception.Data.Add("StatusCode", result.StatusCode);
                throw exception;
            }

            return result.Data.AccessToken;
        }

        internal async Task<IRestResponse<TokenResponseDto>> InternalLoginAsync(Dictionary<string, string> content)
        {
            using (var client = new RestClient(_memberPlusAppConfig.ServerDataUrl)
            {
                IgnoreResponseStatusCode = true
            })
            {
                var request = new RestRequest("Token", Method.POST);

                foreach (var keyValuePair in content)
                {
                    request.AddParameter(keyValuePair.Key, keyValuePair.Value);
                }

                return await client.Execute<TokenResponseDto>(request);
            }
        }
    }
}