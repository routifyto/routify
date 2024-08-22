# Use .NET 8 SDK as the base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["src/Routify.Migrations/Routify.Migrations.csproj", "src/Routify.Migrations/"]
COPY ["src/Routify.Data/Routify.Data.csproj", "src/Routify.Data/"]
RUN dotnet restore "src/Routify.Migrations/Routify.Migrations.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/Routify.Migrations"
RUN dotnet build "Routify.Migrations.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Routify.Migrations.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copy the configuration files
COPY src/Routify.Migrations/appsettings.json ./
COPY src/Routify.Migrations/appsettings.*.json ./

# Set the entry point
ENTRYPOINT ["dotnet", "Routify.Migrations.dll"]