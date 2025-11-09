#!/bin/bash
source ~/.bashrc

# deploy-trackin-pipeline.sh - Deploy integrado com Azure DevOps Pipeline

set -e

echo "🚀 Iniciando deploy da Trackin API no Azure (Versão Pipeline)..."

# ==================== CONFIGURAÇÕES (usa variáveis da pipeline se disponíveis) ====================
RESOURCE_GROUP="${RESOURCE_GROUP:-rg-trackin-sprint}"
LOCATION="West US"
ACI_NAME="aci-trackin-api"

# Imagem do Docker Hub (gerada pela pipeline CI)
DOCKER_IMAGE="${DOCKER_IMAGE:-correialeo/trackin.dotnet.api:latest}"

# Credenciais do Docker Hub (opcionais - só se imagem for privada)
DOCKER_USERNAME="${DOCKER_USERNAME:-}"
DOCKER_PASSWORD="${DOCKER_PASSWORD:-}"

# Configurações do banco (vindas da pipeline como variáveis secretas)
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
echo "ACR Name: $ACR_NAME"
echo "DB Server: $DB_SERVER_NAME"
echo "Using credentials from pipeline: $([ -n "$DB_USER" ] && echo 'Yes' || echo 'No')"

# ==================== REGISTRAR PROVIDERS ====================
echo "📂 Registrando providers necessários..."
az provider register --namespace Microsoft.ContainerRegistry
az provider register --namespace Microsoft.ContainerInstance
az provider register --namespace Microsoft.Sql

echo "⏳ Aguardando providers ficarem disponíveis..."
for provider in "Microsoft.ContainerRegistry" "Microsoft.ContainerInstance" "Microsoft.Sql"; do
    echo "Verificando $provider..."
    while [ "$(az provider show --namespace $provider --query "registrationState" -o tsv)" != "Registered" ]; do
        echo "Aguardando $provider ficar disponível..."
        sleep 10
    done
done

echo "✅ Todos os providers estão registrados e disponíveis!"

# ==================== VERIFICAR OU CRIAR RECURSOS ====================
echo "📦 Verificando Resource Group..."
if ! az group show --name $RESOURCE_GROUP >/dev/null 2>&1; then
    echo "Criando Resource Group..."
    az group create --name $RESOURCE_GROUP --location "$LOCATION"
else
    echo "Resource Group já existe."
fi

echo "🐳 Verificando Azure Container Registry..."
if az acr show --name $ACR_NAME --resource-group $RESOURCE_GROUP >/dev/null 2>&1; then
    echo "ACR $ACR_NAME já existe, usando o existente..."
else
    echo "Criando novo ACR: $ACR_NAME"
    az acr create \
        --resource-group $RESOURCE_GROUP \
        --name $ACR_NAME \
        --sku Basic \
        --admin-enabled true
fi

# Verificar se precisa criar SQL Server
CREATE_DB=false
if [[ "$DB_SERVER_NAME" == *"$(date +%s)"* ]] || ! az sql server show --name $DB_SERVER_NAME --resource-group $RESOURCE_GROUP >/dev/null 2>&1; then
    CREATE_DB=true
    echo "🗄️ Criando SQL Server..."
    az sql server create \
        --name $DB_SERVER_NAME \
        --resource-group $RESOURCE_GROUP \
        --location "$LOCATION" \
        --admin-user $DB_ADMIN \
        --admin-password $DB_PASSWORD

    echo "💾 Criando Database..."
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
# A imagem já foi buildada e enviada para o Docker Hub pela pipeline CI
DOCKER_IMAGE="${DOCKER_IMAGE:-correialeo/trackin.dotnet.api:latest}"
echo "🐳 Usando imagem do Docker Hub: $DOCKER_IMAGE"

# Se houver credenciais do Docker Hub configuradas, usar
if [ -n "$DOCKER_USERNAME" ] && [ -n "$DOCKER_PASSWORD" ]; then
    echo "🔐 Credenciais do Docker Hub detectadas"
    USE_DOCKER_HUB=true
else
    echo "ℹ️  Usando imagem pública do Docker Hub (sem autenticação)"
    USE_DOCKER_HUB=false
fi

# Verificar se container já existe e deletar se necessário
if az container show --resource-group $RESOURCE_GROUP --name $ACI_NAME >/dev/null 2>&1; then
    echo "⚠️  Container já existe. Deletando para recriar..."
    az container delete --resource-group $RESOURCE_GROUP --name $ACI_NAME --yes
    sleep 15
fi

echo "📱 Criando Container Instance..."

# Montar comando base
ACI_CREATE_CMD="az container create \
    --resource-group $RESOURCE_GROUP \
    --name $ACI_NAME \
    --image $DOCKER_IMAGE \
    --dns-name-label trackin-api-sprint-$(date +%s) \
    --ports 8080 80 443 \
    --protocol TCP \
    --ip-address Public \
    --environment-variables \
        \"ASPNETCORE_ENVIRONMENT=Production\" \
        \"ASPNETCORE_URLS=http://0.0.0.0:8080\" \
        \"ASPNETCORE_HTTP_PORTS=8080\" \
        \"DOTNET_RUNNING_IN_CONTAINER=true\" \
        \"DATABASE__SOURCE=$DB_SERVER_NAME.database.windows.net\" \
        \"DATABASE__USER=$DB_ADMIN\" \
        \"DATABASE__PASSWORD=$DB_PASSWORD\" \
        \"DATABASE__NAME=$DB_NAME\" \
        \"ConnectionStrings__DefaultConnection=Server=$DB_SERVER_NAME.database.windows.net;Database=$DB_NAME;User Id=$DB_ADMIN;Password=$DB_PASSWORD;TrustServerCertificate=true;Encrypt=true;\" \
    --cpu 1.0 \
    --memory 2.0 \
    --os-type Linux \
    --restart-policy Always"

# Se houver credenciais do Docker Hub, adicionar autenticação
if [ "$USE_DOCKER_HUB" = true ]; then
    echo "🔐 Configurando autenticação com Docker Hub..."
    az container create \
        --resource-group $RESOURCE_GROUP \
        --name $ACI_NAME \
        --image $DOCKER_IMAGE \
        --registry-login-server docker.io \
        --registry-username "$DOCKER_USERNAME" \
        --registry-password "$DOCKER_PASSWORD" \
        --dns-name-label trackin-api-sprint-$(date +%s) \
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
            "ConnectionStrings__DefaultConnection=Server=$DB_SERVER_NAME.database.windows.net;Database=$DB_NAME;User Id=$DB_ADMIN;Password=$DB_PASSWORD;TrustServerCertificate=true;Encrypt=true;" \
        --cpu 1.0 \
        --memory 2.0 \
        --os-type Linux \
        --restart-policy Always
else
    echo "🌐 Usando imagem pública do Docker Hub..."
    az container create \
        --resource-group $RESOURCE_GROUP \
        --name $ACI_NAME \
        --image $DOCKER_IMAGE \
        --dns-name-label trackin-api-sprint-$(date +%s) \
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
            "ConnectionStrings__DefaultConnection=Server=$DB_SERVER_NAME.database.windows.net;Database=$DB_NAME;User Id=$DB_ADMIN;Password=$DB_PASSWORD;TrustServerCertificate=true;Encrypt=true;" \
        --cpu 1.0 \
        --memory 2.0 \
        --os-type Linux \
        --restart-policy Always
fi

echo "⏳ Aguardando container inicializar..."
sleep 45

# ==================== VERIFICAÇÃO E RESULTADOS ====================
echo "🔍 Verificando estado do container..."
CONTAINER_STATE=$(az container show --resource-group $RESOURCE_GROUP --name $ACI_NAME --query "containers[0].instanceView.currentState.state" --output tsv)
echo "Estado do container: $CONTAINER_STATE"

FQDN=$(az container show --resource-group $RESOURCE_GROUP --name $ACI_NAME --query "ipAddress.fqdn" --output tsv)
IP=$(az container show --resource-group $RESOURCE_GROUP --name $ACI_NAME --query "ipAddress.ip" --output tsv)

echo ""
echo "✅ Deploy concluído com sucesso!"
echo ""
echo "📊 Informações da aplicação:"
echo "🌐 URL Swagger: http://$FQDN:8080/swagger"
echo "🌐 URL API: http://$FQDN:8080"
echo "🔢 IP: $IP"
echo "🗄️ Servidor BD: $DB_SERVER_NAME.database.windows.net"
echo "💾 Database: $DB_NAME"
echo "👤 Usuário BD: $DB_ADMIN"
echo ""
echo "🧪 Testes sugeridos:"
echo "curl http://$FQDN:8080/swagger"
echo "curl http://$IP:8080"
echo ""
echo "📋 Comandos úteis:"
echo "# Ver logs:"
echo "az container logs --resource-group $RESOURCE_GROUP --name $ACI_NAME --follow"
echo ""
echo "# Reiniciar:"
echo "az container restart --resource-group $RESOURCE_GROUP --name $ACI_NAME"
echo ""
echo "# Status:"
echo "az container show --resource-group $RESOURCE_GROUP --name $ACI_NAME --query 'containers[0].instanceView.currentState'"

# ==================== TESTE AUTOMÁTICO ====================
echo ""
echo "🧪 Testando conectividade..."
sleep 20

if curl -s --connect-timeout 10 http://$FQDN:8080 >/dev/null 2>&1; then
    echo "✅ API respondendo corretamente!"
    echo "🎉 Deploy concluído e aplicação acessível!"
else
    echo "⚠️  Container pode estar inicializando..."
    echo "💡 Verifique os logs: az container logs --resource-group $RESOURCE_GROUP --name $ACI_NAME"
fi

# Salvar informações para a pipeline
echo "##vso[task.setvariable variable=APP_URL]http://$FQDN:8080"
echo "##vso[task.setvariable variable=APP_FQDN]$FQDN"
echo "##vso[task.setvariable variable=APP_IP]$IP"