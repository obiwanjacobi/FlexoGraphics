﻿/*
MIT License

Copyright (c) 2020

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using ExoGame2D.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExoGame2D
{
    public class Engine
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly Game _game;
        //private readonly Vector2 _scaledViewPort;
        //private readonly Matrix _spriteScalingFactor;
        private Matrix _spriteScale;

        public Engine(Game game)
        {
            _game = game ?? throw new ArgumentNullException(nameof(game));
            _graphics = new GraphicsDeviceManager(game);
            game.Content.RootDirectory = "Content";
            game.IsMouseVisible = true;
            Instance = this;
        }

        public Point WindowSize;
        public DrawContext DrawContext;
        public readonly GameStateManager GameState = new GameStateManager();
        public static Engine Instance { get; private set; }

        public void Initialize()
        {
            Initialize(1920, 1080);
        }

        public void Initialize(int windowX, int windowY,
            int worldX = 1920, int worldY = 1080)
        {
            if (_game.GraphicsDevice == null)
                throw new InvalidOperationException("Call Engine.Initialize after the Game is initialized.");

            DrawContext = new DrawContext(_game.GraphicsDevice);
            WindowSize = new Point(windowX, windowY);
            WorldSize = new Point(worldX, worldY);
            SetFullScreen(false);
        }

        public ContentManager Content => _game.Content;

        private Point _worldSize;
        public Point WorldSize
        {
            get => _worldSize;
            set
            {
                _worldSize = value;
                _graphics.PreferredBackBufferWidth = WorldSize.X;
                _graphics.PreferredBackBufferHeight = WorldSize.Y;
            }
        }

        public void Exit()
        {
            _game.Exit();
        }

        public Vector2 ScaledViewPort
            => ScreenToWorld(new Vector2(_graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height));

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            Vector2 viewportTopLeft = new Vector2(_game.GraphicsDevice.Viewport.X, _game.GraphicsDevice.Viewport.Y);
            float screenToWorldScale = WorldSize.X / (float)_game.GraphicsDevice.Viewport.Width;

            return (screenPosition - viewportTopLeft) * screenToWorldScale;
        }

        /// <summary>
        /// Scales the window to the desired size, and calculates how the game world should be scaled to fit inside that window.
        /// </summary>
        public void ApplyResolutionSettings(bool fullScreen)
        {
            // make the game full-screen or not
            _graphics.IsFullScreen = fullScreen;

            // get the size of the screen to use: either the window size or the full screen size
            Point screenSize = fullScreen
                ? new Point(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
                : WindowSize;

            // scale the window to the desired size
            _graphics.PreferredBackBufferWidth = screenSize.X;
            _graphics.PreferredBackBufferHeight = screenSize.Y;

            _graphics.ApplyChanges();

            // calculate and set the viewport to use
            _game.GraphicsDevice.Viewport = CalculateViewport(screenSize);

            SetTransformation();

            // calculate how the graphics should be scaled, so that the game world fits inside the window
            _spriteScale = Matrix.CreateScale((float)_game.GraphicsDevice.Viewport.Width / WorldSize.X, (float)_game.GraphicsDevice.Viewport.Height / WorldSize.Y, 1);
        }

        private void SetTransformation()
        {
            if (DrawContext != null)
            {
                DrawContext.Transformation = Matrix.CreateScale(
                                    (float)_game.GraphicsDevice.Viewport.Width / WorldSize.X,
                                    (float)_game.GraphicsDevice.Viewport.Height / WorldSize.Y, 1);
            }
        }

        /// <summary>
        /// Calculates and returns the viewport to use, so that the game world fits on the screen while preserving its aspect ratio.
        /// </summary>
        /// <param name="windowSize">The size of the screen on which the world should be drawn.</param>
        /// <returns>A Viewport object that will show the game world as large as possible while preserving its aspect ratio.</returns>
        public Viewport CalculateViewport(Point windowSize)
        {
            // create a Viewport object
            Viewport viewport = new Viewport();

            // calculate the two aspect ratios
            float gameAspectRatio = (float)WorldSize.X / WorldSize.Y;
            float windowAspectRatio = (float)windowSize.X / windowSize.Y;

            // if the window is relatively wide, use the full window height
            if (windowAspectRatio > gameAspectRatio)
            {
                viewport.Width = (int)(windowSize.Y * gameAspectRatio);
                viewport.Height = windowSize.Y;
            }
            // if the window is relatively high, use the full window width
            else
            {
                viewport.Width = windowSize.X;
                viewport.Height = (int)(windowSize.X / gameAspectRatio);
            }

            // calculate and store the top-left corner of the viewport
            viewport.X = (windowSize.X - viewport.Width) / 2;
            viewport.Y = (windowSize.Y - viewport.Height) / 2;

            return viewport;
        }

        public bool IsFullScreen => _graphics.IsFullScreen;

        public void SetFullScreen(bool fullScreen = true)
        {
            ApplyResolutionSettings(fullScreen);
        }
    }
}
