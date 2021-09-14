using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JewelJam.engine
{
    internal class GameObjList : GameObj
    {
        private readonly List<GameObj> _children;

        public GameObjList()
        {
            _children = new List<GameObj>();
        }

        public void AddChild(GameObj obj)
        {
            _children.Add(obj);
            obj.Parent = this;
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            foreach (GameObj obj in _children)
                obj.HandleInput(inputHelper);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameObj obj in _children)
                obj.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (GameObj obj in _children)
                obj.Draw(gameTime, spriteBatch);
        }

        public override void Reset()
        {
            foreach (GameObj obj in _children)
                obj.Reset();
        }
    }
}