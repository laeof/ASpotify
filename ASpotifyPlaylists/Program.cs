using ASpotifyPlaylists.Consumers;
using ASpotifyPlaylists.Domain;
using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Domain.Repository.Abstract;
using ASpotifyPlaylists.Domain.Repository.Entities;
using ASpotifyPlaylists.Helpers;
using ASpotifyPlaylists.Services.Abstract;
using ASpotifyPlaylists.Services.Service;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<EntityMapper>();

builder.Services.AddScoped<DataManager>();

builder.Services.AddScoped<ICRUDRepository<TrackPlaylist>, CRUDRepository<TrackPlaylist>>();
builder.Services.AddScoped<ICRUDRepository<Playlist>, CRUDRepository<Playlist>>();
builder.Services.AddScoped<ICRUDRepository<Track>, CRUDRepository<Track>>();
builder.Services.AddScoped<ICRUDRepository<Artist>, CRUDRepository<Artist>>();
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();

builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();

//rabbitmq

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
builder.Services.AddSingleton<ArtistConsumer>();
builder.Services.AddSingleton<PlaylistConsumer>();

//redis

builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = "localhost:6379";
    option.InstanceName = "default";
});

builder.Services.AddScoped<ICacheService, CacheService>();

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

app.Services.GetRequiredService<ArtistConsumer>();
app.Services.GetRequiredService<PlaylistConsumer>();

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
