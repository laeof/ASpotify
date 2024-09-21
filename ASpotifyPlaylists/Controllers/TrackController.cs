using ASpotifyPlaylists.Domain.Entities;
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
        private readonly IPlaylistService _playlistService;
        private readonly static SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        public TrackController(ITrackService trackService, 
            IPlaylistService playlistService) 
        {
            _trackService = trackService;
            _playlistService = playlistService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrack(Guid id)
        {
            return Ok(await _trackService.GetTrackById(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrack(TrackDto dto)
        {
            Track track = new Track();
            await _semaphoreSlim.WaitAsync();
            try
            {
                track = await _trackService.CreateTrack(dto);
                await _playlistService.AddToPlaylist(dto.AlbumId, dto.Id);
            }
            finally
            {
                _semaphoreSlim.Release();
            }

            return Ok(track);
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
