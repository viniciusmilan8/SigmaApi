# Sigma - Sistema de Gerenciamento de Projetos

Projeto acadêmico com o objetivo de desenvolver uma aplicação web para **gerenciamento de projetos**, utilizando autenticação segura com **JWT (JSON Web Token)** e controle de acesso por **perfis de usuário (Admin e Leitor)**.

---

## 🚀 Funcionalidades

- 🔐 Autenticação via JWT
- 👥 Perfis de usuário com controle de permissões (`Admin`, `Leitor`)
- 📋 CRUD de Projetos
- 📌 Regras de negócio implementadas (ex: não reabrir projeto encerrado)
- 🧾 API estruturada em camadas com boas práticas de arquitetura

---

## 🛠️ Tecnologias Utilizadas

| Tecnologia            | Descrição                                              |
|-----------------------|--------------------------------------------------------|
| ASP.NET Core 8.0      | Backend da aplicação (API REST)                       |
| Entity Framework Core | ORM para acesso e manipulação do banco de dados       |
| PostgreSQL            | Banco de dados relacional                             |
| JWT Bearer            | Autenticação segura baseada em tokens                 |
| AutoMapper            | Mapeamento de objetos entre DTOs e Entidades          |
| Swagger               | Documentação e testes interativos da API              |

---

## 🧑‍💻 Perfis de Acesso

| Perfil   | Permissões                                            |
|----------|--------------------------------------------------------|
| `Admin`  | Pode cadastrar, editar, excluir e listar projetos     |
| `Leitor` | Pode apenas visualizar a lista de projetos (`GET`)    |

---

## 🔐 Autenticação

A autenticação é feita via JWT:

1. Realize o login via `POST /api/Autenticacao/login`
2. Receba o token JWT no retorno
3. Envie o token em todas as requisições autenticadas no cabeçalho:
