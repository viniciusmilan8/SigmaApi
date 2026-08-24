# SigmaApi

API REST de gerenciamento de projetos em ASP.NET Core 8, com autenticação JWT e autorização por perfil.

> **Projeto acadêmico.** Foi escrito como exercício de arquitetura em camadas e não roda em produção. As limitações conhecidas estão documentadas no fim deste README.

## O que faz

Cadastro e autenticação de usuários, e um CRUD de projetos com controle de acesso por perfil:

| Perfil   | Permissões                                  |
|----------|---------------------------------------------|
| `Admin`  | Criar, alterar, excluir e listar projetos   |
| `Leitor` | Apenas listar projetos                      |

Um projeto percorre oito estados (`EmAnalise` → `AnaliseRealizada` → `AnaliseAprovada` → `Iniciado` → `Planejado` → `EmAndamento` → `Encerrado` / `Cancelado`), e a camada de aplicação impede reabrir um projeto já encerrado.

## Stack

- **ASP.NET Core 8** — API REST com controllers
- **Entity Framework Core 8** + **Npgsql** — persistência e migrations
- **PostgreSQL** — banco relacional
- **JWT Bearer** — autenticação stateless
- **AutoMapper** — mapeamento entidade ↔ DTO
- **Swagger / Swashbuckle** — documentação interativa

## Arquitetura

Separação em camadas:

```
Sigma.API                      → controllers, pipeline, autenticação
Sigma.Application              → serviços, DTOs, regras de negócio
Sigma.Domain                   → entidades, enums, contratos de repositório
Sigma.Infra.Data               → EF Core: contexto, repositórios, migrations
Sigma.Infra.CrossCutting.IoC   → registro de dependências
Sigma.Infra.CrossCutting       → sem código (ver limitações)
```

`Sigma.Domain` é o núcleo e não referencia nenhum outro projeto — seus arquivos importam apenas `System.*`. As interfaces de repositório vivem nele e são implementadas em `Sigma.Infra.Data`, de forma que as entidades e contratos não dependem do EF Core.

A inversão, porém, não está completa: `Sigma.Application` referencia `Sigma.Infra.Data` diretamente, em vez de receber as implementações apenas via injeção de dependência no composition root. Ver limitações.

## Como rodar

**Pré-requisitos:** [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0) e PostgreSQL.

```bash
git clone https://github.com/viniciusmilan8/SigmaApi.git
cd SigmaApi
```

Nenhum segredo vem versionado. Configure os dois via User Secrets:

```bash
cd Sigma.API

dotnet user-secrets set "ConnectionStrings:Database" \
  "Server=localhost;Port=5432;Database=sigma;Userid=postgres;Password=<sua-senha>"

# Chave de 32+ bytes. Para gerar uma:
#   openssl rand -hex 32
dotnet user-secrets set "JwtSettings:SecretKey" "<sua-chave>"
```

A aplicação valida as duas na inicialização e falha com mensagem explícita se faltar alguma.

Aplique as migrations e suba:

```bash
dotnet ef database update --project ../Sigma.Infra.Data --startup-project .
dotnet run
```

Swagger em `https://localhost:<porta>/swagger`.

## Endpoints

| Método   | Rota                          | Acesso          |
|----------|-------------------------------|-----------------|
| `POST`   | `/api/Autenticacao/registrar` | Público         |
| `POST`   | `/api/Autenticacao/login`     | Público         |
| `GET`    | `/api/Projeto`                | `Admin`,`Leitor`|
| `POST`   | `/api/Projeto/inserir`        | `Admin`         |
| `PATCH`  | `/api/Projeto/alterar`        | `Admin`         |
| `DELETE` | `/api/Projeto/deletar`        | `Admin`         |

O login devolve um JWT que deve ir no header das rotas protegidas:

```
Authorization: Bearer <token>
```

## Limitações conhecidas e próximos passos

O projeto é acadêmico e não passou por endurecimento de segurança. O que eu mudaria antes de levar algo assim a produção:

- **`/registrar` é público e aceita o campo `Tipo`** — qualquer requisição pode criar um usuário `Admin`. Em um sistema real, ou a rota exige um `Admin` autenticado, ou o perfil é atribuído fora do payload do cliente.
- **Senhas usam SHA-256 sem salt** — hash rápido e sem salt é vulnerável a rainbow table. O correto é um algoritmo de derivação lento como BCrypt, PBKDF2 ou Argon2.
- **`Sigma.Application` referencia `Sigma.Infra.Data`** — a camada de aplicação enxerga a de infraestrutura, quebrando a regra de dependência. O acoplamento deveria se resolver só pelas interfaces do domínio, com o registro concreto feito no `Sigma.Infra.CrossCutting.IoC`.
- **Resíduos estruturais** — `Sigma.Infra.CrossCutting` está na solution sem nenhum código, e o contrato genérico `IRepository<T>` ficou quase todo comentado, sem ninguém implementando. Ambos deveriam sair.
- **Sem testes automatizados** — as regras de negócio (a de não reabrir projeto encerrado, principalmente) são testáveis e deveriam ter cobertura.
- **`AutoMapper 13.0.1` tem vulnerabilidade conhecida** ([GHSA-rvv3-g6hj-g44x](https://github.com/advisories/GHSA-rvv3-g6hj-g44x)); a correção exige subir para a linha 14+, que tem quebras de API.
