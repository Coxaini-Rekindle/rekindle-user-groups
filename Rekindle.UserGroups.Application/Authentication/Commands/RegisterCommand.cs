using MediatR;
using Microsoft.EntityFrameworkCore;
using Rekindle.UserGroups.Application.Authentication.Exceptions;
using Rekindle.UserGroups.Application.Authentication.Interfaces;
using Rekindle.UserGroups.Application.Authentication.Models;
using Rekindle.UserGroups.DataAccess;
using Rekindle.UserGroups.Domain.Entities.Users;

namespace Rekindle.UserGroups.Application.Authentication.Commands;

public record RegisterCommand(string Name, string Username, string Email, string Password)
    : IRequest<AuthenticationResult>;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
{
    private readonly UserGroupsDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public RegisterCommandHandler(
        UserGroupsDbContext dbContext,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _dbContext.Users.AnyAsync(u => u.Email == request.Email || u.Username == request.Username,
                cancellationToken)) throw new EmailOrUserNameAlreadyUsed();

        var passwordHash = _passwordHasher.HashPassword(request.Password);

        var user = User.Create(
            request.Name,
            request.Username,
            request.Email,
            passwordHash);

        await _dbContext.Users.AddAsync(user, cancellationToken);

        var userClaims = new UserClaims(user.Id, user.Email);
        var accessToken = _jwtTokenGenerator.GenerateToken(userClaims);
        var refreshToken = _refreshTokenGenerator.GenerateRefreshToken();

        // Save refresh token to user
        user.SetRefreshToken(refreshToken.Token, refreshToken.ExpiryTime);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new AuthenticationResult(
            accessToken,
            refreshToken.Token,
            refreshToken.ExpiryTime);
    }
}