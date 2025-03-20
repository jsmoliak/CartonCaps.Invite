# Use the official .NET SDK image as a base image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the solution file and restore NuGet packages
COPY *.sln ./
COPY src/**/*.csproj ./src/
RUN dotnet restore

# Copy the rest of the application code
COPY src/ ./src/

# Publish the API project
RUN dotnet publish src/CartonCaps.Invite.API/CartonCaps.Invite.API.csproj -c Release -o out

# Use the official ASP.NET Core runtime image as a base image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Copy your database file if you have one
COPY invite.db ./

EXPOSE 5000

# Specify the entry point for the application
ENTRYPOINT ["dotnet", "CartonCaps.Invite.API.dll"]