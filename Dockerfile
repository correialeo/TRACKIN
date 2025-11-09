FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copiar os arquivos .csproj
COPY ["Trackin.Api/Trackin.Api.csproj", "Trackin.Api/"]
COPY ["Trackin.Application/Trackin.Application.csproj", "Trackin.Application/"]
COPY ["Trackin.Infrastructure/Trackin.Infrastructure.csproj", "Trackin.Infrastructure/"]
COPY ["Trackin.Domain/Trackin.Domain.csproj", "Trackin.Domain/"]

# Restaurar dependências
RUN dotnet restore "Trackin.Api/Trackin.Api.csproj"

# Copiar o código fonte
COPY ["Trackin.Domain/", "Trackin.Domain/"]
COPY ["Trackin.Application/", "Trackin.Application/"]
COPY ["Trackin.Infrastructure/", "Trackin.Infrastructure/"]
COPY ["Trackin.Api/", "Trackin.Api/"]

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
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV ASPNETCORE_HTTP_PORTS=8080
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "Trackin.Api.dll"]