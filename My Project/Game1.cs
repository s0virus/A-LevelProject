using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using SharpDX.DirectWrite;
using SharpDX.MediaFoundation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Cache;
using System.Timers;
using System.IO;
using System.Diagnostics.Eventing.Reader;
using System.ComponentModel.Design;
using System.CodeDom;
using System.Windows.Markup;
using System.ComponentModel.DataAnnotations;

namespace My_Project
{

    public enum GameState //used to decide which sections of code are run
    { 
        Menu, 
        Play,
        Pause,
    }

    public class Game1 : Game
    {
        public static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static SoundEffect gun, hit, kill, gameOver, jump, Click;
        public static Song BackSound, laser;
        public static Texture2D pixel;
        public static SpriteFont font;
        public static Texture2D PlayerTexture, GunTexture, EnemyTexture;
        public static Screen current;
        public static Player player;
        public static Map map;
        public static bool MyExit = false;
        public static System.Timers.Timer timer;
        public static GameState state = GameState.Menu;
        public static int RecordScore, Attempts, Damage, UpgradePoints, FireRate;
        private static int temp, ResX, ResY;
        public static float MasterVolume = 0.2f; 

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            player = new Player();
            map = new Map(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            base.Initialize();
        }

        protected override void LoadContent()
        {

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            pixel = new Texture2D(_graphics.GraphicsDevice, 1, 1);

            // Create a 1D array of color data to fill the pixel texture with.  
            Color[] colorData = {
                        Color.White,
                    };

            // Set the texture data with our color information.  
            pixel.SetData<Color>(colorData);

            //retrieve high score, attempts and gun upgrades
            StreamReader sr = new StreamReader("GameSave.txt"); //file to read stats from
            Attempts = Convert.ToInt32(sr.ReadLine()); //reads Attempts
            RecordScore = Convert.ToInt32(sr.ReadLine()); //reads Record Score
            Damage = Convert.ToInt32(sr.ReadLine()); //reads Damage
            FireRate = Convert.ToInt32(sr.ReadLine()); //reads Fire rate
            UpgradePoints = Convert.ToInt32(sr.ReadLine()); //reads Upgrade Points
            MasterVolume = (float)(Convert.ToDouble(sr.ReadLine())); //reads Master Volume
            sr.Close(); //stops reading the file

            //Set the font and player
            font = Content.Load<SpriteFont>("File"); //applies the Font from a file
            Vector2 playerPosition = new Vector2(_graphics.PreferredBackBufferWidth/3, _graphics.PreferredBackBufferHeight/2); // Sets player position to center of screen
            player.Initialize(Content.Load<Texture2D>("PlayerTexture"), playerPosition, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight); //initialises the player

            GunTexture = Content.Load<Texture2D>("GunTexture"); //applies GunTexture from a file
            EnemyTexture = Content.Load<Texture2D>("ufoTexture"); //applies ufoTexture from a file

            gun = Content.Load<SoundEffect>("gunSound"); //applies a soundeffect for shooting
            hit = Content.Load<SoundEffect>("hitSound"); //applies a sound effect for player taking damage
            kill = Content.Load<SoundEffect>("killSound"); //applies a sound effect for killing an enemy
            gameOver = Content.Load<SoundEffect>("gameOverSound"); //applies a sound effect for Player death
            jump = Content.Load<SoundEffect>("jumpSound"); //applies a sound effect for different jumps (Pitch changed when played)
            Click = Content.Load<SoundEffect>("ButtonClicked"); //applies a sound effect for when a button is clicked
            BackSound = Content.Load<Song>("BackSoundSong"); //applies a sound to Loop in the Background
            MediaPlayer.Play(BackSound); //Plays the Song
            MediaPlayer.IsRepeating = true; //repeats the song
            MediaPlayer.Volume = MasterVolume; //sets the song Volume

            ResX = GraphicsDevice.Viewport.TitleSafeArea.Width; ResY = GraphicsDevice.Viewport.TitleSafeArea.Height; //sets ResX/Y as screen dimentions

            //create the timer for key presses
            timer = new System.Timers.Timer();
            timer.Interval = 250; //timer interval
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            current = new MainMenu(ResX, ResY);
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            timer.Enabled = false;
        }

        protected override void Update(GameTime gameTime)
        {
            if (MyExit || Keyboard.GetState().IsKeyDown(Keys.Back)) //when the game is closing through esc or Exit button
            {
                SaveStats(); //all stats are saved
                Exit(); //closes the game
            }

            MouseState mouse = Mouse.GetState(); //gets current mouse state

            int x = mouse.X; //sets x to mouse X coordinate
            int y = mouse.Y; //sets y to mouse Y coordinate

            bool LC = mouse.LeftButton == ButtonState.Pressed; //sets LC as bool for Left mouse click
            bool RC = mouse.RightButton == ButtonState.Pressed; //sets RC as bool for Right mouse click

            base.Update(gameTime);

            if(state == GameState.Play) //for when the game is in play state
            {
                int Hcollision = map.HorizontalCollision(player.DrawPosition); //returns Horizontal overlap between player and map segments
                int Vcollision = map.VerticalCollision(player.DrawPosition); //retunrs Vertical overlap between player and map segments
                if (Hcollision != 0) //when there is a horisontal overlap
                {
                    map.Undo(Hcollision, gameTime); //passses the overlap into map subprogram
                    player.VelX = 0; //stops the player walking into a wall
                    player.PreVel = 0; //stops the player walking into a wall
                    player.XAccel = 0; //stops the player walking into a wall
                }
                else
                {
                    player.Move(gameTime); //passes time into player subprogram when no horizontal overlap
                }

                if (Vcollision != 0 || (Hcollision != 0 && player.MoveKey == true)) //when there is a vertical overlap
                {
                    player.Freeze(Vcollision); //passes vertical overlap into player subprogram
                }
                else
                {
                    player.Fall(gameTime); //passes time into player subprogram when no vertical overlap
                }
                player.Update(gameTime); //updates Player
                map.Update((int)player.VelX, Hcollision, player.DrawPosition, x, y, LC, RC, gameTime); //Updates Map
            }
            current.Update(x, y, LC, RC, timer.Enabled, gameTime); //Updates current Menu Screen regardless of game state

            base.Update(gameTime);
        }

        //checks if a new High Score is achieved after a game
        public static void NewScore(int Score)
        {
            if (Score >= RecordScore)
            {
                RecordScore = Score; //replaces RecordScore with current score if a new High Score was achieved
            }
            else
                Attempts--; //removes an attempt if no new High Score
            Game1.SaveStats(); //saves stats at the end of each game
        }

        //returns current armoury stats as a list
        public static List<int> GetStats()
        {
            List <int> values = new List<int>();
            values.Add(FireRate);
            values.Add(Damage);
            values.Add(UpgradePoints);
            return values;
        }

        //changes the stats of the gun. uses Vector2 to return 2 values without creating a list
        public static List<int> Upgrade(string stat)
        {
            List<int> values = new List<int>();
            switch (stat)
            {
                case "rate": //if firerate was upgrades
                    FireRate++;
                    temp = FireRate;
                    break;
                case "damage": //if damage was upgraded
                    Damage++;
                    temp = Damage;
                    break;
            }
            UpgradePoints--; //subtracts one from UpgradePoints
            SaveStats(); //saves new stats
            values.Add(UpgradePoints);
            values.Add(temp);
            return values; //returns all variables necessary to update Armory
        }

        //Chance for enemies to reward upgrade points on death
        public static void UpgradeChance()
        {
            Random random = new Random();

            if (random.Next(1, 8) == 1 && map.MaxScore > RecordScore) //1 in 8 chance aslong as a new High Score is achieved
            {
                UpgradePoints++;
                play.DisplayUpgrade.Enqueue(new ShowUpgrade(ResX, ResY)); //indicates another upgrade without cencelling another upgrade indication
                Debug.WriteLine("+Upgrade");
            }
        }

        //returns MasterVolume after changing if needed
        public static float GetVolume(float change)
        {
            MasterVolume += change;
            if (MasterVolume > 1)
                MasterVolume = 1;
            else if (MasterVolume < 0)
                MasterVolume = 0;
            MediaPlayer.Volume = MasterVolume;

            return MasterVolume;
        }

        //writes currents stats to .txt file
        public static void SaveStats()
        {
            if (Attempts < 1)
            {
                current = new fail(ResX, ResY, RecordScore);
                FullReset();
            }
            StreamWriter sw;
            sw = new StreamWriter("GameSave.txt");
            sw.WriteLine(Attempts);
            sw.WriteLine(RecordScore);
            sw.WriteLine(Damage);
            sw.WriteLine(FireRate);
            sw.WriteLine(UpgradePoints);
            sw.WriteLine(MasterVolume);
            sw.Close();
            Debug.WriteLine("Save");
        }

        //Resets game stats to start
        public static void FullReset()
        {
            Attempts = 3;
            RecordScore = 0;
            Damage = 1;
            FireRate = 2; // number represents double the bullets per second
            UpgradePoints = 0;
            Debug.WriteLine("Reset");
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            if (state != GameState.Menu)
            {
                map.Draw(_spriteBatch);
                player.Draw(_spriteBatch);
            }
            current.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}