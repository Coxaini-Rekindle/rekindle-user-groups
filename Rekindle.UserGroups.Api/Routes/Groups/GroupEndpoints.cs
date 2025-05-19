using System.Security.Claims;
using MediatR;
using Rekindle.UserGroups.Api.Common.Helpers;
using Rekindle.UserGroups.Application.Groups.Commands;
using Rekindle.UserGroups.Application.Groups.DTOs;
using Rekindle.UserGroups.Application.Groups.Queries;

namespace Rekindle.UserGroups.Api.Routes.Groups;

/// <summary>
/// Group endpoints for managing user groups and invitations
/// </summary>
public static class GroupEndpoints
{
    /// <summary>
    /// Maps all group-related endpoints
    /// </summary>
    public static IEndpointRouteBuilder MapGroupEndpoints(this IEndpointRouteBuilder app)
    {
        var groupsEndpoint = app.MapGroup("groups")
            .WithTags("Groups")
            .WithDisplayName("Groups")
            .WithDescription("Group management endpoints")
            .RequireAuthorization();

        // Map the different endpoint categories
        MapGroupManagementEndpoints(groupsEndpoint);
        MapGroupMembershipEndpoints(groupsEndpoint);
        MapGroupInvitationEndpoints(groupsEndpoint);
        MapGroupJoiningEndpoints(app);
        
        return app;
    }

    /// <summary>
    /// Maps endpoints related to group creation and management
    /// </summary>
    private static void MapGroupManagementEndpoints(IEndpointRouteBuilder group)
    {
        group.MapPost("/",
                async (CreateGroupRequest request, ClaimsPrincipal user, IMediator mediator) =>
                {
                    var userId = UserContextHelper.GetCurrentUserId(user);
                    if (userId == null) return Results.Unauthorized();

                    var command = new CreateGroupCommand(
                        request.Name,
                        request.Description,
                        userId.Value
                    );

                    var result = await mediator.Send(command);
                    return TypedResults.Ok(result);
                })
            .WithName("CreateGroup")
            .WithDescription("Create a new group")
            .Produces<GroupDto>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized);
        
        group.MapPut("/{groupId:guid}",
                async (Guid groupId, UpdateGroupRequest request, ClaimsPrincipal user, IMediator mediator) =>
                {
                    var userId = UserContextHelper.GetCurrentUserId(user);
                    if (userId == null) return Results.Unauthorized();

                    var command = new UpdateGroupCommand(
                        groupId,
                        request.Name,
                        request.Description,
                        userId.Value
                    );

                    var result = await mediator.Send(command);
                    return TypedResults.Ok(result);
                })
            .WithName("UpdateGroup")
            .WithDescription("Update a group's details")
            .Produces<GroupDto>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);
        
        group.MapGet("/",
                async (ClaimsPrincipal user, IMediator mediator) =>
                {
                    var userId = UserContextHelper.GetCurrentUserId(user);
                    if (userId == null) return Results.Unauthorized();

                    var query = new GetUserGroupsQuery(userId.Value);
                    var result = await mediator.Send(query);
                    return TypedResults.Ok(result);
                })
            .WithName("GetUserGroups")
            .WithDescription("Get all groups for the current user")
            .Produces<List<GroupDto>>()
            .ProducesProblem(StatusCodes.Status401Unauthorized);
        
        group.MapGet("/{groupId:guid}",
                async (Guid groupId, ClaimsPrincipal user, IMediator mediator) =>
                {
                    var userId = UserContextHelper.GetCurrentUserId(user);

                    var query = new GetGroupDetailsQuery(groupId, userId);
                    var result = await mediator.Send(query);
                    return TypedResults.Ok(result);
                })
            .WithName("GetGroupDetails")
            .WithDescription("Get details for a specific group")
            .Produces<GroupDto>()
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    /// <summary>
    /// Maps endpoints related to group membership
    /// </summary>
    private static void MapGroupMembershipEndpoints(IEndpointRouteBuilder group)
    {
        group.MapGet("/{groupId:guid}/members",
                async (Guid groupId, ClaimsPrincipal user, IMediator mediator) =>
                {
                    var userId = UserContextHelper.GetCurrentUserId(user);
                    if (userId == null) return Results.Unauthorized();

                    var query = new GetGroupMembersQuery(groupId, userId);
                    var result = await mediator.Send(query);
                    return TypedResults.Ok(result);
                })
            .WithName("GetGroupMembers")
            .WithDescription("Get members of a group")
            .Produces<List<GroupMemberDto>>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    /// <summary>
    /// Maps endpoints related to group invitations
    /// </summary>
    private static void MapGroupInvitationEndpoints(IEndpointRouteBuilder group)
    {
    var invitesGroup = group.MapGroup("/{groupId:guid}/invites")
            .WithTags("Group Invitations");
            
        invitesGroup.MapPost("/",
                async (Guid groupId, InviteToGroupRequest request, ClaimsPrincipal user, IMediator mediator) =>
                {
                    var userId = UserContextHelper.GetCurrentUserId(user);
                    if (userId == null) return Results.Unauthorized();

                    var command = new InviteToGroupCommand(
                        groupId,
                        request.Email,
                        userId.Value
                    );

                    var result = await mediator.Send(command);
                    return TypedResults.Ok(result);
                })
            .WithName("InviteToGroup")
            .WithDescription("Invite a user to a group by email")
            .Produces<GroupInviteDto>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);
        
        invitesGroup.MapPost("/link",
                async (Guid groupId, CreateInviteLinkRequest request, ClaimsPrincipal user, IMediator mediator) =>
                {
                    var userId = UserContextHelper.GetCurrentUserId(user);
                    if (userId == null) return Results.Unauthorized();

                    var command = new CreateInviteLinkCommand(
                        groupId,
                        request.MaxUses,
                        request.ExpirationDays,
                        userId.Value
                    );

                    var result = await mediator.Send(command);
                    return TypedResults.Ok(new InviteLinkResponse(result));
                })
            .WithName("CreateGroupInvitationLink")
            .WithDescription("Create a shareable invitation link for a group")
            .Produces<InviteLinkResponse>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    /// <summary>
    /// Maps endpoints related to joining groups
    /// </summary>
    private static void MapGroupJoiningEndpoints(IEndpointRouteBuilder app)
    {
        // User invitations list endpoint
        app.MapGroup("invitations")
            .WithTags("Group Invitations")
            .RequireAuthorization().MapGet("/",
                async (ClaimsPrincipal user, IMediator mediator) =>
                {
                    var userId = UserContextHelper.GetCurrentUserId(user);
                    if (userId == null) return Results.Unauthorized();

                    var query = new GetUserInvitationsQuery(userId.Value);
                    var result = await mediator.Send(query);
                    return TypedResults.Ok(result);
                })
            .WithName("GetUserInvitations")
            .WithDescription("Get all pending invitations for the current user")
            .Produces<List<GroupInviteDto>>()
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        // Accept invitation endpoint
        app.MapPost("invitations/{inviteId:guid}/accept",
                async (Guid inviteId, ClaimsPrincipal user, IMediator mediator) =>
                {
                    var userId = UserContextHelper.GetCurrentUserId(user);
                    if (userId == null) return Results.Unauthorized();

                    var command = new AcceptInviteCommand(inviteId, userId.Value);
                    var result = await mediator.Send(command);
                    return TypedResults.Ok(result);
                })
            .RequireAuthorization()
            .WithTags("Group Invitations")
            .WithName("AcceptGroupInvitation")
            .WithDescription("Accept a group invitation")
            .Produces<GroupDto>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound);
            
        // Join with link endpoint
        app.MapPost("groups/join/{token}",
                async (string token, ClaimsPrincipal user, IMediator mediator) =>
                {
                    var userId = UserContextHelper.GetCurrentUserId(user);
                    if (userId == null) return Results.Unauthorized();

                    var command = new JoinGroupWithLinkCommand(token, userId.Value);
                    var result = await mediator.Send(command);
                    return TypedResults.Ok(result);
                })
            .RequireAuthorization()
            .WithTags("Group Invitations")
            .WithName("JoinGroupWithLink")
            .WithDescription("Join a group using an invitation link")
            .Produces<GroupDto>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    // Request and response models
    public record CreateGroupRequest(string Name, string Description);

    public record UpdateGroupRequest(string Name, string Description);

    public record InviteToGroupRequest(string Email);

    public record CreateInviteLinkRequest(int MaxUses = 10, int ExpirationDays = 30);

    public record InviteLinkResponse(string InvitationLink);
}