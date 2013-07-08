using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Remote.Spotify.Infrastructure;
using Remote.Spotify.Models;

namespace Remote.Spotify.Controller
{
    public class PlaybackController : ApiController
    {
        private static readonly SpotifyLastFmClient Client = new SpotifyLastFmClient();

        [HttpPost]
        public void Pause()
        {
            Client.Pause();
        }

        [HttpPost]
        public void Resume()
        {
            Client.Resume();
        }

        [HttpPost]
        public void Next()
        {
            Client.Next();
        }

        [HttpPost]
        public void Previous()
        {
            Client.Previous();
        }

        [HttpGet]
        public async Task<lfm> Status()
        {
            return await Client.Status();
        }
    }
}