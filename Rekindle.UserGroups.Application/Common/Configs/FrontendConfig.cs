namespace Rekindle.UserGroups.Application.Common.Configs;

public class FrontendConfig
{
    public const string Frontend = "FrontendConfig";
    public string BaseUrl { get; set; } = string.Empty;
    public string PersonalInvitationRoute { get; set; } = string.Empty;
    public string TokenInvitationRoute { get; set; } = string.Empty;

    public string PersonalInvitationUrl => $"{BaseUrl}{PersonalInvitationRoute}";
    public string TokenInvitationUrl => $"{BaseUrl}{TokenInvitationRoute}";
}