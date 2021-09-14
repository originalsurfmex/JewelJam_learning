using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace JewelJam
{
    class GamEx : Game
    {
        protected GraphicsDeviceManager _graphics;
        protected SpriteBatch _spriteBatch;

        //keyboard mouse helpers, viewport/world scaling stuff
        protected InputHelper inputHelper;
        protected Point _worldSize, _windowSize, _screenSize;
        protected Matrix _spriteScale;

        public static Random Rand { get; private set; }
        public static ContentManager ContentMgr { get; private set; }

        public static Point WorldSize { get; protected set; }
        protected List<GameObj> _gameWorld;

        protected GamEx()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            inputHelper = new InputHelper();

            Rand = new Random();

            //wordsize is scaled to background image, grids, etc
            //_worldSize = new Point(1024, 768);
            _worldSize = WorldSize;
            _windowSize = new Point(800, 640);
        }

        //protected override void Initialize()
        //{
        //    base.Initialize();
        //}

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ContentMgr = Content;

            _gameWorld = new List<GameObj>();
            FullScreen = false;
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            HandleInput();

            foreach (GameObj obj in _gameWorld)
                obj.Update(gameTime);
        }

        protected virtual void HandleInput()
        {
            inputHelper.Update();

            if (inputHelper.KeyPressed(Keys.Escape))
                Exit();

            if (inputHelper.KeyPressed(Keys.F5))
                FullScreen = !FullScreen;

            foreach (GameObj obj in _gameWorld)
                obj.HandleInput(inputHelper);
       }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.Deferred, 
                null, null, 
                null, null, 
                null, _spriteScale);

            foreach (GameObj obj in _gameWorld)
                obj.Draw(gameTime, _spriteBatch);
            
            

            _spriteBatch.End();
        }
        /// <summary>
        /// Fullscreen boolean function than will trigger the ApplyResolution function
        /// if it is set "set { ApplyResolution(value); }"
        /// </summary>
        public bool FullScreen
        {
            get { return _graphics.IsFullScreen; }
            protected set { ApplyResolution(value); }
        }

        /// <summary>
        /// Handle scaling of screen, viewport and has a function
        /// for calculating the viewport.
        /// </summary>
        /// <param name="fullScreen"> fullscreen bool: intended to be the setter of a
        /// get/set function that gets the current state of GraphicsDeviceManager.IsFullScreen</param>
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
            {
                _screenSize = _windowSize; //screen size is key variable now
            }

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

            Viewport CalcVport(Point windowSize)
            {
                Viewport vport = new Viewport();

                //aspect ratio = width / height
                float gameAspectR = (float)_worldSize.X / _worldSize.Y;
                float windowAspectR = (float)windowSize.X / windowSize.Y;

                //relatively wide window, use full height
                if (windowAspectR > gameAspectR)
                {
                    vport.Width = (int)(windowSize.Y * gameAspectR);
                    vport.Height = windowSize.Y;
                }
                else //relatively high window, use full width
                {
                    vport.Width = (int)(windowSize.X);
                    vport.Height = (int)(windowSize.X / gameAspectR);
                }

                vport.X = (windowSize.X - vport.Width) / 2;
                vport.Y = (windowSize.Y - vport.Height) / 2;
                return vport;
            }
        }


    }
}
