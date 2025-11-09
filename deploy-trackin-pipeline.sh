#!/bin/bash

# deploy-trackin-pipeline.sh - Deploy usando Docker Hub

set -e

echo "🚀 Iniciando deploy da Trackin API no Azure (Docker Hub)..."

# ==================== CONFIGURAÇÕES ====================
RESOURCE_GROUP="${RESOURCE_GROUP:-rg-trackin-sprint}"
LOCATION="West US"
ACI_NAME="aci-trackin-api"

# Imagem do Docker Hub (gerada pela pipeline CI)
DOCKER_IMAGE="${DOCKER_IMAGE:-correialeo/trackin.dotnet.api:latest}"

# Credenciais do Docker Hub - FORÇAR PÚBLICO se não especificado
FORCE_PUBLIC="${FORCE_PUBLIC:-true}"
DOCKER_USERNAME="${DOCKER_USERNAME:-}"
DOCKER_PASSWORD="${DOCKER_PASSWORD:-}"

# Configurações do banco
if [ -z "$DB_SERVER" ]; then
    DB_SERVER_NAME="sqlserver-trackin-$(date +%s)"
else
    DB_SERVER_NAME="$DB_SERVER"
fi
DB_NAME="${DB_NAME:-trackin_db}"
DB_ADMIN="${DB_USER:-adminuser}"
DB_PASSWORD="${DB_PASSWORD:-Trackin@123!}"

echo "📋 Configurações:"
echo "Resource Group: $RESOURCE_GROUP"
echo "Docker Image: $DOCKER_IMAGE"
echo "DB Server: $DB_SERVER_NAME"
echo "DB Name: $DB_NAME"
echo "Force Public Image: $FORCE_PUBLIC"

# ==================== REGISTRAR PROVIDERS ====================
echo ""
echo "📂 Registrando providers necessários..."
az provider register --namespace Microsoft.ContainerInstance --wait
az provider register --namespace Microsoft.Sql --wait

echo "✅ Providers registrados!"

# ==================== CRIAR RESOURCE GROUP ====================
echo ""
echo "📦 Verificando Resource Group..."
if ! az group show --name $RESOURCE_GROUP >/dev/null 2>&1; then
    echo "Criando Resource Group..."
    az group create --name $RESOURCE_GROUP --location "$LOCATION"
else
    echo "Resource Group já existe."
fi

# ==================== CRIAR SQL SERVER E DATABASE ====================
CREATE_DB=false
if [[ "$DB_SERVER_NAME" == *"$(date +%s)"* ]] || ! az sql server show --name $DB_SERVER_NAME --resource-group $RESOURCE_GROUP >/dev/null 2>&1; then
    CREATE_DB=true
    echo ""
    echo "🗄️ Criando SQL Server: $DB_SERVER_NAME..."
    az sql server create \
        --name $DB_SERVER_NAME \
        --resource-group $RESOURCE_GROUP \
        --location "$LOCATION" \
        --admin-user $DB_ADMIN \
        --admin-password $DB_PASSWORD

    echo "💾 Criando Database: $DB_NAME..."
    az sql db create \
        --resource-group $RESOURCE_GROUP \
        --server $DB_SERVER_NAME \
        --name $DB_NAME \
        --service-objective Basic

    echo "🔥 Configurando Firewall..."
    az sql server firewall-rule create \
        --resource-group $RESOURCE_GROUP \
        --server $DB_SERVER_NAME \
        --name AllowAzureServices \
        --start-ip-address 0.0.0.0 \
        --end-ip-address 0.0.0.0
else
    echo "SQL Server já existe: $DB_SERVER_NAME"
fi

# ==================== CONFIGURAR IMAGEM DO DOCKER HUB ====================
echo ""
echo "🐳 Configurando deploy com Docker Hub..."
echo "Imagem: $DOCKER_IMAGE"

# Decidir se usa autenticação (APENAS se não forçar público E tiver credenciais)
if [ "$FORCE_PUBLIC" = "false" ] && [ -n "$DOCKER_USERNAME" ] && [ -n "$DOCKER_PASSWORD" ]; then
    echo "🔐 Credenciais do Docker Hub detectadas (imagem privada)"
    USE_DOCKER_AUTH=true
else
    echo "ℹ️  Usando imagem pública do Docker Hub (sem autenticação)"
    USE_DOCKER_AUTH=false
fi

# ==================== DEPLOY CONTAINER INSTANCE ====================
echo ""
echo "🔍 Verificando se container já existe..."
if az container show --resource-group $RESOURCE_GROUP --name $ACI_NAME >/dev/null 2>&1; then
    echo "⚠️  Container já existe. Deletando para recriar..."
    az container delete --resource-group $RESOURCE_GROUP --name $ACI_NAME --yes
    sleep 15
fi

echo ""
echo "📱 Criando Container Instance no Azure..."

# String de conexão
CONNECTION_STRING="Server=$DB_SERVER_NAME.database.windows.net;Database=$DB_NAME;User Id=$DB_ADMIN;Password=$DB_PASSWORD;TrustServerCertificate=true;Encrypt=true;"

# Criar container COM ou SEM autenticação
if [ "$USE_DOCKER_AUTH" = true ]; then
    echo "🔐 Criando container com autenticação Docker Hub..."
    az container create \
        --resource-group $RESOURCE_GROUP \
        --name $ACI_NAME \
        --image $DOCKER_IMAGE \
        --registry-login-server docker.io \
        --registry-username "$DOCKER_USERNAME" \
        --registry-password "$DOCKER_PASSWORD" \
        --dns-name-label "trackin-api-$(date +%s)" \
        --ports 8080 80 443 \
        --protocol TCP \
        --ip-address Public \
        --environment-variables \
            "ASPNETCORE_ENVIRONMENT=Production" \
            "ASPNETCORE_URLS=http://0.0.0.0:8080" \
            "ASPNETCORE_HTTP_PORTS=8080" \
            "DOTNET_RUNNING_IN_CONTAINER=true" \
            "DATABASE__SOURCE=$DB_SERVER_NAME.database.windows.net" \
            "DATABASE__USER=$DB_ADMIN" \
            "DATABASE__PASSWORD=$DB_PASSWORD" \
            "DATABASE__NAME=$DB_NAME" \
            "ConnectionStrings__DefaultConnection=$CONNECTION_STRING" \
        --cpu 1.0 \
        --memory 2.0 \
        --os-type Linux \
        --restart-policy Always
else
    echo "🌐 Criando container com imagem pública (SEM autenticação)..."
    az container create \
        --resource-group $RESOURCE_GROUP \
        --name $ACI_NAME \
        --image $DOCKER_IMAGE \
        --dns-name-label "trackin-api-$(date +%s)" \
        --ports 8080 80 443 \
        --protocol TCP \
        --ip-address Public \
        --environment-variables \
            "ASPNETCORE_ENVIRONMENT=Production" \
            "ASPNETCORE_URLS=http://0.0.0.0:8080" \
            "ASPNETCORE_HTTP_PORTS=8080" \
            "DOTNET_RUNNING_IN_CONTAINER=true" \
            "DATABASE__SOURCE=$DB_SERVER_NAME.database.windows.net" \
            "DATABASE__USER=$DB_ADMIN" \
            "DATABASE__PASSWORD=$DB_PASSWORD" \
            "DATABASE__NAME=$DB_NAME" \
            "ConnectionStrings__DefaultConnection=$CONNECTION_STRING" \
        --cpu 1.0 \
        --memory 2.0 \
        --os-type Linux \
        --restart-policy Always
fi

echo ""
echo "⏳ Aguardando container inicializar..."
sleep 30

# ==================== VERIFICAÇÃO E RESULTADOS ====================
echo ""
echo "🔍 Verificando estado do container..."
CONTAINER_STATE=$(az container show --resource-group $RESOURCE_GROUP --name $ACI_NAME --query "containers[0].instanceView.currentState.state" --output tsv)
echo "Estado: $CONTAINER_STATE"

FQDN=$(az container show --resource-group $RESOURCE_GROUP --name $ACI_NAME --query "ipAddress.fqdn" --output tsv)
IP=$(az container show --resource-group $RESOURCE_GROUP --name $ACI_NAME --query "ipAddress.ip" --output tsv)

echo ""
echo "✅ =============================================="
echo "✅ Deploy concluído com sucesso!"
echo "✅ =============================================="
echo ""
echo "📊 Informações da aplicação:"
echo "🌐 URL Swagger: http://$FQDN:8080/swagger"
echo "🌐 URL API: http://$FQDN:8080"
echo "🔢 IP Público: $IP"
echo "🗄️ SQL Server: $DB_SERVER_NAME.database.windows.net"
echo "💾 Database: $DB_NAME"
echo "👤 Usuário: $DB_ADMIN"
echo ""
echo "🧪 Comandos de teste:"
echo "# Testar API via FQDN:"
echo "curl http://$FQDN:8080/swagger"
echo ""
echo "# Testar API via IP:"
echo "curl http://$IP:8080"
echo ""
echo "📋 Comandos úteis:"
echo "# Ver logs em tempo real:"
echo "az container logs --resource-group $RESOURCE_GROUP --name $ACI_NAME --follow"
echo ""
echo "# Ver estado atual:"
echo "az container show --resource-group $RESOURCE_GROUP --name $ACI_NAME --query 'containers[0].instanceView.currentState'"
echo ""
echo "# Reiniciar container:"
echo "az container restart --resource-group $RESOURCE_GROUP --name $ACI_NAME"
echo ""

# ==================== TESTE DE CONECTIVIDADE ====================
echo "🧪 Testando conectividade..."
sleep 20

if curl -s --connect-timeout 10 http://$FQDN:8080 >/dev/null 2>&1; then
    echo "✅ API respondendo corretamente!"
    echo "🎉 Deploy concluído e aplicação acessível!"
else
    echo "⚠️  API ainda está inicializando..."
    echo "💡 Aguarde mais alguns instantes e teste manualmente"
    echo ""
    echo "Para verificar logs:"
    echo "az container logs --resource-group $RESOURCE_GROUP --name $ACI_NAME"
fi

# Salvar variáveis para a pipeline (se estiver rodando no Azure DevOps)
if [ -n "$SYSTEM_TEAMFOUNDATIONCOLLECTIONURI" ]; then
    echo "##vso[task.setvariable variable=APP_URL]http://$FQDN:8080"
    echo "##vso[task.setvariable variable=APP_FQDN]$FQDN"
    echo "##vso[task.setvariable variable=APP_IP]$IP"
    echo "##vso[task.setvariable variable=DB_SERVER_FULL]$DB_SERVER_NAME.database.windows.net"
fi

echo ""
echo "🎊 Script finalizado!"