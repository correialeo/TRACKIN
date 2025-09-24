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