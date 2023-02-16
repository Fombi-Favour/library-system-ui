using Common.Cache;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace LibMS.Forms
{
    public partial class PrintReturn : Form
    {
        public PrintReturn()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Properties.Settings.Default.lang);
            InitializeComponent();
        }

        public static SqlConnection GetConnection()
        {
            string sql = "Data Source=DESKTOP-EJ5S83V\\SQLEXPRESS;Initial Catalog=LibraryDB;Integrated Security=True";
            SqlConnection conn = new SqlConnection(sql);

            return conn;
        }

        // Display method
        private void Display()
        {
            SqlConnection conn = GetConnection();
            conn.Open();
            string query = "select ID, Name, Author, Quantity from BookTbl where UserID = @userID";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@userID", UserLoginCache.ID.ToString());
            SqlDataReader reader = command.ExecuteReader();
            DataTable tbl = new DataTable();
            tbl.Load(reader);
            List2.DataSource = tbl;
            conn.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDate.Text = DateTime.Now.ToLongDateString();
        }

        private void PrintReturn_Load(object sender, EventArgs e)
        {
            lblSchool.Text = UserLoginCache.School;
        }

        // Drag
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private static extern void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void PrintReturn_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnPrint_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(btnPrint, "Print Document");
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bitmap, 0, 0);
        }

        Bitmap bitmap;

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Panel panel = new Panel();
            panelPrint = panel;
            this.Controls.Add(panel);

            Graphics graphics = panel.CreateGraphics();
            Size size = this.ClientSize;
            bitmap = new Bitmap(size.Width, size.Height, graphics);
            graphics = Graphics.FromImage(bitmap);

            Point point = PointToScreen(panel.Location);
            graphics.CopyFromScreen(point.X, point.Y, 10, 10, size);

            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }
    }
}
