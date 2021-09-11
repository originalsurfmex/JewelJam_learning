using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JewelJam
{
    public class InputHelper
    {
        private Vector2 _mousePos;
        private MouseState _mouse, _mousePrev;
        private KeyboardState _key, _keyPrev;
        private bool _mouseClick;

        public void Update()
        {
            _mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            // set up mouse & keyboard states
            _mousePrev = _mouse;
            _mouse = Mouse.GetState();
            _keyPrev = _key;
            _key = Keyboard.GetState();

            _mouseClick = _mouse.LeftButton == ButtonState.Pressed && _mousePrev.LeftButton == ButtonState.Released;
        }

        public Vector2 MousePos
        {
            get { return _mousePos; }
        }

        public bool MouseClick
        {
            get { return _mouseClick; }
        }

        public bool MouseLeftButton()
        {
            return _mouse.LeftButton == ButtonState.Pressed && _mousePrev.LeftButton == ButtonState.Released;
        }

        public bool KeyPressed(Keys key)
        {
            return _key.IsKeyDown(key) && _keyPrev.IsKeyUp(key);
        }
    }
}