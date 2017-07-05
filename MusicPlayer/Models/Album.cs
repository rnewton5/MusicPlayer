using MusicPlayer.Core.Interfaces;
using System.Drawing;

namespace MusicPlayer.Models
{
    public class Album : ILibraryItem
    {
        private static long _ID = 0;

        public Album()
        {
            ID = _ID;
            ++_ID;
        }

        public long ID { get; private set; }
        public Image CoverArt { get; set; }
        public string AlbumName { get; set; }
        public string AlbumArtist { get; set; }
        public int TrackCount { get; set; }
        public string Duration { get; set; }
        public string Genre { get; set; }
        public uint Year { get; set; }
    }
}
