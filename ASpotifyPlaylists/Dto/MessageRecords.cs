namespace ASpotifyPlaylists.Dto
{
    //message data
    public record MessageDto<T>(T Method, QueueNames QueueNames, Action Action);
    //methods
    public record MethodCreate<T>(T Model, QueueNames QueueNames)
        : MessageDto<T>(Model, QueueNames, Action.Create);
    public record MethodUpdate<T>(T Model, QueueNames QueueNames)
        : MessageDto<T>(Model, QueueNames, Action.Update);
    public record MethodDelete<T>(T Model, QueueNames QueueNames)
        : MessageDto<T>(Model, QueueNames, Action.Delete);
    //specific
    public record MethodAddPlaylistToUser(Guid PlaylistId, Guid ArtistId)
        : MessageDto<AddPlaylistToUser>(new AddPlaylistToUser(PlaylistId, ArtistId),
                                        QueueNames.Artist,
                                        Action.AddplaylistToUser);
    public record MethodAddTracksToPlaylist(List<Guid> ListTrackId, Guid PlaylistId)
        : MessageDto<AddTracksToPlaylist>(new AddTracksToPlaylist(ListTrackId, PlaylistId),
                                        QueueNames.Playlist,
                                        Action.AddtrackToPlaylist);
    public record MethodCreateLikedPlaylist(Guid UserId)
        : MessageDto<CreateLikedPlaylist>(new CreateLikedPlaylist(UserId),
                                        QueueNames.Playlist,
                                        Action.CreateLikedPlaylist);
    public record MethodAddPlaylistAsLiked(Guid PlaylistId, Guid UserId)
        : MessageDto<AddPlaylistToUser>(new AddPlaylistToUser(PlaylistId, UserId),
                                        QueueNames.User,
                                        Action.AddPlaylistAsLiked);
    //models
    public record AddPlaylistToUser(Guid PlaylistId, Guid ArtistId);
    public record AddTracksToPlaylist(List<Guid> TrackId, Guid PlaylistId);
    public record CreateLikedPlaylist(Guid UserId);

}
