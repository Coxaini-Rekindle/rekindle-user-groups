using MediatR;
using Microsoft.EntityFrameworkCore;
using Rekindle.UserGroups.Application.Authentication.Exceptions;
using Rekindle.UserGroups.Application.Authentication.Interfaces;
using Rekindle.UserGroups.Application.Authentication.Models;
using Rekindle.UserGroups.DataAccess;

namespace Rekindle.UserGroups.Application.Authentication.Commands;

public record LoginCommand(string Email, string Password) : IRequest<AuthenticationResult>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResult>
{
    private readonly UserGroupsDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public LoginCommandHandler(
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

    public async Task<AuthenticationResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null) throw new InvalidCredentialsException();

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            throw new InvalidCredentialsException();

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