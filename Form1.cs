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

        const int PlayerMax = 1;
        const int EnemyMax = 3;
        const int ItemMax = 3;
        const int charMax = PlayerMax + EnemyMax + ItemMax;

        Label[] chars = new Label[charMax];

        const int PlayerIndex = 0;
        const int EnemyIndex = PlayerIndex + PlayerMax;
        const int ItemIndex = EnemyIndex + EnemyMax;

        const string PlayerText = "(; ﾟдﾟ )";
        const string EnemyText = "(# ﾟДﾟ)";
        const string ItemText = "□";

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
                    break;
                case State.Game:
                    titleLabel.Visible = false;
                    startbutton.Visible = false;
                    copyrightLabel.Visible = false;
                    hiLabel.Visible = false;

                    for(int i = EnemyIndex; i < charMax; i++)
                    {
                        chars[i].Left = rand.Next(ClientSize.Width - chars[i].Width);
                        chars[i].Top = rand.Next(ClientSize.Height - chars[i].Height);
                    }

                    break;
                case State.Gameover:
                    gameoverLabel.Visible = true;
                    titleButton.Visible = true;
                    break;
                case State.Clear:
                    clearLabel.Visible = true;
                    titleButton.Visible = true;
                    break;
            }

        }

        void UpdateGame()
        {
            Point mp = PointToClient(MousePosition);
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
