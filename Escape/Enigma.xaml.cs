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

namespace Escape {
    /// <summary>
    /// Interakční logika pro Enigma.xaml
    /// </summary>
    public partial class Enigma : UserControl {
        public Enigma() {
            InitializeComponent();
        }

        void red(object sender, RoutedEventArgs e) {
            first_red.Visibility = Visibility.Visible;
        }
        void green(object sender, RoutedEventArgs e) {
            last_green.Visibility = Visibility.Visible;
        }
    }
}
