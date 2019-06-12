using System;
using Newtonsoft.Json;

namespace SeedApp.Data.Dtos
{
    public class TokenResponseDto
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }

        [JsonProperty(".issued")]
        public DateTime? IssuedOnDateTimeUtc { get; set; }

        [JsonProperty(PropertyName = ".expires")]
        public string ExpiresOnDateTimeUtc { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("as:client_id")]
        public string ClientId { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
}
