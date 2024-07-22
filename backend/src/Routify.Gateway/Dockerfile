FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Production
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /
COPY ["src/Routify.Gateway/Routify.Gateway.csproj", "src/Routify.Gateway/"]
COPY ["src/Routify.Core/Routify.Core.csproj", "src/Routify.Core/"]
COPY ["src/Routify.Data/Routify.Data.csproj", "src/Routify.Data/"]
RUN dotnet restore "src/Routify.Gateway/Routify.Gateway.csproj"
COPY . .
WORKDIR "/src/Routify.Gateway"
RUN dotnet build "Routify.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Routify.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Routify.Gateway.dll"]