using ASpotifyPlaylists.Domain;
using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Domain.Repository.Abstract;
using ASpotifyPlaylists.Domain.Repository.Entities;
using ASpotifyPlaylists.Helpers;
using ASpotifyPlaylists.Services.Abstract;
using ASpotifyPlaylists.Services.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<EntityMapper>();

builder.Services.AddScoped<DataManager>();

builder.Services.AddScoped<ICRUDRepository<Playlist>, CRUDRepository<Playlist>>();
builder.Services.AddScoped<ICRUDRepository<Track>, CRUDRepository<Track>>();
builder.Services.AddScoped<ICRUDRepository<Artist>, CRUDRepository<Artist>>();

builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();

builder.Services.AddDbContext<ASpotifyDbContext>(x => x.UseNpgsql(
    $"Host={Environment.GetEnvironmentVariable("ASPNETCORE_ASPOTIFY_DB_SERVER")};" +
    $"Port={Environment.GetEnvironmentVariable("ASPNETCORE_ASPOTIFY_DB_PORT")};" +
    $"Username={Environment.GetEnvironmentVariable("ASPNETCORE_ASPOTIFY_DB_USER")};" +
    $"Password={Environment.GetEnvironmentVariable("ASPNETCORE_ASPOTIFY_DB_PASS")};" +
    $"Database={Environment.GetEnvironmentVariable("ASPNETCORE_ASPOTIFY_DB_NAME")};"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
