# Multi-stage build for a .NET 8 Web API
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files with exact paths
COPY ["WatApi/WatApi.sln", "WatApi/"]
COPY ["WatApi/WatApi/WatApi.csproj", "WatApi/WatApi/"]

# Restore only the project dependencies
RUN dotnet restore "WatApi/WatApi.sln"

# Copy everything else
COPY . .
WORKDIR /src/WatApi/WatApi

# Publish the application
RUN dotnet publish "WatApi.csproj" -c Release -o /app/publish --no-restore

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# .NET 8 standard port
EXPOSE 8080

# Copy published app from the build stage
COPY --from=build /app/publish .

# Launch
ENTRYPOINT ["dotnet", "WatApi.dll"]