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
using Tetris.GameState;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private TetrisGameManager game;

        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += MainWindow_KeyDown;
            game = new TetrisGameManager(ref GameCanvas);
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
    }
}
