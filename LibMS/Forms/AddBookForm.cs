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
using System.Windows.Forms;

namespace LibMS.Forms
{
    public partial class AddBookForm : Form
    {
        //Fields
        private readonly BookForm _parent;
        public string iD, name, author, quantity;

        public AddBookForm(BookForm parent)
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Properties.Settings.Default.lang);
            InitializeComponent();
            GetUserID();
            this._parent = parent;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtBkid.Text.Trim().Length < 3)
            {
                if (Properties.Settings.Default.lang == "en-US")
                {
                    DialogResult result = ActivateMessageBox.Show("BookID is empty.");
                }
                else if (Properties.Settings.Default.lang == "fr")
                {
                    DialogResult result = ActivateMessageBox.Show("LivreID est vide.");
                }
                return;
            }
            if (txtBkname.Text.Trim().Length < 1)
            {
                if (Properties.Settings.Default.lang == "en-US")
                {
                    DialogResult result = ActivateMessageBox.Show("Book name is empty.");
                }
                else if (Properties.Settings.Default.lang == "fr")
                {
                    DialogResult result = ActivateMessageBox.Show("Nom du livre est vide.");
                }
                return;
            }
            if (txtBkauthor.Text.Trim().Length == 0)
            {
                if (Properties.Settings.Default.lang == "en-US")
                {
                    DialogResult result = ActivateMessageBox.Show("Book author is empty.");
                }
                else if (Properties.Settings.Default.lang == "fr")
                {
                    DialogResult result = ActivateMessageBox.Show("L'auteur du livre est vide.");
                }
                return;
            }
            if (txtQuantity.Text.Trim().Length == 0)
            {
                if (Properties.Settings.Default.lang == "en-US")
                {
                    DialogResult result = ActivateMessageBox.Show("Book Quantity is empty");
                }
                else if (Properties.Settings.Default.lang == "fr")
                {
                    DialogResult result = ActivateMessageBox.Show("La quantité de livre est vide");
                }
                return;
            }
            if (btnSave.Text == "SAVE" || btnSave.Text == "ENREGISTRER")
            {
                AddBook();
                Clear();
            }
            if (btnSave.Text == "UPDATE" || btnSave.Text == "MISE À JOUR")
            {
                UpdateBook(iD);
            }
            _parent.Display();
        }

        // Drag form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void panelTtleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        // Methods
        public void UpdateInfo()
        {
            if (Properties.Settings.Default.lang == "en-US")
            {
                lblText.Text = "Update Book";
                btnSave.Text = "UPDATE";
                txtBkid.Text = iD;
                txtBkname.Text = name;
                txtBkauthor.Text = author;
                txtQuantity.Text = quantity;
            }
            else if (Properties.Settings.Default.lang == "fr")
            {
                lblText.Text = "Livre de Mise à Jour";
                btnSave.Text = "MISE À JOUR";
                txtBkid.Text = iD;
                txtBkname.Text = name;
                txtBkauthor.Text = author;
                txtQuantity.Text = quantity;
            }
        }

        public static SqlConnection GetConnection()
        {
            string sql = "Data Source=DESKTOP-EJ5S83V\\SQLEXPRESS;Initial Catalog=MyLibrary;Integrated Security=True;";
            SqlConnection conn = new SqlConnection(sql);

            return conn;
        }

        private void GetUserID()
        {
            txtUserId.Text = UserLoginCache.ID.ToString();
        }

        public void Clear()
        {
            txtBkid.Text = txtBkname.Text = txtBkauthor.Text = txtQuantity.Text = string.Empty;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // CRUD methods
        //Insert, Update and Delete Operations
        public void AddBook()
        {
            string sql = "insert into BookTbl values(@ID, @userID, @name, @author, @quantity)";
            SqlConnection conn = GetConnection();
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ID", txtBkid.Text);
            cmd.Parameters.AddWithValue("userID", txtUserId.Text.ToString());
            cmd.Parameters.AddWithValue("@name", txtBkname.Text);
            cmd.Parameters.AddWithValue("author", txtBkauthor.Text);
            cmd.Parameters.AddWithValue("@quantity", txtQuantity.Text);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                if (Properties.Settings.Default.lang == "en-US")
                {
                    var result = ActivateMessageBox.Show("Added Succesfully. \n", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (Properties.Settings.Default.lang == "fr")
                {
                    var result = ActivateMessageBox.Show("Ajouté avec Succès. \n", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (SqlException ex)
            {
                if (Properties.Settings.Default.lang == "en-US")
                {
                    var result = ActivateMessageBox.Show("Book not insert. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (Properties.Settings.Default.lang == "fr")
                {
                    var result = ActivateMessageBox.Show("Livre pas insérer. \n" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            conn.Close();
        }

        public void UpdateBook(string id)
        {
            string sql = "Update BookTbl set Name = @name, Author = @author, Quantity = @quantity where ID = @iD";
            SqlConnection conn = GetConnection();
            SqlCommand command = new SqlCommand(sql, conn);
            command.Parameters.AddWithValue("@name", txtBkname.Text);
            command.Parameters.AddWithValue("@author", txtBkauthor.Text);
            command.Parameters.AddWithValue("@quantity", txtQuantity.Text);
            command.Parameters.AddWithValue("@iD", id);
            try
            {
                conn.Open();
                command.ExecuteNonQuery();
                if (Properties.Settings.Default.lang == "en-US")
                {
                    var result = ActivateMessageBox.Show("Updated Succesfully. \n", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (Properties.Settings.Default.lang == "fr")
                {
                    var result = ActivateMessageBox.Show("Mise à Jour avec Succès. \n", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                if (Properties.Settings.Default.lang == "en-US")
                {
                    var result = ActivateMessageBox.Show("Book not update. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (Properties.Settings.Default.lang == "fr")
                {
                    var result = ActivateMessageBox.Show("Livre non mise à jour. \n" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void DeleteBook(string id)
        {
            string sql = "Delete from BookTbl where ID = @iD";
            SqlConnection conn = GetConnection();
            SqlCommand command = new SqlCommand(sql, conn);
            command.Parameters.AddWithValue("@iD", id);
            try
            {
                conn.Open();
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
            catch (Exception ex)
            {
                if (Properties.Settings.Default.lang == "en-US")
                {
                    var result = ActivateMessageBox.Show("Book not delete. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (Properties.Settings.Default.lang == "fr")
                {
                    var result = ActivateMessageBox.Show("Livre pas supprimer. \n" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
