using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace YoketoruVS21
{
    public partial class Form1 : Form
    {
        const bool isDebug = true;

        const int speedMax = 20;

        const int PlayerMax = 1;
        const int EnemyMax = 3;
        const int ItemMax = 3;
        const int charMax = PlayerMax + EnemyMax + ItemMax;
        const int startTime = 100;
        int ItemCount = 0;
        int time = 0;
        int hiscore = 0;

        Label[] chars = new Label[charMax];

        int[] vx = new int[charMax];
        int[] vy = new int[charMax];

        const int PlayerIndex = 0;
        const int EnemyIndex = PlayerIndex + PlayerMax;
        const int ItemIndex = EnemyIndex + EnemyMax;

        const string PlayerText = "(; ﾟдﾟ )";
        const string EnemyText = "(# ﾟДﾟ)";
        const string ItemText = "■";

        static Random rand = new Random();

        enum State
        {
            None = -1,
            Title,
            Game,
            Gameover,
            Clear
        }

        State currentState = State.None;
        State nextState = State.Title;

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        public Form1()
        {
            InitializeComponent();

            for (int i = 0; i < charMax; i++)
            {
                chars[i] = new Label();
                chars[i].AutoSize = true;
                if (i == PlayerIndex)
                {
                    chars[i].Text = PlayerText;
                }
                else if (i < ItemIndex)
                {
                    chars[i].Text = EnemyText;
                }
                else
                {
                    chars[i].Text = ItemText;
                }
                chars[i].Font = tempLabel.Font;
                Controls.Add(chars[i]);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
             //デバッグ
            if(isDebug)
            {
                if(GetAsyncKeyState((int)Keys.O) < 0)
                {
                    nextState = State.Gameover;
                }else
                {
                    if (GetAsyncKeyState((int)Keys.C) < 0)
                    {
                        nextState = State.Clear;
                    }
                }
            }

            if(nextState != State.None)
            {
                initProc();
            }

            if (currentState == State.Game)
            {
                UpdateGame();
            }

        }

        void initProc()
        {
            currentState = nextState;
            nextState = State.None;

            switch (currentState)
            {
                case State.Title:
                    titleLabel.Visible = true;
                    startbutton.Visible = true;
                    copyrightLabel.Visible = true;
                    hiLabel.Visible = true;
                    gameoverLabel.Visible = false;
                    clearLabel.Visible = false;
                    titleButton.Visible = false;

                    for (int i = PlayerIndex; i < charMax; i++)
                    {
                        chars[i].Visible = false;
                    }

                    break;
                case State.Game:
                    titleLabel.Visible = false;
                    startbutton.Visible = false;
                    copyrightLabel.Visible = false;
                    hiLabel.Visible = false;

                    ItemCount = ItemMax;
                    leftLabel.Text = "■:" + ItemCount;
                    time = startTime + 1;

                    for (int i = PlayerIndex; i < charMax; i++)
                    {
                        chars[i].Left = rand.Next(ClientSize.Width - chars[i].Width);
                        chars[i].Top = rand.Next(ClientSize.Height - chars[i].Height);
                        vx[i] = rand.Next(-speedMax, speedMax + 1);
                        vy[i] = rand.Next(-speedMax, speedMax + 1);
                        chars[i].Visible = true;
                    }

                    break;
                case State.Gameover:
                    gameoverLabel.Visible = true;
                    titleButton.Visible = true;
                    break;
                case State.Clear:
                    clearLabel.Visible = true;
                    titleButton.Visible = true;
                    hiLabel.Visible = true;
                    if(time > hiscore)
                    {
                        hiscore = time;
                        hiLabel.Text = "HIGH_SCORE：" + hiscore;
                    }
                    break;
            }

        }

        void UpdateGame()
        {
            Point mp = PointToClient(MousePosition);

            chars[PlayerIndex].Left = mp.X - chars[PlayerIndex].Width / 2;
            chars[PlayerIndex].Top = mp.Y - chars[PlayerIndex].Height / 2;

            time--;
            timeLabel.Text = "time:" + time;
            if (time <= 0)
            {
                nextState = State.Gameover;
            }

            for (int i = EnemyIndex; i < charMax; i++)
            {
                if (!chars[i].Visible) continue;

                chars[i].Left += vx[i];
                chars[i].Top += vy[i];

                if(chars[i].Left < 0)
                {
                    vx[i] = (Math.Abs(vx[i]));
                }
                if(chars[i].Top < 0)
                {
                    vy[i] = (Math.Abs(vy[i]));
                }
                if (chars[i].Right > ClientSize.Width)
                {
                    vx[i] = (-Math.Abs(vx[i]));
                }
                if (chars[i].Bottom > ClientSize.Height)
                {
                    vy[i] = (-Math.Abs(vy[i]));
                }

                if(     (mp.X >= chars[i].Left)
                   &&   (mp.X < chars[i].Right)
                   &&   (mp.Y >= chars[i].Top)
                   &&   (mp.Y < chars[i].Bottom))
                {
                    //MessageBox.Show("重なった");
                    if(i < ItemIndex)
                    {
                        nextState = State.Gameover;
                    }else
                    {
                        chars[i].Visible = false;
                        
                        ItemCount--;
                        if(ItemCount <= 0)
                        {
                            nextState = State.Clear;
                        }
                        leftLabel.Text = "■:" + ItemCount;

                        /*
                        vx[i] = 0;
                        vy[i] = 0;
                        chars[i].Left = 10000;
                        */
                    }
                }
            }
        }

        private void startbutton_Click(object sender, EventArgs e)
        {
            nextState = State.Game;
        }

        private void titleButton_Click(object sender, EventArgs e)
        {
            nextState = State.Title;
        }
    }
}
