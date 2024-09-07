using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Dto;

namespace ASpotifyPlaylists.Services.Abstract
{
    public interface ITrackService
    {
        Task<Track> CreateTrack(TrackDto dto);
        Task<Track> GetTrackById(Guid id);
        Task<Track> ModifyTrack(TrackDto dto);
        Task<Track> DeleteTrack(Guid id);
    }
}
