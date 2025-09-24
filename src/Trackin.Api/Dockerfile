# Etapa de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Argumentos para configuração do banco de dados
ARG DATABASE__SOURCE
ARG DATABASE__USER
ARG DATABASE__PASSWORD
ARG DATABASE__NAME

# Variáveis de ambiente para o banco de dados
ENV DATABASE__SOURCE=$DATABASE__SOURCE
ENV DATABASE__USER=$DATABASE__USER
ENV DATABASE__PASSWORD=$DATABASE__PASSWORD
ENV DATABASE__NAME=$DATABASE__NAME

# Defina o diretório de trabalho
WORKDIR /App/Trackin.Api

# Copie os arquivos .csproj de todos os projetos para o contêiner (diretórios relativos)
COPY ../Trackin.Api/Trackin.Api.csproj ./Trackin.Api/
COPY ../Trackin.Application/Trackin.Application.csproj ./Trackin.Application/
COPY ../Trackin.Infrastructure/Trackin.Infrastructure.csproj ./Trackin.Infrastructure/

COPY ../Trackin.Domain/Trackin.Domain.csproj ./Trackin.Domain/

# Copie o código-fonte completo para o contêiner
COPY ../ ./ 

RUN dotnet restore Trackin.Api/Trackin.Api.csproj 

RUN dotnet tool install --global dotnet-ef

RUN dotnet publish Trackin.Api/Trackin.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

RUN addgroup --system --gid 1001 dotnetuser
RUN adduser --system --uid 1001 --ingroup dotnetuser dotnetuser

ENV DATABASE__SOURCE=$DATABASE__SOURCE
ENV DATABASE__USER=$DATABASE__USER
ENV DATABASE__PASSWORD=$DATABASE__PASSWORD
ENV DATABASE__NAME=$DATABASE__NAME

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=build /app/publish ./ 

USER dotnetuser

ENTRYPOINT ["dotnet", "Trackin.Api.dll"]