using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    public class Pencil
    {
        public Pen Pen { get; set; }
        public Pencil(Pen pen)
        {
            Pen = pen;
        }
        public void draw(Graphics g, Point x, Point y)
        {
            g.DrawLine(Pen, x, y);
        }
    }
}
