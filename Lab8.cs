using System;
using System.Drawing;
using System.Windows.Forms;

namespace lab8_VideoGameDevelopment
{
    public partial class Lab8 : Form
    {

        PictureBox bow = new PictureBox();
        PictureBox target = new PictureBox();
        PictureBox[] arrows = new PictureBox[5];
        

        bool startFlag = false;
        bool fireFlag = false;
        int cooldown = 12;
        int shooted = 10;
        int score = 0;
        int playerSpeed = 20;
        int targetspeed = 20;
        int arrowSpeed = 20;
        int arrowCount = 0;
        int prev = 0;
        

        Timer timer1;
        Timer timer2;
        Timer timer3;


        public Lab8()
        {
            InitializeComponent();
        }

    
        private void deleteArrow(int i)
        {
            arrows[i].Dispose();
            for (int j = i; j < arrowCount - 1; j++)
            {
                arrows[j] = arrows[j + 1];
            }
            arrowCount--;
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
            bow.Image = new Bitmap(global::lab8_VideoGameDevelopment.Properties.Resources.bow);
            bow.BackColor = Color.Transparent;
            bow.Location = new Point(pictureBox1.Width / 2, pictureBox1.Height-150);
            bow.Size = new Size(bow.Image.Width, bow.Image.Height); 
            bow.BringToFront();
            pictureBox1.Controls.Add(bow);
            target.Image = new Bitmap(Properties.Resources.target);
            target.BackColor = Color.Transparent;
            target.Location = new Point(pictureBox1.Width / 2, 20);
            target.Size = new Size(bow.Image.Width, bow.Image.Height);
            target.BringToFront();
            pictureBox1.Controls.Add(target);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (startFlag)
            {
                if (e.KeyCode == Keys.Right)
                {
                    if (bow.Location.X < pictureBox1.Width - bow.Width)
                        bow.Left += playerSpeed;
                }
                if (e.KeyCode == Keys.Left)
                {
                    if (bow.Location.X > 0)
                        bow.Left -= playerSpeed;
                }
                if (e.KeyCode == Keys.Space)
                    fireFlag = true;
            }
            if (e.KeyCode == Keys.Enter)
                button1_Click(sender, e);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (startFlag)
            {
                startFlag = false;
                button1.Text = "Начать";
                timer1.Dispose();
                timer2.Dispose();
                timer3.Dispose();
                
            }
            else
            {
                startFlag = true;
                button1.Text = "Пауза";
                timer1 = new Timer();
                timer2 = new Timer();
                timer3 = new Timer();
   
                timer1.Interval = 100;
                timer1.Tick += new EventHandler(timer1_Tick);
                timer1.Enabled = true;
                timer2.Interval = 50;
                timer2.Tick += new EventHandler(timer2_Tick);
                timer2.Enabled = true;
                timer3.Interval = 700;
                timer3.Tick += new EventHandler(timer3_Tick);
                timer3.Enabled = true;
              
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            var rnd = new Random();
            prev = rnd.Next(2);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            
            if (prev == 1)
            {
                if (target.Location.X > 0)
                    target.Left -= targetspeed;
            }
            else
            {
                if (target.Location.X < pictureBox1.Width - target.Width)
                    target.Left += targetspeed;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (shooted > 0)
                shooted--;
            if (fireFlag)
            {
                if (shooted == 0)
                {
                    arrowCount++;
                    for (int i = arrowCount - 1; i > 0; i--)
                    {
                        arrows[i] = arrows[i - 1];
                    }
                    arrows[0] = new PictureBox();
                    arrows[0].Image = new Bitmap(global::lab8_VideoGameDevelopment.Properties.Resources.arrow);
                    arrows[0].Location = new Point(bow.Location.X + bow.Image.Width / 2 - (arrows[0].Image.Width / 2), bow.Location.Y - 100);
                    arrows[0].BackColor = Color.Transparent;
                    arrows[0].Size = new Size(arrows[0].Image.Width, arrows[0].Image.Height);
                    arrows[0].Name = "arrows" + arrowCount.ToString();
                    arrows[0].BringToFront();
                    pictureBox1.Controls.Add(arrows[0]);
                    shooted = cooldown;
                }
                fireFlag = false;
            }

           

            for (int i = 0; i < arrowCount; i++)
            {
                arrows[i].Location = new Point(arrows[i].Location.X, arrows[i].Location.Y - arrowSpeed);

                if (arrows[i].Location.Y < 0)
                    deleteArrow(i);
                var r1 = arrows[i].DisplayRectangle;

                

                var r2 = target.DisplayRectangle;
                r1.Location = arrows[i].Location;
                r2.Location = target.Location;
                if (r1.IntersectsWith(r2))
                {
                    deleteArrow(i);
                    score++;
                    hitting.Text = Convert.ToString(score);
                    if (score == 10)
                    {
                        button1_Click(sender, e);
                        string message = "Начать заного?";
                        string caption = "Ты победил!"; 
                        MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                        DialogResult result;

                        result = MessageBox.Show(message, caption, buttons);
                        if (result == System.Windows.Forms.DialogResult.No)
                        {
                            this.Close();
                        }
                        else
                        {
                            score = 0;
                            hitting.Text = Convert.ToString(score);
                            clearGame();
                            Form1_Load(sender, e);
                        }
                    }
                }
            }
        }

        private void clearGame()
        {
            timer1.Dispose();
            timer2.Dispose();
            timer3.Dispose();
           
            pictureBox1.Controls.Clear();
            startFlag = false;
            fireFlag = false;
            cooldown = 12;
            shooted = 10;
            score = 0;
            playerSpeed = 20;
            targetspeed = 20;
            arrowSpeed = 20;
            arrowCount = 0;


        }

        
    }
}
