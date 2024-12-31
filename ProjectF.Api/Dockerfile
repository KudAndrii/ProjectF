FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
#USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ProjectF.Api/ProjectF.Api.csproj", "ProjectF.Api/"]
COPY ["ProjectF.Cache/ProjectF.Cache.csproj", "ProjectF.Cache/"]
COPY ["ProjectF.Core/ProjectF.Core.csproj", "ProjectF.Core/"]
COPY ["ProjectF.DataAccess/ProjectF.DataAccess.csproj", "ProjectF.DataAccess/"]
COPY ["ProjectF.Handlers/ProjectF.Handlers.csproj", "ProjectF.Handlers/"]
COPY ["ProjectF.OmdbClient/ProjectF.OmdbClient.csproj", "ProjectF.OmdbClient/"]
RUN dotnet restore "ProjectF.Api/ProjectF.Api.csproj"
COPY . .
WORKDIR "/src/ProjectF.Api"
RUN dotnet build "ProjectF.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ProjectF.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectF.Api.dll"]
