using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    public class Ellipse : Shape
    {
        public Ellipse(Pen p) : base(p) {}

        public override void draw(Graphics g, int a, int b, int c, int d)
        {
            g.DrawEllipse(Pen, a, b, c, d);
        }
    }
}
