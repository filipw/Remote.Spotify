using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp;
using Remote.Spotify.Models;

namespace Remote.Spotify.Infrastructure
{
    public class SpotifyHttpClient : HttpClient
    {
        private readonly Lazy<HttpClient> _artClient = new Lazy<HttpClient>(() => new HttpClient { BaseAddress = new Uri(Const.DefaultUrlBase) });
        private readonly AsyncLazy<SessionToken> _sessionToken;
        private static readonly AsyncLazy<string> Token = new AsyncLazy<string>(() => GetOAuthToken());

        public SpotifyHttpClient()
        {
            BaseAddress = new Uri(Const.DefaultLocalAddress);
            DefaultRequestHeaders.Add("Origin", Const.DefaultUrlBase);

            _sessionToken = new AsyncLazy<SessionToken>(async () => await GetSessionToken());
        }

        public async Task<Status> Pause()
        {
            var result = await GetAsync(string.Format("remote/pause.json?pause=true&csrf={0}&oauth={1}",
                                               (await _sessionToken.Value).Token, await Token));
            return await result.Content.ReadAsAsync<Status>();
        }

        public async Task<Status> Resume()
        {
            var result = await GetAsync(string.Format("remote/pause.json?pause=false&csrf={0}&oauth={1}",
                                               (await _sessionToken.Value).Token, await Token));
            return await result.Content.ReadAsAsync<Status>();
        }

        public async Task<Status> Status()
        {
            var result = await GetAsync(string.Format("remote/status.json?pause=false&csrf={0}&oauth={1}",
                                               (await _sessionToken.Value).Token, await Token));

            var status = await result.Content.ReadAsAsync<Status>();
            var art = await GetAlbumArt(status.Track.TrackResource.Uri);
            status.ArtSmall = art.Item1;
            status.ArtLarge = art.Item2;

            return status;
        }

        private async Task<Tuple<string, string>> GetAlbumArt(string uri)
        {
            var page = await _artClient.Value.GetStringAsync("?uri=" + uri);
            var document = DocumentBuilder.Html(page);
            var coverElement = document.QuerySelector("#content .track");
            var coverSet = Tuple.Create(coverElement.GetAttribute("data-small-ca"),
                                        coverElement.GetAttribute("data-ca"));
            return coverSet;
        }

        private async Task<SessionToken> GetSessionToken()
        {
            var tokenResponse = await GetAsync("simplecsrf/token.json");
            return await tokenResponse.Content.ReadAsAsync<SessionToken>();
        }

        private static async Task<string> GetOAuthToken()
        {
            using (var client = new HttpClient())
            {
                var page = await client.GetStringAsync(Const.TokenUrl);
                var document = DocumentBuilder.Html(page);
                var scriptTag = document.QuerySelectorAll("script").ElementAt(1);
                var token = scriptTag.TextContent.Substring(scriptTag.TextContent.IndexOf("tokenData = '", System.StringComparison.Ordinal) + 13, 88);
                return token;
            }
        }
    }
}