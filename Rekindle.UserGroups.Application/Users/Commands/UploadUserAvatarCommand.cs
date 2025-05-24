using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rekindle.UserGroups.Application.Storage.Interfaces;
using Rekindle.UserGroups.DataAccess;

namespace Rekindle.UserGroups.Application.Users.Commands;

public record UploadUserAvatarCommand(Guid UserId, Stream FileStream, string ContentType) : IRequest<Guid>;

public class UploadUserAvatarCommandHandler : IRequestHandler<UploadUserAvatarCommand, Guid>
{
    private readonly UserGroupsDbContext _context;
    private readonly IFileStorage _fileStorage;
    private readonly ILogger<UploadUserAvatarCommandHandler> _logger;

    public UploadUserAvatarCommandHandler(
        UserGroupsDbContext context,
        IFileStorage fileStorage, ILogger<UploadUserAvatarCommandHandler> logger)
    {
        _context = context;
        _fileStorage = fileStorage;
        _logger = logger;
    }

    public async Task<Guid> Handle(UploadUserAvatarCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null) throw new InvalidOperationException("User not found.");

        // Validate content type
        var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
        if (!allowedContentTypes.Contains(request.ContentType.ToLowerInvariant()))
            throw new ArgumentException("Invalid file type. Only JPEG, PNG, GIF, and WebP images are allowed.");

        // Delete old avatar if exists
        if (user.AvatarFileId.HasValue)
            try
            {
                await _fileStorage.DeleteAsync(user.AvatarFileId.Value, cancellationToken);
            }
            catch
            {
                _logger.LogWarning("Failed to delete old avatar with ID {AvatarFileId}", user.AvatarFileId);
                throw new InvalidOperationException("Failed to delete old avatar.");
            }

        // Upload new avatar
        var avatarFileId = await _fileStorage.UploadAsync(request.FileStream, request.ContentType, cancellationToken);

        // Update user
        user.SetAvatar(avatarFileId);
        await _context.SaveChangesAsync(cancellationToken);

        return avatarFileId;
    }
}