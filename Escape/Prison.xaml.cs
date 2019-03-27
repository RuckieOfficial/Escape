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

        //Místnost 1
        int light1_state = 0;
        int light2_state = 0;
        bool pic_l_dop = true;
        bool pic_r_dop = true;
        bool enigma_opened = false;
        bool flashlight = false;

        private Frame parentFrame;
        DispatcherTimer dopamin_timer = new DispatcherTimer();
        DispatcherTimer pc_timer = new DispatcherTimer();

        static string ingame = @"sound/ingame.wav";
        SoundPlayer music = new SoundPlayer(ingame);
        MediaPlayer switchon = new MediaPlayer();
        MediaPlayer switchoff = new MediaPlayer();
        MediaPlayer pcnoise = new MediaPlayer();
        MediaPlayer pcswitch = new MediaPlayer();

        Dictionary<string, Uri> prison_bg = new Dictionary<string, Uri>();
        Dictionary<string, Uri> light_state_img = new Dictionary<string, Uri>();

        private void initializeBG() {
            prison_bg.Add("PrisonBG1", new Uri(@"img/bg/bg_game_1.jpg", UriKind.Relative));
            prison_bg.Add("PrisonBG2", new Uri(@"img/bg/bg_game_2.jpg", UriKind.Relative));
            prison_bg.Add("PrisonBG3", new Uri(@"img/bg/bg_game_3.jpg", UriKind.Relative));
            prison_bg.Add("PrisonBG4", new Uri(@"img/bg/bg_game_4.jpg", UriKind.Relative));
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
            initializeAll();
            initializeBars();
            initializeLights();
            initializeFlares();
            initializeRoom1();
            initializeRoom2();
            initializeRoom3();
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
            room2.Visibility = Visibility.Hidden;
            room3.Visibility = Visibility.Hidden;
            room1.Visibility = Visibility.Visible;
        }
        public void druhy(object sender, RoutedEventArgs e) {
            bg.Source = new BitmapImage(prison_bg["PrisonBG2"]);
            pcnoise.Open(new Uri(@"sound/pc_noise.mp3", UriKind.Relative));
            pcnoise.Play();
            dopamin_timer.Tick -= new EventHandler(dopamin_Tick);
            Globals.dopamin_speed = 5000;
            dopamin_counter();
            room1.Visibility = Visibility.Hidden;
            room3.Visibility = Visibility.Hidden;
            room2.Visibility = Visibility.Visible;
        }
        public void treti(object sender, RoutedEventArgs e) {
            bg.Source = new BitmapImage(prison_bg["PrisonBG3"]);
            room1.Visibility = Visibility.Hidden;
            room2.Visibility = Visibility.Hidden;
            room3.Visibility = Visibility.Visible;
            if (flashlight == true) {
                room3_light.Visibility = Visibility.Visible;
                room3_nolight.Visibility = Visibility.Hidden;
            } else {
                room3_nolight.Visibility = Visibility.Visible;
            }
        }

        public void treti2() {
            pc_code.IsVisibleChanged -= _IsVisiblePcChanged;
            bg.Source = new BitmapImage(prison_bg["PrisonBG3"]);
            room1.Visibility = Visibility.Hidden;
            room2.Visibility = Visibility.Hidden;
            room3.Visibility = Visibility.Visible;
            if (flashlight == true) {
                room3_light.Visibility = Visibility.Visible;
                room3_nolight.Visibility = Visibility.Hidden;
            } else {
                room3_nolight.Visibility = Visibility.Visible;
            }
        }

        public void ctvrty(object sender, RoutedEventArgs e) {
            bg.Source = new BitmapImage(prison_bg["PrisonBG4"]);
        }

        public void sound(object sender, RoutedEventArgs e) {
            if (Globals.sound_state == 1) {
                music.Stop();
                Globals.sound_state = 0;

            } else {
                music.Play();
                Globals.sound_state = 1;
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
            item_enigma_opened.close_enigma.Click += new RoutedEventHandler(enigma_opened_close);
            item_enigma_opened.dopaminL.Click += new RoutedEventHandler(enigma_dopamin_l);
            item_enigma_opened.dopaminM.Click += new RoutedEventHandler(enigma_dopamin_m);
            item_enigma_opened.dopaminR.Click += new RoutedEventHandler(enigma_dopamin_r);
            item_enigma_opened.enigmaKEY.Click += new RoutedEventHandler(enigma_Key);
        }

        void initializeRoom2() {
            flaretwo();
        }

        void initializeRoom3() {
            flarethree();
            pc_code.IsVisibleChanged += _IsVisiblePcChanged;
        }

    void informace() {
            hp.Content = Globals.actual_hp + "%";
            if (Globals.actual_dopamine > 100) {
                Globals.actual_dopamine = 100;
                dopamin.Content = Globals.actual_dopamine + "%";
            } else {
                dopamin.Content = Globals.actual_dopamine + "%";
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
            dopamin_timer.Interval = new TimeSpan(0, 0, 0, 0, Globals.dopamin_speed);
            dopamin_timer.Tick += new EventHandler(dopamin_Tick);
            dopamin_timer.Start();
        }

        void dopamin_Tick(object sender, EventArgs e) {
            if (Globals.actual_dopamine > 0) {
                Globals.actual_dopamine--;
            } else {
                Globals.actual_hp--;
                hpBar.Value--;
                hp.Content = Globals.actual_hp + "%";
            }
            drugBar.Value--;
            dopamin.Content = Globals.actual_dopamine + "%";
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
                switchoff.Open(new Uri(@"sound/switch_off.mp3", UriKind.Relative));
                switchoff.Play();
                light1.Source = new BitmapImage(light_state_img["Lightoff"]);
                flare1.Source = new BitmapImage(light_state_img["FlareY"]);
                dopamin_timer.Tick -= new EventHandler(dopamin_Tick);
                Globals.dopamin_speed = 5000;
                dopamin_counter();
                dark1.Visibility = Visibility.Visible;
                pin.Visibility = Visibility.Hidden;
                hand.Visibility = Visibility.Hidden;
                flare1.Visibility = Visibility.Hidden;
            } else {
                light1_state = 1;
                flareone();
                switchon.Open(new Uri(@"sound/switch_on.mp3", UriKind.Relative));
                switchon.Play();
                light1.Source = new BitmapImage(light_state_img["Lighton"]);
                flare1.Source = new BitmapImage(light_state_img["FlareY"]);
                dopamin_timer.Tick -= new EventHandler(dopamin_Tick);
                Globals.dopamin_speed = 3000;
                dopamin_counter();
                dark1.Visibility = Visibility.Hidden;
                pin.Visibility = Visibility.Hidden;
                hand.Visibility = Visibility.Hidden;
                flare1.Visibility = Visibility.Visible;
            }
        }

        void lightSwitchUv(object sender, RoutedEventArgs e) {
            if (light1_state == 1) {
                light1_state = 2;
                flareone();
                switchon.Open(new Uri(@"sound/switch_on.mp3", UriKind.Relative));
                switchon.Play();
                light1.Source = new BitmapImage(light_state_img["Lightuv"]);
                flare1.Source = new BitmapImage(light_state_img["FlareUV"]);
                dopamin_timer.Tick -= new EventHandler(dopamin_Tick);
                Globals.dopamin_speed = 500;
                dopamin_counter();
                dark1.Visibility = Visibility.Hidden;
                pin.Visibility = Visibility.Visible;
                hand.Visibility = Visibility.Visible;
                flare1.Visibility = Visibility.Visible;
            } else {
                light1_state = 0;
                flareone();
                switchoff.Open(new Uri(@"sound/switch_off.mp3", UriKind.Relative));
                switchoff.Play();
                light1.Source = new BitmapImage(light_state_img["Lightoff"]);
                flare1.Source = new BitmapImage(light_state_img["FlareUV"]);
                dopamin_timer.Tick -= new EventHandler(dopamin_Tick);
                Globals.dopamin_speed = 5000;
                dopamin_counter();
                dark1.Visibility = Visibility.Visible;
                pin.Visibility = Visibility.Hidden;
                hand.Visibility = Visibility.Hidden;
                flare1.Visibility = Visibility.Hidden;
            }

        }

        void flareone() {
            DoubleAnimation animace;
            if (light1_state == 1) {
                animace = new DoubleAnimation {
                    From = 0.8,
                    To = 0.5,
                    BeginTime = TimeSpan.FromSeconds(0),
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
            pin.BeginAnimation(UIElement.OpacityProperty, animace);
            hand.BeginAnimation(UIElement.OpacityProperty, animace);
        }

        //Mona lisa

        void openmonalisa(object sender, RoutedEventArgs e) {
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

        void openenigma(object sender, RoutedEventArgs e) {
            light1_state = 0;
            if (!enigma_opened) {
                item_enigma.Visibility = Visibility.Visible;
                item_enigma.IsVisibleChanged += _IsVisibleChanged;
            } else {
                item_enigma_opened.Visibility = Visibility.Visible;
            }
        }
        void enigma_close(object sender, RoutedEventArgs e) {
            item_enigma.Visibility = Visibility.Hidden;
        }
        void enigma_opened_close(object sender, RoutedEventArgs e) {
            item_enigma_opened.Visibility = Visibility.Hidden;
        }

        void enigma_dopamin_l(object sender, RoutedEventArgs e) {
            item_enigma_opened.dopaminL.Visibility = Visibility.Hidden;
            Globals.actual_dopamine += 5;
            informace();
            drugBar.Value = Globals.actual_dopamine;
        }
        void enigma_dopamin_m(object sender, RoutedEventArgs e) {
            item_enigma_opened.dopaminM.Visibility = Visibility.Hidden;
            Globals.actual_dopamine += 5;
            informace();
            drugBar.Value = Globals.actual_dopamine;
        }
        void enigma_dopamin_r(object sender, RoutedEventArgs e) {
            item_enigma_opened.dopaminR.Visibility = Visibility.Hidden;
            Globals.actual_dopamine += 5;
            informace();
            drugBar.Value = Globals.actual_dopamine;
        }
        void enigma_Key(object sender, RoutedEventArgs e) {
            item_enigma_opened.enigmaKEY.Visibility = Visibility.Hidden;
            druhy((Button)sender, e);
        }

        void _IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
            if (!((bool)e.NewValue)) {
                if (item_enigma.success) {
                    item_enigma_opened.Visibility = Visibility.Visible;
                    enigma_opened = true;
                } else {
                    Globals.actual_hp -= 15;
                    hpBar.Value = Globals.actual_hp;
                    informace();
                }
            }
            item_enigma.IsVisibleChanged -= _IsVisibleChanged;
        }

        void takerdopamin(object sender, RoutedEventArgs e) {
            rdopamin.Visibility = Visibility.Hidden;
            Globals.actual_dopamine += 5;
            informace();
            drugBar.Value = Globals.actual_dopamine;
            pic_r_dop = false;
        }

        void takeldopamin(object sender, RoutedEventArgs e) {
            var mona = new ImageBrush();
            ldopamin.Visibility = Visibility.Hidden;
            Globals.actual_dopamine += 5;
            informace();
            drugBar.Value = Globals.actual_dopamine;
            pic_l_dop = false;
        }

        // -------------- Místnost 2 --------------
        void pc(object sender, RoutedEventArgs e) {
            if (light2_state == 0) {
                pcswitch.Open(new Uri(@"sound/pc_on.mp3", UriKind.Relative));
                pcswitch.Play();
                dark2.Opacity = 0.2;
                light2_state = 1;
                pcnoise.Stop();
                monitor.IsEnabled = true;
                flash.Visibility = Visibility.Visible;
                flare2_off.Visibility = Visibility.Hidden;
                flare2.Visibility = Visibility.Visible;
                table_off.Visibility = Visibility.Hidden;
                table_on.Visibility = Visibility.Visible;
                flaretwo();
            } else {
                pcswitch.Open(new Uri(@"sound/pc_off.mp3", UriKind.Relative));
                pcswitch.Play();
                dark2.Opacity = 0.5;
                light2_state = 0;
                pcnoise.Open(new Uri(@"sound/pc_noise.mp3", UriKind.Relative));
                pcnoise.Play();
                monitor.IsEnabled = false;
                flare2.Visibility = Visibility.Hidden;
                flare2_off.Visibility = Visibility.Visible;
                table_on.Visibility = Visibility.Hidden;
                table_off.Visibility = Visibility.Visible;
                flaretwo();
            }
        }

        void use_pc(object sender, RoutedEventArgs e) {
            pc_code.Visibility = Visibility.Visible;
            pc_counter();
        }

        void pc_counter() {
            pc_timer.Interval = new TimeSpan(0, 0, 0, 0, 15000);
            pc_timer.Tick += new EventHandler(pc_Tick);
            pc_timer.Start();
        }

        void pc_Tick(object sender, EventArgs e) {
            pc_code.Visibility = Visibility.Hidden;
            Globals.actual_hp -= 15;
            hpBar.Value = Globals.actual_hp;
            informace();
            pc_timer.Tick -= new EventHandler(pc_Tick);
        }

        void flaretwo() {
            DoubleAnimation animace;
            if (light2_state == 1) {
                animace = new DoubleAnimation {
                    From = 0.25,
                    To = 0.2,
                    BeginTime = TimeSpan.FromSeconds(0),
                    Duration = TimeSpan.FromSeconds(0.5),
                    FillBehavior = FillBehavior.Stop
                };
                animace.AutoReverse = true;
            } else {
                animace = new DoubleAnimation {
                    From = 0.005,
                    To = 0.01,
                    BeginTime = TimeSpan.FromSeconds(0),
                    Duration = TimeSpan.FromSeconds(0.1),
                    FillBehavior = FillBehavior.Stop
                };
                animace.AutoReverse = true;
            }
            animace.RepeatBehavior = RepeatBehavior.Forever;
            flare2.BeginAnimation(UIElement.OpacityProperty, animace);
            flare2_off.BeginAnimation(UIElement.OpacityProperty, animace);
        }

        // -------------- Místnost 3 --------------
        void flarethree() {
            DoubleAnimation animace;
                animace = new DoubleAnimation {
                    From = 0.8,
                    To = 0.5,
                    BeginTime = TimeSpan.FromSeconds(0),
                    Duration = TimeSpan.FromSeconds(3),
                    FillBehavior = FillBehavior.Stop
                };
                animace.AutoReverse = false;
                animace.RepeatBehavior = RepeatBehavior.Forever;
                flare3.BeginAnimation(UIElement.OpacityProperty, animace);
        }

        void pickup_flash(object sender, RoutedEventArgs e) {
            flashlight = true;
            flash.Visibility = Visibility.Hidden;
        }

        void _IsVisiblePcChanged(object sender, DependencyPropertyChangedEventArgs e) {
            if (!((bool)e.NewValue)) {
                if (pc_code.sucess) {
                    pc_code.Visibility = Visibility.Hidden;
                    room2_next.Visibility = Visibility.Visible;
                    pc_timer.Tick -= new EventHandler(pc_Tick);
                    treti2();
                }
            }
        }

        void room3_back(object sender, RoutedEventArgs e) {
            druhy((Button)sender, e);
        }

        void room2_next_btn(object sender, RoutedEventArgs e) {
            treti2();
        }

        // Console
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
                            consoleInput = DateTime.Now.ToString("HH:mm") + "   " + "Příkaz '" + new string(commandArray.Skip(1).ToArray()) + "' byl aktivován\n";
                        } else {
                            gameConsoleInfo.Clear();
                            consoleInput = "";
                        }
                    } else {
                        if (consoleInput != "/color") {
                            consoleInput = DateTime.Now.ToString("HH:mm") + "   " + "Neznámý příkaz '" + new string(commandArray.Skip(1).ToArray()) + "'\n";
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
                Globals.admin = true;
            } else if (command == "logout" && Globals.admin == true) {
                commandExist = true;
                Globals.admin = false;
            } else if (command == "heal" && Globals.admin == true) {
                commandExist = true;
            } else if (command == "get high" && Globals.admin == true) {
                commandExist = true;
                Globals.actual_dopamine = 100;
                drugBar.Value = Globals.actual_dopamine;
                dopamin.Content = Globals.actual_dopamine + "%";
            } else if (command == "freeze" && Globals.admin == true) {
                commandExist = true;
                dopamin_timer.Stop();
            } else if (command == "unfreeze" && Globals.admin == true) {
                commandExist = true;
                dopamin_timer.Start();
            } else if (command == "flashlight" && Globals.admin == true) {
                commandExist = true;
                flashlight = true;
                if (room3.Visibility == Visibility.Visible) {
                    room3_light.Visibility = Visibility.Visible;
                }
            } else if (command == "back monalisa" && Globals.admin == true) {
                var monaorig = new ImageBrush();
                monaorig.ImageSource = new BitmapImage(new Uri(@"img/items/room1/picture_monalisa.png", UriKind.Relative));
                monalisa.Background = monaorig;
                commandExist = true;
            } else if (command == "exit" && Globals.admin == true) {
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
                gameConsoleInfo.Text = gameConsoleInfo.Text + "########## Příkazy ##########\n\n";
                if (Globals.admin == false) {
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
