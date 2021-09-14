using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JewelJam.engine
{
    internal class SpriteGameObj : GameObj
    {
        private readonly Texture2D _sprite;
        private readonly Vector2 _origin;

        public SpriteGameObj(string sprite)
        {
            _sprite = GamEx.ContentMgr.Load<Texture2D>(sprite);
            _origin = Vector2.Zero;
        }

        public int Width => _sprite.Width;
        public int Height => _sprite.Height;

        public Rectangle BBox
        {
            get
            {
                Rectangle spriteBounds = _sprite.Bounds;
                //use the recursive position function to include parent(s) objects
                spriteBounds.Offset(PosGlobal - _origin);
                return spriteBounds;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Visible)
                spriteBatch.Draw(_sprite, PosGlobal, null, Color.White,
                    0.0f, _origin, 1.0f, SpriteEffects.None, 0);
        }
    }
}