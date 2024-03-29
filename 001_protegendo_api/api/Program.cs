using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddAuthentication("Bearer")
	.AddIdentityServerAuthentication(
		"Bearer",
		options =>
		{
			options.ApiName = "myApi";
			options.Authority = "https://localhost:5001";
			// Permite sem https
			// options.RequireHttpsMetadata = false;
		}
	);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Authorization do Swagger
builder.Services.AddSwaggerGen(option =>
{
	option.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
	option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter a valid token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "Bearer"
	});
	option.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type=ReferenceType.SecurityScheme,
					Id="Bearer"
				}
			},
			new string[]{}
		}
	});
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
