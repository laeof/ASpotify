using ASpotifyAuth.Consumers;
using ASpotifyAuth.Domain;
using ASpotifyAuth.Domain.Entities;
using ASpotifyAuth.Domain.Repository.Abstract;
using ASpotifyAuth.Domain.Repository.Entities;
using ASpotifyAuth.Services.Abstract;
using ASpotifyAuth.Services.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("ASPNET_SECRETKEYSPOTIFY")!)),
    };
});

builder.Services.AddHttpContextAccessor();

//rabbit

builder.Services.AddSingleton<IConnectionFactory>(cf =>
{
    return new ConnectionFactory()
    {
        HostName = Environment.GetEnvironmentVariable("ASPNETCORE_ASPOTIFY_RABBITMQ_HOSTNAME"),
        UserName = Environment.GetEnvironmentVariable("ASPNETCORE_ASPOTIFY_RABBITMQ_USERNAME"),
        Password = Environment.GetEnvironmentVariable("ASPNETCORE_ASPOTIFY_RABBITMQ_PASSWORD"),
        VirtualHost = "/"
    };
});

builder.Services.AddScoped<IMessageProducer, MessageProducer>();
builder.Services.AddSingleton<UserConsumer>();

//custom services

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICRUDRepository<User>, CRUDRepository<User>>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<ICRUDRepository<UserRefreshToken>, CRUDRepository<UserRefreshToken>>();

builder.Services.AddScoped<IJWTService, JWtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();

builder.Services.AddScoped<DataManager>();

//builder.Services.AddDbContext<ASpotifyDbContext>(x => x.UseNpgsql(
//    $"Host={Environment.GetEnvironmentVariable("ASPNETCORE_ASPOTIFY_DB_SERVER")};" +
//    $"Port={Environment.GetEnvironmentVariable("ASPNETCORE_ASPOTIFY_DB_PORT")};" +
//    $"Username={Environment.GetEnvironmentVariable("ASPNETCORE_ASPOTIFY_DB_USER")};" +
//    $"Password={Environment.GetEnvironmentVariable("ASPNETCORE_ASPOTIFY_DB_PASS")};" +
//    $"Database={Environment.GetEnvironmentVariable("ASPNETCORE_ASPOTIFY_DB_NAME")};"));

builder.Services.AddDbContext<ASpotifyDbContext>(x => x.UseNpgsql("Host=127.0.0.1;Port=5432;Username=postgres;Password=bt7iC4nN07T0f1nDmyp4ss;Database=Spotify"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });
});

var app = builder.Build();

app.Services.GetRequiredService<UserConsumer>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
