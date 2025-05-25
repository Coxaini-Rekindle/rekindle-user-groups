namespace Rekindle.UserGroups.Contracts.UserEvents;

public class UserAvatarChangedEvent : Event
{
    public Guid UserId { get; set; }
    public Guid? AvatarFileId { get; set; }

    public UserAvatarChangedEvent(Guid userId, Guid? avatarFileId)
    {
        UserId = userId;
        AvatarFileId = avatarFileId;
    }
}