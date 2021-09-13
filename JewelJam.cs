using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace JewelJam
{
    class JewelJam : GamEx
    {

        private Texture2D _background, _cursor;
        private Texture2D[] _jewels;

        private const int xGrids = 5;
        private const int yGrids = 10;
        private int[,] _grid;

        private int _cellSize;
        private Vector2 _gridOffset, _jewelPos;

        public JewelJam()
        {
            IsMouseVisible = true;
            _grid = new int[xGrids, yGrids];

            _jewels = new Texture2D[3];
            for (int x = 0; x < xGrids; x++)
                for (int y = 0; y < yGrids; y++)
                {
                    _grid[x, y] = Rand.Next(_jewels.Length);
                }
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _background = Content.Load<Texture2D>("img/spr_background");
            _cursor = Content.Load<Texture2D>("img/spr_single_jewel1");

            _worldSize = new Point(_background.Width, _background.Height);

            for (int i = 0; i < _jewels.Length; i++)
                _jewels[i] = Content.Load<Texture2D>("img/spr_single_jewel" + (i+1));

            _cellSize = _jewels[0].Width + 20;
            _gridOffset = new Vector2(90, _cellSize * 2 + 20);
            FullScreen = false;
        }

        protected override void HandleInput()
        {
            base.HandleInput();

            if (inputHelper.KeyPressed(Keys.Space))
                MoveRowsDown();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.Deferred, null,
                null, null, null,
                null, _spriteScale);

            _spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            _spriteBatch.Draw(_cursor, Screen2World(inputHelper.MousePos), Color.White);

            for (int x = 0; x < xGrids; x++)
                for (int y = 0; y < yGrids; y++)
                {
                    _jewelPos = _gridOffset + new Vector2(x, y) * _cellSize; 
                    _spriteBatch.Draw(_jewels[_grid[x,y]], _jewelPos, Color.White);
                }

            _spriteBatch.End();
        }

        //----------------------------------------------------------------------------------

        /// <summary>
        /// Scale and locate a screen position into the viewport
        /// by scaling and locating with the world
        /// </summary>
        /// <param name="screenPos">screen position</param>
        /// <returns></returns>
        private Vector2 Screen2World(Vector2 screenPos)
        {
            Vector2 vPortTopLeft =
                new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y);

            float screen2WorldScale = _worldSize.X /
                (float)GraphicsDevice.Viewport.Width;

            return (screenPos - vPortTopLeft) * screen2WorldScale;
        }

        void MoveRowsDown()
        {
            for (int y = yGrids - 1; y > 0; y--)
            {
                for (int x = 0; x < xGrids; x++)
                {
                    _grid[x, y] = _grid[x, y - 1];
                }
            }
            //refill top row
            for (int x = 0; x < xGrids; x++)
            {
                _grid[x, 0] = Rand.Next(3);
            }
        }
     }
}