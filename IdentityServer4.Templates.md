# IdentityServer4.Templates

> [Início](./README.md)

[https://github.com/IdentityServer/IdentityServer4.Templates?tab=readme-ov-file](https://github.com/IdentityServer/IdentityServer4.Templates?tab=readme-ov-file)

### Atualização importante

Esse projeto não se mantém mais. Esse repositório será arquivado quando o fim do suporte do `.NET Core 3.1 for atingido (13 de dezembro de 2022)`. Todas as novidades estão acontecendo na nova organização [Duende Software](https://github.com/duendesoftware).

### dotnet new is4empty

Cria um projeto IdentityServer4 mínimo sem uma interface do usuário.

### dotnet new is4ui

Adiciona a interface do usuário de início rápido ao projeto atual (pode ser adicionada, por exemplo, sobre is4empty)

### dotnet new is4inmem

Adiciona um IdentityServer básico com interface do usuário, usuários de teste e clientes e recursos de exemplo. Mostra o código na memória e a configuração JSON.

### dotnet new is4aspid

Adiciona um IdentityServer básico que usa ASP.NET Identity para gerenciamento de usuários. Se você semear automaticamente o banco de dados, você obterá dois usuários: e - ambos com senha `alice` `bob` `Pass123$`. Verifique o arquivo `SeedData.cs`.

### dotnet new is4ef

Adiciona um IdentityServer básico que usa o Entity Framework para configuração e gerenciamento de estado. Se você propagar o banco de dados, obterá alguns registros básicos de clientes e recursos, verifique o arquivo `SeedData.cs`.

### dotnet new is4admin

Adiciona um IdentityServer que inclui o Rock Solid Knowledge AdminUI Community Edition (aberto no navegador). Isso oferece uma interface de administração baseada na Web para usuários, declarações, clientes e recursos. `http://localhost:5000/admin`

A edição da comunidade destina-se a testar cenários de integração do IdentityServer e está limitada a `localhost:5000`, SQLite, 10 usuários e 2 clientes. A edição comunitária não é adequada para produção.

Consulte identityserver.com para obter mais informações sobre AdminUI ou para solicitar uma licença de avaliação.

### Instalação

Instale com:

```sh
dotnet new -i identityserver4.templates
```

Desinstale com:

```sh
dotnet new -u identityserver4.templates
```

Se você precisar definir sua nova lista dotnet para "padrões de fábrica", use este comando:

```sh
dotnet new --debug:reinit
```

> [Início](./README.md)
