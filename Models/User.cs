using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Models
{
    class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int TerminalId { get; set; }
        public string TerminalName { get; set; }
        public int ShiftId { get; set; }
    }
}