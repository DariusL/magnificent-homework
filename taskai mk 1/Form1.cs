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
        private Point start = new Point(-1, -1);
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

            dataGridView1.Columns.Add("spalva", "Spalva");
            dataGridView1.Columns[0].Width = 60;
            dataGridView1.Columns.Add("plotas", "Suskaiciuotas Plotas");
            dataGridView1.Columns[1].Width = 80;
            dataGridView1.Columns.Add("taskai", "Tasku Kiekis");
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns.Add("superplotas", "Apskaiciuotas Plotas");
            dataGridView1.Columns[3].Width = 130;

            textBox1.KeyPress += textBox1_KeyPress;
        }

        void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                button1_Click(null, null);
        }

        void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing && prev.X != -1)
                drawLine(prev, e.Location);
            prev = e.Location;
        }

        void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
            if (checkBox1.Checked)
                drawLine(start, e.Location);
        }

        void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            start = e.Location;
        }

        private void drawLine(Point s, Point e)
        {
            counter.setWallLine(s, e, wallMap);
            graphics.DrawImage(wallMap, zero);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string number = textBox1.Text;
            int n;
            try
            {
                n = Convert.ToInt32(number, 10);
            }
            catch (FormatException ex)
            {
                MessageBox.Show(number + " nėra skaičius");
                return;
            }
            catch (OverflowException ex)
            {
                MessageBox.Show(number + " didokas");
                return;
            }
            if(n <= 0)
            {
                MessageBox.Show("Skaičius turėtų būti teigiamas");
                return;
            }
            if (n >= 0.8 * panel1.Size.Width * panel1.Size.Height)
            {
                MessageBox.Show(number + " didokas");
                return;
            }
            pointMap = new Bitmap(panel1.Size.Width, panel1.Size.Height);
            counter.generatePoints(n, pointMap);
            graphics.Clear(Color.Silver);
            graphics.DrawImage(pointMap, zero);
            graphics.DrawImage(wallMap, zero);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (counter.getRandomedPoints() <= 0)
                button1_Click(null, null);
            pointMap = new Bitmap(panel1.Size.Width, panel1.Size.Height);
            graphics.Clear(Color.Silver);
            fillGrid(dataGridView1, counter.search(pointMap));
            graphics.DrawImage(pointMap, zero);
            graphics.DrawImage(wallMap, zero);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            graphics.Clear(Color.Silver);
            wallMap = new Bitmap(panel1.Size.Width, panel1.Size.Height);
            counter.clear();
        }

        private void fillGrid(DataGridView grid, List<Area> data)
        {
            grid.Rows.Clear();
            foreach(Area area in data)
            {
                int n = grid.Rows.Add();
                grid.Rows[n].Cells[0].Value = area.color.ToString();
                grid.Rows[n].Cells[0].Style.SelectionForeColor = area.color;
                grid.Rows[n].Cells[0].Style.ForeColor = area.color;
                grid.Rows[n].Cells[0].Style.SelectionBackColor = area.color;
                grid.Rows[n].Cells[0].Style.BackColor = area.color;
                grid.Rows[n].Cells[1].Value = area.size;
                grid.Rows[n].Cells[2].Value = area.pointCount;
                grid.Rows[n].Cells[3].Value = area.getCalculatedSize();
            }
        }
    }
}
