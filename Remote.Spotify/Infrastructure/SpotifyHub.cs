using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Remote.Spotify.Infrastructure
{
    [HubName("spotify")]
    public class SpotifyHub : Hub
    {
        private readonly SpotifyUpdater _spotifyUpdater;

        public SpotifyHub()
        {
            _spotifyUpdater = SpotifyUpdater.Instance;
        }
    }
}