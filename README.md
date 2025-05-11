Trackin.API - Sprint 1
======================

Descrição do Projeto
--------------------

O **Trackin.API** é uma API RESTful desenvolvida com ASP.NET Core 8 para automatizar o mapeamento e monitoramento de motocicletas nos pátios da Mottu. Esta solução integra tecnologias como RFID e visão computacional (ML.NET) para localização em tempo real, utilizando uma arquitetura em camadas robusta. A implementação desta primeira sprint foca nos requisitos iniciais:

-   CRUD completo para entidades principais (`Moto`, `Patio`, `SensorRFID`, `ZonaPatio`) com mais de 3 rotas GET parametrizadas.
-   Integração com banco de dados Oracle via Entity Framework Core (EF Core), utilizando migrations para criação de tabelas.
-   Documentação da API via OpenAPI com interface gráfica (Swagger).

O domínio está completamente mapeado com todas as entidades definidas, mas nem todas as rotas definidas foram implementadas até o momento.

Rotas Implementadas
-------------------

Abaixo estão as rotas implementadas nesta primeira sprint, baseadas nos controllers fornecidos. Todas seguem padrões RESTful e retornam os status HTTP apropriados (200 OK, 201 Created, 204 No Content, 400 Bad Request, 404 Not Found, 500 Internal Server Error).

### MotoController

-   **GET /api/moto**\
    Lista todas as motos cadastradas.
-   **GET /api/moto/{id}**\
    Retorna uma moto específica pelo ID.
-   **GET /api/moto/patio/{patioId}**\
    Lista todas as motos de um determinado pátio.
-   **GET /api/moto/status/{status}**\
    Lista todas as motos com um status específico (ex.: Disponível, Em Manutenção).
-   **POST /api/moto**\
    Cria uma nova moto.
-   **PUT /api/moto/{id}**\
    Atualiza uma moto existente.
-   **DELETE /api/moto/{id}**\
    Exclui uma moto pelo ID.
-   **POST /api/moto/{id}/imagem**\
    Adiciona uma imagem base64 como referência para uma moto.

### PatioController

-   **GET /api/patio**\
    Lista todos os pátios cadastrados.
-   **GET /api/patio/{id}**\
    Retorna um pátio específico pelo ID.
-   **POST /api/patio**\
    Cria um novo pátio.
-   **DELETE /api/patio/{id}**\
    Remove um pátio existente.

### RFIDController

-   **POST /api/rfid**\
    Processa uma leitura RFID e atualiza a localização/status da moto.

### SensorRFIDController

-   **GET /api/sensorRFID**\
    Lista todos os sensores RFID cadastrados.
-   **GET /api/sensorRFID/{id}**\
    Retorna um sensor RFID específico pelo ID.
-   **POST /api/sensorRFID**\
    Cria um novo sensor RFID.
-   **PUT /api/sensorRFID/{id}**\
    Atualiza um sensor RFID existente.
-   **DELETE /api/sensorRFID/{id}**\
    Remove um sensor RFID.

### ZonaPatioController

-   **GET /api/zonaPatio**\
    Lista todas as zonas de pátio cadastradas.
-   **GET /api/zonaPatio/{id}**\
    Retorna uma zona de pátio específica pelo ID.
-   **POST /api/zonaPatio**\
    Cria uma nova zona de pátio.
-   **PUT /api/zonaPatio/{id}**\
    Atualiza uma zona de pátio existente.
-   **DELETE /api/zonaPatio/{id}**\
    Remove uma zona de pátio.

Instalação
----------

Siga os passos abaixo para configurar e executar o projeto localmente:

### Pré-requisitos

-   **.NET 8 SDK**: [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
-   **Oracle Database**: Um servidor Oracle configurado e acessível.
-   **Git**: Para clonar o repositório.

### Passos de Instalação

1.  **Clone o Repositório**

    ```
    git clone https://github.com/seu-usuario/trackin-api.git
    cd trackin-api

    ```

2.  **Configure as Variáveis de Ambiente**

    -   Copie o arquivo `.env.example` para `.env` na raiz do projeto:

        ```
        cp .env.example .env

        ```

    -   Edite o arquivo `.env` com as credenciais do seu banco de dados Oracle:

        ```
        DATABASE__SOURCE='seu_servidor_oracle'  # Exemplo: localhost:1521/orcl
        DATABASE__USER='seu_usuario'
        DATABASE__PASSWORD='sua_senha'

        ```

3.  **Restaure as Dependências**

    -   Execute o comando para restaurar os pacotes NuGet:

        ```
        dotnet restore

        ```

4.  **Configure a Conexão com o Banco de Dados**

    -   Certifique-se de que o servidor Oracle está ativo e acessível usando as credenciais fornecidas no `.env`.
    -   Teste a conexão com o banco, se necessário, utilizando uma ferramenta como SQL Developer ou DBeaver, para garantir que o usuário especificado tem permissões adequadas (ex.: criar tabelas, inserir dados).
5.  **Aplique as Migrations**

    -   Após confirmar que a conexão com o banco está funcionando, aplique as migrations para criar as tabelas no banco de dados Oracle:

        ```
        dotnet ef database update

        ```

    -   Se houver erros, verifique as configurações no `.env`, a string de conexão no `appsettings.json` (se aplicável), e as permissões do usuário no banco.

6.  **Execute a Aplicação**

    -   Inicie o projeto:

        ```
        dotnet run

        ```

    -   A API estará disponível em `https://localhost:5007` (ou a porta configurada).

7.  **Acesse a Documentação Swagger**

    -   Ao compilar o projeto, automaticamente ele irá para `https://localhost:5007/swagger` para explorar e testar os endpoints. (Caso esteja no Visual Studio)

### Observações

-   Verifique se a porta padrão não está em uso.
-   Certifique-se de que o usuário do banco de dados tem permissões para criar tabelas e executar as operações necessárias.

Notas Adicionais
----------------

-   Esta é a implementação da primeira sprint, atendendo aos requisitos mínimos de CRUD, integração com Oracle via EF Core, e documentação Swagger.
-   Nem todas as rotas previstas na arquitetura estão implementadas; o foco foi nos controllers listados acima.
