using POS_qu.Controllers;
using POS_qu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Helpers
{

    public static class StrukSettingHelper
    {
        private static SettingController settingController = new SettingController();

        public static StrukSettingModel Load()
        {
            var row = settingController.GetStrukSetting(); // buat method ini ambil dari DB

            if (row == null) return null;

            return new StrukSettingModel
            {
                Judul = row["judul"]?.ToString() ?? "",
                Alamat = row["alamat"]?.ToString() ?? "",
                NomorTelepon = row["telepon"]?.ToString() ?? "",
                Footer = row["footer"]?.ToString() ?? "",
                LogoBytes = row["logo"] != DBNull.Value ? (byte[])row["logo"] : null,
                IsVisibleNamaToko = row["is_visible_nama_toko"] != DBNull.Value && (bool)row["is_visible_nama_toko"]

            };
        }

        public static void Save(StrukSettingModel model)
        {
            settingController.SaveStrukSetting(model); // buat method ini di SettingController
        }

        public static Image GetLogoImage(byte[] logoBytes)
        {
            if (logoBytes == null) return null;
            using (var ms = new MemoryStream(logoBytes))
            {
                return Image.FromStream(ms);
            }
        }

        public static byte[] ImageToBytes(Image image)
        {
            if (image == null) return null;

            using (var ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
