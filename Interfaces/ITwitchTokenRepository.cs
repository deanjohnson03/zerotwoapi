using zerotwoapi.Models;

namespace zerotwoapi.Repositories;

public interface ITwitchTokenRepository
{
    public Task<TwitchAccessToken> GetTokenFromCache();
}