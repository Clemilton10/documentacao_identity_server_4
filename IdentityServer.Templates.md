# IdentityServer.Templates

> [Início](./README.md)

### Duende Software

[https://github.com/duendesoftware](https://github.com/duendesoftware)

### IdentityServer.Templates

[https://github.com/DuendeSoftware/IdentityServer.Templates](https://github.com/DuendeSoftware/IdentityServer.Templates)

### Documentação

[https://docs.duendesoftware.com/identityserver/v6/samples/](https://docs.duendesoftware.com/identityserver/v6/samples/)

Modelos da CLI do .NET para o Duende IdentityServer

### dotnet new isempty

Cria um projeto Duende IdentityServer mínimo sem uma interface do usuário.

### dotnet new isui

Adiciona a interface do usuário de início rápido ao projeto atual (pode ser adicionada, por exemplo, sobre isempty)

### dotnet new isinmem

Adiciona um Duende IdentityServer básico com interface do usuário, usuários de teste e clientes e recursos de exemplo.

### dotnet new isaspid

Adiciona um Duende IdentityServer básico que usa ASP.NET Identity para gerenciamento de usuários. Se você semear automaticamente o banco de dados, você obterá dois usuários: e - ambos com senha `alice` `bob` `Pass123$`. Verifique o arquivo `SeedData.cs`.

### dotnet new isef

Adiciona um Duende IdentityServer básico que usa o Entity Framework para configuração e gerenciamento de estado. Se você propagar o banco de dados, obterá alguns registros básicos de clientes e recursos, verifique o arquivo `SeedData.cs`.

### dotnet new bff-remoteapi

Cria um host BFF básico baseado em JavaScript que configura e invoca uma API remota por meio do proxy BFF.

### dotnet new bff-localapi

Cria um host BFF básico baseado em JavaScript que invoca uma API local co-hospedada com o BFF.

### Instalação

Instale com:

.NET 6 SDK: `dotnet new install Duende.IdentityServer.Templates`

.NET 7 SDK: `dotnet new install Duende.IdentityServer.Templates`

Se você precisar definir sua nova lista dotnet para <b>padrões de fábrica</b>, use este comando:

```sh
dotnet new --debug:reinit
```

Para desinstalar os modelos, use

.NET 6 SDK: `dotnet new -u Duende.IdentityServer.Templates`

.NET 7 SDK: `dotnet new uninstall Duende.IdentityServer.Templates`

> [Início](./README.md)
