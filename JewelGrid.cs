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

        public JewelGrid(int width, int height, int cellSize, Vector2 offset)
        {
            _xGrids = width;
            _yGrids = height;
            _cellSize = cellSize;
            PosLocal = offset;

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
                    PosLocal = GetCellPos(x,y),
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
        private Vector2 GetCellPos(int x, int y)
        {
            return new Vector2(x * _cellSize, y * _cellSize);
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