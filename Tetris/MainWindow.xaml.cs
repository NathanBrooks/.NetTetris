using System;
using System.IO;
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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Tetris.GameState;
using System.Windows.Threading;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer Timer = new DispatcherTimer();

        private TetrisGameManager game;
        private Boolean GameOver;
        Rectangle overlay = new Rectangle();
        TextBlock pause = new TextBlock();
        TextBlock gameover = new TextBlock();

        private RoutedCommand rNewGame;
        private RoutedCommand rSave;
        private RoutedCommand rLoad;
        private RoutedCommand rPause;
        private RoutedCommand rPlay;

        private bool paused;

        public MainWindow()
        {
            InitializeComponent();
            initializeKeyBindings();
            game = new TetrisGameManager(0, 1, ref GameCanvas);
            GameOver = false;
            this.KeyDown += MainWindow_KeyDown;
            this.Timer.Tick += Timer_Tick;
            this.Timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            this.score_txt.Text = game.Score.ToString();
            this.level_txt.Text = game.Level.ToString();
            this.lvl_txt.Text = game.TotalLines.ToString();
            paused = true;
        }

        private void initializeKeyBindings()
        {
            rPlay = new RoutedCommand();
            rPlay.InputGestures.Add(new KeyGesture(Key.G, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(rPlay, start_btn_Click));

            rPause = new RoutedCommand();
            rPause.InputGestures.Add(new KeyGesture(Key.P, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(rPause, pause_btn_Click));

            rSave = new RoutedCommand();
            rSave.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(rSave, save_btn_Click));

            rLoad = new RoutedCommand();
            rLoad.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(rLoad, load_btn_Click));

            rNewGame = new RoutedCommand();
            rNewGame.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            // CommandBindings.Add(new CommandBinding(rNewGame, startnewgame_click));
        }

        private void Timer_Tick(object sender, EventArgs e)
        { 
            if(!GameOver)
            {
                Update();
            } 
            else
            {
                Timer.Stop();
                gameover.Text = "Game Over";
                gameover.FontSize = 30;
                gameover.Foreground = Brushes.White;
                gameover.Margin = new Thickness(78, 250, 0, 0);
                overlay.Height = 540;
                overlay.Width = 300;
                Brush test = new SolidColorBrush(Color.FromArgb(200, 11, 11, 11));
                overlay.Fill = test;
                GameCanvas.Children.Add(overlay);
                GameCanvas.Children.Add(gameover);
            }
        }

        private void Update()
        {
            int previouslevel = game.Level;
            GameOver = !game.Tick();
            this.score_txt.Text = game.Score.ToString();
            this.level_txt.Text = game.Level.ToString();
            this.lvl_txt.Text = game.TotalLines.ToString();

            if (previouslevel != game.Level)
            {
                Timer.Stop();
                int newtime = 500;
                for (int i = game.Level; i > 1; i--) newtime -= (int)(newtime * .25);
                Console.WriteLine(newtime);
                Timer.Interval = new TimeSpan(0, 0, 0, 0, newtime);
                Timer.Start();
            }
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Left)
            {
                if (!GameOver)
                    game.moveLeft();
            }
            if(e.Key == Key.Right)
            {
                if (!GameOver)
                    game.moveRight();
            }
            if(e.Key == Key.Up)
            {
                if (!GameOver)
                    game.rotate();
            }
            if(e.Key == Key.Down)
            {
                if(!GameOver)
                    GameOver = !game.Tick();
            }
            if(e.Key == Key.Space)
            {
                if(!GameOver)
                    game.moveToBottom();
            }
            if(e.Key == Key.Home)
            {
                game.cheatcode();
            }
        }

        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            if (game.gameHasStarted)
            { 
                pause_btn_Click(null, null);
                Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.Filter = "Tetris Save File|*.tet";
                Nullable<bool> result = dialog.ShowDialog();
                if (result == true)
                {
                    string savefile = dialog.FileName;
                    SaveState saveBundle = new SaveState();

                    game.saveState(ref saveBundle);

                    IFormatter Save = new BinaryFormatter();
                    Stream Write = new FileStream(savefile, FileMode.Create, FileAccess.Write);
                    Save.Serialize(Write, saveBundle);
                    Write.Close();
                }
            }
        }

        private void load_btn_Click(object sender, RoutedEventArgs e)
        {
            pause_btn_Click(null, null);
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Tetris Save File|*.tet";
            Nullable<bool> result = dialog.ShowDialog();
            if(result == true)
            {
                string loadfile = dialog.FileName;

                IFormatter Load = new BinaryFormatter();
                Stream Read = new FileStream(loadfile, FileMode.Open, FileAccess.Read);
                SaveState loadBundle = (SaveState)Load.Deserialize(Read);
                Read.Close();

                game.loadState(loadBundle);
                // update display
                this.score_txt.Text = game.Score.ToString();
                this.level_txt.Text = game.Level.ToString();
            }
        }

        private void start_btn_Click(object sender, RoutedEventArgs e)
        {
            if (paused && !GameOver) {
                paused = false;
                Timer.Start();
                GameCanvas.Focus();
                GameCanvas.Children.Remove(overlay);
                GameCanvas.Children.Remove(pause);
                pause_btn.IsEnabled = true;
                start_btn.IsEnabled = false;
            }
        }

        private void pause_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!paused && !GameOver)
            {
                paused = true;
                pause.Text = "Paused";
                pause.FontSize = 30;
                pause.Foreground = Brushes.White;
                pause.Margin = new Thickness(103, 250, 0, 0);
                Timer.Stop();
                overlay.Height = 540;
                overlay.Width = 300;
                Brush test = new SolidColorBrush(Color.FromArgb(200, 11, 11, 11));
                overlay.Fill = test;
                GameCanvas.Children.Add(overlay);
                GameCanvas.Children.Add(pause);
                pause_btn.IsEnabled = false;
                start_btn.IsEnabled = true;
            }
        }
    }
}
