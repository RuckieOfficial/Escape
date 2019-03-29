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

namespace Escape
{
    /// <summary>
    /// Interakční logika pro Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {

        static string ingame = @"sound/intro.wav";
        SoundPlayer music = new SoundPlayer(ingame);

        private Frame parentFrame;
        bool paused = false;
        public Menu()
        {
            InitializeComponent();
            initializeInfo();
        }
        public Menu(Frame parentFrame) : this() {
            this.parentFrame = parentFrame;
        }

        void initializeInfo() {
            Game_pause.soundoff.Click += new RoutedEventHandler(sound_option);
            music.Play();
            Globals.lastCommandIndex = 0;
            Globals.actual_hp = 100;
            Globals.actual_dopamine = 100;
            Globals.dopamin_speed = 3000;
            Globals.sound_state = 1;
            Globals.admin = false;
        }

        void sound_option(object sender, RoutedEventArgs e) {
            if (Globals.sound_state == 1) {
                music.Play();
            } else {
                music.Stop();
            }
        }

        void Page_loaded(object sender, RoutedEventArgs e) {
            Application.Current.MainWindow.KeyDown += new KeyEventHandler(Controls);
        }

        void go_prison(object sender, RoutedEventArgs e) {
            parentFrame.Navigate(new Prison(parentFrame));
            Application.Current.MainWindow.KeyDown -= new KeyEventHandler(Controls);
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