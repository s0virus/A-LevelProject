using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_Project
{
    public abstract class GameObject
    {
        protected Color Colour = Color.White;
        public Rectangle bounds;
        public bool remove = false;

        public abstract void Draw(SpriteBatch sb);

        public abstract void Scroll(int value, GameTime gameTime);
    }
}
