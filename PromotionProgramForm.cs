using Npgsql;
using POS_qu.Helpers;
using POS_qu.Models;
using System;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class PromotionProgramForm : Form
    {
        private readonly int _promotionId;
        private int _bonusItemId;

        public PromotionProgramForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            _promotionId = 0;

            Hook();
        }

        public PromotionProgramForm(int promotionId)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            _promotionId = promotionId;

            Hook();
        }

        private void Hook()
        {
            Load += PromotionProgramForm_Load;
            btnClose.Click += (s, e) => Close();
            btnSave.Click += btnSave_Click;
            cmbType.SelectedIndexChanged += (s, e) => UpdateTypePanels();
            chkUsePeriod.CheckedChanged += (s, e) => UpdatePeriodState();
            btnPickBonus.Click += btnPickBonus_Click;
        }

        private void PromotionProgramForm_Load(object sender, EventArgs e)
        {
            EnsurePromotionTables();

            cmbType.Items.Clear();
            cmbType.Items.AddRange(new object[] { "DISKON", "PROMO", "CASHBACK" });
            if (cmbType.Items.Count > 0) cmbType.SelectedIndex = 0;

            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new object[] { "aktif", "nonaktif" });
            cmbStatus.SelectedIndex = 0;

            rbDiskonPersen.Checked = true;
            rbCashbackNominal.Checked = true;

            cmbCashbackMethod.Items.Clear();
            cmbCashbackMethod.Items.AddRange(new object[] { "langsung", "saldo" });
            cmbCashbackMethod.SelectedIndex = 0;

            chkUsePeriod.Checked = false;
            dtStart.Value = DateTime.Today;
            dtEnd.Value = DateTime.Today;

            UpdateTypePanels();
            UpdatePeriodState();

            if (_promotionId > 0) LoadPromotion();
        }

        private void EnsurePromotionTables()
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
CREATE TABLE IF NOT EXISTS promotions (
    id BIGSERIAL PRIMARY KEY,
    name VARCHAR(150) NOT NULL,
    promo_type VARCHAR(20) NOT NULL,
    status VARCHAR(20) NOT NULL DEFAULT 'aktif',
    start_date DATE NULL,
    end_date DATE NULL,
    priority INT NOT NULL DEFAULT 0,
    config_json TEXT NOT NULL DEFAULT '{}',
    created_by INT NULL REFERENCES users(id) ON DELETE SET NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NOT NULL DEFAULT NOW()
);
CREATE INDEX IF NOT EXISTS idx_promotions_type_status ON promotions(promo_type, status);
CREATE INDEX IF NOT EXISTS idx_promotions_period ON promotions(start_date, end_date);
CREATE INDEX IF NOT EXISTS idx_promotions_priority ON promotions(priority);
", con);
            cmd.ExecuteNonQuery();
        }

        private void UpdateTypePanels()
        {
            var t = (cmbType.SelectedItem?.ToString() ?? "DISKON").ToUpperInvariant();
            pnlDiskon.Visible = t == "DISKON";
            pnlPromo.Visible = t == "PROMO";
            pnlCashback.Visible = t == "CASHBACK";

            lblTitle.Text = _promotionId > 0 ? "Ubah Program" : "Buat Program";
            Text = lblTitle.Text;
        }

        private void UpdatePeriodState()
        {
            bool use = chkUsePeriod.Checked;
            dtStart.Enabled = use;
            dtEnd.Enabled = use;
        }

        private void btnPickBonus_Click(object sender, EventArgs e)
        {
            using var f = new SearchFormItem("");
            if (f.ShowDialog(this) != DialogResult.OK) return;
            if (f.SelectedItem == null || f.SelectedItem.id <= 0) return;

            _bonusItemId = f.SelectedItem.id;
            txtBonusItem.Text = f.SelectedItem.name ?? "";
        }

        private void LoadPromotion()
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
SELECT id, name, promo_type, status, start_date, end_date, priority, config_json
FROM promotions
WHERE id = @id
LIMIT 1
", con);
            cmd.Parameters.AddWithValue("@id", _promotionId);
            using var r = cmd.ExecuteReader();
            if (!r.Read()) return;

            txtName.Text = r["name"]?.ToString() ?? "";
            string type = (r["promo_type"]?.ToString() ?? "DISKON").ToUpperInvariant();
            string status = (r["status"]?.ToString() ?? "aktif");
            int prio = r["priority"] != DBNull.Value ? Convert.ToInt32(r["priority"]) : 0;
            string cfg = r["config_json"]?.ToString() ?? "{}";

            cmbType.SelectedIndex = Math.Max(0, cmbType.Items.Cast<object>().Select(x => x.ToString()).ToList().FindIndex(x => string.Equals(x, type, StringComparison.OrdinalIgnoreCase)));
            cmbStatus.SelectedIndex = Math.Max(0, cmbStatus.Items.Cast<object>().Select(x => x.ToString()).ToList().FindIndex(x => string.Equals(x, status, StringComparison.OrdinalIgnoreCase)));
            numPriority.Value = prio;

            DateTime? sd = r["start_date"] == DBNull.Value ? null : Convert.ToDateTime(r["start_date"]);
            DateTime? ed = r["end_date"] == DBNull.Value ? null : Convert.ToDateTime(r["end_date"]);
            chkUsePeriod.Checked = sd.HasValue || ed.HasValue;
            if (sd.HasValue) dtStart.Value = sd.Value;
            if (ed.HasValue) dtEnd.Value = ed.Value;

            UpdateTypePanels();
            UpdatePeriodState();

            try
            {
                var doc = JsonDocument.Parse(cfg);
                var root = doc.RootElement;

                if (type == "DISKON")
                {
                    var mode = root.TryGetProperty("mode", out var m) ? (m.GetString() ?? "percent") : "percent";
                    var value = root.TryGetProperty("value", out var v) ? v.GetDecimal() : 0m;
                    rbDiskonPersen.Checked = string.Equals(mode, "percent", StringComparison.OrdinalIgnoreCase);
                    rbDiskonNominal.Checked = !rbDiskonPersen.Checked;
                    numDiskonValue.Value = ClampToNumeric(numDiskonValue, value);
                }
                else if (type == "PROMO")
                {
                    var buy = root.TryGetProperty("buy_qty", out var bq) ? bq.GetInt32() : 1;
                    var get = root.TryGetProperty("get_qty", out var gq) ? gq.GetInt32() : 1;
                    var bonusId = root.TryGetProperty("item_bonus_id", out var bi) ? bi.GetInt32() : 0;
                    var bonusName = root.TryGetProperty("item_bonus_name", out var bn) ? (bn.GetString() ?? "") : "";

                    numBuyQty.Value = Math.Max(numBuyQty.Minimum, Math.Min(numBuyQty.Maximum, buy));
                    numGetQty.Value = Math.Max(numGetQty.Minimum, Math.Min(numGetQty.Maximum, get));
                    _bonusItemId = bonusId;
                    txtBonusItem.Text = bonusName;
                }
                else if (type == "CASHBACK")
                {
                    var mode = root.TryGetProperty("mode", out var m) ? (m.GetString() ?? "nominal") : "nominal";
                    var method = root.TryGetProperty("method", out var mm) ? (mm.GetString() ?? "langsung") : "langsung";
                    var value = root.TryGetProperty("value", out var v) ? v.GetDecimal() : 0m;

                    rbCashbackPersen.Checked = string.Equals(mode, "percent", StringComparison.OrdinalIgnoreCase);
                    rbCashbackNominal.Checked = !rbCashbackPersen.Checked;
                    numCashbackValue.Value = ClampToNumeric(numCashbackValue, value);
                    cmbCashbackMethod.SelectedIndex = Math.Max(0, cmbCashbackMethod.Items.Cast<object>().Select(x => x.ToString()).ToList().FindIndex(x => string.Equals(x, method, StringComparison.OrdinalIgnoreCase)));
                }
            }
            catch
            {
            }
        }

        private static decimal ClampToNumeric(NumericUpDown n, decimal value)
        {
            if (value < n.Minimum) return n.Minimum;
            if (value > n.Maximum) return n.Maximum;
            return value;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var name = (txtName.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Nama Program wajib diisi.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var type = (cmbType.SelectedItem?.ToString() ?? "DISKON").ToUpperInvariant();
            var status = (cmbStatus.SelectedItem?.ToString() ?? "aktif");
            int prio = Convert.ToInt32(numPriority.Value);

            DateTime? sd = chkUsePeriod.Checked ? dtStart.Value.Date : (DateTime?)null;
            DateTime? ed = chkUsePeriod.Checked ? dtEnd.Value.Date : (DateTime?)null;
            if (sd.HasValue && ed.HasValue && ed.Value < sd.Value)
            {
                MessageBox.Show("Tanggal selesai tidak boleh lebih kecil dari tanggal mulai.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string cfgJson = BuildConfigJson(type);

            var user = SessionUser.GetCurrentUser();
            int? createdBy = user?.UserId;

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var tran = con.BeginTransaction();
            try
            {
                if (_promotionId <= 0)
                {
                    using var cmd = new NpgsqlCommand(@"
INSERT INTO promotions (name, promo_type, status, start_date, end_date, priority, config_json, created_by, created_at, updated_at)
VALUES (@name, @type, @status, @sd, @ed, @prio, @cfg, @by, NOW(), NOW())
", con, tran);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@sd", (object?)sd ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ed", (object?)ed ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@prio", prio);
                    cmd.Parameters.AddWithValue("@cfg", cfgJson);
                    cmd.Parameters.AddWithValue("@by", (object?)createdBy ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    using var cmd = new NpgsqlCommand(@"
UPDATE promotions
SET name = @name,
    promo_type = @type,
    status = @status,
    start_date = @sd,
    end_date = @ed,
    priority = @prio,
    config_json = @cfg,
    updated_at = NOW()
WHERE id = @id
", con, tran);
                    cmd.Parameters.AddWithValue("@id", _promotionId);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@sd", (object?)sd ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ed", (object?)ed ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@prio", prio);
                    cmd.Parameters.AddWithValue("@cfg", cfgJson);
                    cmd.ExecuteNonQuery();
                }

                tran.Commit();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                try { tran.Rollback(); } catch { }
                MessageBox.Show("Gagal simpan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string BuildConfigJson(string type)
        {
            if (type == "DISKON")
            {
                var mode = rbDiskonPersen.Checked ? "percent" : "nominal";
                var value = Convert.ToDecimal(numDiskonValue.Value);
                return JsonSerializer.Serialize(new { mode, value });
            }

            if (type == "PROMO")
            {
                int buyQty = Convert.ToInt32(numBuyQty.Value);
                int getQty = Convert.ToInt32(numGetQty.Value);
                if (_bonusItemId <= 0)
                    throw new InvalidOperationException("Item bonus wajib dipilih.");
                return JsonSerializer.Serialize(new
                {
                    buy_qty = buyQty,
                    get_qty = getQty,
                    item_bonus_id = _bonusItemId,
                    item_bonus_name = txtBonusItem.Text ?? ""
                });
            }

            if (type == "CASHBACK")
            {
                var mode = rbCashbackPersen.Checked ? "percent" : "nominal";
                var method = (cmbCashbackMethod.SelectedItem?.ToString() ?? "langsung");
                var value = Convert.ToDecimal(numCashbackValue.Value);
                return JsonSerializer.Serialize(new { mode, method, value });
            }

            return "{}";
        }
    }
}

