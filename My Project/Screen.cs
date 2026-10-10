using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_Project
{
    public abstract class Screen
    {
        protected Rectangle bounds;
        protected float volume;

        public Screen(int width, int height)
        {
            bounds = new Rectangle(0, 0, width, height);
            volume = Game1.GetVolume(0);
        }

        public abstract void Update(int x, int y, bool lc, bool rc, bool timer, GameTime gameTime);

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(Game1.pixel, bounds, Color.Black);
        }

    }
}
