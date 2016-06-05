using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Tetris.GameState
{
    class TetrisGameManager
    {
        int Score;
        int Level;
        GameBoard Game;
        DispatcherTimer Timer = new DispatcherTimer();

        public static SolidColorBrush[] ShapeColors = new SolidColorBrush[7] {
            Brushes.Aqua, // line
            Brushes.Orange, // Foward L
            Brushes.Blue, // Reverse L
            Brushes.LightGreen, // Squigly
            Brushes.Red, // Reverse Squigly
            Brushes.Yellow, // Square
            Brushes.Purple, // T-Block
        };
        public static int BlockHeight = 30;
        public static int BlockWidth = 30;


        private FallingBlock CurrentFallingBlock;
        private GameBoard Board;

        public TetrisGameManager(ref Canvas gameCanvas)
        {
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 0, 500);

            Board = new GameBoard(ref gameCanvas);

            CurrentFallingBlock = new FallingBlock(ref Board, ref gameCanvas);
            SetNewBlock();
        }

        private Boolean SetNewBlock()
        {
            Random rnd = new Random();
            return CurrentFallingBlock.newBlock(rnd.Next(0, 7), rnd.Next(0, 4), rnd.Next(4, 7));
        }
        
        public void moveLeft()
        {
            CurrentFallingBlock.moveLeft();
        }

        public void moveRight()
        {
            CurrentFallingBlock.moveRight();
        }

        public void moveDown()
        {
            CurrentFallingBlock.moveDown();
        }

        public void rotate()
        {
            CurrentFallingBlock.rotatePositive();
        }

        public Boolean Tick()
        {
            if (!CurrentFallingBlock.moveDown())
            {
                if(!SetNewBlock())
                    return false;
            }

            // DO SOME SCORING HERE
            int RowsCleared = Board.clearRows();
            return true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void StartTimer()
        {

        }

        public void StopTimer()
        {

        }

    }
}
