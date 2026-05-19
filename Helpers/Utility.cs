﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using BCrypt.Net;

namespace POS_qu.Helpers
{
    public static class UiNumberFormat
    {
        public static readonly CultureInfo DotCulture = BuildDotCulture();


        private static CultureInfo BuildDotCulture()
        {
            var c = (CultureInfo)CultureInfo.GetCultureInfo("id-ID").Clone();

            // Ribuan
            c.NumberFormat.NumberGroupSeparator = ".";
            c.NumberFormat.CurrencyGroupSeparator = ".";

            // Decimal
            c.NumberFormat.NumberDecimalSeparator = ",";
            c.NumberFormat.CurrencyDecimalSeparator = ",";

            return c;
        }


        public static void ApplyDotCulture()
        {
            CultureInfo.DefaultThreadCurrentCulture = DotCulture;
            CultureInfo.DefaultThreadCurrentUICulture = DotCulture;

            Thread.CurrentThread.CurrentCulture = DotCulture;
            Thread.CurrentThread.CurrentUICulture = DotCulture;
        }


        // =========================
        // FORMAT NUMBER
        // =========================

        public static string FormatNumber(decimal value, int decimals = 0)
        {
            return value.ToString($"N{decimals}", DotCulture);
        }

        public static string FormatNumber(decimal? value, int decimals = 0)
        {
            return (value ?? 0m).ToString($"N{decimals}", DotCulture);
        }


        // =========================
        // FORMAT MONEY
        // =========================

        public static string FormatMoney(decimal value)
        {
            return value.ToString("N0", DotCulture);
        }

        public static string FormatMoney(decimal? value)
        {
            return (value ?? 0m).ToString("N0", DotCulture);
        }


        // =========================
        // PARSE MONEY TEXT
        // =========================

        public static decimal ParseMoney(string? text)
        {
            text ??= "";

            string clean = text
                .Replace("Rp", "", StringComparison.OrdinalIgnoreCase)
                .Replace(".", "")
                .Replace(",", ".")
                .Trim();

            if (decimal.TryParse(
                clean,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out var value))
            {
                return value;
            }

            return 0m;
        }

        // =========================
        // FORMAT TEXT INPUT MONEY
        // =========================

        public static string FormatMoneyText(string? text)
        {
            var v = ParseMoney(text);
            return v.ToString("N0", DotCulture);
        }
    }

    class Utility
    {
        //private static string vStrConnection = "Host=localhost;Port=5433;Username=postgres;Password=postgres11;Database=posqu";

        public static string GetPcId()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .FirstOrDefault(nic => nic.OperationalStatus == OperationalStatus.Up
                                       && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)?
                .GetPhysicalAddress().ToString();
        }
        public static string GenerateTransactionNumber()
        {
            return "TRX-" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }
        public static string GetCurrentShift()
        {
            var hour = DateTime.Now.Hour;
            if (hour >= 6 && hour < 14)
                return "Shift 1";
            else if (hour >= 14 && hour < 22)
                return "Shift 2";
            else
                return "Shift 3"; // Malam
        }

        public static string getTrxNumbering()
        {
            return "TXN-" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        // Method untuk mengambil nama terminal berdasarkan PC ID
        public static string GetTerminalName(string pcId)
        {
            string terminalName = null;

            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT terminal_name FROM terminals WHERE pc_id = @pc_id LIMIT 1";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@pc_id", pcId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                terminalName = reader.GetString(reader.GetOrdinal("terminal_name"));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error fetching terminal name: " + ex.Message);
                }
            }

            return terminalName;
        }

        // 🔥 Helper baru: ambil nama user dari userId
        public static string GetUserName(int userId)
        {
            string userName = "Unknown";

            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT name FROM users WHERE id = @id LIMIT 1";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", userId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userName = reader.GetString(reader.GetOrdinal("name"));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error fetching user name: " + ex.Message);
                }
            }

            return userName;
        }

        public static bool IsAdmin()
        {
            return POS_qu.Models.SessionUser.GetCurrentUser()?.RoleId == 1;
        }

        public static bool HasRole(params int[] roleIds)
        {
            var u = POS_qu.Models.SessionUser.GetCurrentUser();
            if (u == null) return false;
            if (roleIds == null || roleIds.Length == 0) return false;
            return roleIds.Contains(u.RoleId);
        }

        public static bool EnsureAdmin(IWin32Window owner, string actionTitle)
        {
            if (IsAdmin()) return true;
            MessageBox.Show(owner, "Akses ditolak. Hanya Admin yang boleh melakukan aksi ini.", actionTitle ?? "Akses Ditolak", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private static readonly Dictionary<int, HashSet<string>> _permCacheByRole = new();
        private static DateTime _permCacheAtUtc = DateTime.MinValue;

        public static bool HasPermission(string permissionName)
        {
            permissionName ??= "";
            permissionName = permissionName.Trim();
            if (string.IsNullOrWhiteSpace(permissionName)) return false;

            var u = POS_qu.Models.SessionUser.GetCurrentUser();
            if (u == null) return false;
            if (u.RoleId == 1) return true;

            try
            {
                if ((DateTime.UtcNow - _permCacheAtUtc).TotalMinutes > 3)
                {
                    _permCacheByRole.Clear();
                    _permCacheAtUtc = DateTime.UtcNow;
                }

                if (_permCacheByRole.TryGetValue(u.RoleId, out var cached))
                    return cached.Contains(permissionName);

                using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                conn.Open();

                using var cmd = new NpgsqlCommand(@"
SELECT p.name
FROM permission_role pr
JOIN permissions p ON p.id = pr.permission_id
WHERE pr.role_id = @rid
", conn);
                cmd.Parameters.AddWithValue("@rid", u.RoleId);
                using var r = cmd.ExecuteReader();
                var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                while (r.Read())
                {
                    var n = r["name"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(n)) set.Add(n.Trim());
                }
                _permCacheByRole[u.RoleId] = set;
                return set.Contains(permissionName);
            }
            catch
            {
                return false;
            }
        }

        public static bool EnsurePermission(IWin32Window owner, string permissionName, string actionTitle)
        {
            if (HasPermission(permissionName)) return true;
            MessageBox.Show(owner, "Akses ditolak. Kamu tidak punya izin untuk aksi ini.", actionTitle ?? "Akses Ditolak", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        public static bool TrySupervisorApproval(
            IWin32Window owner,
            string actionTitle,
            string? reasonPlaceholder,
            bool requireReason,
            out int approverUserId,
            out string approverUsername,
            out string approvalReason
        )
        {
            approverUserId = 0;
            approverUsername = "";
            approvalReason = "";

            using var modal = new Form
            {
                Text = actionTitle,
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(520, 340),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                Padding = new Padding(18),
                KeyPreview = true
            };

            var lblTitle = new Label
            {
                Text = "Supervisor Approval Required",
                Dock = DockStyle.Top,
                Height = 36,
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold)
            };

            var pnl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                Padding = new Padding(0, 12, 0, 0)
            };
            pnl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            pnl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            pnl.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            pnl.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            pnl.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var lblPin = new Label { Text = "PIN Supervisor", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
            var txtPin = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 16F, FontStyle.Bold), UseSystemPasswordChar = true, MaxLength = 8 };

            var lblReason = new Label { Text = "Reason", Dock = DockStyle.Fill, TextAlign = ContentAlignment.TopLeft, Font = new Font("Segoe UI", 10F) };
            var txtReason = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 11F), Multiline = true, ScrollBars = ScrollBars.Vertical };
            if (!string.IsNullOrWhiteSpace(reasonPlaceholder))
                txtReason.Text = reasonPlaceholder;

            var lblHint = new Label
            {
                Text = "Enter = Approve • Esc = Cancel",
                Dock = DockStyle.Fill,
                ForeColor = Color.DimGray,
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                TextAlign = ContentAlignment.MiddleLeft
            };

            pnl.Controls.Add(lblPin, 0, 0);
            pnl.Controls.Add(txtPin, 1, 0);
            pnl.Controls.Add(lblReason, 0, 1);
            pnl.Controls.Add(txtReason, 1, 1);
            pnl.Controls.Add(lblHint, 1, 2);

            var footer = new Panel { Dock = DockStyle.Bottom, Height = 56 };
            var btnCancel = new Button { Text = "Batal (Esc)", Dock = DockStyle.Right, Width = 120 };
            var btnOk = new Button
            {
                Text = "Approve",
                Dock = DockStyle.Right,
                Width = 120,
                BackColor = Color.FromArgb(0, 122, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnOk.FlatAppearance.BorderSize = 0;
            footer.Controls.Add(btnCancel);
            footer.Controls.Add(btnOk);

            bool approved = false;
            int approverUserIdLocal = 0;
            string approverUsernameLocal = "";
            string approvalReasonLocal = "";

            bool TryCheckCredentials()
            {
                string pin = (txtPin.Text ?? "").Trim();
                string r = (txtReason.Text ?? "").Trim();

                if (string.IsNullOrWhiteSpace(pin))
                {
                    MessageBox.Show(modal, "Masukkan PIN supervisor.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (requireReason && string.IsNullOrWhiteSpace(r))
                {
                    MessageBox.Show(modal, "Alasan wajib diisi untuk aksi ini.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                conn.Open();
                using (var ensure = new NpgsqlCommand("ALTER TABLE users ADD COLUMN IF NOT EXISTS pin_hash TEXT;", conn))
                {
                    ensure.ExecuteNonQuery();
                }

                using var cmd = new NpgsqlCommand(@"
SELECT
    u.id,
    u.username,
    u.pin_hash,
    COALESCE(r.id,0) AS role_id,
    COALESCE(r.name,'') AS role_name
FROM users u
LEFT JOIN role_user ru ON ru.user_id = u.id
LEFT JOIN roles r ON r.id = ru.role_id
WHERE u.pin_hash IS NOT NULL AND u.pin_hash <> ''
", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int userId = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0;
                    string username = reader["username"]?.ToString() ?? "";
                    string hash = reader["pin_hash"]?.ToString() ?? "";
                    int roleId = reader["role_id"] != DBNull.Value ? Convert.ToInt32(reader["role_id"]) : 0;
                    string roleName = (reader["role_name"]?.ToString() ?? "").Trim().ToLowerInvariant();

                    bool roleOk = roleId == 1 || roleId == 3 || roleName == "admin" || roleName == "supervisor";
                    if (!roleOk) continue;

                    if (!string.IsNullOrWhiteSpace(hash) && BCrypt.Net.BCrypt.Verify(pin, hash))
                    {
                        approverUserIdLocal = userId;
                        approverUsernameLocal = username;
                        approvalReasonLocal = r;
                        return true;
                    }
                }

                MessageBox.Show(modal, "PIN salah atau PIN belum diset untuk supervisor/admin.", "Ditolak", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            btnOk.Click += (_, __) =>
            {
                if (!TryCheckCredentials()) return;
                approved = true;
                modal.DialogResult = DialogResult.OK;
                modal.Close();
            };
            btnCancel.Click += (_, __) => { modal.DialogResult = DialogResult.Cancel; modal.Close(); };

            modal.AcceptButton = btnOk;
            modal.CancelButton = btnCancel;
            txtPin.KeyPress += (_, e) =>
            {
                if (char.IsControl(e.KeyChar)) return;
                if (!char.IsDigit(e.KeyChar)) e.Handled = true;
            };

            modal.Shown += (_, __) => txtPin.Focus();
            modal.KeyDown += (_, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    modal.DialogResult = DialogResult.Cancel;
                    modal.Close();
                    e.Handled = true;
                }
            };

            modal.Controls.Add(pnl);
            modal.Controls.Add(footer);
            modal.Controls.Add(lblTitle);

            modal.ShowDialog(owner);
            approverUserId = approverUserIdLocal;
            approverUsername = approverUsernameLocal;
            approvalReason = approvalReasonLocal;
            return approved;
        }

        public static bool TrySupervisorApproval(
            IWin32Window owner,
            string actionTitle,
            string? reasonPlaceholder,
            out int approverUserId,
            out string approverUsername,
            out string approvalReason
        )
        {
            return TrySupervisorApproval(owner, actionTitle, reasonPlaceholder, false, out approverUserId, out approverUsername, out approvalReason);
        }

        public static bool IsSupervisorApprovalReasonRequired(string actionCode)
        {
            try
            {
                using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                conn.Open();
                using var exists = new NpgsqlCommand("SELECT (to_regclass('public.supervisor_approval_settings') IS NOT NULL);", conn);
                var exObj = exists.ExecuteScalar();
                bool tableExists = exObj != null && exObj != DBNull.Value && Convert.ToBoolean(exObj);
                if (!tableExists)
                {
                    return string.Equals(actionCode, "delete_item", StringComparison.OrdinalIgnoreCase);
                }

                using var cmd = new NpgsqlCommand(@"
SELECT require_reason
FROM supervisor_approval_settings
WHERE action_code = @c
LIMIT 1
", conn);
                cmd.Parameters.AddWithValue("@c", (actionCode ?? "").Trim());
                var obj = cmd.ExecuteScalar();
                return obj != null && obj != DBNull.Value && Convert.ToBoolean(obj);
            }
            catch
            {
                return string.Equals(actionCode, "delete_item", StringComparison.OrdinalIgnoreCase);
            }
        }

        public static bool ShouldRequireSupervisorApproval(string actionCode, POS_qu.Models.SessionUser session, decimal amount)
        {
            if (session == null) return false;
            if (session.RoleId == 1 || session.RoleId == 3) return false;
            if (session.RoleId != 2) return false;

            try
            {
                using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                conn.Open();
                using var exists = new NpgsqlCommand("SELECT (to_regclass('public.supervisor_approval_settings') IS NOT NULL);", conn);
                var exObj = exists.ExecuteScalar();
                bool tableExists = exObj != null && exObj != DBNull.Value && Convert.ToBoolean(exObj);

                if (!tableExists)
                {
                    if (string.Equals(actionCode, "close_shift", StringComparison.OrdinalIgnoreCase)) return true;
                    if (string.Equals(actionCode, "reprint_receipt", StringComparison.OrdinalIgnoreCase)) return true;
                    if (string.Equals(actionCode, "delete_item", StringComparison.OrdinalIgnoreCase)) return amount >= 100000m;
                    return false;
                }

                using var cmd = new NpgsqlCommand(@"
SELECT is_enabled, require_for_cashier, COALESCE(min_amount,0) AS min_amount
FROM supervisor_approval_settings
WHERE action_code = @c
LIMIT 1
", conn);
                cmd.Parameters.AddWithValue("@c", (actionCode ?? "").Trim());
                using var r = cmd.ExecuteReader();
                if (!r.Read()) return false;

                bool enabled = r["is_enabled"] != DBNull.Value && Convert.ToBoolean(r["is_enabled"]);
                bool requireCashier = r["require_for_cashier"] != DBNull.Value && Convert.ToBoolean(r["require_for_cashier"]);
                decimal minAmount = r["min_amount"] != DBNull.Value ? Convert.ToDecimal(r["min_amount"]) : 0m;

                if (!enabled) return false;
                if (!requireCashier) return false;
                if (minAmount > 0m && amount < minAmount) return false;
                return true;
            }
            catch
            {
                if (string.Equals(actionCode, "close_shift", StringComparison.OrdinalIgnoreCase)) return true;
                if (string.Equals(actionCode, "reprint_receipt", StringComparison.OrdinalIgnoreCase)) return true;
                if (string.Equals(actionCode, "delete_item", StringComparison.OrdinalIgnoreCase)) return amount >= 100000m;
                return false;
            }
        }













    }
}
