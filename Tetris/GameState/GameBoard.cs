using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.GameState
{
    class GameBoard
    {
        public static int SizeX = 10;
        public static int SizeY = 18;
        private int[,] board;

        public GameBoard()
        {
            board = new int[SizeY, SizeX];
        }

        public int this[int i, int j]
        {
            get { return board[i, j]; }
            set { board[i, j] = value; }
        }

        public int clearRows()
        {
            return 0;
        }

    }
}
