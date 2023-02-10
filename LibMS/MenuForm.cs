using Guna.UI2.WinForms;
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
    public partial class MenuForm : Form
    {
        // Fields
        private Button currentButton;
        private Guna2Panel leftBorderBtn;
        private Form currentForm;

        // Constructor
        public MenuForm()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Properties.Settings.Default.lang);
            InitializeComponent();
            leftBorderBtn = new Guna2Panel();
            leftBorderBtn.Size = new Size(7, 50);
            panelMenu.Controls.Add(leftBorderBtn);
            //Form
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
        }

        // Structs
        private struct RGBColors
        {
            public static Color color1 = Color.FromArgb(172, 126, 241);
            public static Color color2 = Color.FromArgb(249, 118, 176);
            public static Color color3 = Color.FromArgb(253, 138, 114);
            public static Color color4 = Color.FromArgb(95, 77, 221);
            public static Color color5 = Color.FromArgb(249, 88, 155);
            public static Color color6 = Color.FromArgb(24, 161, 251);
        }

        // Methods
        private void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                DisableButton();
                //Button
                currentButton = (Button)senderBtn;
                currentButton.BackColor = Color.FromArgb(37, 36, 81);
                currentButton.ForeColor = color;
                currentButton.TextAlign = ContentAlignment.MiddleCenter;
                currentButton.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentButton.ImageAlign = ContentAlignment.MiddleRight;
                //Left Border Button
                leftBorderBtn.BackColor = color;
                leftBorderBtn.Location = new Point(0, currentButton.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();
                //Icon current child form
                iconCurrentChildForm.Image = currentButton.Image;
            }
        }

        private void DisableButton()
        {
            if (currentButton != null)
            {
                currentButton.BackColor = Color.FromArgb(23, 35, 49);
                currentButton.ForeColor = Color.Silver;
                currentButton.TextAlign = ContentAlignment.MiddleLeft;
                currentButton.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentButton.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        private void OpenChildForm(Form childForm)
        {
            if (currentForm != null)
            {
                //Open only form
                currentForm.Close();
            }
            currentForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelDesktop.Controls.Add(childForm);
            panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            lblTitleChildForm.Text = childForm.Text;
        }

        private void Reset()
        {
            DisableButton();
            leftBorderBtn.Visible = false;
            iconCurrentChildForm.Image = Properties.Resources.icons8_home_32;
            if (Properties.Settings.Default.lang == "en-US")
            {
                lblTitleChildForm.Text = "Home";
            }
            else if (Properties.Settings.Default.lang == "fr")
            {
                lblTitleChildForm.Text = "Accueil";
            }
        }

        // Drag form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.lang == "en-US")
            {
                lblTime.Text = DateTime.Now.ToLongTimeString();
                lblDate.Text = DateTime.Now.ToLongDateString();
            }
            else if (Properties.Settings.Default.lang == "fr")
            {
                lblTime.Text = DateTime.Now.ToString("HH : mm : ss");
                lblDate.Text = DateTime.Now.ToLongDateString();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.lang == "en-US")
            {
                var result = ActivateMessageBox.Show("Are you sure to exit the application?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
            else
            {
                var result = ActivateMessageBox.Show("Êtes-vous sûr de quitter l'application?", "Avertissement", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
        }

        private void btnMax_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            currentForm.Hide();
            Reset();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            OpenChildForm(new Forms.DashboardForm());
        }

        private void btnBooks_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color2);
            OpenChildForm(new Forms.BookForm());
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color3);
            //OpenChildForm(new Forms.BookIssueForm());
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color4);
            //OpenChildForm(new Forms.BookReturnedForm());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color5);
            if (Properties.Settings.Default.lang == "en-US")
            {
                var result = ActivateMessageBox.Show("Are you sure to logout?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    this.Hide();
                    new LoginForm().Show();
                }
            }
            else if (Properties.Settings.Default.lang == "fr")
            {
                var result = ActivateMessageBox.Show("Êtes-vous sûr de vous déconnecter?", "Avertissement", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    this.Hide();
                    new LoginForm().Show();
                }
            }
        }
    }
}
