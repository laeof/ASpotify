using ASpotifyPlaylists.Consumers;
using ASpotifyPlaylists.Dto;
using ASpotifyPlaylists.Helpers;
using ASpotifyPlaylists.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASpotifyPlaylists.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlaylistController: ControllerBase
    {
        private readonly IPlaylistService _playlistService;
        private readonly EntityMapper _entityMapper;
        public PlaylistController(IPlaylistService playlistService, 
            EntityMapper entityMapper)
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
        [Authorize]
        public IActionResult CreatePlaylist(PlaylistDto dto)
        {
            if(_playlistService.CreatePlaylist(dto) == Task.CompletedTask)
                return Ok(dto);

            return BadRequest();
        }
        [HttpPut("addtoplaylist")]
        [Authorize]
        public async Task<IActionResult> AddTrackToPlaylist([FromBody] AddToPlaylist request)
        {
            return Ok(await _playlistService.AddToPlaylist(request.playlistId,
                new List<Guid> { request.trackId }));
        }
        public record AddToPlaylist(Guid playlistId, Guid trackId);
        [HttpPut("removefromplaylist")]
        [Authorize]
        public async Task<IActionResult> RemoveTrackToPlaylist([FromBody] AddToPlaylist request)
        {
            return Ok(await _playlistService.RemoveFromPlaylist(request.playlistId,
                new List<Guid> { request.trackId }));
        }
        [HttpPut]
        [Authorize]
        public IActionResult ModifyPlaylist(PlaylistDto dto)
        {
            if(_playlistService.ModifyPlaylist(_entityMapper.MapDtoPlaylist(dto)) == Task.CompletedTask)
                return Ok(dto);

            return BadRequest();
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemovePlaylist(Guid id)
        {
            return Ok(await _playlistService.DeletePlaylist(id));
        }
    }
}
