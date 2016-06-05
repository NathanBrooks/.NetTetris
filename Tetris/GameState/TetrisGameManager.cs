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

        GameBoard Game;

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
            this.Score = Scr;
            this.Level = Lvl;

            Board = new GameBoard(ref gameCanvas);

            CurrentFallingBlock = new FallingBlock(ref Board, ref gameCanvas);
            SetNewBlock();
        }


    }
}
