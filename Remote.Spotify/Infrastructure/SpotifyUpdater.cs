using System;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Remote.Spotify.Infrastructure
{
    public class SpotifyUpdater
    {
        private static readonly SpotifyHttpClient LocalClient = new SpotifyHttpClient();
        private readonly IHubConnectionContext _clients;

        private static readonly Lazy<SpotifyUpdater> _instance =
            new Lazy<SpotifyUpdater>(
                () => new SpotifyUpdater(GlobalHost.ConnectionManager.GetHubContext<SpotifyHub>().Clients));

        private Timer _timer;

        private SpotifyUpdater(IHubConnectionContext clients)
        {
            _clients = clients;
            _timer = new Timer(UpdateClients, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3));
        }

        public static SpotifyUpdater Instance
        {
            get { return _instance.Value; }
        }

        private void UpdateClients(object state)
        {
            var status = LocalClient.Status().Result;
            _clients.All.UpdateStatus(status);
        }
    }
}