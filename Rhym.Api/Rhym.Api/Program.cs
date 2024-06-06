using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Rhym.Api;
using Rhym.Api.Data;
using Rhym.Api.Services;

var AllOrigins = "AllOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
	options.AddPolicy(name: AllOrigins, policy =>
	{
		policy.WithOrigins("http://localhost:3000" /*, "STATIC-WEB-APP" */);
		policy.AllowAnyMethod();
		policy.AllowAnyHeader();
		policy.AllowCredentials();
	});
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();
builder.Services.AddSwaggerGen(config =>
{
	config.AddSignalRSwaggerGen();
	config.SwaggerDoc("v1", new OpenApiInfo { Title = "Rhym API", Version = "v1" });
	config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.Http,
		Scheme = "Bearer"
	});
	config.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new List<string>()
		}
	});
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Unable to connect to 'DefaultConnection'");
builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(connectionString);
});
builder.Services.AddScoped<DocumentService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<WordService>();
builder.Services.AddScoped<RhymHub>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	db.Database.Migrate();
	await Seeder.Seed(db);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(AllOrigins);

app.UseAuthorization();

app.MapControllers();

app.UseRouting();

app.MapHub<RhymHub>("hub");

app.Run();

public partial class Program { }