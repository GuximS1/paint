using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    public class Eraser
    {
        public Pen eraser { get; set; }
        public Eraser(Pen eraser)
        {
            this.eraser = eraser;
        }
        public void draw(Graphics g, Point x, Point y)
        {
            g.DrawLine(eraser, x, y);
        }
    }
}
