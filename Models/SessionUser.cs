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

        public int LoginId { get; private set; }
        public string Username { get; private set; }
        public int RoleId { get; private set; }
        public string RoleName { get; private set; }
        public int ShiftId { get; private set; }
        public int TerminalId { get; private set; }
        public string TerminalName { get; private set; }
        public int WarehouseId { get; private set; }

        private SessionUser(
            int userId,
            int loginId,
            string username,
            int roleId,
            string roleName,
            int shiftId,
            int terminalId,
            string terminalName,
            int warehouseId
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
            WarehouseId = warehouseId <= 0 ? 1 : warehouseId;
        }

        public static void CreateSession(
            int userId,
            int loginId,
            string username,
            int roleId,
            string roleName,
            int shiftId,
            int terminalId,
            string terminalName,
            int warehouseId = 1
        )
        {
            instance = new SessionUser(userId, loginId, username, roleId, roleName, shiftId, terminalId, terminalName, warehouseId);
        }

        public static SessionUser GetCurrentUser()
        {
            return instance;
        }

        public static void UpdateShiftId(int shiftId)
        {
            if (instance == null) return;
            instance = new SessionUser(
                instance.UserId,
                instance.LoginId,
                instance.Username,
                instance.RoleId,
                instance.RoleName,
                shiftId,
                instance.TerminalId,
                instance.TerminalName,
                instance.WarehouseId
            );
        }

        public static void UpdateWarehouseId(int warehouseId)
        {
            if (instance == null) return;
            int wid = warehouseId <= 0 ? 1 : warehouseId;
            instance = new SessionUser(
                instance.UserId,
                instance.LoginId,
                instance.Username,
                instance.RoleId,
                instance.RoleName,
                instance.ShiftId,
                instance.TerminalId,
                instance.TerminalName,
                wid
            );
        }

        public static void Logout()
        {
            instance = null;
        }
    }
}
