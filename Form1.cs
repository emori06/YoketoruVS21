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
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(isDebug)
            {
                if(GetAsyncKeyState((int)Keys.O) < 0)
                {
                    nextState = State.Gameover;
                }else
                {
                    nextState = State.Clear;
                }
            }

            if(nextState != State.None)
            {
                initProc();
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
                    break;
            }

        }

        private void startbutton_Click(object sender, EventArgs e)
        {
            nextState = State.Game;
        }
    }
}
