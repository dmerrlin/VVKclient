using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VVK
{
   public class MoveblePicrureBox : PictureBox
    {
        Point Location;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Location = e.Location;
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - Location.X;
                this.Top += e.Y - Location.Y;
            }
         
                base.OnMouseMove(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.Dispose();

            }
            base.OnMouseClick(e);
        }
    }
}
