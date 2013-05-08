#define DEBUG

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskai_mk_1
{
    class PointCounter
    {
        private int[,] points;
        private int wallCount = 0;
        private int pointsToRandom = 0;
        private int walls = 0;
        private Point[] generatedPoints;

        public PointCounter(int x, int y)
        {
            points = new int[x, y];
            //setWallLine(newD Point(0, 31), new Point(56, 3));
        }

        public void generatePoints(int count, Bitmap g)
        {
            pointsToRandom = count;
            Random random = new Random();
            int sx = points.GetLength(0);
            int sy = points.GetLength(1);
            for (int i = 0; i < sx; i++)
                for (int j = 0; j < sy; j++)
                    points[i, j] &= PointValues.WALL;
            int x, y;
            for (int i = 0; i < count;)
            {
                x = random.Next(sx);
                y = random.Next(sy);
                if (setPoint(x, y))
                {
                    i++;
                    g.SetPixel(x, y, Color.Black);
                }
            }
        }

        public void setWallLine(Point start, Point end, Bitmap map)
        {
            Point temp;
            if (start.X == end.X)
            {
                if (start.Y > end.Y)
                {
                    temp = start;
                    start = end;
                    end = temp;
                }
                temp = new Point();
                temp.X = start.X;
                temp.Y = start.Y;
                if (inBounds(temp.X, temp.Y))
                    setWall(temp.X, temp.Y, map);
                else
                    return;
                while (temp != end)
                {
                    temp.Y++;
                    if (inBounds(temp.X, temp.Y))
                        setWall(temp.X, temp.Y, map);
                    else
                        return;
                }
            }
            else
            {
                if (start.X > end.X)
                {
                    temp = start;
                    start = end;
                    end = temp;
                }
                double k, b;
                k = ((float)(end.Y - start.Y)) / (end.X - start.X);
                b = start.Y - k * start.X;
                double inter;
                temp = new Point();
                temp.X = start.X;
                temp.Y = start.Y;
                if (inBounds(temp.X, temp.Y))
                    setWall(temp.X, temp.Y, map);
                else
                    return;
                inter = (temp.X + 0.5) * k + b;
                while (temp != end)
                {
                    if (temp.Y + 0.5 < inter)
                        temp.Y++;
                    else if (temp.Y - 0.5 < inter)
                    {
                        temp.X++;
                        inter = (temp.X + 0.5) * k + b;
                    }
                    else
                        temp.Y--;
                    if (inBounds(temp.X, temp.Y))
                        setWall(temp.X, temp.Y, map);
                    else
                        return;
                }
            }
        }

        public bool setWall(int x, int y, Bitmap map)
        {
            if ((points[x, y] & PointValues.WALL) != 0)
                return false;
            else
            {
                wallCount++;
                points[x, y] |= PointValues.WALL;
                map.SetPixel(x, y, Color.Red);
                return true;
            }
        }

        bool setPoint(int x, int y)
        {
            if ((points[x, y] & PointValues.WALL_OR_POINT) != 0)
                return false;
            else
            {
                points[x, y] |= PointValues.POINT;
                return true;
            }
        }

        bool inBounds(int x, int y)
        {
            if (x >= 0 && x < points.GetLength(0) && y >= 0 && y < points.GetLength(1))
                return true;
            else
            {
                System.Diagnostics.Debug.WriteLine("{0} x {1}", x, y);
                return false;
            }
        }

        void search()
        {
            Point first = new Point(1, 1);
            Point last = new Point(points.GetLength(0) - 1, points.GetLength(1) - 1);
            List<Area> areas = new List<Area>();
            Queue<Point> queue = new Queue<Point>();
            while (true)
            {
                Area area = new Area();
                queue.Enqueue(first);
                while (queue.Count != 0)
                {

                }
            }
        }

        class PointValues
        {
            public const int WALL = 1 << 0;
            public const int POINT = 1 << 1;
            public const int VISITED = 1 << 2;
            public const int WALL_OR_POINT = WALL | POINT;
        }
    }
}
