using System;
using System.Windows.Forms;

namespace Database_Compare
{
    public partial class CompareForm : Form
    {
        public CompareForm()
        {
            InitializeComponent();
        }
        private int fontSize = 20;

        private int ScrollSize = 0;


        private void CompareForm_Load(object sender, EventArgs e)
        {

        }


        private void CompareForm_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ScrollRun(e);
        }

        private void ScrollRun(MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                ScrollSize += 50;
            }
            else
            {
                ScrollSize -= 50;
            }
            //// Update the drawing based upon the mouse wheel scrolling.

            //int numberOfTextLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 120;
            //int numberOfPixelsToMove = numberOfTextLinesToMove * fontSize;

            //if (numberOfPixelsToMove != 0)
            //{
            //    System.Drawing.Drawing2D.Matrix translateMatrix = new System.Drawing.Drawing2D.Matrix();
            //    translateMatrix.Translate(0, numberOfPixelsToMove);
            //    //mousePath.Transform(translateMatrix);
            //}

            if (ScrollSize > 0)
            {
                richTextBoxKaynak.SelectionStart = ScrollSize;
                // scroll it automatically
                richTextBoxKaynak.ScrollToCaret();


                richTextBoxHedef.SelectionStart = ScrollSize;
                // scroll it automatically
                richTextBoxHedef.ScrollToCaret();
            }
            else
            {
                ScrollSize = 0;
            }

            this.Invalidate();
        }

    }
}
