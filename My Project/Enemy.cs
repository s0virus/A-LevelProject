using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_Project
{
    //Enemy Spawn
    public class Enemy:GameObject
    {
        public Rectangle Laser;
        protected int PlayerFront = Game1.player.DrawPosition.X + (Game1.player.DrawPosition.Width*2);
        protected int PlayerBack = Game1.player.DrawPosition.X - Game1.player.DrawPosition.Width;
        public int Health;
        Texture2D Texture;

        public Enemy(Rectangle Pos, int health, int height, Texture2D texture)
        {
            Texture = texture;
            bounds = Pos;
            Laser = new Rectangle(Pos.X + Pos.Width / 2, Pos.Y + Pos.Height/2, Pos.Width / 10, height);
            Health = health;
            Texture = texture;
            Colour = Color.LawnGreen;
        }

        public override void Scroll(int Value, GameTime gameTime)
        {
            bounds.X += Value;
            Laser.X += Value;
            if (bounds.X < -50)
            { remove = true; }
        }

        public override void Draw(SpriteBatch sb)
        {
            if (bounds.X < PlayerFront && bounds.X + bounds.Width > PlayerBack)
            {
                sb.Draw(Game1.pixel, Laser, Colour);
            }
            sb.Draw(Texture, bounds, Color.White);
        }
    }
}
