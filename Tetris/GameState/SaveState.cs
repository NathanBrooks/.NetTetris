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
        public int saveLineCount { get; set; }
        public int[,] saveGameBoard { get; set; }
        public int[] saveFallingOfffset { get; set; }
        public int saveFallingType { get; set; }
        public int saveFallingOrientation { get; set; }

        public SaveState()
        {
            this.saveScore = 0;
            this.saveLevel = 1;
            this.saveGameBoard = null;
            this.saveFallingOfffset = null;
            this.saveFallingType = 0;
            this.saveFallingOrientation = 0;
        }

    }
}
