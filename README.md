# ZEROTWO API

The values for the Twitch API are stored locally in the UserSecrets area

See here: https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=linux

```bash
dotnet user-secrets set "TwitchAuthentication:ClientSecret" "value"
```