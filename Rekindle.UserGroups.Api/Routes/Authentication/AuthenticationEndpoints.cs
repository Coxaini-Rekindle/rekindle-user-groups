using MediatR;
using Rekindle.UserGroups.Application.Authentication.Commands;

namespace Rekindle.UserGroups.Api.Routes.Authentication;

public static class AuthenticationEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth")
            .WithTags("Authentication")
            .WithDisplayName("Authentication")
            .WithDescription("Authentication endpoints for user login and registration");

        group.MapPost("/login", async (LoginRequest request, IMediator mediator) =>
            {
                var command = new LoginCommand(request.Email, request.Password);
                var result = await mediator.Send(command);
                return Results.Ok(result);
            })
            .AllowAnonymous();

        group.MapPost("/register", async (RegisterRequest request, IMediator mediator) =>
            {
                var command = new RegisterCommand(
                    request.Name,
                    request.Username,
                    request.Email,
                    request.Password);

                var result = await mediator.Send(command);
                return TypedResults.Ok(result);
            })
            .AllowAnonymous();

        group.MapPost("/refresh", async (RefreshTokenRequest request, IMediator mediator) =>
        {
            var command = new RefreshTokenCommand(request.AccessToken, request.RefreshToken);
            var result = await mediator.Send(command);
            return TypedResults.Ok(result);
        }).AllowAnonymous();

        return app;
    }

    private record LoginRequest(string Email, string Password);

    private record RegisterRequest(string Name, string Username, string Email, string Password);

    private record RefreshTokenRequest(string AccessToken, string RefreshToken);
}