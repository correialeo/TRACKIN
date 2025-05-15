
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Trackin.API/Trackin.API.csproj", "Trackin.API/"]
RUN dotnet restore "Trackin.API/Trackin.API.csproj"

COPY . .
WORKDIR "/src/Trackin.API"
RUN dotnet build "Trackin.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Trackin.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

ENV DATABASE__SOURCE=${DB_SOURCE}
ENV DATABASE__USER=${DB_USER}
ENV DATABASE__PASSWORD=${DB_PASSWORD}

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Trackin.API.dll"]