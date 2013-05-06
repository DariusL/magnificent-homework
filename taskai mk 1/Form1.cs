using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskai_mk_1
{
    public partial class Form1 : Form
    {
        private bool isDrawing = false;
        private Graphics graphics;
        private Point prev = new Point(-1, -1);
        private Pen pen = new Pen(Brushes.Red, 1);

        private PointCounter counter;

        public Form1()
        {
            InitializeComponent();

            panel1.MouseDown += panel1_MouseDown;
            panel1.MouseUp += panel1_MouseUp;
            panel1.MouseMove += panel1_MouseMove;

            this.MouseDown += panel1_MouseDown;
            this.MouseUp += panel1_MouseUp;

            graphics = panel1.CreateGraphics();

            counter = new PointCounter(panel1.Size.Width, panel1.Size.Height);
        }

        void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing && prev.X != -1)
            {
                graphics.DrawLine(pen, prev, e.Location);
                counter.setWallLine(prev, e.Location);
            }
            prev = e.Location;
        }

        void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
        }

        void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
        }
    }
}
