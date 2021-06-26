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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ExoGame2D.UI
{
    public abstract class UIControlBase : IUIControl
    {
        protected UIControlBase(string name)
        {
            Name = name;
        }
        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2 Location { get; set; }
        public bool Enabled { get; set; }
        public bool Visible { get; set; }
        public Color Color { get; set; }
        public Color OutlineColor { get; set; }
        public Color MouseOverColor { get; set; }
        public bool DrawWindowChrome { get; set; }
        public bool MouseOver { get; set; }
        public string Name { get; }

        protected Texture2D ControlTexture { get; set; }

        private string _controlTextureName;
        public string ControlTextureName
        {
            get { return _controlTextureName; }
            set
            {
                ControlTexture = Engine.Instance.Content.Load<Texture2D>(value);
                _controlTextureName = value;
            }
        }
    }
}
