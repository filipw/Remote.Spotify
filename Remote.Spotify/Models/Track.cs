using System.Runtime.Serialization;

namespace Remote.Spotify.Models
{
    [DataContract]
    public class Track
    {
        [DataMember(Name = "length")]
        public int Length { get; set; }

        [DataMember(Name = "track_type")]
        public string TrackType { get; set; }

        [DataMember(Name = "track_resource")]
        public Resource TrackResource { get; set; }

        [DataMember(Name = "artist_resource")]
        public Resource ArtistResource { get; set; }

        [DataMember(Name = "album_resource")]
        public Resource AlbumResource { get; set; }
    }
}