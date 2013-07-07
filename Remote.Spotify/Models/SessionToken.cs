using System.Runtime.Serialization;

namespace Remote.Spotify.Models
{
    [DataContract]
    public class SessionToken
    {
        [DataMember(Name = "error")]
        public Error Error { get; set; }

        [DataMember(Name = "token")]
        public string Token { get; set; }
    }
}