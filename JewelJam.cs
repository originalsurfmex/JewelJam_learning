using JewelJam.engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JewelJam
{
    /// <summary>
    ///     this only adds children objects to the game world parent list via Load Content.  There may
    ///     also be some WIP or custom stuff in its Draw Loop.
    ///
    ///     parent -> child hierachy:
    ///     game world parent list = game object with a list of game objects
    ///     children = game objects
    /// 
    ///     the game world parent list has functions that recursively Update, Draw, handle input,
    ///     etc for each of the children those functions extend the base Monogame Game class
    ///     which handles the game loop.
    /// </summary>
    internal class JewelJam : GamEx
    {
        private const int XGrids = 5;
        private const int YGrids = 10;
        private int _cellSize;
        private Texture2D _cursor;
        private Vector2 _gridOffset;

        protected override void LoadContent()
        {
            base.LoadContent();

            var background = new SpriteGameObj("img/spr_background");
            _worldSize = new Point(background.Width, background.Height);
            _gameWorld.AddChild(background);

            _cellSize = new Jewel(0).Width + 20;
            _gridOffset = new Vector2(90, _cellSize * 2 + 20);
            var jewelGrid = new JewelGrid(XGrids, YGrids, _cellSize, _gridOffset);
            _gameWorld.AddChild(jewelGrid);

            _cursor = Content.Load<Texture2D>("img/spr_single_jewel1");
            FullScreen = false;
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _spriteBatch.Begin(SpriteSortMode.Deferred, null,
                null, null, null,
                null, _spriteScale);

            _spriteBatch.Draw(_cursor, Screen2World(_inputHelper.MousePos), Color.White);

            _spriteBatch.End();
        }
    }
}