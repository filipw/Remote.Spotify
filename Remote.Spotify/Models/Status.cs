using System.Runtime.Serialization;

namespace Remote.Spotify.Models
{
    [DataContract]
    public class Status
    {
        [DataMember(Name = "error")]
        public Error Error { get; set; }

        [DataMember(Name = "version")]
        public int Version { get; set; }

        [DataMember(Name = "client_version")]
        public string ClientVersion { get; set; }

        [DataMember(Name = "playing")]
        public bool Playing { get; set; }

        [DataMember(Name = "shuffle")]
        public bool Shuffle { get; set; }

        [DataMember(Name = "repeat")]
        public bool Repeat { get; set; }

        [DataMember(Name = "play_enabled")]
        public bool PlayEnabled { get; set; }

        [DataMember(Name = "prev_enabled")]
        public bool PreviousEnabled { get; set; }

        [DataMember(Name = "next_enabled")]
        public bool NextEnabled { get; set; }

        [DataMember(Name = "playing_position")]
        public double PlayingPosition { get; set; }

        [DataMember(Name = "server_time")]
        public int ServerTime { get; set; }

        [DataMember(Name = "volume")]
        public double Volume { get; set; }

        [DataMember(Name = "online")]
        public bool Online { get; set; }

        [DataMember(Name = "running")]
        public bool Running { get; set; }

        [DataMember(Name = "track")]
        public Track Track { get; set; }

        [DataMember]
        public string ArtSmall { get; set; }

        [DataMember]
        public string ArtLarge { get; set; }
    }
}