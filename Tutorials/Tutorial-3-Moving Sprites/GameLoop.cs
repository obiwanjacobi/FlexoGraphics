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
using ExoGame2D.SceneManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ExoGame2D.Tutorials.Tutorial3_MovingSprites
{
    public class GameLoop : Game
    {
        private readonly Scene _scene;
        private readonly Sprite _logo;
        private readonly Engine _engine;

        public GameLoop()
        {
            _engine = new Engine(this);
            _scene = new Scene();
            _logo = new Sprite("Logo");
        }

        protected override void Initialize()
        {
            base.Initialize();
            _engine.Initialize();
        }

        protected override void LoadContent()
        {
            Window.AllowUserResizing = true;
            _logo.LoadContent("ExoEngineLogo");
            _logo.Location = new Vector2(100, 100);
            _logo.Velocity = new Vector2(5, 5);

            _scene.Add(RenderLayer.Layer1, _logo);
        }

        protected override void Update(GameTime gameTime)
        {
            InputHelper.Update();

            if (InputHelper.KeyPressed(Keys.Escape))
            {
                Exit();
            }

            if (InputHelper.KeyPressed(Keys.F))
            {
                _engine.SetFullScreen(!_engine.IsFullScreen);
            }

            var worldViewport = Engine.Instance.CoordinateSpace.World.Viewport;

            if (_logo.X + _logo.Width > worldViewport.Width)
            {
                _logo.VX = -_logo.VX;
            }

            if (_logo.X < 0)
            {
                _logo.VX = -_logo.VX;
            }

            if (_logo.Y + (_logo.Height) > worldViewport.Height)
            {
                _logo.VY = -_logo.VY;
            }

            if (_logo.Y < 0)
            {
                _logo.VY = -_logo.VY;
            }

            _logo.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _engine.DrawContext.BeginDraw();
            _scene.Draw(_engine.DrawContext, gameTime);
            _engine.DrawContext.EndDraw();
        }
    }
}