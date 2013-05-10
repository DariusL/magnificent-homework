using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskai_mk_1
{
    class Area
    {
        public Color color;
        public int size;
        public int pointCount;
        private double calculatedSize;

        public Area(Color color)
        {
            this.color = color;
            size = 0;
            pointCount = 0;
        }

        public double getCalculatedSize()
        {
            return calculatedSize;
        }

        public void calculateSize(int allPoints, int allSize)
        {
            calculatedSize = (double)pointCount * allSize / allPoints;
        }

        public static readonly Color[] colors = new Color[] {SystemColors.Highlight, Color.Red,
            Color.Blue, Color.Green, Color.Gold, Color.Purple, Color.Aquamarine,
            Color.Brown, Color.Cyan, Color.BlueViolet, Color.Crimson, 
            Color.DarkGray, Color.LightGreen, Color.LightYellow};
    }
}
