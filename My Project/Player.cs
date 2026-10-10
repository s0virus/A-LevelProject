using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace My_Project
{
    public class Player
    {
        public Texture2D PlayerTexture;

        public Rectangle Position, DrawPosition, StartPos, JumpCoolDown, CoolDownBack;

        public bool AirJump = false, JumpAble, SwingAble, MoveKey;

        protected bool Slide;

        protected int InitY, MaxVX = -5, MinVX = 5, SwingCount = 0, ScreenWidth, ScreenHeight;

        protected double AirTime, immunity, Airjumpinterval = 5, VelY;

        public double VelX = 0, XAccel, PreVel = 0;

        public int Lives = 3;

        float volume;

        public int Width;

        public int Height;
        

        public void Initialize(Texture2D texture, Vector2 playerPosition, int ScreenWidth, int ScreenHeight)
        {
            Width = ScreenWidth / 25; //sets Player width (Width)
            Height = Width * 2; //sets Player height (Height) as double the width
            PlayerTexture = texture; //sets the texture used to draw player

            StartPos = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, Width, Height); //assights the rectangle 'StartPos' to a constant location and size

            this.ScreenWidth = ScreenWidth;
            this.ScreenHeight = ScreenHeight;

            CoolDownBack = new Rectangle(ScreenWidth - ScreenWidth / 50, ScreenHeight / 3, ScreenWidth / 100, ScreenHeight / 7); //creates a rectangle to use to draw the back plate of the Air Jump cooldown bar (visual only)
            JumpCoolDown = CoolDownBack;
        }

        public void Reset() //called when 'Play' is clicked to set everything to start values
        {
            JumpCoolDown = new Rectangle(ScreenWidth - ScreenWidth / 50, ScreenHeight / 3, ScreenWidth / 100, ScreenHeight / 7);
            AirJump = false;
            Airjumpinterval = 5;
            Lives = 3;
            DrawPosition = StartPos; //sets variable 'DawPosition' to constant 'StartPos'
            Slide = false;
            VelX = 0;
            volume = Game1.GetVolume(0); //gets volume without changing it
            Debug.WriteLine("Player reset");
        }

        public void Update(GameTime gameTime)
        {
            //checks for time intervals
            if (Airjumpinterval < 5) //if its less than 5 seconds since the last Air Jump, increase the variable
            {
                Airjumpinterval += gameTime.ElapsedGameTime.TotalSeconds;
                JumpCoolDown.Y = CoolDownBack.Y + CoolDownBack.Height - (int)(Airjumpinterval * ScreenHeight / 35);
                JumpCoolDown.Height = (int)(Airjumpinterval * ScreenHeight / 35);
            }

            if (immunity < 1)
            {
                immunity += gameTime.ElapsedGameTime.TotalSeconds; //immunity frames (1 second)
            }


            MoveKey = false;
            XAccel = VelX * -5; //sets Acceleration to  -5 multiple of Velocity
            if(Keyboard.GetState().IsKeyDown(Keys.LeftControl)) //priority of slide over all other movement
            {
                if (!Slide)
                {
                    Slide = true;
                    DrawPosition.Height /= 2;
                    DrawPosition.Y += Height/2;
                }
                XAccel = VelX * -0.5; //sets acceleration to small value still opposite direction to velocity (Slow deceleration)
            }
            else
            {
                if (Slide)
                {
                    Slide = false; //sets slide to false and corrects player dimentions
                    DrawPosition.Y -= Height/2;
                    DrawPosition.Height *= 2;
                }
                //Left
                if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right)) //detects Key inputs
                {
                    XAccel = -25; //sets acceleration in Left Direction (towards negative X)
                    MoveKey = true;
                }
                //Right
                else if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    XAccel = 25; //sets acceleration in Right direction (towards positive X)
                    MoveKey = true;
                }

                //Jump
                if ((Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Up)) && (JumpAble || SwingAble)) //checks for Jump Key input and conditions
                {
                    if (JumpAble) //standard jump
                    {
                        Game1.jump.Play(volume, 0.0f, 0.0f); //plays lowest frequency jumps sound
                        InitY = -5; //gives initial velocity
                        JumpAble = false; //prevents Jumping more than once
                    }
                    else if (SwingCount < 1) //jump off a wall
                    {
                        Game1.jump.Play(volume, 0.5f, 0.0f); //plays slightly higher pitched jump sound
                        AirJump = true;
                        InitY = -6; //gives slightly larger initial velocity
                        SwingAble = false;
                        SwingCount++; //prevents more than one Wall Jump
                        if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
                        {
                            VelX = 0;
                            PreVel = -6; //gives initial horisontal velocity away from the wall
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
                        {
                            VelX = 0;
                            PreVel = 6; //gives initial horisontal velocity away from the wall
                        }
                    }
                    AirTime = 0; //always resets time to 0 at the start of each jump for gravity accelerated calculations
                }

                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && AirJump && Airjumpinterval >= 5)
                {
                    Game1.jump.Play(volume, 1.0f, 0.0f); //plays highest pitch jump sound
                    InitY = -7; //gives large initial upward velocity
                    AirTime = 0; //resets time to 0 for gravity accelerated calculations
                    AirJump = false; //prevents another airjump
                    Airjumpinterval = 0; //sets 5s timer to 0 (Used on line 76)
                }
            }
        }

        //Move X
        public void Move(GameTime gameTime) //horisontal movement calculations
        {
            if (VelX < MaxVX || VelX > MinVX) { XAccel = VelX * -5; } //slows down the player if velocity is too high
            VelX = PreVel + XAccel * gameTime.ElapsedGameTime.TotalMilliseconds / 1000; //uses time from 0 each calculation
            PreVel = VelX; //sets initial velocity to new velocity (because time is reset each update)
        }

        //Holds Player on a floor or wall
        public void Freeze(int Vcollision) //moves the player Back
        {
            if (Vcollision == 0) //if no overlap between player and a floor/cielng, must be held on a wall
            {
                JumpAble = false;
                if (SwingCount < 1)
                {
                    SwingAble = true;
                }
            }
            else
            {
                JumpAble = true;
                SwingAble = false;
                AirJump = false;
                SwingCount = 0;
                DrawPosition.Y -= Vcollision/3;
            }
            AirTime = 0;
            InitY = 0;
            VelY = 0;
        }

        //Gravity calculations
        public void Fall(GameTime gameTime)
        {
            if (!SwingAble)
            {
                VelY = InitY + AirTime;
                if (VelY > 10) { VelY = 10; }
                AirTime += gameTime.ElapsedGameTime.TotalMilliseconds / 75;
                DrawPosition.Y += (int)VelY;
                if (AirTime > 60)
                {
                    DrawPosition.Y = 60;
                    AirTime = 0;
                }
            }
            SwingAble = false;
        }

        public int Hit(int damage)
        {
            if (immunity >= 1)
            {
                Lives -= damage;
                Game1.hit.Play(volume, 0.0f, 0.0f);
                if (Lives < 1)
                {
                    Lives = 0;
                    Debug.WriteLine("game end");

                    if (Game1.state != GameState.Pause)
                        Game1.gameOver.Play(volume, 0.0f, 0.0f);

                    Game1.state = GameState.Pause;
                    Game1.NewScore(Game1.map.MaxScore);
                }
                immunity = 0;
            }
            return Lives;
        }

        public void Draw(SpriteBatch sb)
        {
            //sb.Draw(Game1.pixel, DrawPosition, Color.Red);
            sb.Draw(PlayerTexture, DrawPosition, Color.White);
            sb.Draw(Game1.pixel, CoolDownBack, Color.DarkViolet);
            sb.Draw(Game1.pixel, JumpCoolDown, Color.Plum);
        }
    }
}