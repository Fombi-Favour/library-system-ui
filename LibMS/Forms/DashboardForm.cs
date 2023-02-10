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
    public partial class DashboardForm : Form
    {
        public DashboardForm()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Properties.Settings.Default.lang);
            InitializeComponent();
        }

        public static SqlConnection GetConnection()
        {
            string sql = "Data Source=DESKTOP-EJ5S83V\\SQLEXPRESS;Initial Catalog=MyLibrary;Integrated Security=True;";
            SqlConnection conn = new SqlConnection(sql);

            return conn;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDate.Text = DateTime.Now.ToLongDateString(); 
        }

        private void btnAbt_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        // userData method
        private void LoadUserData()
        {
            lblName.Text = UserLoginCache.userName;
            lblSchool.Text = UserLoginCache.School;
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            LoadUserData();

            SqlConnection conn = GetConnection();

            conn.Open();
            // Counting Books
            SqlCommand cmd = new SqlCommand("select count(*) from BookTbl where UserID = @userID", conn);
            cmd.Parameters.AddWithValue("@userID", UserLoginCache.ID.ToString());
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            lblBooks.Text = dt.Rows[0][0].ToString();
            // Counting Students
            SqlCommand cmd1 = new SqlCommand("select count(StudName) from IssueTbl where UserID = @userID", conn);
            cmd1.Parameters.AddWithValue("@userID", UserLoginCache.ID.ToString());
            SqlDataAdapter adp1 = new SqlDataAdapter(cmd1);
            DataTable dt1 = new DataTable();
            adp1.Fill(dt1);
            lblStudents.Text = dt1.Rows[0][0].ToString();
            // Counting Issue Books
            SqlCommand cmd2 = new SqlCommand("select count(*) from IssueTbl where UserID = @userID", conn);
            cmd2.Parameters.AddWithValue("@userID", UserLoginCache.ID.ToString());
            SqlDataAdapter adp2 = new SqlDataAdapter(cmd2);
            DataTable dt2 = new DataTable();
            adp2.Fill(dt2);
            lblIssue.Text = dt2.Rows[0][0].ToString();
            // Counting Return Books
            SqlCommand cmd3 = new SqlCommand("select count(*) from ReturnTbl where UserID = @userID", conn);
            cmd3.Parameters.AddWithValue("@userID", UserLoginCache.ID.ToString());
            SqlDataAdapter adp3 = new SqlDataAdapter(cmd3);
            DataTable dt3 = new DataTable();
            adp3.Fill(dt3);
            lblreturn.Text = dt3.Rows[0][0].ToString();
            conn.Close();
        }
    }
}
