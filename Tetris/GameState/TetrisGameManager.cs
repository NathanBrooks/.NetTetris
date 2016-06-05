using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.GameState
{
    class TetrisGameManager
    {
        public int Score { get; set; }
        public int Level { get; set; }

        GameBoard Game;

        public TetrisGameManager(int Scr, int Lvl)
        {
            this.Score = Scr;
            this.Level = Lvl;
        }


    }
}
