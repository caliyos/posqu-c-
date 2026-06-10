using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu
{
    public class RoundedButton : Button
    {
        private int borderRadius = 20;

        public int BorderRadius
        {
            get => borderRadius;
            set
            {
                borderRadius = value;
                Invalidate();
            }
        }

        public RoundedButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Color.RoyalBlue;
            ForeColor = Color.White;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            GraphicsPath path = new GraphicsPath();

            path.AddArc(0, 0, BorderRadius, BorderRadius, 180, 90);
            path.AddArc(Width - BorderRadius, 0, BorderRadius, BorderRadius, 270, 90);
            path.AddArc(Width - BorderRadius, Height - BorderRadius, BorderRadius, BorderRadius, 0, 90);
            path.AddArc(0, Height - BorderRadius, BorderRadius, BorderRadius, 90, 90);

            path.CloseFigure();

            Region = new Region(path);

            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        }
    }
}
