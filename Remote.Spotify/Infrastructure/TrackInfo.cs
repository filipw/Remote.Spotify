//code internalized
//from http://spotifyremote.codeplex.com/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpotifyAPI
{
    struct TrackInfo : ITrackInfo
    {
        public string Artist { get; set; }
        public string ArtistHash { get; set; }
        public string Album { get; set; }
        public string AlbumHash { get; set; }
        public string Title { get; set; }
        public string SongHash { get; set; }
        public string Year { get; set; }
        public int TrackNo { get; set; }
        public int SongLength { get; set; }
    }

    public interface ITrackInfo
    {
        string Artist { get; }
        string Album { get; }
        string Title { get; }
        string Year { get; }
        int TrackNo { get; }
        int SongLength { get; }

        string SongHash { get; }
    }
}
