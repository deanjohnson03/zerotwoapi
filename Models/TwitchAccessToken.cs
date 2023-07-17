using Newtonsoft.Json;

namespace zerotwoapi.Models;

public class TwitchAccessToken : TwitchBaseResponse
{
    //"{\"access_token\":\"ejaqm12mc7qo7ri2kq5ozbkx4gqdqt\",\"expires_in\":4793828,\"token_type\":\"bearer\"}\n"

    [JsonProperty("access_token")]
    public string? AccessToken { get; set; }

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonProperty("token_type")]
    public string? TokenType { get; set; }

    public DateTime DateRequested { get; set; }
}