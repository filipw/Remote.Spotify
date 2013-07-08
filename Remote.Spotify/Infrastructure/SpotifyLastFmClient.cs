using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Remote.Spotify.Models;
using SpotifyAPI;

namespace Remote.Spotify.Infrastructure
{
    public class SpotifyLastFmClient
    {
        private static readonly HttpClient LastFmClient = new HttpClient { BaseAddress = new Uri("http://ws.audioscrobbler.com/2.0/") };
        private static readonly XmlMediaTypeFormatter DefaultFormatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
        private static readonly Lazy<SpotifyApi> SpotifyClient = new Lazy<SpotifyApi>(() =>
            {
                var api = new SpotifyApi();
                api.Initialize();
                Thread.Sleep(300);
                return api;
            });

        private Tuple<bool, lfm> _lastTrack;

        static SpotifyLastFmClient()
        {
            DefaultFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/octet-stream"));
        }

        public void Pause()
        {
            SpotifyClient.Value.PlayPause();
        }

        public void Resume()
        {
            SpotifyClient.Value.PlayPause();
        }

        public void Next()
        {
            SpotifyClient.Value.Next();
        }

        public void Previous()
        {
            SpotifyClient.Value.Previous();
        }

        public async Task<lfm> Status()
        {
            if (SpotifyClient.Value.Track == null) return new lfm();
            if (_lastTrack != null && _lastTrack.Item1 &&
                _lastTrack.Item2.track.name.ToLowerInvariant() == SpotifyClient.Value.Track.Title.ToLowerInvariant())
            {
                _lastTrack.Item2.isPlaying = SpotifyClient.Value.IsPlaying;
                return _lastTrack.Item2;
            }

            var result = await LastFmClient.GetAsync(string.Format("?method=track.getInfo&api_key=796019b30d7e2cfb5bd54a154ded7d42&artist={0}&track={1}", SpotifyClient.Value.Track.Artist, SpotifyClient.Value.Track.Title));

            if (!result.IsSuccessStatusCode)
            {
                _lastTrack = Tuple.Create<bool, lfm>(false, null);
            }

            var trackInfo = await result.Content.ReadAsAsync<lfm>(new[] { DefaultFormatter });
            trackInfo.isPlaying = SpotifyClient.Value.IsPlaying;

            _lastTrack = Tuple.Create(true, trackInfo);
            return trackInfo;
        }
    }
}