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
        public int Score { get; private set; }
        public int Level { get; private set; }
        public int TotalLines { get; private set; }
        private bool gameHasStarted;

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

        public void saveState(ref SaveState bundle)
        {
            bundle.saveScore = this.Score;
            bundle.saveLevel = this.Level;
            bundle.saveLineCount = this.TotalLines;
            Board.saveState(ref bundle);
            CurrentFallingBlock.saveState(ref bundle);
        }

        public void loadState(SaveState bundle)
        {
            this.Score = bundle.saveScore;
            this.Level = bundle.saveLevel;
            this.TotalLines = bundle.saveLineCount;
            Board.loadState(bundle);
            CurrentFallingBlock.loadState(bundle);
        }

        public TetrisGameManager(int Scr, int Lvl, ref Canvas gameCanvas)
        {
            this.gameHasStarted = false;
            this.Score = Scr;
            this.Level = Lvl;
            this.TotalLines = 0;

            Board = new GameBoard(ref gameCanvas);

            CurrentFallingBlock = new FallingBlock(ref Board, ref gameCanvas);
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

        public void moveToBottom()
        {
            CurrentFallingBlock.moveToBottom();
        }

        public void rotate()
        {
            CurrentFallingBlock.rotatePositive();
        }

        public void cheatcode()
        {
            this.Level = Math.Min(this.Level + 1, 10);
            TotalLines = 10 * this.Level;
        }

        public void clearCanvas()
        {
            Board.clearCanvas();
            CurrentFallingBlock.clearfalling();
        }

        public Boolean Tick()
        {
            if(!gameHasStarted)
            {
                SetNewBlock();
                gameHasStarted = true;
            }
            if (!CurrentFallingBlock.moveDown())
            {
                Scoring();
                if (!SetNewBlock())
                    return false;
            }

            return true;
        }

        public void Scoring()
        {
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

            TotalLines += RowsCleared;
            Level = Math.Min((TotalLines / 10) + 1, 10);
        }            

    }
}
