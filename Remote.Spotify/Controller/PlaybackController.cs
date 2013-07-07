using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AngleSharp;
using Remote.Spotify.Infrastructure;
using Remote.Spotify.Models;

namespace Remote.Spotify.Controller
{
    public class PlaybackController : ApiController
    {
        private static readonly SpotifyHttpClient LocalClient = new SpotifyHttpClient();

        [HttpPost]
        public async Task<Status> Pause()
        {
            return await LocalClient.Pause();
        }

        [HttpPost]
        public async Task<Status> Resume()
        {
            return await LocalClient.Resume();
        }

        [HttpGet]
        public async Task<Status> Status()
        {
            return await LocalClient.Status();
        }
    }
}