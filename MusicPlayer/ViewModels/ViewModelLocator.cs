using System;
using System.ComponentModel;
using System.Windows;

namespace MusicPlayer.ViewModels
{
    public class ViewModelLocator
    {
        private DependencyObject dummy = new DependencyObject();

        public MainViewModel MainViewModel
        {
            get
            {
                if (isInDesignMode())
                {
                    return new MockMainViewModel();
                }
                return new MainViewModel();
            }
        }

        private bool isInDesignMode()
        {
            return DesignerProperties.GetIsInDesignMode(dummy);
        }
    }
}