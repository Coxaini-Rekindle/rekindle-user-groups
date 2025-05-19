using Rekindle.UserGroups.Domain.Entities.Groups;
using Rekindle.UserGroups.Domain.Entities.Users;

namespace Rekindle.UserGroups.Domain.Entities.GroupInvites;

public class InvitationLink
{
    public Guid Id { get; private set; }
    public Guid GroupId { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    public string Token { get; private set; } = null!;
    public int MaxUses { get; private set; }
    public int UsedCount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }

    // Navigation properties
    public Group Group { get; private set; } = null!;
    public User CreatedBy { get; private set; } = null!;

    private InvitationLink()
    {
    }

    public static InvitationLink Create(
        Guid groupId,
        Guid createdByUserId,
        int maxUses = 10,
        int expirationDays = 30)
    {
        return new InvitationLink
        {
            Id = Guid.NewGuid(),
            GroupId = groupId,
            CreatedByUserId = createdByUserId,
            Token = GenerateUniqueToken(),
            MaxUses = maxUses,
            UsedCount = 0,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(expirationDays)
        };
    }

    public bool IsValid()
    {
        return !IsExpired() && (MaxUses == 0 || UsedCount < MaxUses);
    }

    public bool IsExpired()
    {
        return DateTime.UtcNow > ExpiresAt;
    }

    public bool IncrementUsageCount()
    {
        if (!IsValid()) return false;

        UsedCount++;
        return true;
    }

    private static string GenerateUniqueToken()
    {
        // Generate a URL-friendly token (e.g., "xYz123Ab")
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("/", "_")
            .Replace("+", "-")
            .Replace("=", "")
            .Substring(0, 8);
    }
}