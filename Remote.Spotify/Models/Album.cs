using System.Runtime.Serialization;

namespace Remote.Spotify.Models
{
    [DataContract]
    public class Album
    {
        [DataMember(Name = "released")]
        public string Released { get; set; }

        [DataMember(Name = "href")]
        public string Href { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}