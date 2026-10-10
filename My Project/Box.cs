using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;

namespace My_Project
{
    //section of map
    public class Box:GameObject
    {
        protected Rectangle inbounds, top, intop;

        public int Width
        {
            get { return bounds.Width; }
        }

        //moves map left and right
        public override void Scroll(int Value, GameTime gameTime)
        {
            bounds.X += Value;
            inbounds.X += Value;
            if (bounds.X < (0 - bounds.Width))
            {
                remove = true;
            }
        }

        public Box(int x, int y, int Width, int Height, Color BoxColor)
        {
            bounds = new Rectangle(x, y, Width, Height);
            Colour = BoxColor;
            inbounds = new Rectangle(x + 1, y + 1, Width - 2, Height - 2);
        }

        //detects collisions with walls
        public int HorizontalCollision(Rectangle Player)
        {
            Rectangle HCL = new Rectangle(bounds.X, bounds.Y + 3, 1, bounds.Height - 6);
            Rectangle HCR = new Rectangle(bounds.X + bounds.Width, bounds.Y + 3, 1, bounds.Height - 6);
            if (HCR.Intersects(Player))
            {
                return (Math.Abs(Player.X - HCR.X + HCR.Width)) * -1;
            }
            else if (HCL.Intersects(Player))
            {
                return (Math.Abs(Player.X + Player.Width - HCL.X));
            }
            else { return 0; }
        }

        //detects collisions with floor
        public int VerticalCollision(Rectangle Player)
        {
            Rectangle VCT = new Rectangle(bounds.X, bounds.Y, bounds.Width, 1);
            Rectangle VCB = new Rectangle(bounds.X, bounds.Y + bounds.Height, bounds.Width, 1);
            if (VCT.Intersects(new Rectangle(Player.X, Player.Y + Player.Height - Player.Height/4 - 2, Player.Width, Player.Height / 4)))
            {
                return Math.Abs((Player.Y + Player.Height) - bounds.Y);
            }
            else if (VCB.Intersects(new Rectangle(Player.X, Player.Y, Player.Width, Player.Height / 4)))
            {
                return Math.Abs(Player.Y - (bounds.Y + bounds.Height)) * -1;
            }
            else 
            { 
                return 0; 
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(Game1.pixel, bounds, Colour);
            sb.Draw(Game1.pixel, inbounds, Color.Black);
        }
    }
}
