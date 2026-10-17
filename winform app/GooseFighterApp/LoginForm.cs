using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace GooseFighterApp
{
    /// <summary>
    /// Display area for Login and Registration
    /// Every decision (wrong password, lockout, duplicate names) is made by the
    /// stored procedures
    /// </summary>
    public partial class LoginForm : Form
    {
        private readonly LoginDao _loginDao = new LoginDao();
        private readonly DatabaseConnection _dbConnection = new DatabaseConnection();

        public LoginForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called by GamePlayForm/AdminForm when the player goes back to the title screen.
        /// </summary>
        public void ShowAgain(string message = "")
        {
            passwordTextBox.Clear();
            emailTextBox.Clear();
            ShowStatus(message, true);
            Show();
        }

        private void TestConnectionButton_Click(object sender, EventArgs e)
        {
            bool success = _dbConnection.TestConnection(out string message);
            ShowStatus(message, success);
        }

        // player login, including lockout
        private void LoginButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text.Trim();
            string password = passwordTextBox.Text;

            if (username == "" || password == "")
            {
                ShowStatus("Enter a username and password.", false);
                return;
            }

            try
            {
                DataTable result = _loginDao.Login(username, password);
                string status = ResultHelper.GetStatus(result, "LoginStatus");

                switch (status)
                {
                    case "SUCCESS":
                        EnterGame(Convert.ToInt32(result.Rows[0]["PlayerID"]),
                                  Convert.ToBoolean(result.Rows[0]["IsAdmin"]));
                        break;
                    case "INCORRECT_PASSWORD":
                        passwordTextBox.Clear();
                        ShowStatus("Incorrect password.", false);
                        break;
                    case "LOCKED_OUT":
                        passwordTextBox.Clear();
                        ShowStatus("Account locked. Please contact an administrator.", false);
                        break;
                    case "NOT_FOUND":
                        ShowStatus("No player has that username. To create an account, fill in the email box and click Register.", false);
                        break;
                    default:
                        ShowStatus("Unexpected result: " + status, false);
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowStatus("Error: " + ex.Message, false);
            }
        }

        //player registration
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            string email = emailTextBox.Text.Trim();
            string username = usernameTextBox.Text.Trim();
            string password = passwordTextBox.Text;

            try
            {
                DataTable result = _loginDao.Register(email, username, password);
                string status = ResultHelper.GetStatus(result, "RegisterStatus");

                switch (status)
                {
                    case "SUCCESS":
                        // Successful registration counts as a successful login
                        EnterGame(Convert.ToInt32(result.Rows[0]["PlayerID"]), false);
                        break;
                    case "MISSING_FIELDS":
                        ShowStatus("Fill in the email, username and password boxes.", false);
                        break;
                    case "USERNAME_TAKEN":
                        ShowStatus("That username is already taken.", false);
                        break;
                    case "EMAIL_TAKEN":
                        ShowStatus("That email address is already registered.", false);
                        break;
                    default:
                        ShowStatus("Unexpected result: " + status, false);
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowStatus("Error: " + ex.Message, false);
            }
        }

        // Navigation: opens the game screen for a player or the admin screen for an admin
        private void EnterGame(int playerId, bool isAdmin)
        {
            ShowStatus("Logged in (PlayerID " + playerId + ").", true);

            if (isAdmin)
            {
                new AdminForm { AdminId = playerId, LoginScreen = this }.Show();
            }
            else
            {
                new GamePlayForm { PlayerId = playerId, LoginScreen = this }.Show();
            }

            Hide();
        }

        private void ShowStatus(string message, bool ok)
        {
            statusLabel.ForeColor = ok ? Color.Green : Color.DarkRed;
            statusLabel.Text = message;
        }
    }
}