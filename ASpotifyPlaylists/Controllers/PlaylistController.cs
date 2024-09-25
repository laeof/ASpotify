using ASpotifyPlaylists.Dto;
using ASpotifyPlaylists.Helpers;
using ASpotifyPlaylists.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ASpotifyPlaylists.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlaylistController: ControllerBase
    {
        private readonly IPlaylistService _playlistService;
        private readonly EntityMapper _entityMapper;
        public PlaylistController(IPlaylistService playlistService, EntityMapper entityMapper)
        {
            _playlistService = playlistService;
            _entityMapper = entityMapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlaylist(Guid id)
        {
            return Ok(await _playlistService.GetPlaylistById(id));
        }
        [HttpGet("popularplaylists")]
        public async Task<IActionResult> GetPopularPlaylists()
        {
            return Ok(await _playlistService.GetPopularPlaylists());
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlaylist(PlaylistDto dto)
        {
            return Ok(await _playlistService.CreatePlaylist(dto));
        }
        [HttpPut("addtoplaylist")]
        public async Task<IActionResult> AddTrackToPlaylist([FromBody] AddToPlaylist request)
        {
            return Ok(await _playlistService.AddToPlaylist(request.playlistId, request.trackId));
        }
        public record AddToPlaylist(Guid playlistId, Guid trackId);
    
        [HttpPut]
        public async Task<IActionResult> ModifyPlaylist(PlaylistDto dto)
        {
            return Ok(await _playlistService.ModifyPlaylist(_entityMapper.MapDtoPlaylist(dto)));
        }
        [HttpDelete]
        public async Task<IActionResult> RemovePlaylist(Guid id)
        {
            return Ok(await _playlistService.DeletePlaylist(id));
        }
    }
}
