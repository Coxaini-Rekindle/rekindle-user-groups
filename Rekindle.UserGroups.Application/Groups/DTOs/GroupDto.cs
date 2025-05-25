using Rekindle.UserGroups.Domain.Entities.Groups;

namespace Rekindle.UserGroups.Application.Groups.DTOs;

public class GroupDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public int MemberCount { get; set; }
    public bool IsCurrentUserMember { get; set; }
    public bool IsCurrentUserOwner { get; set; }
    public IEnumerable<GroupMemberShortDto> Members { get; set; } = new List<GroupMemberShortDto>();
    
    public static GroupDto FromGroup(Group group, Guid? currentUserId = null)
    {
        var isCurrentUserMember = false;
        var isCurrentUserOwner = false;
        
        if (currentUserId.HasValue)
        {
            var member = group.GetMember(currentUserId.Value);
            isCurrentUserMember = member != null;
            isCurrentUserOwner = member?.Role == Domain.Entities.GroupUsers.Enumerations.GroupUserRole.Owner;
        }
        
        return new GroupDto
        {
            Id = group.Id,
            Name = group.Name,
            Description = group.Description,
            CreatedAt = group.CreatedAt,
            MemberCount = group.Members.Count,
            IsCurrentUserMember = isCurrentUserMember,
            IsCurrentUserOwner = isCurrentUserOwner,
            Members = group.Members.OrderByDescending(x => x.JoinedAt).Select(GroupMemberShortDto.FromGroupUser)
        };
    }
}

public class GroupMemberShortDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public Guid? AvatarFileId { get; set; }

    public static GroupMemberShortDto FromGroupUser(Domain.Entities.GroupUsers.GroupUser groupUser)
    {
        return new GroupMemberShortDto
        {
            UserId = groupUser.UserId,
            Name = groupUser.User.Name,
            AvatarFileId = groupUser.User.AvatarFileId
        };
    }
}
