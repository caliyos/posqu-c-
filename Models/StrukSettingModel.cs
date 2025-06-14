using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Models
{
    public class StrukSettingModel
    {
        public string Judul { get; set; }             // Misal: "TOKO ABC"
        public string Alamat { get; set; }
        public string NomorTelepon { get; set; }
        public string Footer { get; set; }            // Misal: "Terima kasih sudah berbelanja"
        public byte[] LogoBytes { get; set; }         // Logo dalam bentuk byte[]

        public bool IsVisibleNamaToko { get; set; }
    }

}
