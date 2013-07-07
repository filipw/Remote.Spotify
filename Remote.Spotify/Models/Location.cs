using System.Runtime.Serialization;

namespace Remote.Spotify.Models
{
    [DataContract]
    public class Location
    {
        [DataMember(Name = "og")]
        public string Og { get; set; }
    }
}