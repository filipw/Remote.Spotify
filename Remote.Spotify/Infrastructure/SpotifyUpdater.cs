using System;
using System.Timers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Remote.Spotify.Infrastructure
{
    public class SpotifyUpdater
    {
        private static readonly SpotifyLastFmClient ApiClient = new SpotifyLastFmClient();
        private readonly IHubConnectionContext _clients;

        private static readonly Lazy<SpotifyUpdater> _instance =
            new Lazy<SpotifyUpdater>(
                () => new SpotifyUpdater(GlobalHost.ConnectionManager.GetHubContext<SpotifyHub>().Clients));

        private readonly Timer _timer;

        private SpotifyUpdater(IHubConnectionContext clients)
        {
            _clients = clients;
            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += new ElapsedEventHandler(UpdateClients);
            _timer.Start();
        }

        public static SpotifyUpdater Instance
        {
            get { return _instance.Value; }
        }

        private void UpdateClients(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var status = ApiClient.Status().Result;
            _clients.All.UpdateStatus(status);
        }
    }
}