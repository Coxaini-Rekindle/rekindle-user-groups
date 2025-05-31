FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy NuGet configuration first
COPY ["NuGet.Config", "."]

# Copy solution file
COPY ["Rekindle.UserGroups.sln", "."]

# Copy all project files first for better Docker layer caching
COPY ["Rekindle.UserGroups.Api/Rekindle.UserGroups.Api.csproj", "Rekindle.UserGroups.Api/"]
COPY ["Rekindle.UserGroups.Application/Rekindle.UserGroups.Application.csproj", "Rekindle.UserGroups.Application/"]
COPY ["Rekindle.UserGroups.Contracts/Rekindle.UserGroups.Contracts.csproj", "Rekindle.UserGroups.Contracts/"]
COPY ["Rekindle.UserGroups.DataAccess/Rekindle.UserGroups.DataAccess.csproj", "Rekindle.UserGroups.DataAccess/"]
COPY ["Rekindle.UserGroups.Domain/Rekindle.UserGroups.Domain.csproj", "Rekindle.UserGroups.Domain/"]
COPY ["Rekindle.UserGroups.Infrastructure/Rekindle.UserGroups.Infrastructure.csproj", "Rekindle.UserGroups.Infrastructure/"]

# Restore packages using the NuGet.Config
RUN dotnet restore "Rekindle.UserGroups.Api/Rekindle.UserGroups.Api.csproj" --configfile "./NuGet.Config"

# Copy everything else
COPY . .

# Build the main project
WORKDIR "/src/Rekindle.UserGroups.Api"
RUN dotnet build "./Rekindle.UserGroups.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Rekindle.UserGroups.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rekindle.UserGroups.Api.dll"]
