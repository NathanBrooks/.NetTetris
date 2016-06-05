using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.GameState
{
    [Serializable]
    class SaveState
    {
        public int saveScore { get; set; }
        public int saveLevel { get; set; }
        public int[] saveGameBoard { get; set; }
        public int saveFallingX { get; set; }
        public int saveFallingY { get; set; }
        public int saveFallingType { get; set; }
        public int saveFallingOrientation { get; set; }

        public SaveState(int scr, int lvl, int[] gb, int x, int y, int type, int ori)
        {
            this.saveScore = scr;
            this.saveLevel = lvl;
            this.saveGameBoard = gb;
            this.saveFallingX = x;
            this.saveFallingY = y;
            this.saveFallingType = type;
            this.saveFallingOrientation = ori;
        }

    }
}
