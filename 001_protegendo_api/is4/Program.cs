var builder = WebApplication.CreateBuilder(args);

// Configurações do Identity Server
builder.Services
	.AddIdentityServer(
		options =>
		{
			options.Events.RaiseErrorEvents = true;
			options.Events.RaiseInformationEvents = true;
			options.Events.RaiseFailureEvents = true;
			options.Events.RaiseSuccessEvents = true;
			options.EmitStaticAudienceClaim = true;
		}
	)

	// certificado de assinatura temporário usado para assinar tokens
	// Desenvolvimento
	.AddDeveloperSigningCredential()
	// Produção
	//.AddSigningCredential()

	.AddInMemoryIdentityResources(Config.IdentityResources)
	.AddInMemoryApiResources(Config.ApiResources)
	.AddInMemoryApiScopes(Config.ApiScopes)
	.AddInMemoryClients(Config.Clients)

	// 002_user_password
	.AddTestUsers(Config.Users);

// ativa os controladores com visualizadores(views)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ativa o https
app.UseHttpsRedirection();

// habilita os arquivos da pasta wwwroot
app.UseStaticFiles();

// ativa as rotas
app.UseRouting();

// Ativa o Identity Server
app.UseIdentityServer();

// [autorize] será verificado antes de ser executado
app.UseAuthorization();

// ativa a rota padrão
app.UseEndpoints(endpoints =>
{
	endpoints.MapDefaultControllerRoute();
});

//app.MapGet("/", () => "Hello World!");

app.Run();
