﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp21
{
    public partial class Form1 : Form
    {
        public delegate void DlHandler(object sender, EventArgs e);
        public delegate void AsyncMove();
        Ball[] redballs = new Ball[2];
        Ball[] blueballs = new Ball[2];

        public Form1()
        {
 
            /*redballs[0] = new Ball(new Point(0, 100), "Right", false, Brushes.Red);
            redballs[1] = new Ball(new Point(0, 300), "Right", false, Brushes.Red);
            blueballs[0] = new Ball(new Point(1085, 200), "Left", false, Brushes.Blue);
            blueballs[1] = new Ball(new Point(1085, 400), "Left", false, Brushes.Blue);*/
            DoubleBuffered = true;
            InitializeComponent();
            ThreadPool.QueueUserWorkItem(CreateObjects);

            /*evAsync += redballs[0].StartMoving;
            evAsync += redballs[1].StartMoving;
            
            Start();

            redballs[0].ChangeDirection += blueballs[0].StartMoving;
            redballs[1].ChangeDirection += blueballs[1].StartMoving;*/

        }
        private void CreateObjects(object o)
        {
            redballs[0] = new Ball(new Point(0, 100), "Right", false, Brushes.Red);
            redballs[1] = new Ball(new Point(0, 300), "Right", false, Brushes.Red);
            blueballs[0] = new Ball(new Point(1085, 200), "Left", false, Brushes.Blue);
            blueballs[1] = new Ball(new Point(1085, 400), "Left", false, Brushes.Blue);
            evAsync += redballs[0].StartMoving;
            evAsync += redballs[1].StartMoving;

            Start();

            redballs[0].ChangeDirection += blueballs[0].StartMoving;
            redballs[1].ChangeDirection += blueballs[1].StartMoving;
            int x = Thread.CurrentThread.ManagedThreadId;

            this.Invoke((Action)delegate
            {
                this.Text = String.Format(" Processing {0} on thread {1}", redballs, x);

            });

        }


        public delegate void DelAsyncEv();
        public event DelAsyncEv evAsync;
        public void Start()
        {

            Delegate[] delList = evAsync.GetInvocationList();
            Parallel.ForEach(delList, (Delegate del) =>
            {
                DelAsyncEv deleg = (DelAsyncEv)del;
                deleg.BeginInvoke(null, null);
            });
        }


    private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            Invalidate();
        }
        private void стартToolStripMenuItem_Click(object sender, EventArgs timer1_Tick)
        {
            timer1.Start();
            
        }
        
        private void стопToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
        public void Finish()
        {
            MessageBox.Show(
            "Финиш");
        }

        class Ball
        {
            public Point position;
            Brush color;
            Size size;
            bool stop;
            string direction;
            public delegate void Delegat();
            public event Delegat ChangeDirection;

            public Ball(Point beginposition, string Direction, bool Stop, Brush Color)
            {
                this.color = Color;
                this.size = new Size(100, 100);
                this.position = beginposition;
                this.direction = Direction;
                this.stop = Stop;

            }
            public void draw(Graphics context)
            {
                context.FillEllipse(this.color, new Rectangle(this.position, this.size));
            }


            public void Move(object obj)
            {
                this.stop = true;
                while (this.direction == "Right" && this.stop == true)
                {
                    Thread.Sleep(100);
                    this.position.X += 10;
                    if (this.position.X >= 1090)
                    {
                        StopMove();
                        if (ChangeDirection != null)
                            ChangeDirection();
                    }
                }

                while (this.direction == "Left" && this.stop == true)
                {
                    Thread.Sleep(100);
                    this.position.X -= 10;

                    if (this.position.X <= 30)
                    {

                        StopMove();
                    }
                }
            }

            public void StopMove()
            {
                this.stop = false;

            }
            public void StartMoving()
            {
                this.stop = true;
                Move(this);

               /* AsyncMove am = new AsyncMove(Move);
                am.BeginInvoke(new AsyncCallback(CallBackMove), am);*/
               
            }

            public void CallBackMove(IAsyncResult res)
            {
                MessageBox.Show("Эллипс достиг края!");
            }

        }

        private void информацияОРазработчикеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Выполнил: студент группы 4307 Антаев М.П.\nВариант: №3", "Информация о разработчике");
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            redballs[0].draw(e.Graphics);
            redballs[1].draw(e.Graphics);
            blueballs[0].draw(e.Graphics);
            blueballs[1].draw(e.Graphics);
        }

        private void зановоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

    }
}