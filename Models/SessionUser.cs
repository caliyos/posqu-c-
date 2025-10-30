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

        public long UserId { get; private set; }

        public long LoginId { get; private set; }
        public string Username { get; private set; }
        public int RoleId { get; private set; }
        public string RoleName { get; private set; }
        public int ShiftId { get; private set; }
        public int TerminalId { get; private set; }
        public string TerminalName { get; private set; }

        private SessionUser(
            int userId,
            int loginId,
            string username,
            int roleId,
            string roleName,
            int shiftId,
            int terminalId,
            string terminalName
        )
        {
            UserId = userId;
            LoginId = loginId;
            Username = username;
            RoleId = roleId;
            RoleName = roleName;
            ShiftId = shiftId;
            TerminalId = terminalId;
            TerminalName = terminalName;
        }

        public static void CreateSession(
            int userId,
            int loginId,
            string username,
            int roleId,
            string roleName,
            int shiftId,
            int terminalId,
            string terminalName
        )
        {
            instance = new SessionUser(userId, loginId,username, roleId, roleName, shiftId, terminalId, terminalName);
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
