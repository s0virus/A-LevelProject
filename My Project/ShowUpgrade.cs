using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_Project
{
    public class ShowUpgrade
    {
        Label Show;
        int height;
        double Move = 0;
        public bool remove = false;
        public ShowUpgrade(int width, int height)
        {
            this.height = height;
            Show = new Label((int)(width * 0.45), (int)(height * 0.25), "+1 Upgrade", Color.White);
        }

        public void Update(GameTime gameTime)
        {
            Move += gameTime.ElapsedGameTime.TotalMilliseconds * 50;
            Show.Position.Y = (int)((height * 0.25) - (Move / height));
            if (Show.Position.Y < height / -10)
            {
                remove = true;
            }
        }
        
        public virtual void Draw(SpriteBatch sb)
        {
            Show.Draw(sb);
        }
    }
}
