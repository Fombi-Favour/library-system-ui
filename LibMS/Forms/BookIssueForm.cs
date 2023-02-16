using Common.Cache;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibMS.Forms
{
    public partial class BookIssueForm : Form
    {
        AddIssueForm form;
        public BookIssueForm()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Properties.Settings.Default.lang);
            InitializeComponent();
            Display();
            form = new AddIssueForm(this);
        }

        // connection to sql
        public static SqlConnection GetConnection()
        {
            string sql = "Data Source=DESKTOP-EJ5S83V\\SQLEXPRESS;Initial Catalog=LibraryDB;Integrated Security=True";
            SqlConnection conn = new SqlConnection(sql);

            return conn;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            form.Clear();
            form.ShowDialog();
        }

        private void BookIssueForm_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.lang == "en-US")
            {
                txtSearch.PlaceholderText = "Search";
            }
            else if (Properties.Settings.Default.lang == "fr")
            {
                txtSearch.PlaceholderText = "Chercher";
            }
        }

        // Methods
        public void ShowAndSearchBook(string query)
        {
            SqlConnection conn = GetConnection();

            string sql = query;
            conn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            DataTable tbl = new DataTable();
            adapter.Fill(tbl);
            GVlist1.DataSource = tbl;
            conn.Close();
        }

        public void Display()
        {
            SqlConnection conn = GetConnection();
            conn.Open();
            string query = "Select Matricule, StudName, StudClass, BookID, BookName, IssueDate from IssueTbl where UserID = @userID";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@userID", UserLoginCache.ID.ToString());
            SqlDataReader reader = command.ExecuteReader();
            DataTable tbl = new DataTable();
            tbl.Load(reader);
            GVlist1.DataSource = tbl;
            conn.Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SqlConnection conn = GetConnection();
            conn.Open();
            string query = "Select Matricule, StudName, StudClass, BookID, BookName, IssueDate from IssueTbl where UserID = @userID and StudName like '%" + txtSearch.Text + "%'";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@userID", UserLoginCache.ID.ToString());
            SqlDataReader reader = command.ExecuteReader();
            DataTable tbl = new DataTable();
            tbl.Load(reader);
            GVlist1.DataSource = tbl;
            conn.Close();
        }

        private void GVlist1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                // Edit
                form.Clear();
                form.iD = GVlist1.Rows[e.RowIndex].Cells[2].Value.ToString();
                form.studName = GVlist1.Rows[e.RowIndex].Cells[3].Value.ToString();
                form.studClass = GVlist1.Rows[e.RowIndex].Cells[4].Value.ToString();
                form.bookID = GVlist1.Rows[e.RowIndex].Cells[5].Value.ToString();
                form.bookName = GVlist1.Rows[e.RowIndex].Cells[6].Value.ToString();
                form.issueDate = GVlist1.Rows[e.RowIndex].Cells[7].Value.ToString();
                form.UpdateInfo();
                form.ShowDialog();
                return;
            }
            if (e.ColumnIndex == 1)
            {
                // Delete
                if (Properties.Settings.Default.lang == "en-US")
                {
                    var result = ActivateMessageBox.Show("Are you sure to delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        form.UpdateQuantityCancelled();
                        form.DeleteIssue(GVlist1.Rows[e.RowIndex].Cells[2].Value.ToString());
                        Display();
                    }
                }
                else if (Properties.Settings.Default.lang == "fr")
                {
                    var result = ActivateMessageBox.Show("Êtes-vous sûr de supprimer?", "Supprimer", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        form.DeleteIssue(GVlist1.Rows[e.RowIndex].Cells[2].Value.ToString());
                        form.UpdateQuantityCancelled();
                        Display();
                    }
                }
                return;
            }
        }
    }
}
