using Ardalis.GuardClauses;
using Rekindle.Exceptions.Api;
using Rekindle.UserGroups.Api.Routes.Authentication;
using Rekindle.UserGroups.Application;
using Rekindle.UserGroups.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
var allowedHost = builder.Configuration.GetValue<string>("AllowedHosts");
Guard.Against.NullOrWhiteSpace(allowedHost, nameof(allowedHost));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Default",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder
                .WithOrigins(allowedHost)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithExposedHeaders("Content-Disposition", "*");
        });
});

builder.Services
    .AddApplication()
    .AddDataAccess(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/openapi/v1.json", "Rekindle User Groups API"); });
}

app.UseCors("Default");

app.UseExceptionHandlingMiddleware();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();

app.Run();