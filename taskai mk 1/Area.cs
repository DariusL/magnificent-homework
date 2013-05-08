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

        public Area(Color color)
        {
            this.color = color;
            size = 0;
            pointCount = 0;
        }
    }
}
