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
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            initializeInfo();
            myFrame.Navigate(new Prison(myFrame));
        }
        void initializeInfo() {
            Globals.lastCommandIndex = 0;
            Globals.actual_hp = 100;
            Globals.actual_dopamine = 100;
            Globals.dopamin_speed = 3000;
            Globals.sound_state = 1;
            Globals.admin = false;
        }
    }
}
