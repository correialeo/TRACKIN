#!/bin/bash
# deploy-trackin.sh - Script completo de deploy da Trackin API

set -e

echo "🚀 Iniciando deploy da Trackin API no Azure..."

# ==================== CONFIGURAÇÕES ====================
RESOURCE_GROUP="rg-trackin-sprint"
LOCATION="West US"
# Gerar nome mais único para ACR
RANDOM_SUFFIX=$(openssl rand -hex 4)
ACR_NAME="acrtrackin${RANDOM_SUFFIX}"
ACI_NAME="aci-trackin-api"
DB_SERVER_NAME="sqlserver-trackin-$(date +%s)"
DB_NAME="trackin_db"
DB_ADMIN="adminuser"
DB_PASSWORD="Trackin@123!"

echo "📋 Configurações:"
echo "Resource Group: $RESOURCE_GROUP"
echo "ACR Name: $ACR_NAME"
echo "DB Server: $DB_SERVER_NAME"

# ==================== REGISTRAR PROVIDERS ====================
echo "📂 Registrando providers necessários..."
az provider register --namespace Microsoft.ContainerRegistry
az provider register --namespace Microsoft.ContainerInstance
az provider register --namespace Microsoft.Sql

echo "⏳ Aguardando providers ficarem disponíveis..."
echo "Verificando Microsoft.ContainerRegistry..."
while [ "$(az provider show --namespace Microsoft.ContainerRegistry --query "registrationState" -o tsv)" != "Registered" ]; do
    echo "Aguardando Microsoft.ContainerRegistry ficar disponível..."
    sleep 10
done

echo "Verificando Microsoft.ContainerInstance..."
while [ "$(az provider show --namespace Microsoft.ContainerInstance --query "registrationState" -o tsv)" != "Registered" ]; do
    echo "Aguardando Microsoft.ContainerInstance ficar disponível..."
    sleep 10
done

echo "✅ Todos os providers estão registrados e disponíveis!"

# ==================== LOGIN AZURE ====================
echo "🔐 Fazendo login no Azure..."
az login
echo "📦 Criando Resource Group..."
az group create \
    --name $RESOURCE_GROUP \
    --location "$LOCATION"

echo "🐳 Criando Azure Container Registry..."
# Primeiro verificar se já existe ACR com esse nome
if az acr show --name $ACR_NAME --resource-group $RESOURCE_GROUP >/dev/null 2>&1; then
    echo "ACR $ACR_NAME já existe, usando o existente..."
else
    echo "Criando novo ACR: $ACR_NAME"
    az acr create \
        --resource-group $RESOURCE_GROUP \
        --name $ACR_NAME \
        --sku Basic \
        --admin-enabled true
    
    # Verificar se foi criado com sucesso
    if ! az acr show --name $ACR_NAME --resource-group $RESOURCE_GROUP >/dev/null 2>&1; then
        echo "❌ Erro ao criar ACR. Tentando com nome alternativo..."
        ACR_NAME="acrtrackin$(date +%s)x"
        echo "Tentando criar ACR: $ACR_NAME"
        az acr create \
            --resource-group $RESOURCE_GROUP \
            --name $ACR_NAME \
            --sku Basic \
            --admin-enabled true
    fi
fi

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

# ==================== BUILD E PUSH ====================
echo "🏗️ Fazendo login no ACR..."
az acr login --name $ACR_NAME

echo "📦 Fazendo build da imagem..."
# Build a partir da pasta src/Trackin.Api onde está o Dockerfile
docker build \
  --build-arg DATABASE__SOURCE="$DB_SERVER_NAME.database.windows.net" \
  --build-arg DATABASE__USER="$DB_ADMIN" \
  --build-arg DATABASE__PASSWORD="$DB_PASSWORD" \
  --build-arg DATABASE__NAME="$DB_NAME" \
  -t $ACR_NAME.azurecr.io/trackin-api:latest \
  -f src/Trackin.Api/Dockerfile \
  src/

echo "⬆️ Fazendo push da imagem..."
docker push $ACR_NAME.azurecr.io/trackin-api:latest

# ==================== DEPLOY CONTAINER ====================
echo "🚀 Obtendo credenciais do ACR..."
ACR_PASSWORD=$(az acr credential show --name $ACR_NAME --query "passwords[0].value" --output tsv)

echo "📱 Criando Container Instance..."
az container create \
    --resource-group $RESOURCE_GROUP \
    --name $ACI_NAME \
    --image $ACR_NAME.azurecr.io/trackin-api:latest \
    --registry-login-server $ACR_NAME.azurecr.io \
    --registry-username $ACR_NAME \
    --registry-password $ACR_PASSWORD \
    --dns-name-label trackin-api-sprint \
    --ports 8080 \
    --environment-variables \
        "DATABASE__SOURCE=$DB_SERVER_NAME.database.windows.net" \
        "DATABASE__USER=$DB_ADMIN" \
        "DATABASE__PASSWORD=$DB_PASSWORD" \
        "DATABASE__NAME=$DB_NAME" \
    --cpu 1 \
    --memory 1 \
    --os-type Linux

# ==================== RESULTADOS ====================
echo ""
echo "✅ Deploy concluído com sucesso!"
echo ""
echo "📊 Informações da aplicação:"
FQDN=$(az container show --resource-group $RESOURCE_GROUP --name $ACI_NAME --query "ipAddress.fqdn" --output tsv)
echo "🌐 URL da API: http://$FQDN:8080"
echo "🗄️ Servidor BD: $DB_SERVER_NAME.database.windows.net"
echo "💾 Database: $DB_NAME"
echo "👤 Usuário BD: $DB_ADMIN"
echo ""
echo "🧪 Testes sugeridos:"
echo "curl http://$FQDN:8080/swagger"
echo "curl http://$FQDN:8080/api/seus-controllers"
echo ""
echo "📋 Para ver logs do container:"
echo "az container logs --resource-group $RESOURCE_GROUP --name $ACI_NAME"
echo ""
echo "🧹 Para limpar recursos (CUIDADO!):"
echo "az group delete --name $RESOURCE_GROUP --yes --no-wait"