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
        public int Score { get; set; }
        public int Level { get; set; }

        //GameBoard Game;

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

        public TetrisGameManager(int Scr, int Lvl, ref Canvas gameCanvas)
        {
            this.Score = Scr;
            this.Level = Lvl;

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
                if (!SetNewBlock())
                    return false;
            }

            // DO SOME SCORING HERE
            int RowsCleared = Board.clearRows();
            if(RowsCleared == 1)
            {
                Score = Score + (Level * 100);
            }
            else if(RowsCleared == 2)
            {
                Score = Score + (Level * 250);
            }
            else if(RowsCleared == 3)
            {
                Score = Score + (Level * 400);
            }
            else if(RowsCleared == 4)
            {
                Score = Score + (Level * 550);
            }
            return true;
        }

    }
}
