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
        private Bitmap pointMap;
        private Bitmap wallMap;
        private Point zero = new Point(0, 0);

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

            wallMap = new Bitmap(panel1.Size.Width, panel1.Size.Height);

            counter = new PointCounter(panel1.Size.Width, panel1.Size.Height);
        }

        void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing && prev.X != -1)
            {
                counter.setWallLine(prev, e.Location, wallMap);
                graphics.DrawImage(wallMap, zero);
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

        private void button1_Click(object sender, EventArgs e)
        {
            pointMap = new Bitmap(panel1.Size.Width, panel1.Size.Height);
            counter.generatePoints(50, pointMap);
            graphics.Clear(Color.Silver);
            graphics.DrawImage(pointMap, zero);
            graphics.DrawImage(wallMap, zero);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pointMap = new Bitmap(panel1.Size.Width, panel1.Size.Height);
            graphics.Clear(Color.Silver);
            counter.search(pointMap, graphics);
            graphics.DrawImage(wallMap, zero);
        }
    }
}
