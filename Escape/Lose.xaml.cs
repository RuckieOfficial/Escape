using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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

namespace Escape {
    /// <summary>
    /// Interakční logika pro Lose.xaml
    /// </summary>
    public partial class Lose : Page {

        private Frame parentFrame;
        bool paused = false;
        static string losegame = @"sound/lose.wav";
        SoundPlayer music = new SoundPlayer(losegame);

        public Lose() {
            InitializeComponent();
            music.Play();
            Game_pause.soundoff.Click += new RoutedEventHandler(sound_option);
            Game_pause.Continue.Click += new RoutedEventHandler(continue_option);
        }

        public Lose(Frame parentFrame) : this() {
            this.parentFrame = parentFrame;
        }

        void Page_loaded(object sender, RoutedEventArgs e) {
            Application.Current.MainWindow.KeyDown += new KeyEventHandler(Controls);
        }

        void sound_option(object sender, RoutedEventArgs e) {
            if (Globals.sound_state == 1) {
                music.Play();
            } else {
                music.Stop();
            }
        }

        void continue_option(object sender, RoutedEventArgs e) {
            Game_pause.Visibility = Visibility.Hidden;
        }

        void go_prison(object sender, RoutedEventArgs e) {
            Globals.lastCommandIndex = 0;
            Globals.actual_hp = 100;
            Globals.actual_dopamine = 100;
            Globals.dopamin_speed = 3000;
            Globals.sound_state = 1;
            Globals.admin = false;
            Application.Current.MainWindow.KeyDown -= new KeyEventHandler(Controls);
            parentFrame.Navigate(new Prison(parentFrame));
        }

        void exit(object sender, RoutedEventArgs e) {
            System.Windows.Application.Current.Shutdown();
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