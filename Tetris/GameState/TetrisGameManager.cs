using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Tetris.GameState
{
    class TetrisGameManager
    {
        int Score;
        int Level;
        GameBoard Game;
        DispatcherTimer Timer = new DispatcherTimer();

        public TetrisGameManager()
        {
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 0, 500);
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
