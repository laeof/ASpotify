using ASpotifyPlaylists.Dto;
using ASpotifyPlaylists.Services.Abstract;
using ASpotifyPlaylists.Services.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASpotifyPlaylists.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class ArtistController: ControllerBase
    {
        private readonly IArtistService _artistService;
        public ArtistController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet]
        public async Task<IActionResult> GetArtist(Guid id)
        {
            return Ok(await _artistService.GetArtistById(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateArtist(ArtistDto dto)
        {
            return Ok(await _artistService.CreateArtist(dto));
        }
        [HttpPut("addplaylist")]
        public async Task<IActionResult> AddTrackToPlaylist(Guid artistId, Guid playlistId)
        {
            return Ok(await _artistService.AddPlaylist(artistId, playlistId));
        }
        [HttpPut]
        public async Task<IActionResult> ModifyArtist(ArtistDto dto)
        {
            return Ok(await _artistService.ModifyArtist(dto));
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveArtist(Guid id)
        {
            return Ok(await _artistService.DeleteArtist(id));
        }
    }
}
