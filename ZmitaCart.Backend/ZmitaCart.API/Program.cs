using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ZmitaCart.API.Common;
using ZmitaCart.API.Services;
using ZmitaCart.Application;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Application.Interfaces.Services;
using ZmitaCart.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var settings = new GlobalSettings();
builder.Configuration.Bind(GlobalSettings.sectionName, settings);
builder.Services.AddSingleton(Options.Create(settings));

var emailServiceSettings = new EmailServiceSettings();
builder.Configuration.Bind(EmailServiceSettings.sectionName, emailServiceSettings);
builder.Services.AddSingleton(Options.Create(emailServiceSettings));

builder.Services.AddControllers();
//builder.Services.AddSwaggerGen(options =>
//{
//	options.AddSecurityDefinition("auth", new OpenApiSecurityScheme
//	{
//		Type = SecuritySchemeType.Http,
//		In = ParameterLocation.Header,
//		Scheme = JwtBearerDefaults.AuthenticationScheme,
//		Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
//		Name = "Authorization",
//	});
//	options.AddSecurityRequirement(new OpenApiSecurityRequirement
//	{
//		{
//			new OpenApiSecurityScheme
//			{
//				Reference = new OpenApiReference
//				{
//					Type = ReferenceType.SecurityScheme,
//					Id = "auth"
//				}
//			},
//			Array.Empty<string>()
//		}
//	});
//	options.DescribeAllParametersInCamelCase();
//});
builder.Services.AddHealthChecks()
	.AddSqlServerCheck(builder.Configuration.GetConnectionString(settings.ApplicationDbName), "Database");

builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration, settings.ApplicationDbName);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IEmailServiceProvider, EmailServiceProvider>();

const string corsApp = "corsapp";
builder.Services.AddCors(options => options.AddPolicy(corsApp, corsBuilder =>
	corsBuilder.WithOrigins(settings.Origin).AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

if (builder.Environment.IsDevelopment())
{
	//builder.Services.AddScoped<ICurrentUserService, FakeCurrentUserService>();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	// app.UseSwagger();
	// app.UseSwaggerUI();
}

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();
await seeder.Seed();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseCors(corsApp);
app.UseAuthentication();
app.UseRouting();
app.MapHealthChecks(settings.HealthCheckUrl, new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
app.UseAuthorization();
app.MapControllers();

app.Run();