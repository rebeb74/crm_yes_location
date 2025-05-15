using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using YesLocation.Api.Middlewares;
using YesLocation.Api.Services;
using YesLocation.Domain.Interfaces;
using YesLocation.Infrastructure.Persistence;
using YesLocation.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors((options) =>
{
    Console.WriteLine("Configuring CORS policies...");

    options.AddPolicy("DevCors", (coreBuilder) =>
    {
        Console.WriteLine("Configuring DevCors policy");
        coreBuilder.WithOrigins(
                "http://localhost:4200",      // Angular dev server
                "http://localhost:5000",      // HTTP
                "https://localhost:5000",     // HTTPS
                "http://localhost:5001",
                "https://localhost:5001")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });

    options.AddPolicy("ProdCors", (coreBuilder) =>
    {
        Console.WriteLine("Configuring ProdCors policy");
        coreBuilder.WithOrigins("https://yes-location.codeattila.ch")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

string? tokenKeyString = builder.Configuration["Jwt:TokenKey"];

string? issuerString = builder.Configuration["Jwt:Issuer"];
string? audienceString = builder.Configuration["Jwt:Audience"];

// Vérifiez que les paramètres ne sont pas vides
if (string.IsNullOrEmpty(tokenKeyString) || string.IsNullOrEmpty(issuerString) || string.IsNullOrEmpty(audienceString))
{
    throw new Exception("Les paramètres JWT (Issuer, Audience, TokenKey) ne sont pas correctement configurés dans appsettings.json.");
}

// Enregistrement des services
builder.Services.AddScoped<IEnvironmentService, EnvironmentService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Configuration du DbContext avec MySQL
builder.Services.AddDbContext<YesLocationDbContext>((serviceProvider, options) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    options.UseMySql(connectionString,
        new MySqlServerVersion(new Version(8, 0, 36)),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure(
            maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)
    );

    var env = serviceProvider.GetService<IEnvironmentService>();
    if (env?.IsDevelopment() ?? false)
    {
        options
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    }
});

// Configuration de l'authentification JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuerString,
            ValidAudience = audienceString,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKeyString ?? "")),
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin"));
});

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "YesLocation API", Version = "v1" });

    // Configurer l'authentification JWT dans Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
});
});

// Ajout des contrôleurs
builder.Services.AddControllers();

// Créer l'application
var app = builder.Build();

// Exécuter le seeding au démarrage
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DatabaseSeeder.SeedAsync(services);
}

// Configure the HTTP request pipeline.
Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");

// Déplacer UseCors tout au début du pipeline
if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Using DevCors policy");
    app.UseCors("DevCors");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    Console.WriteLine("Using ProdCors policy");
    app.UseCors("ProdCors");
}

// Ajouter un middleware pour logger les en-têtes CORS
app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        Console.WriteLine("CORS Headers:");
        foreach (var header in context.Response.Headers)
        {
            Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
        }
        return Task.CompletedTask;
    });
    await next();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<CurrentUserMiddleware>();

app.MapControllers();

app.Run();
