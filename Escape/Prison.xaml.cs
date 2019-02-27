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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Escape {
    /// <summary>
    /// Interakční logika pro Intro.xaml
    /// </summary>
    public partial class Prison : Page {

        int lastCommandIndex = 0;
        int actual_hp = 100;
        int actual_dopamine = 100;
        int dopamin_speed = 3000;
        int sound_state = 1;
        bool admin = false;
        //Místnost 1
        int light1_state = 1;
        bool pic_l_dop = true;
        bool pic_r_dop = true;
        bool enigma = false;

        private Frame parentFrame;
        DispatcherTimer dopamin_timer = new DispatcherTimer();

        static string ingame = @"sound/ingame.wav";
        SoundPlayer music = new SoundPlayer(ingame);
        MediaPlayer switchon = new MediaPlayer();
        MediaPlayer switchoff = new MediaPlayer();

        Dictionary<string, Uri> prison_bg = new Dictionary<string, Uri>();
        Dictionary<string, Uri> light_state_img = new Dictionary<string, Uri>();

        private void initializeBG() {
            prison_bg.Add("PrisonBG1", new Uri(@"img/bg/bg_game_1.jpg", UriKind.Relative));
            prison_bg.Add("PrisonBG2", new Uri(@"img/bg/bg_game_2.jpg", UriKind.Relative));
            prison_bg.Add("PrisonBG3", new Uri(@"img/bg/bg_game_3.jpg", UriKind.Relative));
            prison_bg.Add("PrisonBG4", new Uri(@"img/bg/bg_game_4.jpg", UriKind.Relative));
        }

        private void initializeMusic() {
            switchon.Open(new Uri(@"sound/switch_on.mp3", UriKind.Relative));
            switchoff.Open(new Uri(@"sound/switch_off.mp3", UriKind.Relative));
        }

        private void initializeLights() {
            light_state_img.Add("Lightoff", new Uri(@"img/items/lights/lightoff.png", UriKind.Relative));
            light_state_img.Add("Lighton", new Uri(@"img/items/lights/lighton.png", UriKind.Relative));
            light_state_img.Add("Lightuv", new Uri(@"img/items/lights/lightuv.png", UriKind.Relative));
        }

        private void initializeFlares() {
            light_state_img.Add("FlareY", new Uri(@"img/items/flares/flareY.png", UriKind.Relative));
            light_state_img.Add("FlareUV", new Uri(@"img/items/flares/flareUV.png", UriKind.Relative));
        }

        List<string> consoleCommands = new List<string> { "help", "clear", "color" };
        List<string> consoleCommands_admin = new List<string>
        { "help", "clear", "heal", "get high", "color", "freeze", "unfreeze" , "exit" };
        Stack<string> lastConsoleComands = new Stack<string>();

        public Prison() {
            InitializeComponent();
            initializeBG();
            initializeMusic();
            initializeAll();
            initializeBars();
            initializeLights();
            initializeFlares();
            initializeRoom1();
        }
        public Prison(Frame parentFrame) : this() {
            this.parentFrame = parentFrame;
        }

        private void Page_loaded(object sender, RoutedEventArgs e) {
            Application.Current.MainWindow.KeyDown += new KeyEventHandler(Controls);
            gameConsoleInput.PreviewKeyDown += new KeyEventHandler(Sipky);
        }

        public void prvni(object sender, RoutedEventArgs e) {
            bg.Source = new BitmapImage(prison_bg["PrisonBG1"]);
        }
        public void druhy(object sender, RoutedEventArgs e) {
            bg.Source = new BitmapImage(prison_bg["PrisonBG2"]);
        }
        public void treti(object sender, RoutedEventArgs e) {
            bg.Source = new BitmapImage(prison_bg["PrisonBG3"]);
        }
        public void ctvrty(object sender, RoutedEventArgs e) {
            bg.Source = new BitmapImage(prison_bg["PrisonBG4"]);
        }

        public void sound(object sender, RoutedEventArgs e) {
            if (sound_state == 1) {
                music.Stop();
                sound_state = 0;

            } else {
                music.Play();
                sound_state = 1;
            }
        }

        void initializeAll() {
            music.Play();
        }

        void initializeBars() {
            hpBar.Minimum = 0;
            hpBar.Maximum = 100;
            hpBar.Value = 100;

            drugBar.Minimum = 0;
            drugBar.Maximum = 100;
            drugBar.Value = 100;

            informace();
            dopamin_counter();
        }

        void initializeRoom1() {
            flareone();
            item_enigma.close_enigma.Click += new RoutedEventHandler(enigma_close);
        }

        void informace() {
            hp.Content = actual_hp + "%";
            if (actual_dopamine > 100) {
                actual_dopamine = 100;
                dopamin.Content = actual_dopamine + "%";
            } else {
                dopamin.Content = actual_dopamine + "%";
            }
        }

        //Framy
        private void go_menu(object sender, RoutedEventArgs e) {
            parentFrame.Navigate(new Menu(parentFrame));
            Application.Current.MainWindow.KeyDown -= new KeyEventHandler(Controls);
            gameConsoleInput.PreviewKeyDown -= new KeyEventHandler(Sipky);
        }
        private void go_intro(object sender, RoutedEventArgs e) {
            parentFrame.Navigate(new Intro(parentFrame));
            Application.Current.MainWindow.KeyDown -= new KeyEventHandler(Controls);
            gameConsoleInput.PreviewKeyDown -= new KeyEventHandler(Sipky);
        }

        // DOPAMIN
        void dopamin_counter() {
            dopamin_timer.Interval = new TimeSpan(0, 0, 0, 0, dopamin_speed);
            dopamin_timer.Tick += new EventHandler(dopamin_Tick);
            dopamin_timer.Start();
        }

        void dopamin_Tick(object sender, EventArgs e) {
            if (actual_dopamine > 0) {
                actual_dopamine--;
            }
            drugBar.Value--;
            dopamin.Content = actual_dopamine + "%";
        }

        // Ovládání
        private void Controls(object sender, KeyEventArgs e) {
            // Pauza
            if (e.Key == Key.Escape) {
                if (Game_pause.Visibility == Visibility.Hidden) {
                    Game_pause.Visibility = Visibility.Visible;
                    dopamin_timer.Stop();
                } else {
                    Game_pause.Visibility = Visibility.Hidden;
                    dopamin_timer.Start();
                }
            }
            // Otevření konzole
            if (e.Key == Key.OemTilde) {
                if (gameConsole.Visibility == Visibility.Hidden) {
                    gameConsole.Visibility = Visibility.Visible;
                } else {
                    gameConsole.Visibility = Visibility.Hidden;
                }
            }
        }

        // -------------- Místnost 1 --------------

        void lightSwitch(object sender, RoutedEventArgs e) {
            if (light1_state == 1) {
                light1_state = 0;
                flareone();
                switchoff.Play();
                light1.Source = new BitmapImage(light_state_img["Lightoff"]);
                flare1.Source = new BitmapImage(light_state_img["FlareY"]);
                dopamin_timer.Tick -= new EventHandler(dopamin_Tick);
                dopamin_speed = 5000;
                dopamin_counter();
                dark1.Visibility = Visibility.Visible;
                pin.Visibility = Visibility.Hidden;
                flare1.Visibility = Visibility.Hidden;
            } else {
                light1_state = 1;
                flareone();
                switchon.Play();
                light1.Source = new BitmapImage(light_state_img["Lighton"]);
                flare1.Source = new BitmapImage(light_state_img["FlareY"]);
                dopamin_timer.Tick -= new EventHandler(dopamin_Tick);
                dopamin_speed = 3000;
                dopamin_counter();
                dark1.Visibility = Visibility.Hidden;
                pin.Visibility = Visibility.Hidden;
                flare1.Visibility = Visibility.Visible;
            }
        }

        void lightSwitchUv(object sender, RoutedEventArgs e) {
            if (light1_state == 0 || light1_state == 1) {
                light1_state = 2;
                flareone();
                switchon.Play();
                light1.Source = new BitmapImage(light_state_img["Lightuv"]);
                flare1.Source = new BitmapImage(light_state_img["FlareUV"]);
                dopamin_timer.Tick -= new EventHandler(dopamin_Tick);
                dopamin_speed = 500;
                dopamin_counter();
                dark1.Visibility = Visibility.Hidden;
                pin.Visibility = Visibility.Visible;
                flare1.Visibility = Visibility.Visible;
            } else {
                light1_state = 0;
                flareone();
                switchoff.Play();
                light1.Source = new BitmapImage(light_state_img["Lightoff"]);
                flare1.Source = new BitmapImage(light_state_img["FlareUV"]);
                dopamin_timer.Tick -= new EventHandler(dopamin_Tick);
                dopamin_speed = 5000;
                dopamin_counter();
                dark1.Visibility = Visibility.Visible;
                pin.Visibility = Visibility.Hidden;
                flare1.Visibility = Visibility.Hidden;
            }

        }

        public void flareone() {
            DoubleAnimation animace;
            if (light1_state == 1) {
                animace = new DoubleAnimation {
                    From = 0.8,
                    To = 0.5,
                    BeginTime = TimeSpan.FromSeconds(1),
                    Duration = TimeSpan.FromSeconds(2),
                    FillBehavior = FillBehavior.Stop
                };
                animace.AutoReverse = true;
            } else {
                animace = new DoubleAnimation {
                    From = 0.9,
                    To = 0.7,
                    BeginTime = TimeSpan.FromSeconds(0),
                    Duration = TimeSpan.FromSeconds(0.6),
                    FillBehavior = FillBehavior.Stop
                };
                animace.AutoReverse = false;
            }
            animace.RepeatBehavior = RepeatBehavior.Forever;
            flare1.BeginAnimation(UIElement.OpacityProperty, animace);
        }

        //Mona lisa

        public void openmonalisa(object sender, RoutedEventArgs e) {
            var mona = new ImageBrush();
            mona.ImageSource = new BitmapImage(new Uri(@"img/items/room1/picture_monalisa_opened.png", UriKind.Relative));
            monalisa.Background = mona;
            monalisa.BorderBrush = Brushes.Transparent;
            if (pic_r_dop) {
                rdopamin.Visibility = Visibility.Visible;
            }
            if (pic_l_dop) {
                ldopamin.Visibility = Visibility.Visible;
            }
            enigma_button.Visibility = Visibility.Visible;
        }

        public void openenigma(object sender, RoutedEventArgs e) {
            item_enigma.Visibility = Visibility.Visible;
        }
        void enigma_close(object sender, RoutedEventArgs e) {
            item_enigma.Visibility = Visibility.Hidden;
        }

        public void takerdopamin(object sender, RoutedEventArgs e) {
            rdopamin.Visibility = Visibility.Hidden;
            actual_dopamine += 15;
            informace();
            drugBar.Value = actual_dopamine;
            pic_r_dop = false;
        }

        public void takeldopamin(object sender, RoutedEventArgs e) {
            var mona = new ImageBrush();
            ldopamin.Visibility = Visibility.Hidden;
            actual_dopamine += 15;
            informace();
            drugBar.Value = actual_dopamine;
            pic_l_dop = false;
        }

        // Console
        private void gameConsole_click(object sender, RoutedEventArgs e) {
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
                            consoleInput = DateTime.Now.ToString("HH:mm") + "   " + "Command '" + new string(commandArray.Skip(1).ToArray()) + "' activated\n";
                        } else {
                            gameConsoleInfo.Clear();
                            consoleInput = "";
                        }
                    } else {
                        if (consoleInput != "/color") {
                            consoleInput = DateTime.Now.ToString("HH:mm") + "   " + "Unknown command '" + new string(commandArray.Skip(1).ToArray()) + "'\n";
                        }
                    }
                } else {
                    consoleInput = DateTime.Now.ToString("HH:mm") + "   " + consoleInput + "\n";
                }

                gameConsoleInfo.Text = gameConsoleInfo.Text + consoleInput;
                gameConsoleInfo.ScrollToEnd();
            }
        }

        bool gameConsoleCommands(string command) {
            bool commandExist = false;
            if (command == "login admin") {
                commandExist = true;
                admin = true;
            } else if (command == "logout" && admin == true) {
                commandExist = true;
                admin = false;
            } else if (command == "heal" && admin == true) {
                commandExist = true;
            } else if (command == "get high" && admin == true) {
                commandExist = true;
                actual_dopamine = 100;
                drugBar.Value = 100;
                dopamin.Content = actual_dopamine + "%";
            } else if (command == "freeze" && admin == true) {
                commandExist = true;
                dopamin_timer.Stop();
            } else if (command == "unfreeze" && admin == true) {
                commandExist = true;
                dopamin_timer.Start();
            } else if (command == "back monalisa" && admin == true) {
                var monaorig = new ImageBrush();
                monaorig.ImageSource = new BitmapImage(new Uri(@"img/items/room1/picture_monalisa.png", UriKind.Relative));
                monalisa.Background = monaorig;
                commandExist = true;
            } else if (command == "exit" && admin == true) {
                commandExist = true;
                System.Windows.Application.Current.Shutdown();
            } else if (command == "color") {
                gameConsoleInfo.Text = gameConsoleInfo.Text + DateTime.Now.ToString("HH:mm") + "   " + "Prosím specifikuj barvu! \n";
            } else if (command.Contains("color")) {
                commandExist = true;
                string[] barva = command.Split(' ');
                var barva_cnvrt = (Color)ColorConverter.ConvertFromString(barva[1]);
                gameConsoleInfo.Foreground = new SolidColorBrush(barva_cnvrt);
                gameConsoleInput.Foreground = new SolidColorBrush(barva_cnvrt);
                gameConsoleButton.Foreground = new SolidColorBrush(barva_cnvrt);
            } else if (command == "clear") {
                commandExist = true;
            }

            if (command == "help") {
                gameConsoleInfo.Text = gameConsoleInfo.Text + "######## Příkazy ########\n\n";
                if (admin == false) {
                    foreach (string consoleCommand in consoleCommands) {
                        gameConsoleInfo.Text = gameConsoleInfo.Text + "/" + consoleCommand + "\n";
                    }
                } else {
                    //Pro admina
                    int mezera_commandy = -1;
                    int mezera_commandy_last = 0;
                    foreach (string consoleCommand in consoleCommands_admin) {
                        if (mezera_commandy_last == consoleCommands_admin.Count() - 1) {
                            gameConsoleInfo.Text = gameConsoleInfo.Text + consoleCommand + "\n";
                        } else {
                            if (mezera_commandy == 3) {
                                gameConsoleInfo.Text = gameConsoleInfo.Text + "\n";
                                gameConsoleInfo.Text = gameConsoleInfo.Text + "/" + consoleCommand + " , ";
                                mezera_commandy = 0;
                                mezera_commandy_last++;
                            } else {
                                gameConsoleInfo.Text = gameConsoleInfo.Text + "/" + consoleCommand + " , ";
                                mezera_commandy++;
                                mezera_commandy_last++;
                            }
                        }
                    }
                }
                gameConsoleInfo.Text = gameConsoleInfo.Text + "\n##########################\n";
                commandExist = true;
            }
            if (commandExist) {
                lastConsoleComands.Push(command);
                lastCommandIndex = 0;
            }
            return commandExist;
        }

        void lastCommandUp() {
            try {
                gameConsoleInput.Text = "/" + lastConsoleComands.ElementAt(lastCommandIndex);

                if (lastCommandIndex + 1 < lastConsoleComands.Count) {
                    lastCommandIndex++;
                }
            } catch (Exception) {
            }
        }

        void lastCommandDown() {
            try {
                if (lastCommandIndex - 1 >= 0) {
                    lastCommandIndex--;
                }
                gameConsoleInput.Text = "/" + lastConsoleComands.ElementAt(lastCommandIndex);
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
