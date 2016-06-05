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
        //TetrisGameManager test = new TetrisGameManager(20000, 3);
        //SaveState save1 = new SaveState(40000, 5 , null, 2, 3, 4, 3);
        DispatcherTimer Timer = new DispatcherTimer();

        private TetrisGameManager game;

        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += MainWindow_KeyDown;
            this.Timer.Tick += Timer_Tick;
            this.Timer.Interval = new TimeSpan(0, 0, 0, 500);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Left)
            {
                game.moveLeft();
            }
            if(e.Key == Key.Right)
            {
                game.moveRight();
            }
            if(e.Key == Key.Up)
            {
                game.rotate();
            }
            if(e.Key == Key.Down)
            {
                game.Tick();
            }
            if(e.Key == Key.Space)
            {

            }
        }

        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            

            /*IFormatter Save = new BinaryFormatter();
            Stream Write = new FileStream("savefile.tet", FileMode.Create, FileAccess.Write);
            Save.Serialize(Write, save1);
            Write.Close();*/
        }

        private void load_btn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.ShowDialog();
            /*IFormatter Load = new BinaryFormatter();
            Stream Read = new FileStream("savefile.tet", FileMode.Open, FileAccess.Read);
            save1 = (SaveState) Load.Deserialize(Read);
            test.Level = save1.saveLevel;
            test.Score = save1.saveScore;
            Read.Close();
            temp1.Content = test.Score;
            temp2.Content = test.Level;*/
        }
    }
}
