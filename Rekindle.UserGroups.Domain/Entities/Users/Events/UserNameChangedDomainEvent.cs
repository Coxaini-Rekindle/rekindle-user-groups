using Rekindle.UserGroups.Domain.Common;

namespace Rekindle.UserGroups.Domain.Entities.Users.Events;

public class UserNameChangedDomainEvent : DomainEvent
{
    public Guid UserId { get; }
    public string OldName { get; }
    public string NewName { get; }

    public UserNameChangedDomainEvent(Guid userId, string oldName, string newName)
    {
        UserId = userId;
        OldName = oldName;
        NewName = newName;
    }
}