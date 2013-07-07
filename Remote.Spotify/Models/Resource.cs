using System.Runtime.Serialization;

namespace Remote.Spotify.Models
{
    [DataContract]
    public class Resource
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "uri")]
        public string Uri { get; set; }

        [DataMember(Name = "location")]
        public Location Location { get; set; }
    }
}