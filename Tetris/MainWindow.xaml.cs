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

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameState.GameBoard board;
        private GameState.FallingBlock block;

        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += MainWindow_KeyDown;
            board = new GameState.GameBoard();
            block = new GameState.FallingBlock(0,0,0, ref board, ref GameCanvas);
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Left)
            {
                block.moveLeft();
            }
            if(e.Key == Key.Right)
            {
                block.moveRight();
            }
            if(e.Key == Key.Up)
            {
                block.rotatePositive();
            }
            if(e.Key == Key.Down)
            {
                block.moveDown();
            }
            if(e.Key == Key.Space)
            {

            }
        }
    }
}
