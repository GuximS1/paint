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
        Color New_Color;
        enum ITEM
        {
            Default, Color, Paint, Pencil, Eraser, Ellipse, Rectangle, Line
        }
        ITEM currItem;
        public Form1()
        {
            InitializeComponent();
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
                //p.Color = cd.Color;
            }
        }
    }
}
