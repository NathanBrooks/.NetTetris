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
        {{{0,1,0,0},{0,1,0,0},{0,1,0,0},{0,1,0,0}},{{0,0,0,0},{1,1,1,1},{0,0,0,0},{0,0,0,0}},{{0,0,1,0},{0,0,1,0},{0,0,1,0},{0,0,1,0}},{{0,0,0,0},{0,0,0,0},{1,1,1,1},{0,0,0,0}}}, // line
        {{{1,0,0,0},{1,0,0,0},{1,1,0,0},{0,0,0,0}},{{0,0,0,0},{0,0,1,0},{1,1,1,0},{0,0,0,0}},{{1,1,0,0},{0,1,0,0},{0,1,0,0},{0,0,0,0}},{{0,0,0,0},{1,1,1,0},{1,0,0,0},{0,0,0,0}}}, // Reverse L
        {{{0,1,0,0},{0,1,0,0},{1,1,0,0},{0,0,0,0}},{{0,0,0,0},{1,1,1,0},{0,0,1,0},{0,0,0,0}},{{1,1,0,0},{1,0,0,0},{1,0,0,0},{0,0,0,0}},{{0,0,0,0},{1,0,0,0},{1,1,1,0},{0,0,0,0}}}, // Foward L
        {{{0,1,1,0},{1,1,0,0},{0,0,0,0},{0,0,0,0}},{{1,0,0,0},{1,1,0,0},{0,1,0,0},{0,0,0,0}},{{0,1,1,0},{1,1,0,0},{0,0,0,0},{0,0,0,0}},{{1,0,0,0},{1,1,0,0},{0,1,0,0},{0,0,0,0}}}, // Squigly
        {{{1,1,0,0},{0,1,1,0},{0,0,0,0},{0,0,0,0}},{{0,1,0,0},{1,1,0,0},{1,0,0,0},{0,0,0,0}},{{1,1,0,0},{0,1,1,0},{0,0,0,0},{0,0,0,0}},{{0,1,0,0},{1,1,0,0},{1,0,0,0},{0,0,0,0}}}, // Reverse Squigly
        {{{1,1,0,0},{1,1,0,0},{0,0,0,0},{0,0,0,0}},{{1,1,0,0},{1,1,0,0},{0,0,0,0},{0,0,0,0}},{{1,1,0,0},{1,1,0,0},{0,0,0,0},{0,0,0,0}},{{1,1,0,0},{1,1,0,0},{0,0,0,0},{0,0,0,0}}}, // Square
        {{{0,1,0,0},{1,1,0,0},{0,1,0,0},{0,0,0,0}},{{0,1,0,0},{1,1,1,0},{0,0,0,0},{0,0,0,0}},{{0,1,0,0},{0,1,1,0},{0,1,0,0},{0,0,0,0}},{{0,0,0,0},{1,1,1,0},{0,1,0,0},{0,0,0,0}}}  // T-Block 
        };

        // top, bottom, left, right
        private static int[,,] ShapeOffsets = new int[7, 4, 4]
        {
            { {0,0,1,2}, {1,2,0,0}, {0,0,2,1}, {2,1,0,0} },
            { {0,1,0,2}, {1,1,0,1}, {0,1,0,2}, {1,1,0,1} },
            { {0,1,0,2}, {1,1,0,1}, {0,1,0,2}, {1,1,0,1} },
            { {0,2,0,1}, {0,1,0,2}, {0,2,0,1}, {0,1,0,2} },
            { {0,2,0,1}, {0,1,0,2}, {0,2,0,1}, {0,1,0,2} },
            { {0,2,0,2}, {0,2,0,2}, {0,2,0,2}, {0,2,0,2} },
            { {0,1,0,2}, {0,2,0,1}, {0,1,1,1}, {1,1,0,1} },
        };

        // TODO: static array of colors

        private int[] offset = new int[2] { 0, 0 };
        private int Type;
        private int Orientation;
        private GameBoard Game;
        private Canvas GameCanvas;

        private ArrayList Rectangles = new ArrayList();

        public void saveState(ref SaveState bundle)
        {
            bundle.saveFallingOfffset = this.offset;
            bundle.saveFallingType = this.Type;
            bundle.saveFallingOrientation = this.Orientation;
        }

        public void loadState(SaveState bundle)
        {
            this.offset = bundle.saveFallingOfffset;
            this.Type = bundle.saveFallingType;
            this.Orientation = bundle.saveFallingOrientation;
            reDraw();
        }

        public FallingBlock(ref GameBoard board, ref Canvas gameCanvas)
        {
            this.Game = board;
            this.GameCanvas = gameCanvas;
        }

        public Boolean newBlock(int type, int orientation, int xOffset)
        {
            this.Type = type;
            this.Orientation = orientation;
            this.offset[0] = xOffset;
            this.offset[1] = 0;
            if(isValid(Orientation, offset[0], offset[1]))
            {
                reDraw();
                return true;
            } else
            {
                return false;
            }
        }

        public void rotatePositive()
        {
            for(int i=1; i<4; i++)
            {
                int newOrientation = (Orientation + 1) % 4;
                int newOffsetX = offset[0] + (ShapeOffsets[Type, newOrientation, 2] - ShapeOffsets[Type, Orientation, 2]);
                int newOffsetY = offset[1] + (ShapeOffsets[Type, newOrientation, 0] - ShapeOffsets[Type, Orientation, 0]);
                while(!isValid(newOrientation, newOffsetX, newOffsetY))
                {
                    newOrientation = (newOrientation + 1) % 4;
                    newOffsetX = offset[0] + (ShapeOffsets[Type, newOrientation, 2] - ShapeOffsets[Type, Orientation, 2]);
                    newOffsetY = offset[1] + (ShapeOffsets[Type, newOrientation, 0] - ShapeOffsets[Type, Orientation, 0]);
                }

                this.offset[0] = newOffsetX;
                this.offset[1] = newOffsetY;
                Orientation = newOrientation;
                break;
            }
            reDraw();
        }

        public Boolean moveLeft()
        {
            if (isValid(Orientation, this.offset[0] - 1, this.offset[1]))
            {
                this.offset[0]--;
                reDraw();
                return true;
            }
            else return false;
        }

        public Boolean moveRight()
        {
            if (isValid(Orientation, this.offset[0] + 1, this.offset[1]))
            {
                this.offset[0]++;
                reDraw();
                return true;
            }
            else return false;
        }

        public Boolean moveDown()
        {
            if (isValid(Orientation, this.offset[0], this.offset[1] + 1))
            {
                this.offset[1]++;
                reDraw();
                return true;
            } else
            { 
                addToGameBoard();
                return false;
            }
        }

        public Boolean moveToBottom()
        {
            while (moveDown());
            return true;
        }

        private void addToGameBoard()
        {
            for (int y = ShapeOffsets[Type, Orientation, 0]; y < 4 - ShapeOffsets[Type, Orientation, 1]; y++)
            {
                for (int x = ShapeOffsets[Type, Orientation, 2]; x < 4 - ShapeOffsets[Type, Orientation, 3]; x++)
                {
                    if (ShapePositions[Type, Orientation, y, x] == 1)
                    {
                        Game[
                                offset[1] + (y - ShapeOffsets[Type, Orientation, 0]),
                                offset[0] + (x - ShapeOffsets[Type, Orientation, 2])
                            ] = Type + 1;
                    }
                }
            }
        }

        private bool isValid(int orientation, int offsetX, int offsetY)
        {
            if(offsetX < 0 || offsetY < 0 ||
                ((offsetY + (4 - (ShapeOffsets[Type, orientation, 0] + ShapeOffsets[Type, orientation, 1]))) > GameBoard.SizeY) ||
                ((offsetX + (4 - (ShapeOffsets[Type, orientation, 2] + ShapeOffsets[Type, orientation, 3]))) > GameBoard.SizeX))
                return false;

            for (int y = ShapeOffsets[Type, orientation, 0]; y < 4 - ShapeOffsets[Type, orientation, 1]; y++)
            {
                for (int x = ShapeOffsets[Type, orientation, 2]; x < 4 - ShapeOffsets[Type, orientation, 3]; x++)
                {
                    if (ShapePositions[Type, orientation, y, x] == 1 
                        && Game[
                            offsetY + (y - ShapeOffsets[Type, orientation, 0]),
                            offsetX + (x - ShapeOffsets[Type, orientation, 2])
                        ] != 0)
                        return false;
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

            for(int y=ShapeOffsets[Type, Orientation, 0]; y < 4 - ShapeOffsets[Type, Orientation, 1]; y++)
            {
                for(int x=ShapeOffsets[Type, Orientation, 2]; x < 4 - ShapeOffsets[Type, Orientation, 3]; x++)
                {
                    if(ShapePositions[Type, Orientation, y, x] == 1)
                    {
                        Rectangle rect = new Rectangle() { Height = TetrisGameManager.BlockHeight, Width = TetrisGameManager.BlockWidth };
                        Canvas.SetTop(rect, (TetrisGameManager.BlockHeight * (y - ShapeOffsets[Type, Orientation, 0])) + (offset[1] * TetrisGameManager.BlockHeight));
                        Canvas.SetLeft(rect, (TetrisGameManager.BlockHeight * (x - ShapeOffsets[Type, Orientation, 2])) + (offset[0] * TetrisGameManager.BlockWidth));
                        rect.Fill = TetrisGameManager.ShapeColors[Type];

                        Rectangles.Add(rect);
                        GameCanvas.Children.Add(rect);
                    }
                }
            }
        }
    }
}
