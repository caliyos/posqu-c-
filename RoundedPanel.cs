using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu
{
    [ToolboxItem(true)]
    public class RoundedPanel : Panel
    {
        private int borderRadius = 20;
        private int borderSize = 1;
        private Color borderColor = Color.Gray;

        [Category("Custom")]
        public int BorderRadius
        {
            get => borderRadius;
            set { borderRadius = value; this.Invalidate(); }
        }

        [Category("Custom")]
        public int BorderSize
        {
            get => borderSize;
            set { borderSize = value; this.Invalidate(); }
        }

        [Category("Custom")]
        public Color BorderColor
        {
            get => borderColor;
            set { borderColor = value; this.Invalidate(); }
        }

        public RoundedPanel()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.White;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            using (GraphicsPath path = GetRoundedPath(rect, borderRadius))
            {
                // bikin sudut rounded
                this.Region = new Region(path);

                // border
                using (Pen pen = new Pen(borderColor, borderSize))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            int d = radius * 2;

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
    }
