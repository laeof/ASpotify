using ASpotifyPlaylists.Dto;
using ASpotifyPlaylists.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ASpotifyPlaylists.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlaylistController: ControllerBase
    {
        private readonly IPlaylistService _playlistService;
        public PlaylistController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlaylist(Guid id)
        {
            return Ok(await _playlistService.GetPlaylistById(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlaylist(PlaylistDto dto)
        {
            return Ok(await _playlistService.CreatePlaylist(dto));
        }
        [HttpPut("addtoplaylist")]
        public async Task<IActionResult> AddTrackToPlaylist(Guid playlistId, Guid trackId)
        {
            return Ok(await _playlistService.AddToPlaylist(playlistId, trackId));
        }
        [HttpPut]
        public async Task<IActionResult> ModifyPlaylist(PlaylistDto dto)
        {
            return Ok(await _playlistService.ModifyPlaylist(dto));
        }
        [HttpDelete]
        public async Task<IActionResult> RemovePlaylist(Guid id)
        {
            return Ok(await _playlistService.DeletePlaylist(id));
        }
    }
}
