namespace Rekindle.UserGroups.Contracts.Models;

public class UserContractDto
{
    public Guid Id { get; }
    public string UserName { get; }
    public string Name { get; }
    public Guid? AvatarFileId { get; }

    public UserContractDto(Guid id, string userName, string name, Guid? avatarFileId)
    {
        Id = id;
        UserName = userName;
        Name = name;
        AvatarFileId = avatarFileId;
    }
}