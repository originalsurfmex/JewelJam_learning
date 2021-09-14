using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JewelJam.engine
{
    public class InputHelper
    {
        private KeyboardState _key, _keyPrev;
        private MouseState _mouse, _mousePrev;

        /// <summary>
        ///     Vector2 get mouse position
        /// </summary>
        public Vector2 MousePos { get; private set; }

        /// <summary>
        ///     boolean: register a mouse click
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private bool MouseClick { get; set; }

        public void Update()
        {
            MousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            // set up mouse & keyboard states
            _mousePrev = _mouse;
            _mouse = Mouse.GetState();
            _keyPrev = _key;
            _key = Keyboard.GetState();

            MouseClick = _mouse.LeftButton == ButtonState.Pressed && _mousePrev.LeftButton == ButtonState.Released;
        }

        /// <summary>
        ///     boolean: if left mouse button was clicked
        /// </summary>
        /// <returns> true if button was clicked and previously released</returns>
        public bool MouseLeftButton()
        {
            return _mouse.LeftButton == ButtonState.Pressed && _mousePrev.LeftButton == ButtonState.Released;
        }

        /// <summary>
        ///     boolean:
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyPressed(Keys key)
        {
            return _key.IsKeyDown(key) && _keyPrev.IsKeyUp(key);
        }
    }
}