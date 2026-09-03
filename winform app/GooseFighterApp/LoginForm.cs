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
    /// Display area for Login and Registration.
    /// </summary>
    public partial class LoginForm : Form
    {
        private readonly LoginDao _loginDao = new LoginDao();
        private readonly DatabaseConnection _dbConnection = new DatabaseConnection();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void TestConnectionButton_Click(object sender, EventArgs e)
        {
            bool success = _dbConnection.TestConnection(out string message);
            statusLabel.ForeColor = success ? Color.Green : Color.DarkRed;
            statusLabel.Text = message;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text.Trim();
            string password = passwordTextBox.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                statusLabel.ForeColor = Color.DarkRed;
                statusLabel.Text = "Enter a username and password.";
                return;
            }

            try
            {
                var result = _loginDao.AttemptLogin(username, password);

                if (result.Rows.Count == 0)
                {
                    statusLabel.ForeColor = Color.DarkRed;
                    statusLabel.Text = "Incorrect username or password.";
                    return;
                }

                bool isAdmin = (bool)result.Rows[0]["IsAdmin"];
                bool isLockedOut = (bool)result.Rows[0]["IsLockedOut"];
                int playerId = (int)result.Rows[0]["PlayerID"];

                if (isLockedOut)
                {
                    statusLabel.ForeColor = Color.DarkRed;
                    statusLabel.Text = "Account locked. Contact an administrator.";
                    return;
                }

                statusLabel.ForeColor = Color.Green;
                statusLabel.Text = "Login OK, PlayerID " + playerId;

                if (isAdmin)
                {
                    new AdminForm().Show();
                }
                else
                {
                    new GamePlayForm { PlayerId = playerId }.Show();
                }

                this.Hide();
            }
            catch (Exception ex)
            {
                statusLabel.ForeColor = Color.DarkRed;
                statusLabel.Text = "Error: " + ex.Message;
            }
        }
    }
}