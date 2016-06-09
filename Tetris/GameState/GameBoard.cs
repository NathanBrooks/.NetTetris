/* GameBoard
 * this is the gameboard class, it holds the game board
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Tetris.GameState
{
    class GameBoard
    {
        public static int SizeX = 10;
        public static int SizeY = 18;
        private int[,] Board;

        Canvas GameCanvas;

        ArrayList Rectangles = new ArrayList();

        public void saveState(ref SaveState bundle)
        {
            bundle.saveGameBoard = Board;
        }

        public void loadState(SaveState bundle)
        {
            Board = bundle.saveGameBoard;
            reDraw();
        }

        public GameBoard(ref Canvas gameCanvas)
        {
            this.GameCanvas = gameCanvas;
            Board = new int[SizeY, SizeX];
        }

        public int this[int i, int j]
        {
            get { return Board[i, j]; }
            set { Board[i, j] = value; }
        }

        public int clearRows()
        {
            int rowscleared = 0;
            for(int i = 0; i < SizeY; i++)
            {
                bool clear = true;
                for(int j = 0; j < 10; j++)
                {
                    if(Board[i,j] == 0)
                    {
                        clear = false;
                    }
                }
                if (clear == true)
                {
                    for(int x = i; x > 0; x--)
                    {
                        for(int y = 0; y < 10; y++)
                        {
                            Board[x, y] = Board[x - 1, y];
                        }
                    }

                    for(int h = 0; h < 10; h++)
                    {
                        Board[0, h] = 0;
                    }
                    rowscleared++;
                }
            }
            reDraw();
            return rowscleared;
        }

        public void clearCanvas()
        {
            foreach (Rectangle rect in Rectangles)
            {
                GameCanvas.Children.Remove(rect);
            }
        }

        private void reDraw()
        {
            clearCanvas();

            for (int y=0; y<SizeY; y++)
            {
                for(int x=0; x<SizeX; x++)
                {
                    if(Board[y,x] != 0)
                    {
                        Rectangle rect = new Rectangle() { Height = TetrisGameManager.BlockHeight, Width = TetrisGameManager.BlockWidth };
                        Canvas.SetTop(rect, (TetrisGameManager.BlockHeight * y));
                        Canvas.SetLeft(rect, (TetrisGameManager.BlockHeight * x));
                        rect.Fill = TetrisGameManager.ShapeColors[Board[y, x] - 1];
                        rect.StrokeThickness = 1;
                        rect.Stroke = System.Windows.Media.Brushes.White;

                        Rectangles.Add(rect);
                        GameCanvas.Children.Add(rect);
                    }
                }
            }
        }
    }
}
