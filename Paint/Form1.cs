using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{
    public partial class Form1 : Form
    {
        Bitmap bm;
        Graphics g;
        enum ITEM
        {
            Default, Color, Paint, Pencil, Eraser, Ellipse, Rectangle, Line
        }
        ITEM currItem;
        Color New_Color;
        int x, y, sx, sy, cx, cy;
        Point px, py;
        bool paint = false;
        Pen p = new Pen(Color.Black, 1);
        Pen Eraser = new Pen(Color.White, 10);

        public Form1()
        {
            InitializeComponent();
            bm = new Bitmap(drawPanel.Width, drawPanel.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.White);
            drawPanel.BackgroundImage = bm;
            DoubleBuffered = true;
        }

        private void pbPencil_Click(object sender, EventArgs e)
        {
            currItem = ITEM.Pencil;
        }

        private void pbErase_Click(object sender, EventArgs e)
        {
            currItem = ITEM.Eraser;
        }

        

        private void pbRectangle_Click(object sender, EventArgs e)
        {
            currItem = ITEM.Rectangle;
        }

        private void pbEllipse_Click(object sender, EventArgs e)
        {
            currItem = ITEM.Ellipse;
        }

        private void pbLine_Click(object sender, EventArgs e)
        {
            currItem = ITEM.Line;
        }

        private void pbPaint_Click(object sender, EventArgs e)
        {
            currItem = ITEM.Paint;
        }
        
        private void clr_Click(object sender, EventArgs e)
        {
            PictureBox clr = (PictureBox)sender;
            New_Color = clr.BackColor;
            pbColor.BackColor = New_Color;
            p.Color = New_Color;
        }

        

        private void btnEditColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.FullOpen = true;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                New_Color = cd.Color;
                pbColor.BackColor = New_Color;
                //drawPanel.BackColor = cd.Color;
                p.Color = cd.Color;
            }
        }


        private void drawPanel_MouseDown(object sender, MouseEventArgs e)
        {
            paint = true;
            py = e.Location;

            cx = e.X;
            cy = e.Y;
        }

        private void drawPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (paint)
            {
                if (currItem == ITEM.Pencil)
                {
                    px = e.Location;
                    Pencil pencil = new Pencil(p);
                    pencil.draw(g, px, py);
                    py = px;
                }
                if (currItem == ITEM.Eraser)
                {
                    px = e.Location;
                    Eraser eraser = new Eraser(Eraser);
                    eraser.draw(g, px, py);
                    py = px;
                }
            }
            drawPanel.Refresh();

            x = e.X;
            y = e.Y;
            sx = e.X - cx;
            sy = e.Y - cy;
        }

        private void pbClear_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            drawPanel.Image = bm;
            currItem = ITEM.Default;
        }

        private void drawPanel_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;

            sx = x - cx;
            sy = y - cy;
            Pen pen = new Pen(Color.Black, 1);
            if (currItem == ITEM.Ellipse)
            {
                Shape ellipse = new Ellipse(pen);
                ellipse.draw(g, cx, cy, sx, sy);
            }
            if (currItem == ITEM.Rectangle)
            {
                Shape box = new Box(pen);
                box.draw(g, cx, cy, sx, sy);
            }
            if (currItem == ITEM.Line)
            {
                Shape line = new Line(pen);
                line.draw(g, cx, cy, sx, sy);
            }
        }

        private void drawPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (paint)
            {
                Pen pen = new Pen(Color.Black, 1);
                if (currItem == ITEM.Ellipse)
                {
                    Shape ellipse = new Ellipse(pen);
                    ellipse.draw(g, cx, cy, sx, sy);
                }
                if (currItem == ITEM.Rectangle)
                {
                    Shape box = new Box(pen);
                    box.draw(g, cx, cy, sx, sy);
                }
                if (currItem == ITEM.Line)
                {
                    Shape line = new Line(pen);
                    line.draw(g, cx, cy, sx, sy);
                }
            }
        }
        private void drawPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (currItem == ITEM.Paint)
            {
                Point point = set_Point(drawPanel, e.Location);
                Fill(bm, point.X, point.Y, New_Color);
            }
        }
        static Point set_Point(PictureBox pb, Point pt)
        {
            float px = 1f * pb.Width / pb.Width;
            float py = 1f * pb.Height / pb.Height;
            return new Point((int)(pt.X * px), (int)(pt.Y * py));
        }
        private void Validate(Bitmap bm, Stack<Point> sp, int x, int y, Color Old_Color, Color New_Color)
        {
            Color cx = bm.GetPixel(x, y);
            if (cx == Old_Color)
            {
                sp.Push(new Point(x, y));
                bm.SetPixel(x, y, New_Color);
            }
        }
        public void Fill(Bitmap bm, int x, int y, Color New_Clr)
        {
            Color Old_Color = bm.GetPixel(x, y);
            Stack<Point> pixel = new Stack<Point>();
            pixel.Push(new Point(x, y));
            bm.SetPixel(x, y, New_Clr);
            if (Old_Color == New_Clr) { return; }

            while (pixel.Count > 0)
            {
                Point pt = (Point)pixel.Pop();
                if (pt.X > 0 && pt.Y > 0 && pt.X < bm.Width - 1 && pt.Y < bm.Height - 1)
                {
                    Validate(bm, pixel, pt.X - 1, pt.Y, Old_Color, New_Clr);
                    Validate(bm, pixel, pt.X, pt.Y - 1, Old_Color, New_Clr);
                    Validate(bm, pixel, pt.X + 1, pt.Y, Old_Color, New_Clr);
                    Validate(bm, pixel, pt.X, pt.Y + 1, Old_Color, New_Clr);
                }
            }
        }
    }
}
