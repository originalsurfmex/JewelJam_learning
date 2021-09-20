using JewelJam.engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace JewelJam
{
    /// <summary>
    /// game object that holds a 2D array of other game objects in a grid.
    /// this acts as a parent object that holds the grid locations of child objects
    /// locations are recursive so if this move then all the child objects move too
    ///
    /// the recursive location function for globally locating objects is found
    /// within the class of a basic game object, so each child has this global location function
    /// </summary>
    internal class JewelGrid : GameObj
    {
        private readonly int _cellSize;
        private Jewel[,] _jewelGrid;
        private readonly int _xGrids;
        private readonly int _yGrids;

        public int XGrids => _xGrids;
        public int YGrids => _yGrids;


        public JewelGrid(int width, int height, int cellSize)
        {
            _xGrids = width;
            _yGrids = height;
            _cellSize = cellSize;
            //PosLocal = offset; //no longer needed, now part of the jewel grid

            Reset();
        }

        public sealed override void Reset()
        {
            _jewelGrid = new Jewel[_xGrids, _yGrids];

            for (int x = 0; x < _xGrids; x++)
            for (int y = 0; y < _yGrids; y++)
            {
                // hard coded based on number of jewel sprites
                _jewelGrid[x, y] = new Jewel(GamEx.Rand.Next(3))
                {
                    PosLocal = GetCellPos(x, y),
                    Parent = this
                };
            }
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            if (inputHelper.KeyPressed(Keys.Space))
                MoveRowsDown();
            //test the recursive location function
            //PosLocal = inputHelper.MousePos;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Jewel jewel in _jewelGrid)
                jewel.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        ///     Get a position based on [x,y] of a 2D grid array
        /// </summary>
        /// <param name="x">grid cell # in the x direction</param>
        /// <param name="y">grid cell # in the y direction</param>
        /// <returns>Position vector (x,y coords in game world space)</returns>
        public Vector2 GetCellPos(int x, int y)
        {
            return new Vector2(x * _cellSize, y * _cellSize);
        }

        /// <summary>
        /// shift jewels left or right and move the first jewel to the opposite end
        /// </summary>
        /// <param name="selectedRow">whichever row is currently selected</param>
        /// <param name="leftRight">bool: right==true, left==false</param>
        public void ShiftColumns(int selectedRow, bool leftRight)
        {
            // store the first and last jewel so they can be moved later
            Jewel first = _jewelGrid[0, selectedRow];
            Jewel last = _jewelGrid[_xGrids - 1, selectedRow];

            // replace all jewels with their neighbor to the right
            // true = right, false = left
            if (leftRight)
            {
                // go through jewel positions starting from the beginning 
                // don't go through the last one otherwise you're out of bounds
                for (int x = 0; x < _xGrids - 1; x++)
                {
                    // replace each jewel with the one on the right, using grid coordinates
                    _jewelGrid[x, selectedRow] = _jewelGrid[x + 1, selectedRow];
                    // update each jewels local coords (the parent class uses for world coords)
                    _jewelGrid[x, selectedRow].PosLocal = GetCellPos(x, selectedRow);
                }
                // replace the last jewel with the first one that was saved earlier
                _jewelGrid[_xGrids - 1, selectedRow] = first;
                // update the position of the newly replaced jewel, using the game object local position function
                first.PosLocal = GetCellPos(_xGrids - 1, selectedRow);
            }
            else
            {
                // go through jewel positions starting from the end
                // dont go through the first one otherwise you're out of bounds
                for (int x = _xGrids - 1; x > 0; x--)
                {
                    // replace each jewel with the one on the right, using grid coordinates
                    _jewelGrid[x, selectedRow] = _jewelGrid[x - 1, selectedRow];
                    // update each jewels local coords (the parent class uses for world coords)
                    _jewelGrid[x, selectedRow].PosLocal = GetCellPos(x, selectedRow);
                }
                // replace the first jewel with the last one that was saved earlier
                _jewelGrid[0, selectedRow] = last;
                // update the position of the newly replaced jewel, using the game object local position function
                last.PosLocal = GetCellPos(0, selectedRow);
            }
        }

        private void MoveRowsDown()
        {
            for (int y = _yGrids - 1; y > 0; y--)
            for (int x = 0; x < _xGrids; x++)
            {
                _jewelGrid[x, y] = _jewelGrid[x, y - 1];
                _jewelGrid[x, y].PosLocal = GetCellPos(x, y);
            }

            //refill top row
            for (int x = 0; x < _xGrids; x++)
            {
                _jewelGrid[x, 0] = new Jewel(GamEx.Rand.Next(3))
                {
                    PosLocal = GetCellPos(x, 0),
                    Parent = this
                };
            }
        }
    }
}