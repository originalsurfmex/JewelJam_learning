﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JewelJam.engine
{
    /// <summary>
    /// extends the Monogame Game loop with some boilerplate like GraphicsDeviceManager, InputHelper,
    /// window sizing/scaling, ContentManager, SpriteBatch
    ///
    /// also uses a Game Object that is a List of Game Objects called a _gameWorld
    /// this game world recursively updates all the Game Objects as part of the Draw, Update, Input etc loops
    /// those objects are part of a Parent -> Child hierarchy, this allows them to be recursively updated
    /// </summary>
    internal class GamEx : Game
    {
        private readonly GraphicsDeviceManager _graphics;

        //keyboard mouse helpers, viewport/world scaling stuff
        protected readonly InputHelper _inputHelper;

        private readonly Point _windowSize;
        //private static Point WorldSize => default; //this is a default value return

        //protected List<GameObj> _gameWorld;
        protected GameObjList _gameWorld;
        private Point _screenSize;
        protected SpriteBatch _spriteBatch;
        protected Matrix _spriteScale;
        protected Point _worldSize;

        public static Random Rand { get; private set; }
        private static Point WorldSize => default;
        //public static Point WorldSize { get; protected set; } //same as default return
        public static ContentManager ContentMgr { get; private set; }
        
        protected GamEx()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _inputHelper = new InputHelper();
            Rand = new Random();

            //world size is scaled to background image, grids, etc
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

            //_gameWorld = new List<GameObj>();
            _gameWorld = new GameObjList();
            FullScreen = false;
        }

        protected override void Update(GameTime gameTime)
        {
            HandleInput();
            _gameWorld.Update(gameTime);
        }

        protected virtual void HandleInput()
        {
            _inputHelper.Update();

            if (_inputHelper.KeyPressed(Keys.Escape))
                Exit();
            if (_inputHelper.KeyPressed(Keys.F5))
                FullScreen = !FullScreen;

            _gameWorld.HandleInput(_inputHelper);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.Deferred,
                null, null,
                null, null,
                null, _spriteScale);

            // foreach (GameObj obj in _gameWorld)
            //     obj.Draw(gameTime, _spriteBatch);

            _gameWorld.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();
        }


        /// <summary>
        ///     Fullscreen boolean function than will trigger the ApplyResolution function
        ///     if it is set "set { ApplyResolution(value); }"
        /// </summary>
        protected bool FullScreen
        {
            get => _graphics.IsFullScreen;
            set => ApplyResolution(value);
        }

        /// <summary>
        ///     Handle scaling of screen, viewport and has a function
        ///     for calculating the viewport.
        /// </summary>
        /// <param name="fullScreen">
        ///     fullscreen bool: intended to be the setter of a
        ///     get/set function that gets the current state of GraphicsDeviceManager.IsFullScreen
        /// </param>
        private void ApplyResolution(bool fullScreen)
        {
            _graphics.IsFullScreen = fullScreen;
            //set full screen to actual screen size if true
            if (fullScreen)
                _screenSize = new Point(
                    GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                    GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            else
                _screenSize = _windowSize; //screen size is key variable now

            //setup window sizes based on hard coded values in load content
            _graphics.PreferredBackBufferWidth = _screenSize.X;
            _graphics.PreferredBackBufferHeight = _screenSize.Y;
            _graphics.ApplyChanges();

            //calculate and set the viewport to use
            GraphicsDevice.Viewport = CalcViewport(_screenSize);

            //scale graphics so game fits in viewport window
            _spriteScale = Matrix.CreateScale(
                (float) GraphicsDevice.Viewport.Width / _worldSize.X,
                (float) GraphicsDevice.Viewport.Height / _worldSize.Y, 1);

            Viewport CalcViewport(Point windowSize)
            {
                var viewport = new Viewport();

                //aspect ratio = width / height
                float gameAspectR = (float) _worldSize.X / _worldSize.Y;
                float windowAspectR = (float) windowSize.X / windowSize.Y;

                //relatively wide window, use full height
                if (windowAspectR > gameAspectR)
                {
                    viewport.Width = (int) (windowSize.Y * gameAspectR);
                    viewport.Height = windowSize.Y;
                }
                else //relatively high window, use full width
                {
                    viewport.Width = windowSize.X;
                    viewport.Height = (int) (windowSize.X / gameAspectR);
                }

                viewport.X = (windowSize.X - viewport.Width) / 2;
                viewport.Y = (windowSize.Y - viewport.Height) / 2;
                return viewport;
            }
        }

        /// <summary>
        ///     Scale and locate a screen position into the viewport
        ///     by scaling and locating with the world
        /// </summary>
        /// <param name="screenPos">screen position</param>
        /// <returns></returns>
        protected Vector2 Screen2World(Vector2 screenPos)
        {
            var vPortTopLeft =
                new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y);

            float screen2WorldScale = _worldSize.X /
                                      (float) GraphicsDevice.Viewport.Width;

            return (screenPos - vPortTopLeft) * screen2WorldScale;
        }
    }
}