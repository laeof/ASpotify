using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Dto;
using ASpotifyPlaylists.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrack(Guid id)
        {
            return Ok(await _trackService.GetTrackById(id));
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateTrack(List<TrackDto> dto)
        {
            return Ok(_trackService.CreateTrack(dto));
        }
        [HttpPut]
        [Authorize]
        public IActionResult ModifyTrack(TrackDto dto)
        {
            if(_trackService.ModifyTrack(dto) == Task.CompletedTask)
                return Ok(dto);

            return BadRequest();
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveTrack(Guid id)
        {
            return Ok(await _trackService.DeleteTrack(id));
        }
    }
}
