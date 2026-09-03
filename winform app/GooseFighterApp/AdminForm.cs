using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GooseFighterApp
{
    /// <summary>
    /// Display area for Administration.
    /// This prototype only tests adding a player 
    /// </summary>
    public partial class AdminForm : Form
    {
        private readonly AdminDao _adminDao = new AdminDao();

        public AdminForm()
        {
            InitializeComponent();
        }

        private void AddPlayerButton_Click(object sender, EventArgs e)
        {
            string email = emailTextBox.Text.Trim();
            string username = usernameTextBox.Text.Trim();
            string password = passwordTextBox.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                statusLabel.ForeColor = Color.DarkRed;
                statusLabel.Text = "Fill in all fields.";
                return;
            }

            try
            {
                int rowsAffected = _adminDao.AddPlayer(email, username, password, isAdminCheckBox.Checked);
                statusLabel.ForeColor = Color.Green;
                statusLabel.Text = rowsAffected > 0 ? "Player added." : "Insert failed.";
            }
            catch (Exception ex)
            {
                statusLabel.ForeColor = Color.DarkRed;
                statusLabel.Text = "Error: " + ex.Message;
            }
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            new LoginForm().Show();
            this.Close();
        }
    }
}