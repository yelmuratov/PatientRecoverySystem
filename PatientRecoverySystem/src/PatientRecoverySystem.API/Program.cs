using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PatientRecoverySystem.Infrastructure.Data;
using PatientRecoverySystem.Application.Interfaces;
using PatientRecoverySystem.Infrastructure.Services;
using PatientRecoverySystem.Domain.Interfaces;
using PatientRecoverySystem.Infrastructure.Repositories;
using PatientRecoverySystem.Application.Mappings;
using Microsoft.AspNetCore.Identity;
using PatientRecoverySystem.Domain.Entities;
using MassTransit;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// =========================
// Configure Services
// =========================

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IRehabilitationRepository, RehabilitationRepository>();
builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();

// Services
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<ITokenBlacklistService, TokenBlacklistService>();
builder.Services.AddScoped<IRecoveryLogService, RecoveryLogService>();
builder.Services.AddScoped<IRehabilitationService, RehabilitationService>();
builder.Services.AddScoped<IConsultationService, ConsultationService>();
builder.Services.AddScoped<IGeminiService, GeminiService>();
builder.Services.AddHttpClient<IGeminiService, GeminiService>();

// Password hashing
builder.Services.AddScoped<IPasswordHasher<Doctor>, PasswordHasher<Doctor>>();
builder.Services.AddScoped<IPasswordHasher<Patient>, PasswordHasher<Patient>>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

            NameClaimType = ClaimTypes.NameIdentifier,
            RoleClaimType = ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var tokenBlacklistService = context.HttpContext.RequestServices.GetRequiredService<ITokenBlacklistService>();
                var accessToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (tokenBlacklistService.IsTokenBlacklisted(accessToken))
                {
                    context.Fail("Token has been revoked.");
                }

                return Task.CompletedTask;
            }
        };
    });

// MassTransit with RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqHost = Environment.GetEnvironmentVariable("RabbitMQ__Host") ?? "localhost";

        cfg.Host(rabbitMqHost, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.UseRetry(retryConfig =>
        {
            retryConfig.Interval(10, TimeSpan.FromSeconds(5));
        });
    });
});

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "PatientRecoverySystem.API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// CORS -- PRODUCTION CONFIG: ONLY ALLOW frontend domain curevia.tech
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("https://curevia.tech", "https://my.curevia.tech", "https://www.curevia.tech", "http://164.92.252.187:4200", "http://localhost:4200", "https://164.92.252.187:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// =========================
// Middleware Pipeline (ORDER IS CRITICAL)
// =========================

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PatientRecoverySystem.API v1");
        c.RoutePrefix = "swagger";
        c.ConfigObject.AdditionalItems["url"] = "/swagger/v1/swagger.json";
    });
}

app.UseMiddleware<PatientRecoverySystem.API.Middlewares.ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseRouting(); // ✅ Required for correct CORS processing

app.UseCors("AllowFrontend");  // ✅ Must be AFTER routing, BEFORE authentication

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    DbInitializer.Initialize(context);
}

app.Run();
