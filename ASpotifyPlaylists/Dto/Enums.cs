namespace ASpotifyPlaylists.Dto
{
    public enum PlaylistTypes
    {
        Playlist = 0,
        Album = 1,
        Single = 2,
    }

    public enum QueueNames
    {
        Playlist = 0,
        Artist = 1,
        Track = 2,
        User = 3,
    }

    public enum Action
    {
        Create = 0,
        Update = 1,
        Delete = 2,
        AddplaylistToUser = 3,
        AddtrackToPlaylist = 4,
        CreateLikedPlaylist = 5,
        AddPlaylistAsLiked = 6,
    }
}
