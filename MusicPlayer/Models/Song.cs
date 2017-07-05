using MusicPlayer.Core.Interfaces;

namespace MusicPlayer.Models
{
    public class Song : ILibraryItem
    {
        private static long _ID = 0;

        public Song()
        {
            ID = _ID;
            ++_ID;           
        }
        
        public long ID { get; private set; }
        public string ArtistName { get; set; }
        public string AlbumName { get; set; }
        public string AlbumArtist { get; set; }
        public string TrackName { get; set; }
        public string Duration { get; set; }
        public string Genre { get; set; }
        public uint Year { get; set; }
        public string FilePath { get; set; }
    }
}
