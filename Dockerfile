# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CartonCaps.Invite.API/CartonCaps.Invite.API.csproj", "src/CartonCaps.Invite.API/"]
COPY ["src/CartonCaps.Invite.Data/CartonCaps.Invite.Data.csproj", "src/CartonCaps.Invite.Data/"]
COPY ["src/CartonCaps.Invite.Model/CartonCaps.Invite.Model.csproj", "src/CartonCaps.Invite.Model/"]
RUN dotnet restore "./src/CartonCaps.Invite.API/CartonCaps.Invite.API.csproj"
COPY . .
WORKDIR "/src/src/CartonCaps.Invite.API"
RUN dotnet build "./CartonCaps.Invite.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CartonCaps.Invite.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CartonCaps.Invite.API.dll"]