using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JewelJam
{
    class GameObj
    {
        public Vector2 Position { get; set; }
        protected Vector2 _velocity;

        public bool Visible { get; set; }

        public GameObj()
        {
            Position = Vector2.Zero;
            _velocity = Vector2.Zero;
            Visible = true;
        }

        public virtual void HandleInput(InputHelper inputHelper)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            Position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        public virtual void Reset()
        {
            _velocity = Vector2.Zero;
        }

    }
}
