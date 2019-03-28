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

namespace Escape
{
    /// <summary>
    /// Interakční logika pro Lose.xaml
    /// </summary>
    public partial class Lose : Page
    {
        private Frame parentFrame;

        public Lose()
        {
            InitializeComponent();
        }

        public Lose(Frame parentFrame) : this() {
            this.parentFrame = parentFrame;
        }
    }
}
