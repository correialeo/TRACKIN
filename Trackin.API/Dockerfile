FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG DATABASE__SOURCE
ARG DATABASE__USER
ARG DATABASE__PASSWORD

ENV DATABASE__SOURCE=$DATABASE__SOURCE
ENV DATABASE__USER=$DATABASE__USER
ENV DATABASE__PASSWORD=$DATABASE__PASSWORD

WORKDIR /App/Trackin.API

ENV PATH=$PATH:/root/.dotnet/tools
RUN dotnet tool install --global dotnet-ef

COPY . ./
RUN dotnet restore Trackin.API.csproj

RUN dotnet publish Trackin.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

ENV DATABASE__SOURCE=$DATABASE__SOURCE
ENV DATABASE__USER=$DATABASE__USER
ENV DATABASE__PASSWORD=$DATABASE__PASSWORD

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=build /app/publish ./

ENTRYPOINT ["dotnet", "Trackin.API.dll"]