using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services
	.AddAuthentication(
		options =>
		{
			options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
		}
	)
	.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddOpenIdConnect(
		OpenIdConnectDefaults.AuthenticationScheme,
		options =>
		{
			options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			options.SignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
			options.Authority = "https://localhost:5001";
			options.ClientId = "mvc";
			options.ClientSecret = "secret";
			options.ResponseType = "code";
			options.SaveTokens = true;
			options.GetClaimsFromUserInfoEndpoint = true;
		}
	);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
