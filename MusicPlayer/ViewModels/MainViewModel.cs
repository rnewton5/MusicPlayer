using MusicPlayer.Core;
using MusicPlayer.Core.Interfaces;
using MusicPlayer.Core.Utilities;
using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MusicPlayer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ICommand _playPauseSelectedTrackCommand;
        public ICommand PlayPauseSelectedTrackCommand
        {
            get
            {
                return _playPauseSelectedTrackCommand ?? (_playPauseSelectedTrackCommand = new RelayCommand(param => playMediaItem()));
            }
        }

        private ObservableCollection<ILibraryItem> _libraryItems;
        public ObservableCollection<ILibraryItem> LibraryItems
        {
            get { return _libraryItems; }
            set { SetProperty(ref _libraryItems, value); }
        }

        private ILibraryItem _selectedMediaItem;
        public ILibraryItem SelectedMediaItem
        {
            get { return _selectedMediaItem; }
            set { SetProperty(ref _selectedMediaItem, value); }
        }

        private ILibraryItem _nowPlaying;
        public ILibraryItem NowPlaying
        {
            get
            {
                return _nowPlaying;
            }
            set { SetProperty(ref _nowPlaying, value); }
        }

        private BitmapSource _nowPlayingAlbumArtwork;
        public BitmapSource NowPlayingAlbumArtwork
        {
            get { return _nowPlayingAlbumArtwork; }
            set { SetProperty(ref _nowPlayingAlbumArtwork, value); }
        }

        public double ElapsedTimePercentage
        {
            get
            {
                if (_player.NaturalDuration.HasTimeSpan)
                {
                    return (((double)_player.Position.Ticks) / _player.NaturalDuration.TimeSpan.Ticks) * 10;
                }
                return 0.0;                
            }
            set
            {
                if (_player.NaturalDuration.HasTimeSpan)
                {
                    double percentage = value / 10;
                    long ticks = (long)(_player.NaturalDuration.TimeSpan.Ticks * percentage);
                    _player.Position = new TimeSpan(ticks);
                }
            }
        }

        private MediaPlayer _player;
        private MediaLibraryFactory _mediaLibraryFactory;

        public MainViewModel()
        {
            string[] directories = { @"C:\Users\Rhett\Music" };
            string[] fileTypes = { "*.mp3" };
            _mediaLibraryFactory = new MediaLibraryFactory(directories, fileTypes);
            LibraryItems = _mediaLibraryFactory.Songs;
            _player = new MediaPlayer();
            _player.ScrubbingEnabled = true;
        }

        public void playMediaItem()
        {
            if (_selectedMediaItem == null)
            {
                _selectedMediaItem = _libraryItems.First();
            }
            if (_selectedMediaItem.GetType() == typeof(Song))
            {
                _player.Open(new Uri(((Song)SelectedMediaItem).FilePath));
                _player.Play();
                NowPlaying = SelectedMediaItem;
                var x = _mediaLibraryFactory.Albums.Where(a => ((Album)a).AlbumArtist == ((Song)NowPlaying).AlbumArtist 
                                                             && ((Album)a).AlbumName == ((Song)NowPlaying).AlbumName);
                NowPlayingAlbumArtwork = Converters.Image_toBitmapSource((x.ElementAt(0) as Album).CoverArt);
            }
        }
    }


    public class MockMainViewModel : MainViewModel
    {
        private ObservableCollection<ILibraryItem> _libraryItems;
        public ObservableCollection<ILibraryItem> LibraryItems
        {
            get { return _libraryItems; }
            set { SetProperty(ref _libraryItems, value); }
        }

        public MockMainViewModel()
        {
            _libraryItems = new ObservableCollection<ILibraryItem>();


            _libraryItems.Add(new Song()
            {
                AlbumArtist = "The Testers",
                AlbumName = "Testing",
                ArtistName = "The Testers",
                Duration = "99:99",
                FilePath = "C:\\test\\path",
                Genre = "Test Rock",
                TrackName = "Testing name",
                Year = 2017
            });
            _libraryItems.Add(new Song()
            {
                AlbumArtist = "The Testers",
                AlbumName = "Testing",
                ArtistName = "The Testers",
                Duration = "99:99",
                FilePath = "C:\\test\\path",
                Genre = "Test Rock",
                TrackName = "Testing name",
                Year = 2017
            });
        }
    }
}
