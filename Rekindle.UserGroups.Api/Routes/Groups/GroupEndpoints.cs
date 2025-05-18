namespace Rekindle.UserGroups.Api.Routes.Groups;

public static class GroupEndpoints
{
    public static IEndpointRouteBuilder MapGroupEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/groups")
            .WithTags("Groups")
            .WithDisplayName("Groups")
            .WithDescription("Group management endpoints");

        return app;
    }
}