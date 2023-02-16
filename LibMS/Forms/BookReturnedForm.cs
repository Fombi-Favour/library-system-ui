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
    public partial class BookReturnedForm : Form
    {
        AddReturnForm form;
        public BookReturnedForm()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Properties.Settings.Default.lang);
            InitializeComponent();
            ShowIssue();
            DisplayReturn();
            form = new AddReturnForm(this);
        }

        // connection to sql
        public static SqlConnection GetConnection()
        {
            string sql = "Data Source=DESKTOP-EJ5S83V\\SQLEXPRESS;Initial Catalog=LibraryDB;Integrated Security=True";
            SqlConnection conn = new SqlConnection(sql);

            return conn;
        }

        private void BookReturnedForm_Load(object sender, EventArgs e)
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
        public void ShowIssue()
        {
            SqlConnection conn = GetConnection();
            conn.Open();
            string query = "Select Matricule, StudName, StudClass, BookID, BookName, IssueDate from IssueTbl where UserID = @userID";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@userID", UserLoginCache.ID.ToString());
            SqlDataReader reader = command.ExecuteReader();
            DataTable tbl = new DataTable();
            tbl.Load(reader);
            GVlist3.DataSource = tbl;
            conn.Close();
        }

        public void DisplayReturn()
        {
            SqlConnection conn = GetConnection();
            conn.Open();
            string query = "Select Matricule, StudName, StudClass, BookID, BookName, IssueDate, ReturnDate from ReturnTbl where UserID = @userID";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@userID", UserLoginCache.ID.ToString());
            SqlDataReader reader = command.ExecuteReader();
            DataTable tbl = new DataTable();
            tbl.Load(reader);
            GVlist2.DataSource = tbl;
            conn.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            new SelectTable().ShowDialog();
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
            GVlist3.DataSource = tbl;
            conn.Close();
        }

        private void GVlist3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            form.iD = GVlist3.Rows[e.RowIndex].Cells[0].Value.ToString();
            form.studName = GVlist3.Rows[e.RowIndex].Cells[1].Value.ToString();
            form.studClass = GVlist3.Rows[e.RowIndex].Cells[2].Value.ToString();
            form.bookID = GVlist3.Rows[e.RowIndex].Cells[3].Value.ToString();
            form.bookName = GVlist3.Rows[e.RowIndex].Cells[4].Value.ToString();
            form.issueDate = GVlist3.Rows[e.RowIndex].Cells[5].Value.ToString();
            form.ReturnInfo();
            form.ShowDialog();
        }
    }
}
