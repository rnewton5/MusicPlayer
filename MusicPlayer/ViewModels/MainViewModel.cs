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
                return _playPauseSelectedTrackCommand ?? (_playPauseSelectedTrackCommand = new RelayCommand(param => playOrPauseMediaItem()));
            }
        }

        private ICommand _playNextTrackCommand;
        public ICommand PlayNextTrackCommand
        {
            get
            {
                return _playNextTrackCommand ?? (_playNextTrackCommand = new RelayCommand(param => playNextMediaItem()));
            }
        }

        public ICommand _playPrevTrackCommand;
        public ICommand PlayPrevTrackCommand
        {
            get
            {
                return _playPrevTrackCommand ?? (_playPrevTrackCommand = new RelayCommand(param => playPrevMediaItem()));
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
            get { return _nowPlaying; }
            set { SetProperty(ref _nowPlaying, value); }
        }

        private BitmapSource _nowPlayingAlbumArtwork;
        public BitmapSource NowPlayingAlbumArtwork
        {
            get { return _nowPlayingAlbumArtwork; }
            set { SetProperty(ref _nowPlayingAlbumArtwork, value); }
        }

        private bool _isPlaying;
        public bool IsPlaying
        {
            get { return _isPlaying; }
            set { SetProperty(ref _isPlaying, value); }
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
            SelectedMediaItem = LibraryItems.First();
            _player = new MediaPlayer();
        }

        private void playPrevMediaItem()
        {
            if (SelectedMediaItem == null)
            {
                return;
            }
            if (_player.Position.Seconds > 3)
            {
                ElapsedTimePercentage = 0;
            }
            else
            {
                int index = LibraryItems.IndexOf(SelectedMediaItem) - 1;
                if (index < 0)
                {
                    index = LibraryItems.Count - 1;
                }
                SelectedMediaItem = LibraryItems.ElementAt(index);
                openAndPlaySong();
            }
        }

        private void playNextMediaItem()
        {
            if (SelectedMediaItem == null)
            {
                return;
            }
            int index = LibraryItems.IndexOf(SelectedMediaItem) + 1;
            if (index >= LibraryItems.Count)
            {
                index = 0;
            }
            SelectedMediaItem = LibraryItems.ElementAt(index);
            openAndPlaySong();
        }

        private void playOrPauseMediaItem()
        {
            if (SelectedMediaItem == null)
            {
                SelectedMediaItem = LibraryItems.First();
            }
            if (SelectedMediaItem.GetType() == typeof(Song))
            {
                if (SelectedMediaItem == NowPlaying && IsPlaying)
                {
                    _player.Pause();
                    IsPlaying = false;
                }
                else if (SelectedMediaItem == NowPlaying && !IsPlaying)
                {
                    _player.Play();
                    IsPlaying = true;
                }
                else
                {
                    openAndPlaySong();
                }
            }
        }

        private void openAndPlaySong()
        {
            _player.Open(new Uri(((Song)SelectedMediaItem).FilePath));
            _player.Play();
            NowPlaying = SelectedMediaItem;
            var x = _mediaLibraryFactory.Albums.Where(a => ((Album)a).AlbumArtist == ((Song)NowPlaying).AlbumArtist
                                                         && ((Album)a).AlbumName == ((Song)NowPlaying).AlbumName);
            NowPlayingAlbumArtwork = Converters.Image_toBitmapSource((x.ElementAt(0) as Album).CoverArt);
            IsPlaying = true;
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
