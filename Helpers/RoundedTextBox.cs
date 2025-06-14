using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace POS_qu.Helpers
{

    public class RoundedTextBox : UserControl
    {
        private TextBox textBox = new TextBox();

        [Category("Appearance")]
        public int BorderRadius { get; set; } = 10;

        [Category("Appearance")]
        public Color BorderColor { get; set; } = Color.Gray;

        [Category("Appearance")]
        public Color FocusBorderColor { get; set; } = Color.DodgerBlue;

        [Category("Appearance")]
        public string PlaceholderText { get; set; } = "Enter text...";

        public string TextValue
        {
            get => textBox.Text;
            set => textBox.Text = value;
        }

        public RoundedTextBox()
        {
            this.DoubleBuffered = true;
            this.Padding = new Padding(10);
            this.BackColor = Color.White;
            this.Size = new Size(250, 40);

            textBox.BorderStyle = BorderStyle.None;
            textBox.BackColor = Color.White;
            textBox.ForeColor = Color.Black;
            textBox.Font = new Font("Segoe UI", 10);
            textBox.Location = new Point(10, 10);
            textBox.Width = this.Width - 20;
            textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox.TextChanged += (s, e) => this.Invalidate();

            textBox.GotFocus += (s, e) => this.Invalidate();
            textBox.LostFocus += (s, e) => this.Invalidate();

            this.Controls.Add(textBox);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            textBox.Width = this.Width - 20;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int radius = BorderRadius;
            Color border = textBox.Focused ? FocusBorderColor : BorderColor;

            using (GraphicsPath path = new GraphicsPath())
            using (Pen pen = new Pen(border, 1.5f))
            {
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(this.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(this.Width - radius, this.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, this.Height - radius, radius, radius, 90, 90);
                path.CloseFigure();

                this.Region = new Region(path);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
            }

            // Placeholder
            if (string.IsNullOrEmpty(textBox.Text) && !textBox.Focused && !string.IsNullOrEmpty(PlaceholderText))
            {
                using (Brush b = new SolidBrush(Color.Gray))
                {
                    e.Graphics.DrawString(PlaceholderText, textBox.Font, b, new PointF(12, (this.Height - textBox.Font.Height) / 2));
                }
            }
        }
    }
}
