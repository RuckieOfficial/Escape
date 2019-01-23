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

        List<string> consoleCommands = new List<string> { "help", "clear", "heal", "get high" };
        List<string> last_command = new List<string>();

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

        //Framy
        private void go_menu(object sender, RoutedEventArgs e) {
            parentFrame.Navigate(new Menu(parentFrame));
            Application.Current.MainWindow.KeyDown -= new KeyEventHandler(Controls);
        }
        private void go_intro(object sender, RoutedEventArgs e) {
            parentFrame.Navigate(new Intro(parentFrame));
            Application.Current.MainWindow.KeyDown -= new KeyEventHandler(Controls);
        }
        private void Controls(object sender, KeyEventArgs e) {
            //Pauza
            if (e.Key == Key.Escape) {
                if (Game_pause.Visibility == Visibility.Hidden) {
                    Game_pause.Visibility = Visibility.Visible;
                } else {
                    Game_pause.Visibility = Visibility.Hidden;
                }
            }
            //Otevření konzole
            if (e.Key == Key.OemTilde) {
                if (gameConsole.Visibility == Visibility.Hidden) {
                    last_command = new List<string>();
                    gameConsole.Visibility = Visibility.Visible;
                } else {
                    gameConsole.Visibility = Visibility.Hidden;
                }
            }
            //Odeslání příkazu
            if (e.Key == Key.Enter) {

                sendGameConsoleData();
            }
            if (gameConsole.Visibility == Visibility.Visible && (e.Key == Key.F1)) {
                foreach (var item in last_command) {
                }
                if(last_command.Count() > 0) {
                    gameConsoleInput.Text = last_command[last_command.Count - 1];
                }
            }
        }
        // CONSOLE
        private void gameConsole_click(object sender, RoutedEventArgs e) {
            last_command.Add(gameConsoleInput.Text);
            sendGameConsoleData();
        }

        bool gameConsoleCommands(string command) {
            bool commandExist = false;
            if (command == "clear") {
                commandExist = true;
            } else if (command == "heal") {
                commandExist = true;
            } else if (command == "get high") {
                commandExist = true;
                actual_dopamine = 100;
            } else if (command == "exit") {
                commandExist = true;
                System.Windows.Application.Current.Shutdown();
            }
            if (command == "help") {
                gameConsoleInfo.Text = gameConsoleInfo.Text + "######## Commands ########\n\n";
                foreach (string consoleCommand in consoleCommands) {
                    gameConsoleInfo.Text = gameConsoleInfo.Text + "/" + consoleCommand + "\n";
                }
                gameConsoleInfo.Text = gameConsoleInfo.Text + "\n##########################\n";
                commandExist = true;
            }

            return commandExist;
        }

        void sendGameConsoleData() {
            if (gameConsoleInput.Text != "") {
                string conseleInput = gameConsoleInput.Text;
                deleteGameConsoleInput();

                char[] commandArray = conseleInput.ToCharArray();

                if (commandArray[0] == 47) {
                    if (gameConsoleCommands(new string(commandArray.Skip(1).ToArray()))) {
                        if (conseleInput != "/clear") {
                            conseleInput = "Command '" + new string(commandArray.Skip(1).ToArray()) + "' activated\n";
                        } else {
                            gameConsoleInfo.Clear();
                            conseleInput = "";
                        }
                    } else {
                        conseleInput = "Unknown command '" + new string(commandArray.Skip(1).ToArray()) + "'\n";
                    }
                } else {
                    conseleInput = conseleInput + "\n";
                }

                gameConsoleInfo.Text = gameConsoleInfo.Text + conseleInput;
                gameConsoleInfo.ScrollToEnd();
            }
        }

        void deleteGameConsoleInput() {
            gameConsoleInput.Clear();
        }

        private void gameConsolInfo_click(object sender, RoutedEventArgs e) {
            sendGameConsoleData();
            gameConsoleInput.Focus();
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
