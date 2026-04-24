using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class PrinterTestForm : Form
    {
        private string _selectedPrinterName = "";
        private int _paperWidthMm = 58;

        public PrinterTestForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;

            Load += PrinterTestForm_Load;
            btnClose.Click += (s, e) => Close();
            btnRefresh.Click += (s, e) => LoadPrinters();
            cmbPrinters.SelectedIndexChanged += (s, e) => UpdateSelectedPrinter();
            rb58.CheckedChanged += (s, e) => { if (rb58.Checked) _paperWidthMm = 58; };
            rb80.CheckedChanged += (s, e) => { if (rb80.Checked) _paperWidthMm = 80; };
            btnTestPrint.Click += (s, e) => TestPrintWithDriver();
            btnTestRaw.Click += (s, e) => TestPrintRaw();
        }

        private void PrinterTestForm_Load(object sender, EventArgs e)
        {
            rb58.Checked = true;
            LoadPrinters();
        }

        private void LoadPrinters()
        {
            cmbPrinters.Items.Clear();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                cmbPrinters.Items.Add(printer);
            }

            if (cmbPrinters.Items.Count > 0)
            {
                cmbPrinters.SelectedIndex = 0;
            }
            else
            {
                _selectedPrinterName = "";
                lblStatus.Text = "Status: Tidak ada printer terdeteksi di Windows.";
            }

            AppendLog("Refresh printer list: " + cmbPrinters.Items.Count + " printer ditemukan.");
        }

        private void UpdateSelectedPrinter()
        {
            _selectedPrinterName = cmbPrinters.SelectedItem?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(_selectedPrinterName))
            {
                lblStatus.Text = "Status: -";
                return;
            }

            var ps = new PrinterSettings { PrinterName = _selectedPrinterName };
            lblStatus.Text = ps.IsValid
                ? "Status: OK (Printer terdaftar di Windows)"
                : "Status: Tidak valid / driver belum terpasang";

            AppendLog("Selected printer: " + _selectedPrinterName + " | IsValid=" + ps.IsValid);
        }

        private void TestPrintWithDriver()
        {
            if (string.IsNullOrWhiteSpace(_selectedPrinterName))
            {
                MessageBox.Show("Pilih printer dulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var ps = new PrinterSettings { PrinterName = _selectedPrinterName };
            if (!ps.IsValid)
            {
                MessageBox.Show("Printer tidak valid. Pastikan driver sudah terpasang.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var doc = new PrintDocument();
            doc.PrinterSettings = ps;
            doc.DocumentName = "POS-qu Test Print";
            doc.PrintPage += (s, e) =>
            {
                DrawReceipt(e.Graphics, e.MarginBounds);
                e.HasMorePages = false;
            };

            try
            {
                doc.Print();
                AppendLog("Test Print (Driver) sukses.");
            }
            catch (Exception ex)
            {
                AppendLog("Test Print (Driver) gagal: " + ex.Message);
                MessageBox.Show("Gagal print: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TestPrintRaw()
        {
            if (string.IsNullOrWhiteSpace(_selectedPrinterName))
            {
                MessageBox.Show("Pilih printer dulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("POS-qu - TEST PRINT (RAW)");
            sb.AppendLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            sb.AppendLine("--------------------------------");
            sb.AppendLine("Printer: " + _selectedPrinterName);
            sb.AppendLine("Paper: " + _paperWidthMm + "mm");
            sb.AppendLine("--------------------------------");
            sb.AppendLine("Jika struk ini keluar: RAW OK.");
            sb.AppendLine("");

            byte[] init = new byte[] { 0x1B, 0x40 };
            byte[] cut = new byte[] { 0x1D, 0x56, 0x41, 0x10 };
            byte[] payload = Combine(init, Encoding.ASCII.GetBytes(sb.ToString()), cut);

            bool ok = RawPrinter.SendBytesToPrinter(_selectedPrinterName, payload);
            AppendLog(ok ? "Test Print (RAW) sukses." : "Test Print (RAW) gagal.");
            if (!ok)
                MessageBox.Show("RAW print gagal. Pastikan printer support RAW / driver RAW tersedia.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DrawReceipt(Graphics g, Rectangle marginBounds)
        {
            int widthPx = MmToPx(_paperWidthMm, g.DpiX);
            int left = marginBounds.Left;
            int top = marginBounds.Top;
            int y = top;

            var titleFont = new Font("Segoe UI", 12, FontStyle.Bold);
            var normalFont = new Font("Segoe UI", 9);
            var monoFont = new Font("Consolas", 9);

            string title = "POS-qu - TEST PRINT";
            var titleSize = g.MeasureString(title, titleFont);
            int titleX = left + Math.Max(0, (widthPx - (int)titleSize.Width) / 2);
            g.DrawString(title, titleFont, Brushes.Black, new PointF(titleX, y));
            y += (int)titleSize.Height + 6;

            g.DrawString(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), normalFont, Brushes.Black, left, y);
            y += 22;

            string line = new string('-', Math.Max(20, _paperWidthMm == 58 ? 32 : 42));
            g.DrawString(line, monoFont, Brushes.Black, left, y);
            y += 22;

            g.DrawString("Printer: " + _selectedPrinterName, normalFont, Brushes.Black, left, y);
            y += 22;
            g.DrawString("Paper: " + _paperWidthMm + "mm", normalFont, Brushes.Black, left, y);
            y += 22;

            g.DrawString(line, monoFont, Brushes.Black, left, y);
            y += 22;

            g.DrawString("Jika struk ini keluar: OK.", normalFont, Brushes.Black, left, y);
            y += 22;

            g.DrawString(line, monoFont, Brushes.Black, left, y);
        }

        private static int MmToPx(int mm, float dpi)
        {
            return (int)Math.Round(mm / 25.4f * dpi);
        }

        private static byte[] Combine(params byte[][] arrays)
        {
            int len = 0;
            foreach (var a in arrays) len += a?.Length ?? 0;
            var result = new byte[len];
            int offset = 0;
            foreach (var a in arrays)
            {
                if (a == null || a.Length == 0) continue;
                Buffer.BlockCopy(a, 0, result, offset, a.Length);
                offset += a.Length;
            }
            return result;
        }

        private void AppendLog(string msg)
        {
            string line = DateTime.Now.ToString("HH:mm:ss") + " - " + msg;
            txtLog.AppendText(line + Environment.NewLine);
        }

        private static class RawPrinter
        {
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            private class DOCINFO
            {
                [MarshalAs(UnmanagedType.LPWStr)] public string pDocName;
                [MarshalAs(UnmanagedType.LPWStr)] public string pOutputFile;
                [MarshalAs(UnmanagedType.LPWStr)] public string pDataType;
            }

            [DllImport("winspool.Drv", EntryPoint = "OpenPrinterW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true)]
            private static extern bool OpenPrinter(string src, out IntPtr hPrinter, IntPtr pd);

            [DllImport("winspool.Drv", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true)]
            private static extern bool ClosePrinter(IntPtr hPrinter);

            [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true)]
            private static extern bool StartDocPrinter(IntPtr hPrinter, int level, [In] DOCINFO di);

            [DllImport("winspool.Drv", SetLastError = true, ExactSpelling = true)]
            private static extern bool EndDocPrinter(IntPtr hPrinter);

            [DllImport("winspool.Drv", SetLastError = true, ExactSpelling = true)]
            private static extern bool StartPagePrinter(IntPtr hPrinter);

            [DllImport("winspool.Drv", SetLastError = true, ExactSpelling = true)]
            private static extern bool EndPagePrinter(IntPtr hPrinter);

            [DllImport("winspool.Drv", SetLastError = true, ExactSpelling = true)]
            private static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);

            public static bool SendBytesToPrinter(string printerName, byte[] bytes)
            {
                if (string.IsNullOrWhiteSpace(printerName) || bytes == null || bytes.Length == 0) return false;

                IntPtr pUnmanagedBytes = IntPtr.Zero;
                IntPtr hPrinter = IntPtr.Zero;
                try
                {
                    if (!OpenPrinter(printerName, out hPrinter, IntPtr.Zero))
                        return false;

                    var di = new DOCINFO
                    {
                        pDocName = "POS-qu RAW Test",
                        pDataType = "RAW",
                        pOutputFile = null
                    };

                    if (!StartDocPrinter(hPrinter, 1, di))
                        return false;

                    if (!StartPagePrinter(hPrinter))
                        return false;

                    pUnmanagedBytes = Marshal.AllocCoTaskMem(bytes.Length);
                    Marshal.Copy(bytes, 0, pUnmanagedBytes, bytes.Length);

                    int written;
                    bool ok = WritePrinter(hPrinter, pUnmanagedBytes, bytes.Length, out written);
                    EndPagePrinter(hPrinter);
                    EndDocPrinter(hPrinter);

                    return ok && written == bytes.Length;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    if (pUnmanagedBytes != IntPtr.Zero)
                        Marshal.FreeCoTaskMem(pUnmanagedBytes);
                    if (hPrinter != IntPtr.Zero)
                        ClosePrinter(hPrinter);
                }
            }
        }
    }
}

