using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JewelJam
{
    /// <summary>
    /// this is basically the game world, it extends whatever
    /// base game classes there are to extend
    /// </summary>
    class JewelJam : GamEx
    {
        private Texture2D _cursor;

        private const int _xGrids = 5;
        private const int _yGrids = 10;
        private int _cellSize;
        private Vector2 _gridOffset;

        public JewelJam()
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            
            SpriteGameObj _background = new SpriteGameObj("img/spr_background");
            _worldSize = new Point(_background.Width, _background.Height);
            _gameWorld.Add(_background);

            _cellSize = new Jewel(0).Width + 20;
            _gridOffset = new Vector2(90, _cellSize * 2 + 20);
            JewelGrid jewelGrid = new JewelGrid(_xGrids, _yGrids, _cellSize, _gridOffset);
            _gameWorld.Add(jewelGrid);
            
            _cursor = Content.Load<Texture2D>("img/spr_single_jewel1");
            FullScreen = false;
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _spriteBatch.Begin(SpriteSortMode.Deferred, null,
                null, null, null,
                null, _spriteScale);
            
            _spriteBatch.Draw(_cursor, Screen2World(inputHelper.MousePos), Color.White);

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
     }
}