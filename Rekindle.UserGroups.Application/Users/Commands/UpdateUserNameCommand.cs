using MediatR;
using Microsoft.EntityFrameworkCore;
using Rekindle.UserGroups.DataAccess;

namespace Rekindle.UserGroups.Application.Users.Commands;

public record UpdateUserNameCommand(Guid UserId, string Name) : IRequest;

public class UpdateUserNameCommandHandler : IRequestHandler<UpdateUserNameCommand>
{
    private readonly UserGroupsDbContext _context;

    public UpdateUserNameCommandHandler(UserGroupsDbContext context)
    {
        _context = context;
    }    public async Task Handle(UpdateUserNameCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        user.UpdateName(request.Name);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
