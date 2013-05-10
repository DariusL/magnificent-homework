#define DEBUG

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

namespace taskai_mk_1
{
    class PointCounter
    {
        private int[,] points;
        private int wallCount = 0;
        public int randomedPoints = 0;
        private int sizeX = 0, sizeY = 0;

        public PointCounter(int x, int y)
        {
            sizeX = x;
            sizeY = y;
            points = new int[x, y];
        }

        public void clear()
        {
            points = new int[sizeX, sizeY];
        }

        public void generatePoints(int count, Bitmap g)
        {
            randomedPoints = count;
            Random random = new Random();
            clear(PointValues.WALL);
            int x, y;
            for (int i = 0; i < count;)
            {
                x = random.Next(sizeX);
                y = random.Next(sizeY);
                if (setPoint(x, y))
                {
                    i++;
                    g.SetPixel(x, y, Color.Black);
                }
            }
        }

        private void clear(int flagsToLeave)
        {
            for (int i = 0; i < sizeX; i++)
                for (int j = 0; j < sizeY; j++)
                    points[i, j] &= flagsToLeave;
        }

        public int getRandomedPoints()
        {
            return randomedPoints;
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
                map.SetPixel(x, y, Color.Black);
                return true;
            }
        }

        public bool setPoint(int x, int y)
        {
            if ((points[x, y] & PointValues.WALL_OR_RANDOMED) != 0)
                return false;
            else
            {
                points[x, y] |= PointValues.RANDOMED;
                return true;
            }
        }

        public bool inBounds(int x, int y)
        {
            if (x >= 0 && x < points.GetLength(0) && y >= 0 && y < points.GetLength(1))
                return true;
            else
            {
                System.Diagnostics.Debug.WriteLine("{0} x {1}", x, y);
                return false;
            }
        }

        public List<Area> search(Bitmap bitmap)
        {
            Point first = new Point(0, 0);
            Point last = new Point(points.GetLength(0) - 1, points.GetLength(1) - 1);
            Point point, temp;
            Point zero = new Point(0, 0);
            List<Area> areas = new List<Area>();
            Queue<Point> queue = new Queue<Point>();
            clear(PointValues.WALL_OR_RANDOMED);
            int color = 0;
            while (true)
            {
                while (first != last && (points[first.X, first.Y] & PointValues.WALL_OR_IN_QUEUE) != 0)
                    if (first.X == points.GetLength(0) - 1)
                    {
                        first.X = 0;
                        first.Y++;
                    }
                    else
                        first.X++;
                if (first == last)
                    break;

                queue.Enqueue(first);
                points[first.X, first.Y] |= PointValues.IN_QUEUE;
                Area area = new Area(Area.colors[color]);

                while (queue.Count != 0)
                {
                    point = queue.Dequeue();
                    temp = point;

                    temp.X++;
                    maybeEnqueue(temp, queue);
                    temp.X -= 2;
                    maybeEnqueue(temp, queue);
                    temp.X++;
                    temp.Y++;
                    maybeEnqueue(temp, queue);
                    temp.Y -= 2;
                    maybeEnqueue(temp, queue);

                    area.size++;
                    if ((points[point.X, point.Y] & PointValues.RANDOMED) != 0)
                    {
                        area.pointCount++;
                        bitmap.SetPixel(point.X, point.Y, Color.Black);
                    }
                    else
                        bitmap.SetPixel(point.X, point.Y, area.color);
                }
                area.calculateSize(randomedPoints, sizeX * sizeY);
                color++;
                if (color >= Area.colors.Length)
                    color = 0;
                areas.Add(area);
            }
            return areas;
        }

        private void maybeEnqueue(Point point, Queue<Point> queue)
        {
            if (shouldSearch(point))
            {
                queue.Enqueue(point);
                points[point.X, point.Y] |= PointValues.IN_QUEUE;
            }
        }

        private bool shouldSearch(Point point)
        {
            if (point.X < 0 || point.Y < 0 || point.X >= points.GetLength(0) || point.Y >= points.GetLength(1))
                return false;
            return (points[point.X, point.Y] & PointValues.WALL_OR_IN_QUEUE) == 0;
        }

        class PointValues
        {
            public const int WALL = 1 << 0;
            public const int RANDOMED = 1 << 1;
            public const int IN_QUEUE = 1 << 2;
            public const int WALL_OR_RANDOMED = WALL | RANDOMED;
            public const int WALL_OR_IN_QUEUE = WALL | IN_QUEUE;
        }
    }
}