using System;
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
using System.Windows.Threading;

namespace Escape {
    /// <summary>
    /// Interakční logika pro Intro.xaml
    /// </summary>
    public partial class Prison : Page {

        int actual_hp = 100;
        int actual_dopamine = 100;
        private Frame parentFrame;
        DispatcherTimer dopamin_timer = new DispatcherTimer();

        bool paused = false;
        public Prison() {
            InitializeComponent();
            informace();
            dopamin_counter();
        }
        public Prison(Frame parentFrame) : this() {
            this.parentFrame = parentFrame;
        }

        private void Page_loaded(object sender, RoutedEventArgs e) {
            Application.Current.MainWindow.KeyDown += new KeyEventHandler(Controls);
        }

        void informace() {
            hp.Content = actual_hp + "%";
            dopamin.Content = actual_dopamine + "%";
        }

        private void go_menu(object sender, RoutedEventArgs e) {
            parentFrame.Navigate(new Menu(parentFrame));
            Application.Current.MainWindow.KeyDown -= new KeyEventHandler(Controls);
        }
        private void go_intro(object sender, RoutedEventArgs e) {
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

        void dopamin_counter() {
            dopamin_timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            dopamin_timer.Tick += new EventHandler(dopamin_Tick);
            dopamin_timer.Start();
        }

        void dopamin_Tick(object sender, EventArgs e) {
            actual_dopamine--;
            dopamin.Content = actual_dopamine + "%";
        }
    }
}
