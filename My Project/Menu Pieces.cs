using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_Project
{
    public class Button
    {
        protected Rectangle bounds;

        public string text;

        bool hover;

        protected Color HoverColor;
        protected Color TextColour;
        protected Color BackColor;


        //Creates a customisable button
        public Button(int x, int y, int width, int height, string text, Color Text, Color Hover, Color Back)
        {
            hover = false;
            bounds = new Rectangle(x, y, width, height);
            this.text = text;
            TextColour = Text;
            HoverColor = Hover;
            BackColor = Back;
        }

        public bool CheckClick(int x, int y)
        {
            Rectangle mousepos = new Rectangle(x, y, 1, 1);
            if (mousepos.Intersects(bounds))
            {
                return true;
            }
            else { return false; }
        }

        //checks if the mousse is hovering over a button
        public void CheckHover(int x, int y)
        {
            Rectangle mousepos = new Rectangle(x, y, 1, 1);
            if (mousepos.Intersects(bounds))
            {
                hover = true;
            }
            else
            {
                hover = false;
            }
        }

        public virtual void Draw(SpriteBatch sb)
        {
            if (hover)
            {
                sb.Draw(Game1.pixel, bounds, BackColor);
                sb.DrawString(Game1.font, text, new Vector2(bounds.X, bounds.Y), HoverColor);
            }
            else
            {
                sb.Draw(Game1.pixel, bounds, BackColor);
                sb.DrawString(Game1.font, text, new Vector2(bounds.X, bounds.Y), TextColour);
            }
        }

    }

    //Creates displayed text
    public class Label
    {
        public Vector2 Position;
        protected Color LabelColor;
        public string text;

        public Label(int x, int y, string text, Color LabelColor)
        {
            Position = new Vector2(x, y);
            this.text = text;
            this.LabelColor = LabelColor;
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.DrawString(Game1.font, text, Position, LabelColor);
        }
    }
}
