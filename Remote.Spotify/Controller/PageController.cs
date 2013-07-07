using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Remote.Spotify.Controller
{
    public class PageController : ApiController
    {
        private const string DefaultPath = @"C:\Users\Filip\Documents\Visual Studio 2012\Projects\Remote.Spotify\Remote.Spotify";

        public HttpResponseMessage Get(string page)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var loadedPage = LoadPage(page);
            response.Content = new StringContent(loadedPage);
            response.Content.Headers.ContentType = page.EndsWith(".html") ? new MediaTypeHeaderValue("text/html") : new MediaTypeWithQualityHeaderValue("application/javascript");

            return response;
        }

        private static string LoadPage(string name)
        {
            try
            {
                var view = File.ReadAllText(System.IO.Path.Combine(DefaultPath, name));
                return view;
            } catch (FileNotFoundException e) {}

            return string.Empty;
        }
    }
}