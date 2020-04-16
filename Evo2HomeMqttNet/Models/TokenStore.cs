using System;
using System.Text.Json.Serialization;

namespace Evo2HomeMqttNet.Models
{
    public class TokenStore : CacheObject
    {
        public TokenStore(IdentityModel.Client.TokenResponse result)
        {
            this.AccessToken = result.AccessToken;
            this.TokenType = result.TokenType;
            this.RefreshToken = result.RefreshToken;
            this.ExpiresOn = DateTime.Now.AddSeconds(result.ExpiresIn - 60);
        }

        // ReSharper disable once UnusedMember.Global Used when hydrating from store
        public TokenStore()
        {
        }
        
        public DateTime ExpiresOn { get; set; }

        [JsonIgnore]
        public override TimeSpan Expiry => TimeSpan.FromMinutes(60);

        [JsonIgnore]
        public bool Expired => ExpiresOn < DateTime.Now;
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public string RefreshToken { get; set; }
    }
}