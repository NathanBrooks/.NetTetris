/* MainWindow
 * controls user interface 
 */

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

        private bool OverlayShown;

        Rectangle OverlayRect = new Rectangle();
        TextBlock PauseText = new TextBlock();
        TextBlock GameOverText = new TextBlock();

        private RoutedCommand rNewGame;
        private RoutedCommand rSave;
        private RoutedCommand rLoad;
        private RoutedCommand rPause;
        private RoutedCommand rPlay;
        private RoutedCommand rQuit;
        private bool paused;

        public MainWindow()
        {
            InitializeComponent();
            InitializeKeyBindings();
            InitializeOverlayObjects();

            game = new TetrisGameManager(0, 1, ref GameCanvas);

            GameOver = false;
            paused = false;

            this.KeyDown += MainWindow_KeyDown;
            this.Timer.Tick += Timer_Tick;
            this.Timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            this.score_txt.Text = game.Score.ToString();
            this.level_txt.Text = game.Level.ToString();
            this.lvl_txt.Text = game.TotalLines.ToString();

            PauseGame();
        }

        private void InitializeOverlayObjects()
        {
            // initialize pause text
            PauseText.Text = "Paused";
            PauseText.FontSize = 30;
            PauseText.Foreground = Brushes.White;
            PauseText.Margin = new Thickness(103, 250, 0, 0);

            // initialize game over text
            GameOverText.Text = "Game Over";
            GameOverText.FontSize = 30;
            GameOverText.Foreground = Brushes.White;
            GameOverText.Margin = new Thickness(78, 250, 0, 0);

            // initialize overlay
            OverlayRect.Height = 540;
            OverlayRect.Width = 300;
            Brush test = new SolidColorBrush(Color.FromArgb(200, 11, 11, 11));
            OverlayRect.Fill = test;
        }

        private void ShowGameOver()
        {
            if (OverlayShown) RemoveOverlays();
            GameCanvas.Children.Add(OverlayRect);
            GameCanvas.Children.Add(GameOverText);
            OverlayShown = true;   
        }

        private void ShowPause()
        {
            if (OverlayShown) RemoveOverlays();
            GameCanvas.Children.Add(OverlayRect);
            GameCanvas.Children.Add(PauseText);
            OverlayShown = true;
        }

        private void RemoveOverlays()
        {
            GameCanvas.Children.Remove(OverlayRect);
            GameCanvas.Children.Remove(PauseText);
            GameCanvas.Children.Remove(GameOverText);
            OverlayShown = false;
        }

        private void PauseGame()
        {
            paused = true;
            Timer.Stop();
            ShowPause();
            pause_btn.IsEnabled = false;
            start_btn.IsEnabled = true;
        }

        private void ResumeGame()
        {
            paused = false;
            Timer.Start();
            GameCanvas.Focus();
            RemoveOverlays();
            pause_btn.IsEnabled = true;
            start_btn.IsEnabled = false;
        }

        private void SetGameOver()
        {
            Timer.Stop();
            ShowGameOver();
            start_btn.IsEnabled = false;
            pause_btn.IsEnabled = false;
        }

        private void UpdateGameSpeed()
        {
            Timer.Stop();
            int newtime = 500;
            for (int i = game.Level; i > 1; i--) newtime -= (int)(newtime * .25);
            Console.WriteLine(newtime);
            Timer.Interval = new TimeSpan(0, 0, 0, 0, newtime);
            Timer.Start();
        }

        private void InitializeKeyBindings()
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
            CommandBindings.Add(new CommandBinding(rNewGame, startnewgame_click));

            rQuit = new RoutedCommand();
            rQuit.InputGestures.Add(new KeyGesture(Key.Q, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(rQuit, quit_click));
        }

        private void Timer_Tick(object sender, EventArgs e)
        { 
            if(!GameOver)
            {
                Update();
            } 
            else
            {
                SetGameOver();
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
                UpdateGameSpeed();
            }
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Left)
            {
                if (!GameOver && !paused)
                    game.moveLeft();
            }
            if(e.Key == Key.Right)
            {
                if (!GameOver && !paused)
                    game.moveRight();
            }
            if(e.Key == Key.Up)
            {
                if (!GameOver && !paused)
                    game.rotate();
            }
            if(e.Key == Key.Down)
            {
                if(!GameOver && !paused)
                    game.rotate();
            }
            if(e.Key == Key.Space && !paused)
            {
                if(!GameOver)
                    game.moveToBottom();
            }
            if(e.Key == Key.Home)
            {
                game.cheatcode();

                UpdateGameSpeed();
                
                // update display
                this.score_txt.Text = game.Score.ToString();
                this.level_txt.Text = game.Level.ToString();
                this.lvl_txt.Text = game.TotalLines.ToString();
            }
        }

        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            if (game.gameHasStarted && !GameOver)
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
            PauseGame();

            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Tetris Save File|*.tet";
            Nullable<bool> result = dialog.ShowDialog();
            if(result == true)
            {
                string loadfile = dialog.FileName;

                IFormatter Load = new BinaryFormatter();
                Stream Read = new FileStream(loadfile, FileMode.Open, FileAccess.Read);
                try
                {
                    SaveState loadBundle = (SaveState)Load.Deserialize(Read);
                    game.loadState(loadBundle);
                    // update display
                    this.score_txt.Text = game.Score.ToString();
                    this.level_txt.Text = game.Level.ToString();

                    UpdateGameSpeed();
                    PauseGame();
                } catch (Exception exception)
                {
                    MessageBox.Show("That .tet file is corrupt or not valid, please load a different file!", "Invalid!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                Read.Close();
            }
        }

        private void start_btn_Click(object sender, RoutedEventArgs e)
        {
            if (paused && !GameOver) {
                ResumeGame();
            }
        }

        private void pause_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!paused && !GameOver)
            {
                PauseGame();
            }
        }

        private void startnewgame_click(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
            game.clearCanvas();
            RemoveOverlays();
            game = new TetrisGameManager(0, 1, ref GameCanvas);
            this.score_txt.Text = game.Score.ToString();
            this.level_txt.Text = game.Level.ToString();
            this.lvl_txt.Text = game.TotalLines.ToString();
            UpdateGameSpeed();
            PauseGame();
            GameOver = false;
        }

        private void quit_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void about_click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Developers: Nathan Brooks, Vlad Sinitsa\nVersion: 1.0\n.Net Version: 4.5.2");
        }
    }
}
