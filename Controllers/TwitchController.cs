using Microsoft.AspNetCore.Mvc;
using zerotwoapi.Interfaces;

namespace zerotwoapi.Controllers;

[ApiController]
[Route("[controller]")]
public class TwitchController : ControllerBase
{
    private readonly ITwitchRepository _twitchRepository;

    public TwitchController(ITwitchRepository twitchRepository)
    {
        _twitchRepository = twitchRepository;
    }

    [HttpGet]
    public string Get()
    {
        return "Hello World!";
    }

    [Route("CreateVote")]
    [HttpGet]
    public async Task<string> CreateVote()
    {
        return await _twitchRepository.StartTwitchVote();
    }
}