﻿using Rekindle.UserGroups.Domain.Common;
using Rekindle.UserGroups.Domain.Entities.Groups.Events;
using Rekindle.UserGroups.Domain.Entities.GroupUsers;
using Rekindle.UserGroups.Domain.Entities.GroupUsers.Enumerations;
using Rekindle.UserGroups.Domain.Entities.Users;

namespace Rekindle.UserGroups.Domain.Entities.Groups;

public class Group : Entity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public ICollection<GroupUser> Members { get; private set; } = new List<GroupUser>();

    public static Group Create(string name, string description, DateTime createdAt, User creator)
    {
        var id = Guid.NewGuid();
        var group = new Group
        {
            Id = id,
            Name = name,
            Description = description,
            CreatedAt = createdAt,
            Members = [GroupUser.Create(creator.Id, id, GroupUserRole.Owner)]
        };

        group.AddDomainEvent(new GroupCreatedDomainEvent(
            group.Id,
            group.Name,
            group.Description,
            new CreatorUser(
                creator.Id,
                creator.Name,
                creator.Username,
                creator.AvatarFileId
            ),
            group.CreatedAt
        ));

        return group;
    }

    public GroupUser AddMember(User user, GroupUserRole role = GroupUserRole.Member)
    {
        var groupUser = GroupUser.Create(user.Id, Id, role);
        Members.Add(groupUser);

        // Add domain event for user joining a group
        AddDomainEvent(new UserJoinedGroupDomainEvent(
            Id,
            user.Id,
            user.Username,
            user.Name,
            user.AvatarFileId,
            role,
            groupUser.JoinedAt
        ));
        
        return groupUser;
    }

    public GroupUser? GetMember(Guid userId)
    {
        return Members.FirstOrDefault(m => m.UserId == userId);
    }

    public bool RemoveMember(Guid userId, bool wasRemoved = false, Guid? removedByUserId = null)
    {
        var member = GetMember(userId);
        if (member == null) return false;
        
        var result = Members.Remove(member);
        
        if (result)
        {
            // Add domain event for user leaving the group
            AddDomainEvent(new UserLeftGroupDomainEvent(
                Id,
                userId,
                wasRemoved,
                removedByUserId
            ));
        }
        
        return result;
    }

    public void Update(string name, string description, Guid updatedByUserId)
    {
        Name = name;
        Description = description;
        
        // Add domain event for group updated
        AddDomainEvent(new GroupUpdatedDomainEvent(
            Id,
            Name,
            Description,
            updatedByUserId
        ));
    }

    public void Delete(Guid deletedByUserId)
    {
        // Add domain event for group deleted
        AddDomainEvent(new GroupDeletedDomainEvent(
            Id,
            Name,
            Description,
            deletedByUserId,
            DateTime.UtcNow
        ));
    }

    private Group()
    {
    }
}

