using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
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

        private static int BlockHeight = 30;
        private static int BlockWidth = 30;

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
            reDraw();

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
                this.offset[0]--;
                reDraw();
            }
        }

        public void moveRight()
        {
            if (isValid(Orientation, this.offset[0] + 1, this.offset[1]))
            {
                this.offset[0]++;
                reDraw();
            }
        }

        public void moveDown()
        {
            if (isValid(Orientation, this.offset[0], this.offset[1] + 1))
            {
                this.offset[1]++;
                reDraw();
            }
        }

        private bool isValid(int orientation, int offsetX, int offsetY)
        {
            if (
                (offsetY + Math.Abs((ShapeOffsets[Type, orientation, 0] - (4 - ShapeOffsets[Type, orientation, 1]))) >= GameBoard.SizeY) ||
                (offsetX + Math.Abs((ShapeOffsets[Type, orientation, 2] - (4 - ShapeOffsets[Type, orientation, 3]))) >= GameBoard.SizeX) ||
                (offsetY < 0) ||
                (offsetX < 0)
                ) return false;

            for (int y=ShapeOffsets[Type, orientation, 0]; y<(4 - ShapeOffsets[Type, orientation, 1]); y++)
            {
                for(int x=ShapeOffsets[Type, orientation, 2]; x<(4 - ShapeOffsets[Type, orientation, 3]); x++)
                {
                    if (ShapePositions[Type, orientation, y, x] == 1 && Game[offsetY + y, offsetX + x] != 0) return false;
                }
            }
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
                        Canvas.SetTop(rect, (BlockHeight * y) + (offset[1] * BlockHeight));
                        Canvas.SetLeft(rect, (BlockWidth * x) + (offset[0] * BlockWidth));
                        rect.Fill = Brushes.Aqua;

                        Rectangles.Add(rect);
                        GameCanvas.Children.Add(rect);
                    }
                }
            }
        }
    }
}
