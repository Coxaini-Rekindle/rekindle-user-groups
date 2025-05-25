using Rekindle.UserGroups.Domain.Common;

namespace Rekindle.UserGroups.Domain.Entities.Users.Events;

public class UserAvatarChangedDomainEvent : DomainEvent
{
    public Guid UserId { get; }
    public Guid? OldAvatarFileId { get; }
    public Guid? NewAvatarFileId { get; }

    public UserAvatarChangedDomainEvent(Guid userId, Guid? oldAvatarFileId, Guid? newAvatarFileId)
    {
        UserId = userId;
        OldAvatarFileId = oldAvatarFileId;
        NewAvatarFileId = newAvatarFileId;
    }
}