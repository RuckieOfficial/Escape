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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Escape {
    /// <summary>
    /// Interakční logika pro Enigma.xaml
    /// </summary>
    public partial class Enigma : UserControl {

        int click_count = 0;
        int led_pos = 1;
        string word = null;
        bool validate = true;
        public bool success = false;

        DispatcherTimer reset_timer = new DispatcherTimer();
        DispatcherTimer leds = new DispatcherTimer();

        public Enigma() {
            InitializeComponent();
            setintervals();
        }

        void setintervals() {
            reset_timer.Interval = new TimeSpan(0, 0, 0, 0, 4000);
            reset_timer.Tick += new EventHandler(reset);
            leds.Interval = TimeSpan.FromMilliseconds(500);
            leds.Tick += new EventHandler(leds_animation);
        }

        void validator() {
            if (click_count == 4) {
                if (word != "help") {
                    validate = false;
                }
                leds.Start();
                reset_timer.Start();
                word = null;
            }
        }

        void one(object sender, RoutedEventArgs e) {
            letter_p.Template = FindResource("circle_active") as ControlTemplate;
            letter_p.Foreground = Brushes.Black;
            word += "p";
            click_count++;
            validator();
        }
        void two(object sender, RoutedEventArgs e) {
            letter_e.Template = FindResource("circle_active") as ControlTemplate;
            letter_e.Foreground = Brushes.Black;
            word += "e";
            click_count++;
            validator();
        }
        void three(object sender, RoutedEventArgs e) {
            letter_l.Template = FindResource("circle_active") as ControlTemplate;
            letter_l.Foreground = Brushes.Black;
            word += "l";
            click_count++;
            validator();
        }
        void four(object sender, RoutedEventArgs e) {
            letter_b.Template = FindResource("circle_active") as ControlTemplate;
            letter_b.Foreground = Brushes.Black;
            letter_n.Template = FindResource("circle_active") as ControlTemplate;
            letter_n.Foreground = Brushes.Black;
            letter_j.Template = FindResource("circle_active") as ControlTemplate;
            letter_j.Foreground = Brushes.Black;
            click_count++;
            validate = false;
            validator();
        }
        void five(object sender, RoutedEventArgs e) {
            letter_a.Template = FindResource("circle_active") as ControlTemplate;
            letter_a.Foreground = Brushes.Black;
            letter_o.Template = FindResource("circle_active") as ControlTemplate;
            letter_o.Foreground = Brushes.Black;
            click_count++;
            validate = false;
            validator();
        }
        void six(object sender, RoutedEventArgs e) {
            letter_u.Template = FindResource("circle_active") as ControlTemplate;
            letter_u.Foreground = Brushes.Black;
            click_count++;
            validate = false;
            validator();
        }
        void seven(object sender, RoutedEventArgs e) {
            letter_d.Template = FindResource("circle_active") as ControlTemplate;
            letter_d.Foreground = Brushes.Black;
            letter_p.Template = FindResource("circle_active") as ControlTemplate;
            letter_p.Foreground = Brushes.Black;
            letter_i.Template = FindResource("circle_active") as ControlTemplate;
            letter_i.Foreground = Brushes.Black;
            click_count++;
            validate = false;
            validator();
        }
        void eight(object sender, RoutedEventArgs e) {
            letter_h.Template = FindResource("circle_active") as ControlTemplate;
            letter_h.Foreground = Brushes.Black;
            word += "h";
            click_count++;
            validator();
        }
        void nine(object sender, RoutedEventArgs e) {
            letter_k.Template = FindResource("circle_active") as ControlTemplate;
            letter_k.Foreground = Brushes.Black;
            click_count++;
            validate = false;
            validator();
        }

        void leds_animation(object sender, EventArgs e) {
            if (validate) {
                if (led_pos == 1) {
                    first_green.Visibility = Visibility.Visible;
                }
                if (led_pos == 2) {
                    second_green.Visibility = Visibility.Visible;
                }
                if (led_pos == 3) {
                    third_green.Visibility = Visibility.Visible;
                }
                if (led_pos == 4) {
                    fourth_green.Visibility = Visibility.Visible;
                }
                if (led_pos == 5) {
                    first_green.Visibility = Visibility.Hidden;
                    second_green.Visibility = Visibility.Hidden;
                    third_green.Visibility = Visibility.Hidden;
                    fourth_green.Visibility = Visibility.Hidden;
                }
                if (led_pos == 6) {
                    first_green.Visibility = Visibility.Visible;
                    second_green.Visibility = Visibility.Visible;
                    third_green.Visibility = Visibility.Visible;
                    fourth_green.Visibility = Visibility.Visible;
                }
                if (led_pos == 7) {
                    first_green.Visibility = Visibility.Hidden;
                    second_green.Visibility = Visibility.Hidden;
                    third_green.Visibility = Visibility.Hidden;
                    fourth_green.Visibility = Visibility.Hidden;
                }
                if (led_pos == 8) {
                    led_pos = 0;
                    validate = true;
                    leds.Stop();
                    success = true;
                    this.Visibility = Visibility.Hidden;

                }
            } else {
                if (led_pos == 1) {
                    first_red.Visibility = Visibility.Visible;
                }
                if (led_pos == 2) {
                    second_red.Visibility = Visibility.Visible;
                }
                if (led_pos == 3) {
                    third_red.Visibility = Visibility.Visible;
                }
                if (led_pos == 4) {
                    fourth_red.Visibility = Visibility.Visible;
                }
                if (led_pos == 5) {
                    first_red.Visibility = Visibility.Hidden;
                    second_red.Visibility = Visibility.Hidden;
                    third_red.Visibility = Visibility.Hidden;
                    fourth_red.Visibility = Visibility.Hidden;
                }
                if (led_pos == 6) {
                    first_red.Visibility = Visibility.Visible;
                    second_red.Visibility = Visibility.Visible;
                    third_red.Visibility = Visibility.Visible;
                    fourth_red.Visibility = Visibility.Visible;
                }
                if (led_pos == 7) {
                    first_red.Visibility = Visibility.Hidden;
                    second_red.Visibility = Visibility.Hidden;
                    third_red.Visibility = Visibility.Hidden;
                    fourth_red.Visibility = Visibility.Hidden;
                }
                if (led_pos == 8) {
                    led_pos = 0;
                    validate = true;
                    leds.Stop();
                    this.Visibility = Visibility.Hidden;
                }
            }
            led_pos++;
        }

            void reset(object sender, EventArgs e) {
            letter_q.Template = FindResource("circle") as ControlTemplate;
            letter_q.Foreground = Brushes.White;
            letter_w.Template = FindResource("circle") as ControlTemplate;
            letter_w.Foreground = Brushes.White;
            letter_e.Template = FindResource("circle") as ControlTemplate;
            letter_e.Foreground = Brushes.White;
            letter_r.Template = FindResource("circle") as ControlTemplate;
            letter_r.Foreground = Brushes.White;
            letter_t.Template = FindResource("circle") as ControlTemplate;
            letter_t.Foreground = Brushes.White;
            letter_z.Template = FindResource("circle") as ControlTemplate;
            letter_z.Foreground = Brushes.White;
            letter_u.Template = FindResource("circle") as ControlTemplate;
            letter_u.Foreground = Brushes.White;
            letter_i.Template = FindResource("circle") as ControlTemplate;
            letter_i.Foreground = Brushes.White;
            letter_o.Template = FindResource("circle") as ControlTemplate;
            letter_o.Foreground = Brushes.White;
            letter_p.Template = FindResource("circle") as ControlTemplate;
            letter_p.Foreground = Brushes.White;
            letter_a.Template = FindResource("circle") as ControlTemplate;
            letter_a.Foreground = Brushes.White;
            letter_s.Template = FindResource("circle") as ControlTemplate;
            letter_s.Foreground = Brushes.White;
            letter_d.Template = FindResource("circle") as ControlTemplate;
            letter_d.Foreground = Brushes.White;
            letter_f.Template = FindResource("circle") as ControlTemplate;
            letter_f.Foreground = Brushes.White;
            letter_g.Template = FindResource("circle") as ControlTemplate;
            letter_g.Foreground = Brushes.White;
            letter_h.Template = FindResource("circle") as ControlTemplate;
            letter_h.Foreground = Brushes.White;
            letter_j.Template = FindResource("circle") as ControlTemplate;
            letter_j.Foreground = Brushes.White;
            letter_k.Template = FindResource("circle") as ControlTemplate;
            letter_k.Foreground = Brushes.White;
            letter_l.Template = FindResource("circle") as ControlTemplate;
            letter_l.Foreground = Brushes.White;
            letter_y.Template = FindResource("circle") as ControlTemplate;
            letter_y.Foreground = Brushes.White;
            letter_x.Template = FindResource("circle") as ControlTemplate;
            letter_x.Foreground = Brushes.White;
            letter_c.Template = FindResource("circle") as ControlTemplate;
            letter_c.Foreground = Brushes.White;
            letter_v.Template = FindResource("circle") as ControlTemplate;
            letter_v.Foreground = Brushes.White;
            letter_b.Template = FindResource("circle") as ControlTemplate;
            letter_b.Foreground = Brushes.White;
            letter_n.Template = FindResource("circle") as ControlTemplate;
            letter_n.Foreground = Brushes.White;
            letter_m.Template = FindResource("circle") as ControlTemplate;
            letter_m.Foreground = Brushes.White;
            click_count = 0;
            reset_timer.Stop();
        }
    }
}
