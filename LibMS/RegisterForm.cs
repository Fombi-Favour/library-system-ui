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
using System.Windows.Forms;

namespace LibMS
{
    public partial class RegisterForm : Form
    {
        // Sql connection definition (manual work)
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-EJ5S83V\SQLEXPRESS;Initial Catalog=LibraryDB;Integrated Security=True");
        SqlCommand cmd;
        public RegisterForm()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Properties.Settings.Default.lang);
            InitializeComponent();
        }

        // Drag method
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private static extern void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void RegisterForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void linkLblLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            new LoginForm().ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimise_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (txtuser.Text == "" || txtpass.Text == "" || txtsch.Text == "")
            {
                if (Properties.Settings.Default.lang == "en-US")
                {
                    var result = ActivateMessageBox.Show("Fill in the information", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (Properties.Settings.Default.lang == "fr")
                {
                    var result = ActivateMessageBox.Show("Remplir des informations", "Manquant", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                conn.Open();
                cmd = new SqlCommand("insert into UserTbl values('" + txtuser.Text + "', '" + txtpass.Text + "', '" + txtsch.Text + "')", conn);
                cmd.ExecuteNonQuery();
                new LoginForm().Show();
                this.Hide();
                conn.Close();
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (txtpass.PasswordChar == '*')
            {
                btnHide.BringToFront();
                txtpass.PasswordChar = '\0';
            }
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            if (txtpass.PasswordChar == '\0')
            {
                btnShow.BringToFront();
                txtpass.PasswordChar = '*';
            }
        }
    }
}
