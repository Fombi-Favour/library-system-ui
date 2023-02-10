using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibMS
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Properties.Settings.Default.lang);
            InitializeComponent();
        }

        // Drag method
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private static extern void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void LoginForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void linkLblReg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            new RegisterForm().ShowDialog();
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnmin_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            if (txtuser.Text != "")
            {
                if (txtpass.Text != "")
                {
                    UserModel user = new UserModel();
                    var validLogin = user.LoginUser(txtuser.Text, txtpass.Text);
                    if (validLogin == true)
                    {
                        WelcomeForm welcomeForm = new WelcomeForm();
                        welcomeForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        if (Properties.Settings.Default.lang == "en-US")
                        {
                            var result = ActivateMessageBox.Show("Incorrect username or password entered.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                            if (result == DialogResult.Retry) { Clear(); }
                        }
                        else if (Properties.Settings.Default.lang == "fr")
                        {
                            var result = ActivateMessageBox.Show("Nom d'utilisateur ou mot de passe \n incorrect entré", "Erreur", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                            if (result == DialogResult.Retry) { Clear(); }
                        }
                    }
                }
                else
                {
                    if (Properties.Settings.Default.lang == "en-US")
                    {
                        var result = ActivateMessageBox.Show("Password incorrect!!!", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    }
                    else if (Properties.Settings.Default.lang == "fr")
                    {
                        var result = ActivateMessageBox.Show("Mot de passe incorrect!!!", "Erreur", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    }

                }
            }
            else
            {
                if (Properties.Settings.Default.lang == "en-US")
                {
                    var result = ActivateMessageBox.Show("Username incorrect!!!", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                }
                else if (Properties.Settings.Default.lang == "fr")
                {
                    var result = ActivateMessageBox.Show("Nom d'utilisateur incorrect!!!", "Erreur", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                }
            }
        }

        // Clear method
        private void Clear()
        {
            txtuser.Text = txtpass.Text = string.Empty;
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            if (txtpass.PasswordChar == '\0')
            {
                btnShow.BringToFront();
                txtpass.PasswordChar = '*';
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
    }
}
