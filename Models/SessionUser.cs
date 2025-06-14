using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Models
{
    public class SessionUser
    {
        private static SessionUser instance;

        public int UserId { get; private set; }
        public string Username { get; private set; }
        public int Role { get; private set; }
        public int ShiftId { get; private set; }  // Menambahkan Shift
        public int TerminalId { get; private set; }  // Menambahkan Terminal

        private SessionUser(int userId, string username, int role, int shiftId, int terminalId)
        {
            UserId = userId;
            Username = username;
            Role = role;
            ShiftId = shiftId;
            TerminalId = terminalId;
        }


        public static void CreateSession(int userId, string username, int role, int shiftId, int terminalId)
        {
            instance = new SessionUser(userId, username, role, shiftId, terminalId);
        }
        public static SessionUser GetCurrentUser()
        {
            return instance;
        }

        public static void Logout()
        {
            instance = null;
        }
    }

}
