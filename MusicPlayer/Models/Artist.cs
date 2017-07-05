using MusicPlayer.Core.Interfaces;
using System.Drawing;

namespace MusicPlayer.Models
{
    public class Artist : ILibraryItem
    {
        private static long _ID = 0;

        public Artist()
        {
            ID = _ID;
            ++_ID;
        }

        public long ID { get; private set; }
        public Image ArtistPicture { get; set; }
        public string ArtistName { get; set; }
        public int TrackCount { get; set; }
    }
}
