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
    public partial class BookForm : Form
    {
        // field and identity
        AddBookForm form;
        public string id;
        public BookForm()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Properties.Settings.Default.lang);
            InitializeComponent();
            Display();
            form = new AddBookForm(this);
        }

        // Connection to sql
        public static SqlConnection GetConnection()
        {
            string sql = "Data Source=DESKTOP-EJ5S83V\\SQLEXPRESS;Initial Catalog=MyLibrary;Integrated Security=True;";
            SqlConnection conn = new SqlConnection(sql);

            return conn;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            form.Clear();
            form.ShowDialog();
        }

        private void BookForm_Load(object sender, EventArgs e)
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
            GVlist.DataSource = tbl;
            conn.Close();
        }

        public void Display()
        {
            SqlConnection conn = GetConnection();
            conn.Open();
            string query = "select ID, Name, Author, Quantity from BookTbl where UserID = @userID";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@userID", UserLoginCache.ID.ToString());
            SqlDataReader reader = command.ExecuteReader();
            DataTable tbl = new DataTable();
            tbl.Load(reader);
            GVlist.DataSource = tbl;
            conn.Close();
        }

        private void GVlist_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                //Edit
                form.Clear();
                form.iD = GVlist.Rows[e.RowIndex].Cells[2].Value.ToString();
                form.name = GVlist.Rows[e.RowIndex].Cells[3].Value.ToString();
                form.author = GVlist.Rows[e.RowIndex].Cells[4].Value.ToString();
                form.quantity = GVlist.Rows[e.RowIndex].Cells[5].Value.ToString();
                form.UpdateInfo();
                form.ShowDialog();
                return;
            }
            if (e.ColumnIndex == 1)
            {
                //Delete
                if (Properties.Settings.Default.lang == "en-US")
                {
                    var result = ActivateMessageBox.Show("Are you sure to delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        form.DeleteBook(GVlist.Rows[e.RowIndex].Cells[2].Value.ToString());
                        Display();
                    }
                }
                else if (Properties.Settings.Default.lang == "fr")
                {
                    var result = ActivateMessageBox.Show("Êtes-vous sûr de supprimer?", "Supprimer", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        form.DeleteBook(GVlist.Rows[e.RowIndex].Cells[2].Value.ToString());
                        Display();
                    }
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SqlConnection conn = GetConnection();
            conn.Open();
            string query = "select ID, Name, Author, Quantity from BookTbl where UserID = @userID and Name like '%" + txtSearch.Text + "%'";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@userID", UserLoginCache.ID.ToString());
            SqlDataReader reader = command.ExecuteReader();
            DataTable tbl = new DataTable();
            tbl.Load(reader);
            GVlist.DataSource = tbl;
            conn.Close();
        }
    }
}
