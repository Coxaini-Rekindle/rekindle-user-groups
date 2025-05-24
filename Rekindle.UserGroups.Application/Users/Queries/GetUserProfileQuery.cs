using MediatR;
using Microsoft.EntityFrameworkCore;
using Rekindle.UserGroups.DataAccess;

namespace Rekindle.UserGroups.Application.Users.Queries;

public record GetUserProfileQuery(Guid UserId) : IRequest<UserProfileDto>;

public record UserProfileDto(
    Guid Id,
    string Name,
    string Username,
    string Email,
    Guid? AvatarFileId);

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto>
{
    private readonly UserGroupsDbContext _context;    public GetUserProfileQueryHandler(UserGroupsDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Where(u => u.Id == request.UserId)
            .Select(u => new UserProfileDto(
                u.Id,
                u.Name,
                u.Username,
                u.Email,
                u.AvatarFileId))
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        return user;
    }
}
