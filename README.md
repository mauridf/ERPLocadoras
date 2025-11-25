# ERP Locadoras - API Documentation

## Sobre o Projeto

Sistema ERP robusto para gestão de locadoras de veículos, baseado em
.NET 10, arquitetura em camadas e multi‑tenancy.

## Funcionalidades

-   Multi‑tenancy com isolamento total de dados\
-   Autenticação JWT\
-   Controle de usuários (Global, Admin, Atendente, Mecânico, Cliente)\
-   Gestão de frota\
-   Locações completas\
-   Manutenções preventivas e corretivas\
-   Dashboards e relatórios

## Tecnologias

-   **Backend:** .NET 10, C# 11\
-   **Banco:** SQL Server\
-   **ORM:** EF Core\
-   **Auth:** JWT + BCrypt\
-   **Docs:** Swagger\
-   **Arquitetura:** Clean Architecture + DDD

## Estrutura

    ERPLocadoras/
    ├── ERPLocadoras.API/
    ├── ERPLocadoras.Application/
    ├── ERPLocadoras.Core/
    ├── ERPLocadoras.Infra.Data/
    └── ERPLocadoras.sln

## Execução

### Requisitos

.NET 10 SDK\
SQL Server

### Configuração

Clone:

    git clone <repository-url>
    cd ERP-Locadoras

Ajuste a connection string no `appsettings.json`.

### Migrations

    cd ERPLocadoras.API
    dotnet ef database update

### Executar

    dotnet run

Acesso:\
- `https://localhost:5001`\
- `http://localhost:5000`\
- Swagger: `/swagger`

Usuário inicial: **admin@erplocadoras.com / Admin123!**

## Sistema de Autenticação
Tipos de Usuário
-   Global - Acesso total ao sistema
-   Admin - Administrador de uma locadora
-   Atendente - Gestão de locações
-   Mecânico - Controle de manutenções
-   Cliente - Acesso ao portal do cliente

## Fluxos

### Locação

Reserva → Checklist Entrega → Ativa → Checklist Devolução → Finalizada

### Manutenção

Abertura → Em andamento → Concluída → Atualização do veículo

## Modelo de Dados

-   Locadora\
-   Usuario\
-   Cliente\
-   Veiculo\
-   Locacao\
-   Manutencao

## Segurança

-   JWT com expiração\
-   Hash BCrypt\
-   Multi‑tenancy\
-   Validações e logs

## Contribuição

Fork → Branch → Commit → PR