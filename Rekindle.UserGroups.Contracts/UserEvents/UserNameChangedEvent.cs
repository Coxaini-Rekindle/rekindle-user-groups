namespace Rekindle.UserGroups.Contracts.UserEvents;

public class UserNameChangedEvent : Event
{
    public Guid UserId { get; set; }
    public string NewName { get; set; }

    public UserNameChangedEvent(Guid userId, string newName)
    {
        UserId = userId;
        NewName = newName;
    }
}