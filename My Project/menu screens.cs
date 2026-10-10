using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace My_Project
{
    public class MainMenu : Screen
    {
        Label title;
        Label gameName;
        Label highscore;
        Label Attempts;
        Button play;
        Button settings;
        Button armoury;
        Button EXIT;
        Box underline;
        int Scoreboard = Game1.RecordScore;
        int width, height;
        bool press;

        public MainMenu(int width, int height) : base(width, height)
        {
            this.width = width;
            this.height = height;
            play = new Button(width / 50, (int)(height / 6), width / 8, height / 11, "Play", Color.Cyan, Color.Red, Color.Transparent);
            settings = new Button(width / 50, (int)(height / 3.6), width / 8, height / 11, "Settings", Color.Cyan, Color.Red, Color.Transparent);
            armoury = new Button(width / 50, (int)(height / 2.58), width / 8, height / 11, "Armoury", Color.Cyan, Color.Red, Color.Transparent);
            EXIT = new Button((int)(width / 2.3), (int)(height / 1.5), width / 8, height / 11, "EXIT", Color.Red, Color.Orange, Color.Transparent);

            gameName = new Label((int)(width / 2.5), height / 20, "Light Dash", Color.Cyan);
            title = new Label((int)(width / 2.5), (int)(height / 7.5), "Main Menu", Color.Blue);
            highscore = new Label((int)(width / 1.5), (int)(height / 6), "High Score:" + Scoreboard, Color.LightGreen);
            Attempts = new Label((int)(width / 1.7), (int)(height / 3.6), "Attempts Remaining: " + Game1.Attempts, Color.Green);

            underline = new Box((int)(width / 2.5), height / 8, width / 6, height / 400, Color.Cyan);
        }

        public override void Update(int x, int y, bool lc, bool rc, bool timer, GameTime gameTime)
        {

            play.CheckHover(x, y);
            settings.CheckHover(x, y);
            armoury.CheckHover(x, y);
            EXIT.CheckHover(x, y);
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Game1.MyExit = true;
            }
            if (lc)
            {
                press = true;
            }
            else if (press)
            {
                press = false;
                if (play.CheckClick(x, y))
                {
                    Game1.Click.Play(volume, 0.0f, 0.0f);
                    Game1.map.Reset();
                    Game1.player.Reset();
                    Game1.current = new play(width, height);
                    Game1.state = GameState.Play;
                }
                else if (settings.CheckClick(x, y))
                {
                    Game1.Click.Play(volume, 0.0f, 0.0f);
                    Game1.current = new settings(width, height);
                }
                else if (armoury.CheckClick(x, y))
                {
                    Game1.Click.Play(volume, 0.0f, 0.0f);
                    Game1.current = new armoury(width, height, Game1.GunTexture);
                }
                else if (EXIT.CheckClick(x, y))
                {
                    Game1.MyExit = true;
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            title.Draw(sb);
            gameName.Draw(sb);
            play.Draw(sb);
            settings.Draw(sb);
            highscore.Draw(sb);
            armoury.Draw(sb);
            underline.Draw(sb);
            Attempts.Draw(sb);
            EXIT.Draw(sb);
        }
    }

    public class settings : Screen
    {
        Label title;
        Label gameName;
        Label Vol;
        Button MainMenu;
        Button EXIT;
        Button Reset;
        Button UVolume, DVolume;
        Button Controls;
        Box underline, VolBox;
        Rectangle Volume;
        bool press = false;
        int width, height;

        public settings(int width, int height) : base(width, height)
        {
            this.width = width;
            this.height = height;
            MainMenu = new Button(width / 50, (int)(height / 6), width / 6, height / 11, "Main Menu", Color.Cyan, Color.Red, Color.Transparent);
            EXIT = new Button((int)(width / 2.3), (int)(height / 1.5), width / 8, height / 11, "EXIT", Color.Red, Color.Orange, Color.Transparent);
            Reset = new Button((int)(width / 2.5), (int)(height / 2.5), width / 5, height / 11, "Reset Game", Color.Magenta, Color.Red, Color.Transparent);
            Controls = new Button(width / 50, height / 3, width / 6, height / 11, "Controls", Color.Cyan, Color.Red, Color.Transparent);
            UVolume = new Button((int)(width / 6), height / 2, width / 60, height / 15, "+", Color.Blue, Color.Cyan, Color.Transparent);
            DVolume = new Button((int)(width / 50), height / 2, width / 60, height / 15, "-", Color.Blue, Color.Red, Color.Transparent);

            gameName = new Label((int)(width / 2.5), height / 20, "Light Dash", Color.Cyan);
            title = new Label((int)(width / 2.5), (int)(height / 7.5), "Settings", Color.Red);
            Vol = new Label(width / 25, height / 2, "Volume", Color.Cyan);

            underline = new Box((int)(width / 2.5), height / 8, width / 6, height / 400, Color.Cyan);
            VolBox = new Box(width / 50, (int)(height / 1.75), width / 6, height / 40, Color.Purple);
        }

        public override void Update(int x, int y, bool lc, bool rc, bool timer, GameTime gameTime)
        {

            Volume = new Rectangle(width / 50, (int)(height / 1.75), (int)(volume * (width / 6)), height / 40);
            Controls.CheckHover(x, y);
            MainMenu.CheckHover(x, y);
            EXIT.CheckHover(x, y);
            Reset.CheckHover(x, y);
            UVolume.CheckHover(x, y);
            DVolume.CheckHover(x, y);
            if (lc)
            {
                press = true;
            }
            else if (press)
            {
                press = false;
                if (MainMenu.CheckClick(x, y))
                {
                    Game1.Click.Play(volume, 0.0f, 0.0f);
                    Game1.current = new MainMenu(width, height);
                }
                else if (Controls.CheckClick(x, y))
                {
                    Game1.Click.Play(volume, 0.0f, 0.0f);
                    Game1.current = new controls(width, height);
                }
                else if (EXIT.CheckClick(x, y))
                {
                    Game1.MyExit = true;
                }
                else if (Reset.CheckClick(x, y))
                {
                    Game1.Click.Play(volume, 0.0f, 0.0f);
                    Game1.FullReset();
                    Game1.SaveStats();
                }
                else if (UVolume.CheckClick(x, y))
                {
                    volume = Game1.GetVolume(0.1f);
                    Game1.Click.Play(volume, 0.0f, 0.0f);
                }
                else if (DVolume.CheckClick(x, y))
                {
                    volume = Game1.GetVolume(-0.1f);
                    Game1.Click.Play(volume, 0.0f, 0.0f);
                }
            }
        }
        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            title.Draw(sb);
            MainMenu.Draw(sb);
            EXIT.Draw(sb);
            underline.Draw(sb);
            gameName.Draw(sb);
            Reset.Draw(sb);
            UVolume.Draw(sb);
            DVolume.Draw(sb);
            Vol.Draw(sb);
            VolBox.Draw(sb);
            Controls.Draw(sb);
            sb.Draw(Game1.pixel, Volume, Color.Plum);
        }
    }

    public class controls : Screen
    {
        Button Back;
        Label Title;
        Label move;
        Label Jump;
        Label AirJump;
        Label Slide;
        bool press = false;
        int width, height;

        public controls(int width, int height) : base(width, height)
        {
            this.width = width;
            this.height = height;
            Back = new Button((int)(width / 2.4), (int)(height / 1.2), width / 8, height / 11, "Back", Color.Red, Color.Cyan, Color.Transparent);

            Title = new Label((int)(width / 2.5), (int)(height / 8), "Controls", Color.Red);
            move = new Label((int)(width / 3.3), (int)(height / 5), "Left / Right: A / D", Color.White);
            Jump = new Label((int)(width / 3.3), (int)(height / 2.8), "Jump/Wall Bounce: Space", Color.White);
            AirJump = new Label((int)(width / 3.3), (int)(height / 2), "Air Jump: L-shift", Color.White);
            Slide = new Label((int)(width / 3.3), (int)(height / 1.5), "Slide: L-ctrl", Color.White);
        }

        public override void Update(int x, int y, bool lc, bool rc, bool timer, GameTime gameTime)
        {
            Back.CheckHover(x, y);
            if (lc)
            {
                press = true;
            }
            else if (press)
            {
                press = false;
                if (Back.CheckClick(x, y))
                {
                    Game1.Click.Play(volume, 0.0f, 0.0f);
                    Game1.current = new settings(width, height);
                }
            }
        }
        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            Back.Draw(sb);
            Title.Draw(sb);
            move.Draw(sb);
            Jump.Draw(sb);
            AirJump.Draw(sb);
            Slide.Draw(sb);
        }
    }

    public class armoury : Screen
    {
        List <int> values = new List<int>(), statsReturn;
        Label title;
        Label gameName;
        Label UpgradePoints;
        Button MainMenu;
        Button FireRate;
        Button Damage;
        Box underline;
        Rectangle Gun;
        Texture2D GunTexture;
        bool press = false;
        int Upgrades, rate, damage;

        public armoury(int width, int height, Texture2D texture) : base(width, height)
        {
            values = Game1.GetStats();
            rate = values[0];
            damage = values[1];
            Upgrades = values[2];

            GunTexture = texture;

            MainMenu = new Button(width / 50, height / 6, width / 8, height / 11, "Main Menu", Color.Red, Color.Cyan, Color.Transparent);
            FireRate = new Button(width / 4, height / 2, width / 8, height / 11, "FireRate: " + (rate - 2), Color.Blue, Color.Cyan, Color.Transparent);
            Damage = new Button((int)(width / 1.6), height / 2, width / 8, height / 11, "Gun Damage: " + (damage - 1), Color.Blue, Color.Cyan, Color.Transparent);

            gameName = new Label((int)(width / 2.5), height / 20, "Light Dash", Color.Cyan);
            title = new Label((int)(width / 2.5), (int)(height / 7.5), "Armoury", Color.Red);
            UpgradePoints = new Label(width / 3, (int)(height / 1.5), "Upgrades Remaining: " + Upgrades, Color.Green);

            Gun = new Rectangle(width / 2 - width / 16, height / 2 - height / 16, width / 8, height / 8);

            underline = new Box((int)(width / 2.5), height / 8, width / 6, height / 400, Color.Cyan);
        }
        public override void Update(int x, int y, bool lc, bool rc, bool timer, GameTime gameTime)
        {
            Damage.CheckHover(x, y);
            FireRate.CheckHover(x, y);
            MainMenu.CheckHover(x, y);
            if (lc)
            {
                press = true;
            }
            else if (press)
            {
                press = false;
                if (MainMenu.CheckClick(x, y))
                {
                    Game1.Click.Play(volume, 0.0f, 0.0f);
                    Game1.current = new MainMenu(800, 480);
                }
                else if (Damage.CheckClick(x, y) && Upgrades > 0)
                {
                    Game1.Click.Play(volume, 0.0f, 0.0f);
                    statsReturn = Game1.Upgrade("damage");
                    Upgrades = statsReturn[0];
                    damage = (int)(statsReturn[1]);
                    Damage.text = "Gun Damage: " + (damage - 1);
                    UpgradePoints.text = "Upgrades Remaining: " + Upgrades;
                }
                else if (FireRate.CheckClick(x, y) && Upgrades > 0)
                {
                    Game1.Click.Play(volume, 0.0f, 0.0f);
                    statsReturn = Game1.Upgrade("rate");
                    Upgrades = statsReturn[0];
                    rate = (int)(statsReturn[1]);
                    FireRate.text = "FireRate: " + (rate - 2);
                    UpgradePoints.text = "Upgrades Remaining: " + Upgrades;
                }
            }
        }
        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            title.Draw(sb);
            MainMenu.Draw(sb);
            underline.Draw(sb);
            gameName.Draw(sb);
            FireRate.Draw(sb);
            Damage.Draw(sb);
            UpgradePoints.Draw(sb);
            sb.Draw(GunTexture, Gun, Color.White);
        }
    }

    public class fail : Screen
    {
        Button Restart;
        Label HighScore;
        Label GameOver;
        int width, height;

        public fail(int width, int height, int Score) : base(width, height)
        {
            this.width = width; this.height = height;
            GameOver = new Label((int)(width / 2.5), (int)(height / 7.5), "Game Over", Color.Red);
            HighScore = new Label((int)(width / 2.5), (int)(height / 5), "Final Score: " + Convert.ToString(Score), Color.Cyan);

            Restart = new Button((int)(width / 2.5), height / 2, width / 8, height / 11, "Restart", Color.Cyan, Color.Red, Color.Transparent);
        }

        public override void Update(int x, int y, bool lc, bool rc, bool timer, GameTime gameTime)
        {
            Restart.CheckHover(x, y);
            if (!timer)
            {
                if (lc && Restart.CheckClick(x, y))
                {
                    Game1.Click.Play(volume, 0.0f, 0.0f);
                    Game1.current = new MainMenu(width, height);
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            GameOver.Draw(sb);
            HighScore.Draw(sb);
            Restart.Draw(sb);
        }
    }

    public class play : Screen
    {
        Label Score;
        Label Lives;
        Label End;
        Label DisplayBox;
        Label Record;
        Button Exit;
        public static Queue<ShowUpgrade> DisplayUpgrade = new Queue<ShowUpgrade>();
        ShowUpgrade temp;
        bool press = false;
        int width, height, MaxDistance = 0;
        public static int displaytime = 1;

        public play(int width, int height) : base(width, height)
        {
            Exit = new Button(width / 2, height / 2, width / 8, height / 11, "Exit", Color.Magenta, Color.Red, Color.Transparent);

            Score = new Label(width / 20, height / 40, "Distance: " + Convert.ToString(MaxDistance), Color.Cyan);
            Lives = new Label((int)(width / 1.3), height / -40, "Lives Remaining: " + Convert.ToString(Game1.player.Lives), Color.Orange);
            DisplayBox = new Label((int)(width / 1.8), (int)(height / -3), "+1 Upgrade", Color.White);
            Record = new Label(width / 20, height / 12, "Record: " + Convert.ToString(Game1.RecordScore), Color.Red);
            End = new Label(width / 2, height / 3, "Game Over!", Color.Red);

            this.width = width;
            this.height = height;
        }

        public override void Update(int x, int y, bool lc, bool rc, bool timer, GameTime gameTime)
        {
            foreach (ShowUpgrade Upgrade in DisplayUpgrade)
            {
                Upgrade.Update(gameTime);
                if (Upgrade.remove)
                {
                    temp = Upgrade;
                    break;
                }
            }
            if (temp != null)
            {
                DisplayUpgrade.Dequeue();
                temp = null;
            }



            Score = new Label(width / 20, height / 40, "Distance: " + Convert.ToString(MaxDistance), Color.Cyan);
            Lives = new Label((int)(width / 1.5), height / 40, "Lives Remaining: " + Convert.ToString(Game1.player.Lives), Color.Red);

            Exit.CheckHover(x, y);
            if (!timer)
            {
                //toggle pause
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    if (Game1.state == GameState.Play)
                    {
                        Game1.state = GameState.Pause;
                    }
                    else if (Game1.state == GameState.Pause && Game1.player.Lives > 0)
                    {
                        Game1.state = GameState.Play;
                    }
                    else
                    {
                        Game1.state = GameState.Menu;
                        Game1.current = new MainMenu(width, height);
                    }
                    Game1.timer.Enabled = true;
                }
            }

            if (lc && Game1.state == GameState.Pause)
            {
                press = true;
            }
            else if (press)
            {
                press = false;
                if (Exit.CheckClick(x, y) && Game1.state == GameState.Pause)
                {
                    Game1.Click.Play(volume, 0.0f, 0.0f);
                    Game1.player.Hit(3);
                    int S = Game1.map.ReturnScore();
                    Game1.state = GameState.Menu;
                    Game1.current = new MainMenu(width, height);
                }
            }

            //Score display
            MaxDistance = Game1.map.MaxScore;
        }

        public override void Draw(SpriteBatch sb)
        {
            foreach (ShowUpgrade Upgrade in DisplayUpgrade)
            {
                Upgrade.Draw(sb);
            }
            Score.Draw(sb);
            Lives.Draw(sb);
            DisplayBox.Draw(sb);
            Record.Draw(sb);
            if (Game1.state == GameState.Pause)
            {
                Exit.Draw(sb);
                if (Game1.player.Lives < 1)
                {
                    End.Draw(sb);
                }
            }
        }
    }
}
