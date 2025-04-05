using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Helpers
{
    class DataGridViewManager
    {
        private DataGridView dataGridView;
        private DataTable originalData;
        private int pageSize = 10;
        private int currentPage = 1;
        private int totalPages = 1;

        public int TotalRecords { get; private set; }
        public Label PagingInfoLabel { get; set; } // Optional label for paging info

        public string SearchText { get; set; }
        public string SearchColumn { get; set; }

        public DataGridViewManager(DataGridView dgv, DataTable data, int pageSize = 10)
        {
            this.dataGridView = dgv;
            this.originalData = data.Copy();
            this.pageSize = pageSize;
            CalculateTotalPages();
            LoadPage();
        }

        private void CalculateTotalPages()
        {
            TotalRecords = originalData.Rows.Count;
            totalPages = (int)Math.Ceiling((double)TotalRecords / pageSize);
        }

        public void LoadPage()
        {
            IEnumerable<DataRow> filteredRows = originalData.AsEnumerable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(SearchText) && !string.IsNullOrWhiteSpace(SearchColumn))
            {
                filteredRows = filteredRows.Where(row => row[SearchColumn]?.ToString().IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            TotalRecords = filteredRows.Count();
            totalPages = (int)Math.Ceiling((double)TotalRecords / pageSize);

            var pagedRows = filteredRows
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize);

            dataGridView.DataSource = pagedRows.Any() ? pagedRows.CopyToDataTable() : originalData.Clone();
            UpdatePagingInfo();
        }

        public void NextPage()
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadPage();
            }
        }

        public void PreviousPage()
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadPage();
            }
        }

        public void FirstPage()
        {
            currentPage = 1;
            LoadPage();
        }

        public void LastPage()
        {
            currentPage = totalPages;
            LoadPage();
        }

        public void Filter(string searchTerm, string columnName)
        {
            SearchText = searchTerm;
            SearchColumn = columnName;
            currentPage = 1;
            LoadPage();
        }

        public void Reset(DataTable data)
        {
            originalData = data.Copy();
            currentPage = 1;
            SearchText = null;
            SearchColumn = null;
            CalculateTotalPages();
            LoadPage();
        }

        public void ToggleColumnVisibility(string columnName, bool visible)
        {
            if (dataGridView.Columns.Contains(columnName))
            {
                dataGridView.Columns[columnName].Visible = visible;
            }
        }

        private void UpdatePagingInfo()
        {
            if (PagingInfoLabel != null)
            {
                int start = (currentPage - 1) * pageSize + 1;
                int end = Math.Min(currentPage * pageSize, TotalRecords);
                PagingInfoLabel.Text = $"Showing {start}–{end} of {TotalRecords}";
            }
        }
        public void SetPageSize(int newPageSize)
        {
            pageSize = newPageSize;
            currentPage = 1;
            LoadPage();
        }
    }
}
