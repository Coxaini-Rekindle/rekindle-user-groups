using System.Security.Claims;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rekindle.Authentication.Interfaces;
using Rekindle.Authentication.Models;
using Rekindle.UserGroups.Application.Authentication.Exceptions;
using Rekindle.UserGroups.DataAccess;

namespace Rekindle.UserGroups.Application.Authentication.Commands;

public record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<AuthenticationResult>;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthenticationResult>
{
    private readonly UserGroupsDbContext _dbContext;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public RefreshTokenCommandHandler(
        UserGroupsDbContext dbContext,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        _dbContext = dbContext;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    public async Task<AuthenticationResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null)
            throw new InvalidTokenException();

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            throw new InvalidTokenException();

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId), cancellationToken);

        if (user == null)
            throw new UserNotFoundException();

        if (!user.IsRefreshTokenValid(request.RefreshToken))
            throw new TokenExpiredException();

        var userClaims = new UserClaims(user.Id, user.Email);
        var accessToken = _jwtTokenGenerator.GenerateToken(userClaims);
        var refreshToken = _refreshTokenGenerator.GenerateRefreshToken();

        user.SetRefreshToken(refreshToken.Token, refreshToken.ExpiryTime);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new AuthenticationResult(
            accessToken,
            refreshToken.Token,
            refreshToken.ExpiryTime);
    }
}