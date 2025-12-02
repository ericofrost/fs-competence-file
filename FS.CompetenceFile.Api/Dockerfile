FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FS.CompetenceFile.Api/FS.CompetenceFile.Api.csproj", "FS.CompetenceFile.Api/"]
RUN dotnet restore "FS.CompetenceFile.Api/FS.CompetenceFile.Api.csproj"
COPY . .
WORKDIR "/src/FS.CompetenceFile.Api"
RUN dotnet build "FS.CompetenceFile.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FS.CompetenceFile.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FS.CompetenceFile.Api.dll"]
