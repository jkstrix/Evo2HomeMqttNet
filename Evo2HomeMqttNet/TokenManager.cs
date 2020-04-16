using System.Net.Http;
using System.Threading.Tasks;
using Evo2HomeMqttNet.Models;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;

namespace Evo2HomeMqttNet
{
    public class TokenManager
    {
        private const string TokenUrl = "https://tccna.honeywell.com/Auth/OAuth/Token";

        private const string BasicAuthHeader =
            "Basic NGEyMzEwODktZDJiNi00MWJkLWE1ZWItMTZhMGE0MjJiOTk5OjFhMTVjZGI4LTQyZGUtNDA3Yi1hZGQwLTA1OWY5MmM1MzBjYg==";

        private const string Scope = "EMEA-V1-Basic EMEA-V1-Anonymous EMEA-V1-Get-Current-User-Account";
        private const string RefreshTokenScope = "EMEA-V1-Basic EMEA-V1-Anonymous";
        private const string AuthHeaderName = "Authorization";
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<TokenManager> _logger;
        private readonly Store _store;

        private TokenStore _currentToken;

        public TokenManager(ILogger<TokenManager> logger, IHttpClientFactory clientFactory, Store store)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _store = store;

            logger.LogDebug("Getting Existing Token From Store");
            _currentToken = _store.Get<TokenStore>();
        }

        public async Task<TokenStore> GetToken(string userName, string password)
        {
            if (_currentToken == null || string.IsNullOrWhiteSpace(_currentToken.RefreshToken))
            {
                _logger.LogDebug("Token does not exist or is invalid");
                var request = new PasswordTokenRequest
                {
                    Address = TokenUrl,
                    GrantType = OidcConstants.GrantTypes.Password,
                    Scope = Scope,
                    UserName = userName,
                    Password = password
                };

                request.Headers.Add(AuthHeaderName, BasicAuthHeader);

                var client = _clientFactory.CreateClient(Constants.TokenServiceClientName);
                var result = await client.RequestPasswordTokenAsync(request);
                var tokenStore = new TokenStore(result);
                _store.AddOrUpdate(tokenStore);
                _currentToken = tokenStore;
            }
            else if (_currentToken.Expired)
            {
                _logger.LogDebug("Token has expired, requesting new token from refresh token");
                var refreshRequest = new RefreshTokenRequest
                {
                    Address = TokenUrl,
                    GrantType = OidcConstants.GrantTypes.RefreshToken,
                    Scope = RefreshTokenScope,
                    RefreshToken = _currentToken.RefreshToken
                };

                refreshRequest.Headers.Add(AuthHeaderName, BasicAuthHeader);

                var client = _clientFactory.CreateClient(Constants.TokenServiceClientName);
                var refreshResult = await client.RequestRefreshTokenAsync(refreshRequest);

                var tokenStore = new TokenStore(refreshResult);

                _store.AddOrUpdate(tokenStore);

                _currentToken = tokenStore;
            }

            return _currentToken;
        }
    }
}