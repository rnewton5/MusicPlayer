using MusicPlayer.ViewModels;
using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MusicPlayer.Views.MainViewResources
{
    /// <summary>
    /// Interaction logic for CustomSlider.xaml
    /// </summary>
    public partial class CustomSlider : UserControl
    {
        private DispatcherTimer timer;
        private bool isDragging;

        public CustomSlider()
        {
            InitializeComponent();
            isDragging = false;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += new EventHandler(timer_tick);
            timer.Start();
        }

        private void timer_tick(object sender, EventArgs e)
        {
            if (!isDragging)
            {
                timerSlider.Value = (this.DataContext as MainViewModel).ElapsedTimePercentage;
            }
        }

        private void timerSlider_dragStarted(object sender, EventArgs e)
        {
            isDragging = true;
        }

        private void timerSlider_dragCompleted(object sender, EventArgs e)
        {
            (this.DataContext as MainViewModel).ElapsedTimePercentage = timerSlider.Value;
            isDragging = false;
        }
    }
}
