using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_Project
{
    public class Projectile:GameObject
    {
        public Vector2 Pos;
        Vector2 move;
        protected int ScreenWidth, ScreenHeight;

        public Projectile(Vector2 move, Vector2 Pos, int Width, int Height)
        {
            this.Pos = Pos;
            this.move = move;
            bounds = new Rectangle((int)Pos.X, (int)Pos.Y, 5, 5);
            ScreenWidth = Width;
            ScreenHeight = Height;
        }

 

        public override void Scroll(int Value, GameTime gameTime)
        {
            Pos += move * gameTime.ElapsedGameTime.Milliseconds;
            Pos.X += Value;
            bounds = new Rectangle((int)Pos.X, (int)Pos.Y, 5, 5);
            if (Pos.X<0 || Pos.X>ScreenWidth || Pos.Y>ScreenHeight || Pos.Y<0)
            {
                remove = true;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(Game1.pixel, bounds, Colour);
        }
    }
}
