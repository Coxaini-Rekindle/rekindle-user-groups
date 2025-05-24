using MediatR;
using Microsoft.AspNetCore.Authorization;
using Rekindle.UserGroups.Application.Storage.Interfaces;
using Rekindle.UserGroups.Application.Users.Commands;
using Rekindle.UserGroups.Application.Users.Queries;
using System.Security.Claims;

namespace Rekindle.UserGroups.Api.Routes.Users;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users")
            .WithTags("Users")
            .WithDisplayName("Users")
            .WithDescription("User management endpoints")
            .RequireAuthorization();

        group.MapGet("/profile", async (ClaimsPrincipal user, IMediator mediator) =>
            {
                var userId = GetUserIdFromClaims(user);
                var query = new GetUserProfileQuery(userId);
                var result = await mediator.Send(query);
                return Results.Ok(result);
            })
            .WithDisplayName("Get User Profile")
            .WithDescription("Get the current user's profile information");

        group.MapPut("/name", async (UpdateNameRequest request, ClaimsPrincipal user, IMediator mediator) =>
            {
                var userId = GetUserIdFromClaims(user);
                var command = new UpdateUserNameCommand(userId, request.Name);
                await mediator.Send(command);
                return Results.Ok();
            })
            .WithDisplayName("Update User Name")
            .WithDescription("Update the current user's name");

        group.MapPost("/avatar", async (IFormFile avatar, ClaimsPrincipal user, IMediator mediator) =>
            {
                var userId = GetUserIdFromClaims(user);
                
                // Validate file
                if (avatar == null || avatar.Length == 0)
                {
                    return Results.BadRequest("No file uploaded.");
                }

                // Check file size (max 5MB)
                if (avatar.Length > 5 * 1024 * 1024)
                {
                    return Results.BadRequest("File size cannot exceed 5MB.");
                }

                using var stream = avatar.OpenReadStream();
                var command = new UploadUserAvatarCommand(userId, stream, avatar.ContentType);
                var avatarFileId = await mediator.Send(command);
                
                return Results.Ok(new { AvatarFileId = avatarFileId });
            })
            .WithDisplayName("Upload User Avatar")
            .WithDescription("Upload a new avatar for the current user")
            .DisableAntiforgery();

        group.MapGet("/avatar/{fileId:guid}", async (Guid fileId, IFileStorage fileStorage) =>
            {
                try
                {
                    var fileResponse = await fileStorage.GetAsync(fileId);
                    return Results.Stream(fileResponse.Stream, fileResponse.ContentType);
                }
                catch
                {
                    return Results.NotFound();
                }
            })
            .WithDisplayName("Get Avatar")
            .WithDescription("Get avatar image by file ID")
            .AllowAnonymous();

        return app;
    }

    private static Guid GetUserIdFromClaims(ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user token.");
        }
        return userId;
    }

    private record UpdateNameRequest(string Name);
}
