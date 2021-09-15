using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JewelJam.engine
{
    /// <summary>
    /// the game world calls on this Game Object which is a list of Game Objects
    /// the methods in this class recursively update each Game Object as part of the extended
    /// Monogame Game loop (its extended with a custom class that extends Game)
    ///
    /// this Game Object List is part of a recursive Parent -> Child hierarchy
    /// </summary>
    internal class GameObjList : GameObj
    {
        private readonly List<GameObj> _children;

        public GameObjList()
        {
            _children = new List<GameObj>();
        }

        /// <summary>
        /// sets up a recursive Parent->Child hierarchy
        /// </summary>
        /// <param name="obj">this is a Game Object</param>
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