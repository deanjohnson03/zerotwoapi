namespace zerotwoapi.Interfaces;

public interface ITwitchRepository
{
    public Task<string> StartTwitchVote();
}