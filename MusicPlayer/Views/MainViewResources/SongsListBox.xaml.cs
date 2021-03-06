﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicPlayer.Views.MainViewResources
{
    /// <summary>
    /// Interaction logic for SongsListBox.xaml
    /// </summary>
    public partial class SongsListBox : UserControl
    {
        public SongsListBox()
        {
            InitializeComponent();
        }

        private void GridColumnSizer(object sender, RoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            ColumnDefinitionCollection cdc = grid.ColumnDefinitions;
            double width = grid.Width;

            if (width < 1000)
            {
                cdc.ElementAt(4).Width = new GridLength(0);
                if (width < 850)
                {
                    cdc.ElementAt(3).Width = new GridLength(0);
                    if (width < 725)
                    {
                        cdc.ElementAt(2).Width = new GridLength(0);
                    }
                    else
                    {
                        cdc.ElementAt(2).Width = new GridLength(2, GridUnitType.Star);
                    }
                }
                else
                {
                    cdc.ElementAt(3).Width = new GridLength(1, GridUnitType.Star);
                }
            }
            else
            {
                cdc.ElementAt(4).Width = new GridLength(50);
            }

            grid.UpdateLayout();
        }

        /// <summary>
        /// Handles double click for a grid in the list box
        /// </summary>
        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                Console.WriteLine("double click");
                // TODO: trigger a command
            }
        }

        private void ListBoxItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            hiddenButton.Command.Execute(null);
        }
    }
}
