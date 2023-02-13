using System;
using System.Collections;
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

namespace LibMS.Forms
{
    public partial class DeleteAllForm : Form
    {
        public DeleteAllForm()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Properties.Settings.Default.lang);
            InitializeComponent();
        }

        // connection to sql
        public static SqlConnection GetConnection()
        {
            string sql = "Data Source=DESKTOP-EJ5S83V\\SQLEXPRESS;Initial Catalog=MyLibrary;Integrated Security=True;";
            SqlConnection conn = new SqlConnection(sql);

            return conn;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Drag form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void DeleteAllForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.lang == "en-US")
            {
                var result = ActivateMessageBox.Show("Are you sure to delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DeleteAllBooks();
                }
            }
            else if (Properties.Settings.Default.lang == "fr")
            {
                var result = ActivateMessageBox.Show("Êtes-vous sûr de supprimer?", "Supprimer", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DeleteAllBooks();
                }
            }
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.lang == "en-US")
            {
                var result = ActivateMessageBox.Show("Are you sure to delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DeleteAllIssues();
                }
            }
            else if (Properties.Settings.Default.lang == "fr")
            {
                var result = ActivateMessageBox.Show("Êtes-vous sûr de supprimer?", "Supprimer", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DeleteAllIssues();
                }
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.lang == "en-US")
            {
                var result = ActivateMessageBox.Show("Are you sure to delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DeleteAllReturn();
                }
            }
            else if (Properties.Settings.Default.lang == "fr")
            {
                var result = ActivateMessageBox.Show("Êtes-vous sûr de supprimer?", "Supprimer", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DeleteAllReturn();
                }
            }
        }

        // Delete operations
        private void DeleteAllBooks()
        {
            string sql = "Delete from BookTbl";
            SqlConnection conn = GetConnection();
            conn.Open();
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            if (Properties.Settings.Default.lang == "en-US")
            {
                var result = ActivateMessageBox.Show("Deleted Succesfully. \n", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (Properties.Settings.Default.lang == "fr")
            {
                var result = ActivateMessageBox.Show("Suprimé avec Succès. \n", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            conn.Close();
        }

        private void DeleteAllIssues()
        {
            string sql = "Delete from IssueTbl";
            SqlConnection conn = GetConnection();
            conn.Open();
            SqlCommand command1 = new SqlCommand(sql, conn);
            command1.ExecuteNonQuery();
            if (Properties.Settings.Default.lang == "en-US")
            {
                var result = ActivateMessageBox.Show("Deleted Succesfully. \n", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (Properties.Settings.Default.lang == "fr")
            {
                var result = ActivateMessageBox.Show("Suprimé avec Succès. \n", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            conn.Close();
        }

        private void DeleteAllReturn()
        {
            string sql = "Delete from ReturnTbl";
            SqlConnection conn = GetConnection();
            conn.Open();
            SqlCommand command2 = new SqlCommand(sql, conn);
            command2.ExecuteNonQuery();
            if (Properties.Settings.Default.lang == "en-US")
            {
                var result = ActivateMessageBox.Show("Deleted Succesfully. \n", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (Properties.Settings.Default.lang == "fr")
            {
                var result = ActivateMessageBox.Show("Suprimé avec Succès. \n", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            conn.Close();
        }
    }
}
