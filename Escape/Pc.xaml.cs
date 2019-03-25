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
    /// Interakční logika pro Pc.xaml
    /// </summary>
    public partial class Pc : UserControl {


        List<string> consoleCommands = new List<string> { "help", "clear", "color", "dir" };
        Stack<string> lastConsoleComands = new Stack<string>();

        public Pc() {
            InitializeComponent();
            textinPC();
        }

        void textinPC() {
            gameConsoleInfo.Text = gameConsoleInfo.Text + "#############################################" +
                "\n#           Welcome to Prison cumputer                                   #\n" +
                "#           ver.1.2.0                                                                     #\n" +
                "#############################################\nType /help for all commands\n\n";
        }

        void Page_loaded(object sender, RoutedEventArgs e) {
            Application.Current.MainWindow.KeyDown += new KeyEventHandler(Controls);
            gameConsoleInput.PreviewKeyDown -= new KeyEventHandler(Sipky);
            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(focus_input);
        }


        void focus_input(object sender, DependencyPropertyChangedEventArgs e) {
            if ((bool)e.NewValue == true) {
                Dispatcher.BeginInvoke(
                new Action(delegate () {
                    gameConsoleInput.Focus();
                }));
            }
        }

        void Controls(object sender, KeyEventArgs e) {
            // Send
            if (e.Key == Key.Enter) {
                sendGameConsoleData();
            }
            if (e.Key == Key.Escape) {
                this.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(focus_input);
                this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(focus_input);
            }
        }

        void gameConsole_click(object sender, RoutedEventArgs e) {
            sendGameConsoleData();
        }

        void sendGameConsoleData() {
            if (gameConsoleInput.Text != "") {
                string consoleInput = gameConsoleInput.Text;
                deleteGameConsoleInput();

                char[] commandArray = consoleInput.ToCharArray();

                if (commandArray[0] == 47) {
                    if (gameConsoleCommands(new string(commandArray.Skip(1).ToArray()))) {
                        if (consoleInput != "/clear") {
                            consoleInput = "Command '" + new string(commandArray.Skip(1).ToArray()) + "' was activated\n";
                        } else {
                            gameConsoleInfo.Clear();
                            consoleInput = "";
                        }
                    } else {
                        if (consoleInput != "/color") {
                            consoleInput = "Undefined command '" + new string(commandArray.Skip(1).ToArray()) + "'\n";
                        }
                    }
                } else {
                    consoleInput = "C:/Prisoner/Desktop>"  + consoleInput + "\n";
                }

                gameConsoleInfo.Text = gameConsoleInfo.Text + consoleInput;
                gameConsoleInfo.ScrollToEnd();
            }
        }

        bool gameConsoleCommands(string command) {
            bool commandExist = false;
            if (command == "color") {
                gameConsoleInfo.Text = gameConsoleInfo.Text + "Please specify color! \n";
            } else if (command.Contains("color")) {
                commandExist = true;
                string[] barva = command.Split(' ');
                var barva_cnvrt = (Color)ColorConverter.ConvertFromString(barva[1]);
                gameConsoleInfo.Foreground = new SolidColorBrush(barva_cnvrt);
                gameConsoleInput.Foreground = new SolidColorBrush(barva_cnvrt);
                gameConsoleInput.CaretBrush = new SolidColorBrush(barva_cnvrt);
            } else if (command == "clear") {
                commandExist = true;
            }

            if (command == "help") {
                gameConsoleInfo.Text = gameConsoleInfo.Text + "################## Commands ##################\n\n";
                foreach (string consoleCommand in consoleCommands) {
                    gameConsoleInfo.Text = gameConsoleInfo.Text + "/" + consoleCommand + "\n";

                }
                gameConsoleInfo.Text = gameConsoleInfo.Text + "\n#############################################\n";
                commandExist = true;
            }
            if (commandExist) {
                lastConsoleComands.Push(command);
                Globals.lastCommandIndex = 0;
            }
            return commandExist;
        }

        void lastCommandUp() {
            try {
                gameConsoleInput.Text = "/" + lastConsoleComands.ElementAt(Globals.lastCommandIndex);

                if (Globals.lastCommandIndex + 1 < lastConsoleComands.Count) {
                    Globals.lastCommandIndex++;
                }
            } catch (Exception) {
            }
        }

        void lastCommandDown() {
            try {
                if (Globals.lastCommandIndex - 1 >= 0) {
                    Globals.lastCommandIndex--;
                }
                gameConsoleInput.Text = "/" + lastConsoleComands.ElementAt(Globals.lastCommandIndex);
            } catch (Exception) {
            }
        }

        void deleteGameConsoleInput() {
            gameConsoleInput.Clear();
        }

        private void gameConsolInfo_click(object sender, RoutedEventArgs e) {
            sendGameConsoleData();
            gameConsoleInput.Focus();
        }

        public void Sipky(object sender, KeyEventArgs e) {
            // Poslední a předchozí příkaz
            if (e.Key == Key.Up) {
                lastCommandUp();
            }

            if (e.Key == Key.Down) {
                lastCommandDown();
            }

            // Odeslání příkazu
            if (e.Key == Key.Enter) {
                sendGameConsoleData();
            }
        }
    }
}
