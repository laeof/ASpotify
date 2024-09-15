using ASpotifyPlaylists.Domain;
using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Dto;
using ASpotifyPlaylists.Helpers;
using ASpotifyPlaylists.Services.Abstract;

namespace ASpotifyPlaylists.Services.Service
{
    public class PlaylistService : IPlaylistService
    {
        private readonly DataManager _dataManager;
        private readonly ASpotifyDbContext _context;
        private readonly EntityMapper _entityMapper;
        private readonly IArtistService _artistService;
        public PlaylistService(DataManager dataManager,
            ASpotifyDbContext aSpotifyDbContext,
            EntityMapper entityMapper,
            IArtistService artistService)
        {
            _dataManager = dataManager;
            _context = aSpotifyDbContext;
            _entityMapper = entityMapper;
            _artistService = artistService;
        }
        public async Task<Playlist> CreateArtistPlaylist(PlaylistDto dto)
        {
            var newentity = new Playlist
            {
                Name = dto.Name,
                AuthorId = dto.AuthorId,
                Types = dto.Types,
                ImagePath = dto.ImagePath,
                Tracks = dto.Tracks,
            };

            var entity = await _dataManager.Playlists.Create(newentity, _context.Playlists);

            return entity;
        }
        public async Task<Playlist> CreatePlaylist(PlaylistDto dto)
        {
            var newentity = new Playlist
            {
                Name = dto.Name,
                AuthorId = dto.AuthorId,
                Types = dto.Types,
                ImagePath = dto.ImagePath,
                Tracks = dto.Tracks,
            };

            await _artistService.AddPlaylist(dto.AuthorId, newentity.Id);

            var entity = await _dataManager.Playlists.Create(newentity, _context.Playlists);

            return entity;
        }

        public async Task<Playlist> GetPlaylistById(Guid id)
        {
            return await _dataManager.Playlists.GetById(id, _context.Playlists);
        }

        public async Task<Playlist> ModifyPlaylist(PlaylistDto dto)
        {
            var entity = new Playlist();

            entity.Id = dto.Id;
            entity.Name = dto.Name;
            entity.AuthorId = dto.AuthorId;
            entity.ImagePath = dto.ImagePath;
            entity.Tracks = dto.Tracks;
            entity.UpdatedDate = entity.CreatedDate;
            entity.CreatedDate = dto.CreatedDate;
            entity.Types = dto.Types;

            return await _dataManager.Playlists.Modify(entity, _context.Playlists);
        }

        public async Task<Playlist> DeletePlaylist(Guid id)
        {
            return await _dataManager.Playlists.RemoveById(id, _context.Playlists);
        }

        public async Task<Playlist> AddToPlaylist(Guid playlistId, Guid trackId)
        {
            var playlist = await GetPlaylistById(playlistId);

            playlist.Tracks.Add(trackId);

            await ModifyPlaylist(_entityMapper.MapPlaylistDto(playlist));

            return playlist;
        }
    }
}
