using Common.Cache;
using Guna.UI2.WinForms;
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
    public partial class AddReturnForm : Form
    {
        // Fields
        private readonly BookReturnedForm _parent;
        public string iD, studName, studClass, bookID, bookName, issueDate;

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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

        public AddReturnForm(BookReturnedForm parent)
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Properties.Settings.Default.lang);
            InitializeComponent();
            GetUserID();
            GetBookID(BookidCb);
            _parent = parent;
        }

        private void BookidCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetBookName();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            AddReturn();
            UpdateBookQuantity();
            _parent.DisplayReturn();
            this.Close();
        }

        // Methods
        public void ReturnInfo()
        {
            txtStudName.Text = studName;
            txtClass.Text = studClass;
            txtMatricule.Text = iD;
            BookidCb.SelectedValue = bookID;
            txtBkname.Text = bookName;
            DPdate.Text = issueDate;
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

        public static void GetBookID(Guna2ComboBox cb)
        {
            SqlConnection conn = GetConnection();

            conn.Open();
            SqlCommand command = new SqlCommand("Select ID from BookTbl where UserID = @userID", conn);
            command.Parameters.AddWithValue("@userID", UserLoginCache.ID.ToString());
            SqlDataReader reader = command.ExecuteReader();
            DataTable tbl = new DataTable();
            tbl.Columns.Add("ID", typeof(string));
            tbl.Load(reader);
            cb.ValueMember = "ID";
            cb.DataSource = tbl;
            conn.Close();
        }

        public void GetBookName()
        {
            SqlConnection conn = GetConnection();
            conn.Open();
            string query = "Select * from BookTbl where ID = @ID and UserID = @userID";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@userID", UserLoginCache.ID.ToString());
            command.Parameters.Add("@ID", SqlDbType.NVarChar);
            command.Parameters["@ID"].Value = BookidCb.SelectedValue;
            command.CommandType = CommandType.Text;
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    txtBkname.Text = reader.GetString(2);
                }
            }
            conn.Close();
        }

        private void UpdateBookQuantity()
        {
            int Qty, newQty;
            SqlConnection conn = GetConnection();
            conn.Open();
            string query = "select * from BookTbl where ID = @id and UserID = @userID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", BookidCb.SelectedValue.ToString());
            cmd.Parameters.AddWithValue("@userID", txtUserId.Text);
            DataTable tbl = new DataTable();
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            adp.Fill(tbl);
            foreach (DataRow dr in tbl.Rows)
            {
                Qty = Convert.ToInt32(dr["Quantity"].ToString());
                newQty = Qty + 1;
                string sql = "update BookTbl set Quantity = @quantity where ID = @id and UserID = @userID";
                SqlCommand cmd1 = new SqlCommand(sql, conn);
                cmd1.Parameters.AddWithValue("@quantity", newQty);
                cmd1.Parameters.AddWithValue("@id", BookidCb.SelectedValue.ToString());
                cmd1.Parameters.AddWithValue("@userID", txtUserId.Text);
                cmd1.ExecuteNonQuery();
            }
            conn.Close();
        }

        // CRUD methods (only Insert and Delete operations)
        public void AddReturn()
        {
            SqlConnection conn = GetConnection();
            conn.Open();
            string query = "Insert into ReturnTbl values (@matricule, @studName, @studClass, @userID, @bookID, @bookName, @issueDate, @ReturnDate)";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@matricule", txtMatricule.Text);
            command.Parameters.AddWithValue("@studName", txtStudName.Text);
            command.Parameters.AddWithValue("@studClass", txtClass.Text);
            command.Parameters.AddWithValue("@userID", txtUserId.Text);
            command.Parameters.AddWithValue("@bookID", BookidCb.SelectedValue.ToString());
            command.Parameters.AddWithValue("@bookName", txtBkname.Text);
            command.Parameters.AddWithValue("@issueDate", DPdate.Value.Date);
            command.Parameters.AddWithValue("@returnDate", DPdate1.Value.Date);
            try
            {
                command.ExecuteNonQuery();
                if (Properties.Settings.Default.lang == "en-US")
                {
                    var result = ActivateMessageBox.Show("Added Succesfully. \n", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (Properties.Settings.Default.lang == "fr")
                {
                    var result = ActivateMessageBox.Show("Ajouté avec Succès. \n", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                if (Properties.Settings.Default.lang == "en-US")
                {
                    var result = ActivateMessageBox.Show("Book return not insert. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (Properties.Settings.Default.lang == "fr")
                {
                    var result = ActivateMessageBox.Show("Retour de livre pas insérer. \n" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            conn.Close();
        }

        public void DeleteReturn(string id)
        {
            SqlConnection conn = GetConnection();
            conn.Open();
            string query = "Delete ReturnTbl where Matricule = @matricule";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@matricule", id);
            try
            {
                command.ExecuteNonQuery();
                if (Properties.Settings.Default.lang == "en-US")
                {
                    var result = ActivateMessageBox.Show("Deleted Succesfully. \n", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (Properties.Settings.Default.lang == "fr")
                {
                    var result = ActivateMessageBox.Show("Suprimé avec Succès. \n", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                if (Properties.Settings.Default.lang == "en-US")
                {
                    var result = ActivateMessageBox.Show("Book return not delete. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (Properties.Settings.Default.lang == "fr")
                {
                    var result = ActivateMessageBox.Show("Retour de livre pas supprimer. \n" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            conn.Close();
        }
    }
}
