using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JewelJam
{
    public class JewelJam : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _background, _cursor;
        private Point _worldSize, _windowSize, _screenSize;
        private Matrix _spriteScale;

        InputHelper inputHelper;

        public JewelJam()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            inputHelper = new InputHelper();
        }

        //protected override void Initialize()
        //{
        //    base.Initialize();
        //}

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _background = Content.Load<Texture2D>("img/spr_background");
            _cursor = Content.Load<Texture2D>("img/spr_single_jewel1");
            _worldSize = new Point(_background.Width, _background.Height);
            _windowSize = new Point(1024, 768);

            FullScreen = false;
        }

        protected override void Update(GameTime gameTime)
        {
            inputHelper.Update();

            if (inputHelper.KeyPressed(Keys.Escape))
                Exit();

            if (inputHelper.KeyPressed(Keys.F5))
                FullScreen = !FullScreen;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
            _spriteBatch.Begin(SpriteSortMode.Deferred, null,
                null, null, null,
                null, _spriteScale);

            _spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            _spriteBatch.Draw(_cursor, Screen2World(inputHelper.MousePos), Color.White);

            _spriteBatch.End();
        }

        //----------------------------------------------------------------------------------
        bool FullScreen
        {
            get { return _graphics.IsFullScreen; }
            set { ApplyResolution(value); }
        }
        private void ApplyResolution(bool fullScreen)
        {
            _graphics.IsFullScreen = fullScreen;
            //set full screen to actual screen size if true
            if (fullScreen)
            {
                _screenSize = new Point(
                    GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                    GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            }
            else
                _screenSize = _windowSize; //screen size is key variable now
                //_screenSize = _worldSize; //screen size is key variable now

            //setup window sizes based on hard coded values in load content
            _graphics.PreferredBackBufferWidth = _screenSize.X;
            _graphics.PreferredBackBufferHeight = _screenSize.Y;
            _graphics.ApplyChanges();

            //calculate and set the viewport to use
            GraphicsDevice.Viewport = CalcVport(_screenSize);

            //scale graphics so game fits in viewport window
            _spriteScale = Matrix.CreateScale(
                (float)GraphicsDevice.Viewport.Width / _worldSize.X,
                (float)GraphicsDevice.Viewport.Height / _worldSize.Y, 1);
        }

        Viewport CalcVport(Point windowSize)
        {
            Viewport vport = new Viewport();

            float gameAspectR = (float)_worldSize.X / _worldSize.Y;
            float windowAspectR = (float)windowSize.X / windowSize.Y;

            if(windowAspectR > gameAspectR)
            {
                vport.Width = (int)(windowSize.Y * gameAspectR);
                vport.Height = windowSize.Y;
            }
            else
            {
                vport.Width = (int)(windowSize.X);
                vport.Height = (int)(windowSize.X / gameAspectR);
            }

            vport.X = (windowSize.X - vport.Width) / 2;
            vport.Y = (windowSize.Y - vport.Height) / 2;
            return vport;
        }

        Vector2 Screen2World(Vector2 screenPos)
        {
            Vector2 vPortTopLeft =
                new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y);

            float screen2WorldScale = _worldSize.X /
                (float)GraphicsDevice.Viewport.Width;

            return (screenPos - vPortTopLeft) * screen2WorldScale;
        }
    }
}