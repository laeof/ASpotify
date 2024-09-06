using ASpotifyPlaylists.Dto;
using ASpotifyPlaylists.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ASpotifyPlaylists.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrackController: ControllerBase
    {
        private readonly ITrackService _trackService;
        public TrackController(ITrackService trackService) 
        {
            _trackService = trackService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrack(Guid id)
        {
            return Ok(await _trackService.GetTrackById(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrack(TrackDto dto)
        {
            return Ok(await _trackService.CreateTrack(dto));
        }
        [HttpPut]
        public async Task<IActionResult> ModifyTrack(TrackDto dto)
        {
            return Ok(await _trackService.ModifyTrack(dto));
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveTrack(Guid id)
        {
            return Ok(await _trackService.DeleteTrack(id));
        }
    }
}
