using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
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
        PictureBox last; //for saving the last selected tool
        Point line1, line2; // points for drawing a Line

        enum ITEM
        {
            Color, Paint, Pencil, Eraser, Ellipse, Rectangle, Line
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
            drawPanel.Image = bm;
            DoubleBuffered = true;
            last = pbPencil;
            tool_Select(pbPencil, null);
            currItem = ITEM.Pencil;
            New_Color = Color.Black;
            line1 = line2 = Point.Empty;
        }
        /// <summary>
        /// This function enables us when we pick one of the tools to change the color of it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tool_Select(object sender, MouseEventArgs e)
        {
            PictureBox picture = (PictureBox)sender;
            last.BackColor = Color.Transparent;

            picture.BackColor = Color.Blue;
            last = picture;

            line1 = line2 = Point.Empty;
        }


        /// <summary>
        /// 6 of the functions below allow us, so when we click on one of the tools, the value of enum to be saved on which tool we have selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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


        /// <summary>
        /// The 2 functions below allow us to choose the color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                p.Color = cd.Color;
            }
        }

        /// <summary>
        /// This function allow us when we do the event MouseDown it initializes the variable paint to true, which means that we are allowed to draw and it initializes the locations of the points.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drawPanel_MouseDown(object sender, MouseEventArgs e)
        {
            paint = true;
            py = e.Location;

            cx = e.X;
            cy = e.Y;
        }


        /// <summary>
        /// This function allows when the MouseMove event happens, to draw with a pencil or erase with an eraser that depends on which tool we have selected. And after that it initializes the variables  in the locations where we are at.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// This function allows us when we click on the PictureBox which is all white, to erase all of the draw panel and turn it into white.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbClear_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            drawPanel.Image = bm;
            currItem = ITEM.Pencil;
        }

        /// <summary>
        /// Functions tool_MouseHover() and tool_MouseHoverOut() allow the tools to hover when the mouse is on top of it and to go transparent when the mouse is removed from the square.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tool_MouseHover(object sender, EventArgs e)
        {
            PictureBox picture = (PictureBox)sender;
            picture.BackColor = Color.FromArgb(158, 154, 153);
        }

        private void tool_MouseHoverOut(object sender, EventArgs e)
        {
            PictureBox picture = (PictureBox)sender;
            picture.BackColor = Color.Transparent;
            last.BackColor = Color.Blue;
        }

        /// <summary>
        /// The functions drawPanel_MouseUp() and drawPanel_Paint(), allow so when we move the mouse up meanwhile we are painting to draw one of the forms that we have selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drawPanel_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;

            sx = x - cx;
            sy = y - cy;
            Pen pen = new Pen(New_Color, 1);
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
                if (!line2.IsEmpty)
                {
                    Shape line = new Line(pen);
                    line.draw(g, line1.X, line1.Y, line2.X, line2.Y);
                    line1 = line2 = Point.Empty;
                }
            }
        }

        private void drawPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (paint)
            {
                Pen pen = new Pen(New_Color, 1);
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
                    if (!line2.IsEmpty)
                    {
                        Shape line = new Line(pen);
                        line.draw(g, line1.X, line1.Y, line2.X, line2.Y);
                        line1 = line2 = Point.Empty;
                    }                    
                }
            }
        }

        /// <summary>
        /// This function allows so that when we click to paint the area if we have selected the bucket, as well with 2 clicks, adds value to line1 and line2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drawPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (currItem == ITEM.Paint)
            {
                Point point = set_Point(drawPanel, e.Location);
                Fill(bm, point.X, point.Y, New_Color);
            }
            else if (currItem == ITEM.Line)
            {
                if (line1.IsEmpty)
                {
                    line1 = e.Location;
                } 
                else
                {
                    line2 = e.Location;
                }
            }
        }


        /// <summary>
        /// The functions set_Point(), Validate() and Fill() allows that a form to find the sides so that when the bucket will be used to paint, to know that it needs to paint only that area.
        /// </summary>
        /// <param name="pb"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
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


        /// <summary>
        /// functions below are for SERIALIZATION
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            drawPanel.Image = bm;
            currItem = ITEM.Pencil;
        }


        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Image(*.jpg)|*.jpg|(*.*)|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Bitmap btm = bm.Clone(new Rectangle(0, 0, drawPanel.Width, drawPanel.Height), bm.PixelFormat);
                btm.Save(sfd.FileName, ImageFormat.Jpeg);
            }
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    drawPanel.Image = new Bitmap(dlg.FileName);
                }
                dlg.Dispose();
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();
            PrintDocument printDoc = new PrintDocument();
            printDoc.DocumentName = "Print Document";
            printDlg.Document = printDoc;
            if (printDlg.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }
    }
}
