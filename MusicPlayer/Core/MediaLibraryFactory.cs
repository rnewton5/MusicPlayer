using MusicPlayer.Core.Utilities;
using MusicPlayer.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Drawing;
using MusicPlayer.Core.Interfaces;

namespace MusicPlayer.Core
{
    public class MediaLibraryFactory
    {
        public ObservableCollection<ILibraryItem> Artists
        {
            get; private set;
        }

        public ObservableCollection<ILibraryItem> Albums
        {
            get; private set;
        }

        public ObservableCollection<ILibraryItem> Songs
        {
            get; private set;
        }

        public MediaLibraryFactory(string[] directories, string[] fileTypes)
        {
            Songs = new ObservableCollection<ILibraryItem>();
            Albums = new ObservableCollection<ILibraryItem>();
            Artists = new ObservableCollection<ILibraryItem>();

            var files = SubdirectoryFileFinder.GetFilesFromSubdirectories(directories, fileTypes);
            // songs
            foreach (var filePath in files)
            {
                var file = TagLib.File.Create(filePath);
                Songs.Add(
                    new Song()
                    {
                        ArtistName = file.Tag.FirstPerformer,
                        AlbumName = file.Tag.Album,
                        AlbumArtist = file.Tag.AlbumArtists.Count() == 1 ? file.Tag.FirstAlbumArtist : "Various Artists",
                        TrackName = file.Tag.Title,
                        Duration = Converters.TimeSpan_toString(file.Properties.Duration),
                        Genre = file.Tag.FirstGenre,
                        Year = file.Tag.Year,
                        FilePath = filePath
                    });
            }

            // albums
            foreach(Song song in Songs)
            {
                var matches = Albums.Where(a => ((Album)a).AlbumName == song.AlbumName && ((Album)a).AlbumArtist == song.AlbumArtist);
                if (matches.Count() == 0)
                {
                    var tagFile = TagLib.File.Create(song.FilePath);
                    MemoryStream ms = new MemoryStream(tagFile.Tag.Pictures[0].Data.Data);
                    Albums.Add(
                        new Album()
                        {
                            AlbumArtist = song.AlbumArtist,
                            AlbumName = song.AlbumName,
                            Duration = "",  // this shit needs fixing, maybe store time in song as TimeSpan?
                            Genre = song.Genre,
                            Year = song.Year,
                            TrackCount = matches.Count(),
                            CoverArt = Image.FromStream(ms)
                        });
                    ms.Close();
                }
            }

            // artists
            foreach (Song song in Songs)
            {
                var matches = Artists.Where(a => ((Artist)a).ArtistName == song.ArtistName);
                if (matches.Count() == 0)
                {
                    var tagFile = TagLib.File.Create(song.FilePath);
                    MemoryStream ms = new MemoryStream(tagFile.Tag.Pictures[0].Data.Data);
                    Artists.Add(
                        new Artist()
                        {
                            ArtistName = song.ArtistName,
                            TrackCount = matches.Count(),
                            ArtistPicture = Image.FromStream(ms)
                        });
                    ms.Close();
                }
            }
        }
    }
}
