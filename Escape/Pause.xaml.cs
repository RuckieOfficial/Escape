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
    /// Interakční logika pro Pause.xaml
    /// </summary>
    public partial class Pause : UserControl
    {
        bool fullscreen = false;

        public Pause()
        {
            InitializeComponent();
        }
        void Exit(object sender, RoutedEventArgs e) {
            System.Windows.Application.Current.Shutdown();
        }
        void Fullscreen(object sender, RoutedEventArgs e) {
            if (fullscreen == false) {
                Window window = Window.GetWindow(this);
                window.WindowState = System.Windows.WindowState.Maximized;
                fullscreen = true;
            } else {
                Window window = Window.GetWindow(this);
                window.WindowState = System.Windows.WindowState.Normal;
                fullscreen = false;
            }
        }
    }
}
