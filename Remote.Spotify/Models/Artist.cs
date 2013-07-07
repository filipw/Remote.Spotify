using System.Runtime.Serialization;

namespace Remote.Spotify.Models
{
    [DataContract]
    public class Artist
    {
        [DataMember(Name = "href")]
        public string Href { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}