using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JewelJam
{
    class JewelGrid : GameObj
    {
        private Jewel[,] _jewelGrid;
        private int _xGrids;
        private int _yGrids;
        private int _cellSize;

        public JewelGrid(int width, int height, int cellSize, Vector2 offset)
        {
            _xGrids = width;
            _yGrids = height;
            this._cellSize = cellSize;
            Position = offset;

            Reset();
        }

        public override void Reset()
        {
            //base.Reset();
            _jewelGrid = new Jewel[_xGrids, _yGrids];

            for (int x = 0; x < _xGrids; x++)
            {
                for (int y = 0; y < _yGrids; y++)
                {
                    // hard coded based on number of jewel sprites
                    _jewelGrid[x, y] = new Jewel(GamEx.Rand.Next(3));
                    _jewelGrid[x, y].Position = Position + new Vector2(x * _cellSize, y * _cellSize);
                }
            }
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            if (inputHelper.KeyPressed(Keys.Space))
                MoveRowsDown();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Jewel jewel in _jewelGrid)
                jewel.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// Get a position based on [x,y] of a 2D grid array
        /// </summary>
        /// <param name="x">grid cell # in the x direction</param>
        /// <param name="y">grid cell # in the y direction</param>
        /// <returns>Position vector (x,y coords in game world space)</returns>
        Vector2 GetCellPos(int x, int y)
        {
            return Position + new Vector2(x * _cellSize, y * _cellSize);
        }

        void MoveRowsDown()
        {
            for (int y = _yGrids - 1; y > 0; y--)
            {
                for (int x = 0; x < _xGrids; x++)
                {
                    _jewelGrid[x, y] = _jewelGrid[x, y - 1];
                    _jewelGrid[x, y].Position = GetCellPos(x, y);
                }
            }

            //refill top row
            for (int x = 0; x < _xGrids; x++)
            {
                _jewelGrid[x, 0] = new Jewel(GamEx.Rand.Next(3));
                _jewelGrid[x, 0].Position = GetCellPos(x, 0);
            }
        }
    }
}