using zerotwoapi.Interfaces;
using zerotwoapi.Repositories;

namespace zerotwoapi.Repositories;

public class TwitchRepository : ITwitchRepository
{
    public readonly IConfiguration _configuration;
    public readonly ITwitchTokenRepository _twitchTokenRepository;

    public TwitchRepository(IConfiguration configuration, ITwitchTokenRepository twitchTokenRepository)
    {
        _configuration = configuration;
        _twitchTokenRepository = twitchTokenRepository;
    }

    public async Task<string> StartTwitchVote()
    {
        var token = await _twitchTokenRepository.GetTokenFromCache();

        if (token.IsRequestSuccess)
        {
            return token.AccessToken;
        }
        else
        {
            return token.ErrorMessage;
        }
    }
}