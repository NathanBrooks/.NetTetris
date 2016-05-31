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
    class FallingBlock
    {
        private static int[,,,] ShapePositions = new int[7, 4, 4, 4]
        {
        {{{1,0,0,0},{1,0,0,0},{1,0,0,0},{1,0,0,0}},{{1,1,1,1},{0,0,0,0},{0,0,0,0},{0,0,0,0}},{{0,0,0,1},{0,0,0,1},{0,0,0,1},{0,0,0,1}},{{0,0,0,0},{1,1,1,1},{0,0,0,0},{0,0,0,0}}}, // line
        {{{0,0,0,1},{0,1,1,1},{0,0,0,0},{0,0,0,0}},{{1,0,0,0},{1,0,0,0},{1,1,0,0},{0,0,0,0}},{{1,1,1,0},{1,0,0,0},{0,0,0,0},{0,0,0,0}},{{0,0,1,1},{0,0,0,1},{0,0,0,1},{0,0,0,0}}}, // Reverse L
        {{{1,0,0,0},{1,1,1,0},{0,0,0,0},{0,0,0,0}},{{1,1,0,0},{1,0,0,0},{1,0,0,0},{0,0,0,0}},{{0,1,1,1},{0,0,0,1},{0,0,0,0},{0,0,0,0}},{{0,0,0,1},{0,0,0,1},{0,0,1,1},{0,0,0,0}}}, // Foward L
        {{{0,1,1,0},{1,1,0,0},{0,0,0,0},{0,0,0,0}},{{1,0,0,0},{1,1,0,0},{0,1,0,0},{0,0,0,0}},{{0,1,1,0},{1,1,0,0},{0,0,0,0},{0,0,0,0}},{{1,0,0,0},{1,1,0,0},{0,1,0,0},{0,0,0,0}}}, // Squigly
        {{{1,1,0,0},{0,1,1,0},{0,0,0,0},{0,0,0,0}},{{0,1,0,0},{1,1,0,0},{1,0,0,0},{0,0,0,0}},{{1,1,0,0},{0,1,1,0},{0,0,0,0},{0,0,0,0}},{{0,1,0,0},{1,1,0,0},{1,0,0,0},{0,0,0,0}}}, // Reverse Squigly
        {{{1,1,0,0},{1,1,0,0},{0,0,0,0},{0,0,0,0}},{{1,1,0,0},{1,1,0,0},{0,0,0,0},{0,0,0,0}},{{1,1,0,0},{1,1,0,0},{0,0,0,0},{0,0,0,0}},{{1,1,0,0},{1,1,0,0},{0,0,0,0},{0,0,0,0}}}, // Square
        {{{1,1,1,0},{0,1,0,0},{0,0,0,0},{0,0,0,0}},{{0,0,0,1},{0,0,1,1},{0,0,0,1},{0,0,0,0}},{{0,1,0,0},{1,1,1,0},{0,0,0,0},{0,0,0,0}},{{1,0,0,0},{1,1,0,0},{1,0,0,0},{0,0,0,0}}}  // Pyramid 
        };

        // top, bottom, left, right
        private static int[,,] ShapeOffsets = new int[7, 4, 4]
        {
            { {0,0,0,3}, {0,3,0,0}, {0,0,3,0}, {1,2,0,0} },
            { {0,2,1,0}, {0,1,0,2}, {0,2,0,1}, {0,1,2,0} },
            { {0,2,0,1}, {0,1,0,2}, {0,2,1,0}, {0,1,2,0} },
            { {0,2,0,1}, {0,1,0,2}, {0,2,0,1}, {0,1,0,2} },
            { {0,2,0,1}, {0,1,0,2}, {0,2,0,1}, {0,1,0,2} },
            { {0,2,0,2}, {0,2,0,2}, {0,2,0,2}, {0,2,0,2} },
            { {0,2,0,1}, {0,1,2,0}, {0,2,0,1}, {0,1,2,0} },
        };

        private static int BlockHeight = 10;
        private static int BlockWidth = 10;

        // TODO: static array of colors

        private int[] offset = new int[2] { 0, 0 };
        private int Type;
        private int Orientation;
        private GameBoard Game;
        private Canvas GameCanvas;

        private ArrayList Rectangles = new ArrayList();

        public FallingBlock(int type, int orientation, int xOffset, ref GameBoard board, ref Canvas gameCanvas)
        {
            this.Type = type;
            this.Orientation = orientation;
            this.Game = board;
            this.GameCanvas = gameCanvas;
            this.offset[0] = xOffset;
            this.offset[1] = 0;

        }

        public void rotatePositive()
        {
            for(int i=1; i<4; i++)
            {
                int newOrientation = (Orientation + 1) % 4;
                if (isValid(newOrientation, this.offset[0], this.offset[1]))
                {
                    Orientation = newOrientation;
                    break;
                }
            }
            reDraw();
        }

        public void rotateNegative()
        {
            for (int i = 1; i < 4; i++)
            {
                int newOrientation = (Orientation - 1) % 4;
                if (isValid(newOrientation, this.offset[0], this.offset[1]))
                {
                    Orientation = newOrientation;
                    break;
                }
            }
            reDraw();
        }

        public void moveLeft()
        {
            if (isValid(Orientation, this.offset[0] - 1, this.offset[1]))
            {
                this.offset[0] -= 1;
                reDraw();
            }
        }

        public void moveRight()
        {
            if (isValid(Orientation, this.offset[0] + 1, this.offset[1]))
            {
                this.offset[0] += 1;
                reDraw();
            }
        }

        public void moveDown()
        {
            if (isValid(Orientation, this.offset[0], this.offset[1] + 1))
            {
                this.offset[1] += 1;
                reDraw();
            }
        }

        private bool isValid(int orientation, int offsetX, int offsetY)
        {
            // TODO: actually check validity
            return true;
        }

        public void reDraw()
        {
            // clear out the old positions
            foreach (Rectangle rect in Rectangles)
            {
                GameCanvas.Children.Remove(rect);
            }

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (ShapePositions[Type, Orientation, x, y] == 1)
                    {
                        Rectangle rect = new Rectangle() { Height = BlockHeight, Width = BlockWidth };
                        Canvas.SetTop(rect, y + offset[1]);
                        Canvas.SetLeft(rect, x + offset[0]);

                        Rectangles.Add(rect);
                        GameCanvas.Children.Add(rect);
                    }
                }
            }
        }
    }
}
