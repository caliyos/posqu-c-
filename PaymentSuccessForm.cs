using POS_qu.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class PaymentSuccessForm : Form
    {
        private readonly Dictionary<Control, Color> _focusOriginalBackColor = new();
        private readonly Dictionary<Button, (Color BackColor, Color ForeColor)> _focusOriginalButtonColors = new();
        private string _receiptText = "";

        public PaymentSuccessForm()
        {
            InitializeComponent();
            WireFocusHighlight(this);
            this.KeyPreview = true;
            this.KeyDown += PaymentSuccessForm_KeyDown;
        }

        public void SetReceiptText(string text)
        {
            _receiptText = text ?? "";
        }

        private void PaymentSuccessForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
                return;
            }
            if (e.KeyCode == Keys.Enter && ActiveControl == btnDone)
            {
                Close();
                return;
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintReceipt();
        }

        private void btnSavePng_Click(object sender, EventArgs e)
        {
            SavePng();
        }

        private void btnSavePdf_Click(object sender, EventArgs e)
        {
            SavePdf();
        }

        private void btnWaText_Click(object sender, EventArgs e)
        {
            SendToWhatsApp();
        }

        private void btnWaPng_Click(object sender, EventArgs e)
        {
            SendToWhatsAppPng();
        }

        private void btnEmailText_Click(object sender, EventArgs e)
        {
            SendToEmail(txtEmail.Text);
        }

        private void btnEmailPng_Click(object sender, EventArgs e)
        {
            SendToEmailPng(txtEmail.Text);
        }

        private void PrintReceipt()
        {
            var text = _receiptText ?? "";
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Nota kosong.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var doc = new PrintDocument();
            doc.PrintPage += (_, e) =>
            {
                using var font = new Font("Consolas", 9);
                float y = 10;
                foreach (var line in text.Replace("\r\n", "\n").Split('\n'))
                {
                    e.Graphics.DrawString(line, font, Brushes.Black, new PointF(10, y));
                    y += font.GetHeight(e.Graphics) + 2;
                }
            };
            using var dlg = new PrintDialog { Document = doc, UseEXDialog = true };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                doc.PrinterSettings = dlg.PrinterSettings;
                doc.Print();
            }
        }

        private void SavePng()
        {
            var text = _receiptText ?? "";
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Nota kosong.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "PNG Image (*.png)|*.png",
                FileName = $"nota_{DateTime.Now:yyyyMMdd_HHmmss}.png"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            using var bmp = RenderReceiptTextToBitmap(text);
            bmp.Save(sfd.FileName, ImageFormat.Png);
        }

        private void SavePdf()
        {
            var text = _receiptText ?? "";
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Nota kosong.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var doc = new PrintDocument();
            doc.PrintPage += (_, e) =>
            {
                using var font = new Font("Consolas", 9);
                float y = 10;
                foreach (var line in text.Replace("\r\n", "\n").Split('\n'))
                {
                    e.Graphics.DrawString(line, font, Brushes.Black, new PointF(10, y));
                    y += font.GetHeight(e.Graphics) + 2;
                }
            };
            using var dlg = new PrintDialog { Document = doc, UseEXDialog = true };
            try
            {
                var ps = new PrinterSettings();
                ps.PrinterName = "Microsoft Print to PDF";
                if (ps.IsValid)
                    doc.PrinterSettings = ps;
            }
            catch { }

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                doc.PrinterSettings = dlg.PrinterSettings;
                doc.Print();
            }
        }

        private void SendToWhatsApp()
        {
            var text = _receiptText ?? "";
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Nota kosong.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var url = "https://wa.me/?text=" + Uri.EscapeDataString(text);
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private void SendToWhatsAppPng()
        {
            var text = _receiptText ?? "";
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Nota kosong.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string path;
            try
            {
                path = SaveReceiptPngToTemp();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var bmp = RenderReceiptTextToBitmap(text);
                using var clone = (Bitmap)bmp.Clone();
                Clipboard.SetImage(clone);
            }
            catch
            {
            }

            try
            {
                var url = "https://wa.me/?text=" + Uri.EscapeDataString("Nota siap. Jika pakai WhatsApp Desktop/Web: paste (Ctrl+V) untuk kirim PNG.");
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch
            {
            }

            try
            {
                Process.Start(new ProcessStartInfo("explorer.exe", "/select,\"" + path + "\"") { UseShellExecute = true });
            }
            catch
            {
            }
        }

        private void SendToEmail(string email)
        {
            var text = _receiptText ?? "";
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Nota kosong.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            email = (email ?? "").Trim();
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Isi email tujuan.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var url = $"mailto:{Uri.EscapeDataString(email)}?subject={Uri.EscapeDataString("Nota Pembayaran")}&body={Uri.EscapeDataString(text)}";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private void SendToEmailPng(string email)
        {
            var text = _receiptText ?? "";
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Nota kosong.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            email = (email ?? "").Trim();
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Isi email tujuan.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string path;
            try
            {
                path = SaveReceiptPngToTemp();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var outlookType = Type.GetTypeFromProgID("Outlook.Application");
                if (outlookType != null)
                {
                    dynamic outlook = Activator.CreateInstance(outlookType);
                    dynamic mail = outlook.CreateItem(0);
                    mail.To = email;
                    mail.Subject = "Nota Pembayaran";
                    mail.Body = text;
                    mail.Attachments.Add(path);
                    mail.Display();
                    return;
                }
            }
            catch
            {
            }

            try
            {
                var url = $"mailto:{Uri.EscapeDataString(email)}?subject={Uri.EscapeDataString("Nota Pembayaran")}&body={Uri.EscapeDataString("Terlampir nota (PNG). Silakan attach file dari folder yang terbuka.\\n\\n" + text)}";
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch
            {
            }

            try
            {
                Process.Start(new ProcessStartInfo("explorer.exe", "/select,\"" + path + "\"") { UseShellExecute = true });
            }
            catch
            {
            }
        }

        private string SaveReceiptPngToTemp()
        {
            var text = _receiptText ?? "";
            var dir = Path.Combine(Path.GetTempPath(), "POS-qu");
            Directory.CreateDirectory(dir);
            var path = Path.Combine(dir, $"nota_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            using var bmp = RenderReceiptTextToBitmap(text);
            bmp.Save(path, ImageFormat.Png);
            return path;
        }

        private static Bitmap RenderReceiptTextToBitmap(string text)
        {
            var lines = (text ?? "").Replace("\r\n", "\n").Split('\n');
            using var tmp = new Bitmap(1, 1);
            using var g = Graphics.FromImage(tmp);
            using var font = new Font("Consolas", 10);

            float width = 520;
            float height = 20;
            foreach (var line in lines)
            {
                var size = g.MeasureString(line, font);
                if (size.Width + 40 > width) width = size.Width + 40;
                height += size.Height + 4;
            }

            var bmp = new Bitmap((int)Math.Ceiling(width), (int)Math.Ceiling(height));
            using var g2 = Graphics.FromImage(bmp);
            g2.Clear(Color.White);
            g2.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            float y = 12;
            foreach (var line in lines)
            {
                g2.DrawString(line, font, Brushes.Black, new PointF(12, y));
                y += font.GetHeight(g2) + 4;
            }
            return bmp;
        }

        private void WireFocusHighlight(Control root)
        {
            foreach (Control c in root.Controls)
            {
                if (c is TextBox || c is ComboBox || c is NumericUpDown || c is DateTimePicker)
                {
                    if (!_focusOriginalBackColor.ContainsKey(c))
                        _focusOriginalBackColor[c] = c.BackColor;
                    c.Enter -= OnFocusEnter;
                    c.Leave -= OnFocusLeave;
                    c.Enter += OnFocusEnter;
                    c.Leave += OnFocusLeave;
                }

                if (c is Button b)
                {
                    if (!_focusOriginalButtonColors.ContainsKey(b))
                        _focusOriginalButtonColors[b] = (b.BackColor, b.ForeColor);
                    b.Enter -= OnButtonFocusEnter;
                    b.Leave -= OnButtonFocusLeave;
                    b.Enter += OnButtonFocusEnter;
                    b.Leave += OnButtonFocusLeave;
                }

                if (c.HasChildren)
                    WireFocusHighlight(c);
            }
        }

        private void OnFocusEnter(object? sender, EventArgs e)
        {
            if (sender is Control c)
                c.BackColor = Color.FromArgb(255, 249, 196);
        }

        private void OnFocusLeave(object? sender, EventArgs e)
        {
            if (sender is Control c && _focusOriginalBackColor.TryGetValue(c, out var color))
                c.BackColor = color;
        }

        private void OnButtonFocusEnter(object? sender, EventArgs e)
        {
            if (sender is Button b)
            {
                b.FlatAppearance.BorderSize = 2;
                b.FlatAppearance.BorderColor = Color.Black;
            }
        }

        private void OnButtonFocusLeave(object? sender, EventArgs e)
        {
            if (sender is Button b)
            {
                b.FlatAppearance.BorderSize = 0;
            }
        }
    }
}

