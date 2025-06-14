using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Helpers
{

    public static class ImageHelper
    {
        /// <summary>
        /// Menampilkan dialog untuk memilih gambar, menyimpannya ke folder images/[category], menampilkan preview, dan mengembalikan relative path-nya.
        /// </summary>
        /// <param name="previewBox">PictureBox tempat preview gambar</param>
        /// <param name="category">Subfolder dalam folder 'images', misalnya 'applogo' atau 'produk'</param>
        /// <returns>Relative path gambar (contoh: "images/produk/abc123.png") atau null jika dibatalkan</returns>
        public static long MaxSizeBytes { get; set; } = 2 * 1024 * 1024; // 2 MB
        public static string SelectAndSaveImage(PictureBox previewBox, string category = "default")
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "PNG Files|*.png";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // Validasi ekstensi file
                    string fileExt = Path.GetExtension(ofd.FileName).ToLower();
                    if (fileExt != ".png")
                    {
                        MessageBox.Show("Hanya file PNG yang diizinkan.", "Format Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return null;
                    }

                    // Validasi ukuran file
                    FileInfo fileInfo = new FileInfo(ofd.FileName);
                    if (fileInfo.Length > MaxSizeBytes)
                    {
                        MessageBox.Show("Ukuran file terlalu besar. Maksimal 2 MB.", "Ukuran Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return null;
                    }

                    // Generate unique filename
                    string newFileName = Guid.NewGuid().ToString() + fileExt;
                    string imagesFolder = Path.Combine(Application.StartupPath, "images", category);
                    string destinationPath = Path.Combine(imagesFolder, newFileName);

                    // Buat folder jika belum ada
                    if (!Directory.Exists(imagesFolder))
                    {
                        Directory.CreateDirectory(imagesFolder);
                    }

                    // Salin file
                    File.Copy(ofd.FileName, destinationPath);

                    // Tampilkan preview
                    if (previewBox != null)
                    {
                        previewBox.Image = Image.FromFile(destinationPath);
                    }

                    // Return path relatif
                    return Path.Combine("images", category, newFileName);
                }
            }

            return null;
        }
    }
}
