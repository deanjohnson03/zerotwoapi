using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RestSharp;
using zerotwoapi.Models;

namespace zerotwoapi.Repositories;

public class TwitchTokenRepository : ITwitchTokenRepository
{
    public readonly IConfiguration _configuration;
    public readonly IMemoryCache _memoryCache;

    public TwitchTokenRepository(IConfiguration configuration, IMemoryCache memoryCache)
    {
        _configuration = configuration;
        _memoryCache = memoryCache;
    }

    private async Task<TwitchAccessToken> GetTwitchToken()
    {
        var secret = _configuration["TwitchAuthentication:ClientSecret"];
        var clientId = _configuration["TwitchAuthentication:ClientId"];
        var grantType = "client_credentials";
        var baseUrl = "https://id.twitch.tv";
        var accessToken = new TwitchAccessToken();

        var client = new RestClient(baseUrl);
        var request = new RestRequest("oauth2/token", Method.Post);
        
        request.AddParameter("client_id", clientId);
        request.AddParameter("client_secret", secret);
        request.AddParameter("grant_type", grantType);

        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            accessToken = JsonConvert.DeserializeObject<TwitchAccessToken>(response.Content);
            accessToken.IsRequestSuccess = true;
        }

        return accessToken;
    }

    public async Task<TwitchAccessToken> GetTokenFromCache()
    {
        TwitchAccessToken token;

        token = _memoryCache.Get<TwitchAccessToken>("TwitchAuthToken");

        if (token is null)
        {
            token = await GetTwitchToken();

            if (token.IsRequestSuccess)
            {
                _memoryCache.Set("TwitchAuthToken", token);
                return token;
            }
            else
            {
                token.IsRequestSuccess = false;
                token.ErrorMessage = "Unable to get twitch authentication token";
                return token;
            }
        }
        else
        {
            var tokenExpiryTime = DateTimeOffset.FromUnixTimeSeconds(token.ExpiresIn);

            var currentEpochTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var tokenExpiryTime2 = currentEpochTime + token.ExpiresIn;

            // TODO - This is broken, I want to work out the expiry time so I know when
            //        to get another token.
            var x = DateTimeOffset.FromUnixTimeSeconds(tokenExpiryTime2);

            if (tokenExpiryTime >= DateTime.Now.AddMinutes(-5))
            {
                _memoryCache.Remove("TwitchAuthToken");

                token = await GetTwitchToken();

                if (token.IsRequestSuccess)
                {
                    _memoryCache.Set("TwitchAuthToken", token);
                    return token;
                }
                else
                {
                    token.IsRequestSuccess = false;
                    token.ErrorMessage = "Unable to get twitch authentication token";
                    return token;
                }
            }
            else
            {
                return token;
            }
        }
    }
}