//code internalized
//from http://spotifyremote.codeplex.com/

using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace SpotifyAPI
{
    public class SpotifyApi : IDisposable
    {
        public SpotifyApi()
        {
        }

        #region Constants
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x202;
        #endregion

        #region Private Members
        Process _Spotify;
        Timer _Timer;
        IntPtr _SpotifyHandle = IntPtr.Zero;

        private static string _MetaPath
        {
            get
            {
                DateTime latest = DateTime.MinValue;
                string path = "";
                foreach (string p in System.IO.Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Spotify\\Users"))
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(p + "\\guistate");
                    if (latest < f.LastWriteTime)
                    {
                        latest = f.LastWriteTime;
                        path = p;
                    }
                }

                return path;
            }
        }

        // Spotify states
        string _TitleCache;
        bool _IsPlaying;
        TrackInfo _TrackInfo;
        #endregion

        #region Properties
        public Process Proc
        {
            get
            {
                if (_Spotify != null && _Spotify.HasExited)
                    _Spotify = null;

                if (_Spotify == null)
                {
                    Process[] tmp = Process.GetProcessesByName("spotify");
                    if (tmp.Length > 0)
                        _Spotify = tmp[0];
                }

                if (_Spotify != null)
                    _Spotify.Refresh();

                return _Spotify;
            }
        }

        public Rectangle WindowRect
        {
            get
            {
                Rectangle r = new Rectangle();
                if (Proc != null)
                    GetClientRect(Proc.MainWindowHandle, ref r);
                return r;
            }
        }

        public bool IsPlaying
        {
            get { return _IsPlaying; }
        }

        public ITrackInfo Track
        {
            get { return _TrackInfo; }
        }
        #endregion

        #region Events
        public event EventHandler IsPlayingChanged;
        public event EventHandler TrackChanged;
        #endregion

        #region Private Methods
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int GetClientRect(IntPtr hWnd, ref Rectangle lpRect);

        [DllImport("user32.dll")]
        internal static extern int PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern int GetWindowText(IntPtr hWnd, [Out] StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int GetWindowTextLength(IntPtr hWnd);

        private string GetSpotifyTitle()
        {
            if (_SpotifyHandle != IntPtr.Zero)
            {
                StringBuilder s = new StringBuilder(GetWindowTextLength(_SpotifyHandle) + 1);
                GetWindowText(_SpotifyHandle, s, s.Capacity);
                return s.ToString();
            }
            else
                return "";
        }
        #endregion

        #region Public Methods
        public void Initialize()
        {
            _Timer = new Timer();
            _Timer.Interval = 100;
            _Timer.Elapsed += new ElapsedEventHandler(_Timer_Tick);
            _Timer.Start();
        }

        public void Dispose()
        {
            if (_Timer != null)
            {
                _Timer.Stop();
                _Timer = null;
            }
        }

        public void Next()
        {
            PostMessage(_SpotifyHandle, 0x319, IntPtr.Zero, new IntPtr(0xb0000L));
        }

        public void Previous()
        {
            PostMessage(_SpotifyHandle, 0x319, IntPtr.Zero, new IntPtr(0xc0000L));
        }

        public void PlayPause()
        {
            PostMessage(_SpotifyHandle, 0x319, IntPtr.Zero, new IntPtr(0xe0000L));
        }

        public void Stop()
        {
            PostMessage(_SpotifyHandle, 0x319, IntPtr.Zero, new IntPtr(0xd0000L));
            Previous();
        }

        public void VolumeUp()
        {
            PostMessage(_SpotifyHandle, 0x319, IntPtr.Zero, new IntPtr(0xa0000L));
        }

        public void VolumeDown()
        {
            PostMessage(_SpotifyHandle, 0x319, IntPtr.Zero, new IntPtr(0x90000L));
        }

        public void Mute()
        {
            PostMessage(_SpotifyHandle, 0x319, IntPtr.Zero, new IntPtr(0x80000L));
        }
        #endregion

        #region EventCallbacks
        void _Timer_Tick(object sender, EventArgs e)
        {
            if (Proc == null)
            {
                // Reset states
                _IsPlaying = false;
                _TitleCache = "Spotify";
                if (_IsPlaying)
                {
                    _IsPlaying = false;
                    if (IsPlayingChanged != null)
                        IsPlayingChanged(this, new EventArgs());
                }
            }
            else
            {
                string spotifyTitle = GetSpotifyTitle();

                if (spotifyTitle == "Spotify")
                {
                    // Nothing is playing.
                    if (_IsPlaying)
                    {
                        _IsPlaying = false;
                        if (IsPlayingChanged != null)
                            IsPlayingChanged(this, new EventArgs());
                    }
                }
                else
                {
                    // Something is playing
                    if (!_IsPlaying)
                    {
                        _IsPlaying = true;
                        if (IsPlayingChanged != null)
                            IsPlayingChanged(this, new EventArgs());
                    }
                }


                //Proc.Refresh();

                // Update spotify window handle
                if (Proc.MainWindowHandle != IntPtr.Zero)
                    _SpotifyHandle = Proc.MainWindowHandle;

                string playing = _TrackInfo.Artist + " – " + _TrackInfo.Title;
                if (_TitleCache != string.Concat("Spotify - ", spotifyTitle) && spotifyTitle.Length > 0)
                {
                    // Track changed
                    _TitleCache = spotifyTitle.Replace("Spotify - ", "");
                    //_TrackInfo = new TrackInfo();
                    if (_TitleCache != playing)
                    {

                        // Gather information about current track
                        ArrayList artists = new ArrayList();
                        ArrayList albums = new ArrayList();
                        ArrayList songs = new ArrayList();
                        if (System.IO.File.Exists(_MetaPath + "\\metadata"))
                        {
                            string[] meta = System.IO.File.ReadAllLines(_MetaPath + "\\metadata");
                            int mi = 0;
                            for (; mi < meta.Length; mi++)
                            {
                                string l = meta[mi];

                                if (l.Length == 0)
                                {
                                    mi++;
                                    break;
                                }

                                artists.Add(l);
                            }
                            for (; mi < meta.Length; mi++)
                            {
                                string l = meta[mi];

                                if (l.Length == 0)
                                {
                                    mi++;
                                    break;
                                }

                                albums.Add(l);
                            }
                            for (; mi < meta.Length; mi++)
                            {
                                string l = meta[mi];

                                if (l.Length == 0)
                                    break;

                                songs.Add(l);
                            }
                            mi = 0;

                            foreach (string l in artists)
                            {
                                string[] tmp = l.Split(new[] { "" }, StringSplitOptions.None);
                                if (tmp.Length > 1 && _TitleCache.StartsWith(tmp[1]))
                                {
                                    _TrackInfo.ArtistHash = tmp[0];
                                    _TrackInfo.Artist = tmp[1];
                                }
                                tmp = null;
                            }
                            foreach (string l in songs)
                            {
                                string[] tmp = l.Split(new[] { "" }, StringSplitOptions.None);
                                if (tmp.Length > 1 && _TitleCache.EndsWith(tmp[1]))
                                {
                                    _TrackInfo.SongHash = tmp[0];
                                    _TrackInfo.Title = tmp[1];
                                    int sl;
                                    if (int.TryParse(tmp[3], out sl))
                                        _TrackInfo.SongLength = sl;
                                    else
                                        _TrackInfo.SongLength = 0;
                                    int tn;
                                    if (int.TryParse(tmp[4], out tn))
                                        _TrackInfo.TrackNo = tn;
                                    else
                                        _TrackInfo.TrackNo = 0;
                                    _TrackInfo.AlbumHash = tmp[5];
                                }
                                tmp = null;
                            }
                            if (_TrackInfo.AlbumHash != null && _TrackInfo.AlbumHash.Length > 0)
                            {
                                foreach (string l in albums)
                                {
                                    string[] tmp = l.Split(new[] { "" }, StringSplitOptions.None);
                                    if (tmp.Length > 1 && tmp[0] == _TrackInfo.AlbumHash)
                                    {
                                        _TrackInfo.Album = tmp[1];
                                        _TrackInfo.Year = tmp[4];
                                    }
                                    tmp = null;
                                }
                            }
                        }
                        else
                        {
                            // Get title and artist
                            string[] tmp = _TitleCache.Split(new[] { " – " }, StringSplitOptions.None);

                            if (tmp.Length > 1)
                            {
                                _TrackInfo.Artist = tmp[0];
                                _TrackInfo.Title = tmp[1];
                            }
                        }


                        if (TrackChanged != null)
                            TrackChanged(this, new EventArgs());
                    }
                }
            }
        }
        #endregion
    }
}
