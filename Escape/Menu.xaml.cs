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

namespace Escape
{
    /// <summary>
    /// Interakční logika pro Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {

        private Frame parentFrame;
        bool paused = false;
        public Menu()
        {
            InitializeComponent();
        }
        public Menu(Frame parentFrame) : this() {
            this.parentFrame = parentFrame;
        }
        private void Page_loaded(object sender, RoutedEventArgs e) {
            Application.Current.MainWindow.KeyDown += new KeyEventHandler(Controls);
        }

        private void prepage(object sender, RoutedEventArgs e) {
            parentFrame.Navigate(new Intro(parentFrame));
            Application.Current.MainWindow.KeyDown -= new KeyEventHandler(Controls);
        }
        private void Controls(object sender, KeyEventArgs e) {
            if (e.Key == Key.Escape) {
                if (paused == false) {
                    Game_pause.Visibility = Visibility.Visible;
                    paused = true;
                } else {
                    Game_pause.Visibility = Visibility.Hidden;
                    paused = false;
                }
            }
        }
    }
}