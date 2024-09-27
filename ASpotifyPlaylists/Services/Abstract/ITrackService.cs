using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Dto;

namespace ASpotifyPlaylists.Services.Abstract
{
    public interface ITrackService
    {
        Task CreateTrack(List<TrackDto> dto);
        Task<Track> GetTrackById(Guid id);
        Task ModifyTrack(TrackDto dto);
        Task<Track> DeleteTrack(Guid id);
    }
}
