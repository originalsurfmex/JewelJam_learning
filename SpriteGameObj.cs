using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JewelJam
{
    class SpriteGameObj : GameObj
    {
        protected Texture2D _sprite;
        protected Vector2 _origin;

        public SpriteGameObj(string sprite)
        {
            _sprite = GamEx.ContentMgr.Load<Texture2D>(sprite);
            _origin = Vector2.Zero;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(Visible)
            {
                spriteBatch.Draw(_sprite, Position, null, Color.White,
                    0.0f, _origin, 1.0f, SpriteEffects.None, 0);
            }
        }

        public int Width { get { return _sprite.Width; } }
        public int Height { get { return _sprite.Height; } }

        public Rectangle BBox
        {
            get
            {
                Rectangle spriteBounds = _sprite.Bounds;
                spriteBounds.Offset(Position - _origin);
                return spriteBounds;
            }
        }
    }
}
