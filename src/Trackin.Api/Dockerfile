FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["src/Trackin.Api/Trackin.Api.csproj", "./Trackin.Api/"]
COPY ["src/Trackin.Application/Trackin.Application.csproj", "./Trackin.Application/"]
COPY ["src/Trackin.Infrastructure/Trackin.Infrastructure.csproj", "./Trackin.Infrastructure/"]
COPY ["src/Trackin.Domain/Trackin.Domain.csproj", "./Trackin.Domain/"]

RUN dotnet restore "./Trackin.Api/Trackin.Api.csproj"

# Copiar todo o código fonte
COPY src/ .

WORKDIR /src/Trackin.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

RUN addgroup --system --gid 1001 dotnetuser && \
    adduser --system --uid 1001 --ingroup dotnetuser dotnetuser

WORKDIR /app

COPY --from=build /app/publish .

RUN chown -R dotnetuser:dotnetuser /app
USER dotnetuser

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "Trackin.Api.dll"]