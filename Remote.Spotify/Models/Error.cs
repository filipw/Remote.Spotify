using System.Runtime.Serialization;

namespace Remote.Spotify.Models
{
    [DataContract]
    public class Error
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}