using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace My_Project
{
    public class Map
    {
        Queue<Box> Blocks = new Queue<Box>();
        List<Enemy> targets = new List<Enemy>();
        List<Projectile> bullets = new List<Projectile>();
        Box TopBox;
        Rectangle Wall;
        Vector2 Direction, MousePos;
        int ScreenHeight, ScreenWidth;
        int Distance = 0;
        int MaxWidth, MinWidth;
        int MaxHeight, MinHeight;
        int w = 0, h = 0;
        int totaldistance = 0;
        public int MaxScore = 0, gunDamage;
        protected double shootTimer = 0;
        public double FireRate;
        float volume;

        Random random = new Random();

        public Map(int Width, int Height) //assigns all constant values relative to Resolution when the map is created
        {
            ScreenWidth = Width;
            ScreenHeight = Height;
            MaxWidth = Width / 2;
            MinWidth = Width / 5;
            MaxHeight = Height / 2;
            MinHeight = Height / 5;
            Generate();
        }

        public int HorizontalCollision(Rectangle Player) //checks if each box wall collieds with the player
        {
            foreach (Box box in Blocks)
            {
                int temp = box.HorizontalCollision(Player); //assigns temp to the overlap of the player and the Box
                if (temp != 0)
                    return temp; //returns temp and breaks the 'foreach' if overlap is not 0
            }
            if (TopBox != null) //only checks when temp is not returned previously
            {
                int temp = TopBox.HorizontalCollision(Player); //assigns temp to overlap of the player and the TopBox (High obstacle)
                if (temp != 0)
                    return temp; //returns the overlap if it's not 0
            }
            return 0; //returns 0 if no overlap
        }

        public int VerticalCollision(Rectangle Player) //checks if each box cieling or floor collides with the player
        {
            foreach (Box box in Blocks)
            {
                int temp = box.VerticalCollision(Player); //assigns temp to the overlap between the player and a horisontal surface of each box
                if (temp != 0)
                    return temp; //returns temp if it's not 0
            }
            if (TopBox != null) //only checks if temp is not returned previously
            {
                int temp = TopBox.VerticalCollision(Player); //assigns temp to overlap of the player and the TopBox (High obstacle)
                if (temp != 0)
                    return temp; //returns the overlap if its not 0
            }

            return 0; //returns 0 if no overlap
        }


        public void Generate() //generates flatter terrain to start on
        {
            int h, w;
            for (int i = 0; i < 3; i++) //creates 3 boxes with shorter range of heights
            {
                w = random.Next(MinWidth * 2, MaxWidth); //random width (w) from min value to max value
                h = random.Next(MinHeight,(int)(MinHeight * 1.5)); //ranwom height (h) form min value to almost double min value (not as large as max value)
                Blocks.Enqueue(new Box(Distance, ScreenHeight - h, w, h, Color.Cyan)); //adds new Box to Blocks using the values generated
                Distance += w; //increases Distance by Box width (w)
            }
        }

        //moves map back when the player overlaps a map Box
        public void Undo(int Hcollision, GameTime gameTime)
        {
            totaldistance += Hcollision;
            foreach (Box box in Blocks)
            {
                box.Scroll(Hcollision, gameTime); //moves map segments back
            }
            foreach (Enemy enemy in targets)
            {
                enemy.Scroll(Hcollision, gameTime); //moves enemies back
            }
            foreach (Projectile bullet in bullets)
            {
                bullet.Scroll(Hcollision, gameTime); //moves projectiles back
            }
            if(TopBox != null)
                TopBox.Scroll(Hcollision, gameTime); //moves sliding obstacle back
        }

        public int ReturnScore() //retruns current score
        {
            return MaxScore;
        }

        //sets all map variables to starting values
        public void Reset()
        {
            shootTimer = 2/FireRate;
            totaldistance = 0;
            MaxScore = 0;
            Distance = 0;
            Blocks.Clear();
            targets.Clear();
            bullets.Clear();
            Generate(); //generates starting (easy) map section
            Wall = new Rectangle(ScreenWidth / 100, 0, ScreenWidth / 40, ScreenHeight);
            FireRate = Game1.FireRate;
            gunDamage = Game1.Damage;
            TopBox = null;
            volume = Game1.GetVolume(0);
            Debug.WriteLine("map reset");
        }

        public void Update(int Move, int Hcollision, Rectangle Player, int x, int y, bool lc, bool rc, GameTime gameTime)
        {

            MousePos = new Vector2(x, y);

            //Wall kill and move
            if (Player.X < Wall.X+Wall.Width)
            {
                Game1.player.Hit(3);
            }
            else if (Wall.X < 10) //keeps wall on screen 
                Wall.X = 10;
            Wall.X += (int)gameTime.ElapsedGameTime.TotalMilliseconds / (ScreenWidth/120); //moves wall at speed relative to screenwidth and elapsed time

            //scoreboard value
            if (totaldistance / -100 > MaxScore)
                MaxScore = totaldistance / -100;

            //Move Everything according to Player Velocity
            if (Hcollision == 0)
            {
                totaldistance += Move;
                Wall.X += Move;
                foreach (Box box in Blocks)
                {
                    box.Scroll(Move, gameTime);
                }
                foreach (Enemy enemy in targets)
                {
                    enemy.Scroll(Move, gameTime);
                }
                foreach (Projectile bullet in bullets)
                {
                    bullet.Scroll(Move, gameTime);
                }
                if(TopBox!=null)
                    TopBox.Scroll(Move, gameTime);
            }
            else
            {
                foreach (Projectile bullet in bullets)
                {
                    bullet.Scroll(0, gameTime); // Moves Projectiles even if screen stays stationary due to collisions
                }
            }

            Box Boxtemp;
            Blocks.TryPeek(out Boxtemp);
            if (Boxtemp != null)
            {
                if (Boxtemp.remove) //checks for remove bool in Box class
                {
                    Blocks.Dequeue(); //removes left most block
                    Distance = 0;
                    foreach (Box box in Blocks)
                    {
                        Distance += box.Width; //gets total distance of map coverage
                    }

                    //Keeps distance of the generated map above screen width
                    while (Distance < ScreenWidth + MaxWidth * 5) //checks if map coverage is greater than a set minimum value relative to the screen and max width
                    {
                        w = random.Next(MinWidth, MaxWidth); //random width widthin a range
                        h = random.Next(MinHeight, MaxHeight + MinHeight); //random height within a range
                        int Rand = random.Next(1, 4); //random colour
                        Color Colour = Color.Blue;
                        switch (Rand)
                        {
                            case 1:
                                Colour = Color.Cyan;
                                break;
                            case 2:
                                Colour = Color.Red;
                                break;
                            case 3:
                                Colour = Color.Magenta;
                                break;
                            case 4:
                                Colour = Color.Lime;
                                break;
                        }
                        if (w > MaxWidth / 2 && TopBox == null) //checks if high obstacle is compatible with each new map section
                        {
                            TopBox = new Box(Distance + w / 3, 0, w / 4, ScreenHeight - (h + ScreenHeight /10), Colour); //places high obstacle in the centre of a long block
                        }
                        else if (TopBox != null && TopBox.remove == true)
                            TopBox = null; //removes high obstacle when off screen
                        Blocks.Enqueue(new Box(Distance, ScreenHeight - h, w, h, Colour)); //adds the new box to Blocks
                        Distance += w; //increases total map coverage by new box width
                    }
                }
            }

            Enemy Enemtemp = null;
            Projectile Projtemp = null;

            //keeps enemies generated at all times
            while (targets.Count < 5 + MaxScore / 150) //if there are less than a set number of enemies (increases with Score), loop this
            {
                Rectangle Rectemp;
                bool place;
                do
                {
                    place = true;
                    Rectemp = new Rectangle(ScreenWidth + random.Next(0, ScreenWidth), random.Next(ScreenHeight/40, ScreenHeight/8), ScreenWidth/30, ScreenWidth/30); //creates a rectangle in a random high place
                    foreach (Enemy enemy in targets)
                    {
                        if (Rectemp.Intersects(enemy.bounds)) //checks if the rectangle overlaps any existing enemies
                        {
                            place = false; //disables spawning if there is an overlap
                            break;
                        }
                    }
                } while (!place); //repeats untill rectangle is in an open area
                int H = random.Next(1, 1 + MaxScore/15); //sets H to random int between 1 and a variable depending on current score
                targets.Add(new Enemy(Rectemp, H, ScreenHeight, Game1.EnemyTexture)); //adds new enemy (Rectemp = position, H = health) to targets list
            }

            //kill and Hit detection
            foreach (Enemy enemy in targets)
            {
                //player hit
                if (enemy.Laser.Intersects(Player)) //checks if Player is hit by an enemy laser
                {
                    Game1.player.Hit(1); //removes 1 life
                }

                //enemy hit
                foreach (Projectile bullet in bullets)
                {
                    if (bullet.bounds.Intersects(enemy.bounds)) //checks if an enemy is hit by a projectile
                    {
                        enemy.Health -= gunDamage; //decreases enemy health by gun damage
                        if (enemy.Health < 1) //check health
                        {
                            Game1.kill.Play(volume, 0.0f, 0.0f); //plays enemy death sound
                            Game1.UpgradeChance(); //chance to get a gun upgrade point
                            enemy.remove = true; //makes enemy able to be removed when dead
                        }
                        Projtemp = bullet; //sets projectile to be remove
                        break;
                    }
                }
                if (enemy.remove)
                {
                    Enemtemp = enemy; //sets enemy to be removed if needed
                    break;
                }
                if (Projtemp != null) //stops overwriting the projectile to be removed
                    break;
            }

            //remove items from lists
            if (Projtemp == null) //stops overwriting the projectile to be removed
            {
                foreach (Projectile bullet in bullets) //cycles all projectiles in bullet
                {
                    if (bullet.remove)
                    {
                        Projtemp = bullet; //sets projectile to be removed
                        break;
                    }
                }
            }
            else
            {
                bullets.Remove(Projtemp); //removes projectile stored in Projtemp from the bullet list
            }
            if (Enemtemp != null)
            {
                targets.Remove(Enemtemp); //removes enemy stored in Enemtemp from targets list
            }

            //shoot gun
            if (lc && shootTimer >= 2/FireRate) //checks for left click and fire rate cool down
            {
                Vector2 temp4 = new Vector2(Player.X+Player.Width/2, Player.Y+Player.Height/2); //Gets Position of the middle of the player
                Direction = MousePos - temp4; //sets Direction as a magnitude and direction between the mouse position and the player position
                Direction.Normalize(); //removes the magnitude from Direction
                bullets.Add(new Projectile(Direction, temp4, ScreenWidth, ScreenHeight)); //creates new projectile (Direction = direction, Temp4 = start position, ScreenLenghts = remove bounds) and adds it to bullets list
                shootTimer = 0; //Sets timer to 0 for shoot cool down
                Game1.gun.Play(volume, 0.0f, 0.0f); //plays shoot sound effect
            }
            if (shootTimer < FireRate)
                shootTimer += gameTime.ElapsedGameTime.TotalSeconds; //increments shoot cool down if less than firerate (if not yet ready to shoot)
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Game1.pixel, Wall, Color.Orange); //draws orange rectangle in the bounds of Wall
            sb.Draw(Game1.pixel, new Rectangle(Wall.X + 2, Wall.Y + 2, Wall.Width - 4, Wall.Height - 4), Color.Black); //fills in the middle of the wall
            sb.Draw(Game1.pixel, new Rectangle((int)MousePos.X, (int)MousePos.Y, 5, 5), Color.White); //draws small box at the mouse pointer
            foreach (Box box in Blocks)
            {
                box.Draw(sb); //cycles and draws all Boxes in Blocks (Draws Map)
            }
            if (TopBox != null)
                TopBox.Draw(sb); //Draws high obstacle

            foreach (Enemy enemy in targets)
            {
                enemy.Draw(sb); //cycles and draws all enemies in targets list
            }
            foreach (Projectile bullet in bullets)
            {
                bullet.Draw(sb); //cycles and draws all Projectiles in bullets list
            }
        }
    }
}
