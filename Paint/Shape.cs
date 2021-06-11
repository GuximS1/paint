using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    public abstract class Shape
    {
        public Pen Pen { get; set; }

        public Shape(Pen p)
        {
            Pen = p;
        }

        public abstract void draw(Graphics g, int a, int b, int c, int d);
    }
}
