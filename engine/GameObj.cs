using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JewelJam.engine
{
    internal class GameObj
    {
        private Vector2 _velocity;

        protected GameObj()
        {
            PosLocal = Vector2.Zero;
            _velocity = Vector2.Zero;
            Visible = true;
        }

        // parent object
        public GameObj Parent { get; set; }
        public Vector2 PosLocal { get; set; }

        /// <summary>
        ///     recursive function: keep on adjusting the local position
        ///     to match the parent until Parent == null
        /// </summary>
        protected Vector2 PosGlobal
        {
            get
            {
                if (Parent == null)
                    return PosLocal;
                return PosLocal + Parent.PosGlobal;
            }
        }

        protected bool Visible { get; set; }

        public virtual void HandleInput(InputHelper inputHelper)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
            PosLocal += _velocity * (float) gameTime.ElapsedGameTime.TotalSeconds;
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