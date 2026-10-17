using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace GooseFighterApp
{
    /// <summary>
    /// Display area for Game Play
    /// All game rules live in the stored procedures. Each button calls one DAO method,
    /// writes the returned status to the log, then reloads the screen from the database.
    /// </summary>
    public partial class GamePlayForm : Form
    {
        // Set by LoginForm right after construction
        public int PlayerId { get; set; }
        public LoginForm LoginScreen { get; set; }

        // There is only ever one NPC row in the database
        private const int NpcId = 1;

        private readonly GamePlayDao _gamePlayDao = new GamePlayDao();
        private readonly LoginDao _loginDao = new LoginDao();
        private readonly AdminDao _adminDao = new AdminDao();
        private bool _returningToLogin;

        public GamePlayForm()
        {
            InitializeComponent();
        }

        private void GamePlayForm_Load(object sender, EventArgs e)
        {
            Text = "Ultimate Goose Fighter - Game Play (Player ID " + PlayerId + ")";
            facingComboBox.SelectedItem = "DOWN";

            // Put the player on the map: a returning player keeps their saved tile, a new player gets a random free tile and a nest.
            RunAction("Enter game", () => _gamePlayDao.SpawnPlayer(PlayerId), "SpawnStatus");
        }

        // movement. All four direction buttons share this 
        private void MoveButton_Click(object sender, EventArgs e)
        {
            string direction = (string)((Button)sender).Tag;
            string status = RunAction("Move " + direction,
                                      () => _gamePlayDao.MovePlayer(PlayerId, direction), "MoveStatus");
            if (status == "SUCCESS")
            {
                // The goose faces the way it last moved
                facingComboBox.SelectedItem = direction;
            }
        }

        // attacking and scoring
        private void AttackButton_Click(object sender, EventArgs e)
        {
            string facing = Convert.ToString(facingComboBox.SelectedItem);
            RunAction("Attack " + facing, () => _gamePlayDao.Attack(PlayerId, facing), "AttackStatus");
        }

        //acquiring inventory
        private void PickUpButton_Click(object sender, EventArgs e)
        {
            RunAction("Pick up item", () => _gamePlayDao.PickUpItem(PlayerId), "PickUpStatus");
        }

        private void DropButton_Click(object sender, EventArgs e)
        {
            RunAction("Drop item", () => _gamePlayDao.DropItem(PlayerId), "DropStatus");
        }

        private void TakeFromNestButton_Click(object sender, EventArgs e)
        {
            object selected = nestListBox.SelectedValue;
            if (selected == null)
            {
                Report("Take from nest: select an item in the nest list first.", false);
                return;
            }

            int itemId = Convert.ToInt32(selected);
            RunAction("Take from nest", () => _gamePlayDao.TakeFromNest(PlayerId, itemId), "TakeStatus");
        }

        // (random spawning and npc.
        private void SpawnItemButton_Click(object sender, EventArgs e)
        {
            RunAction("Spawn random item", () => _gamePlayDao.SpawnRandomItem(), "SpawnStatus");
        }

        private void MoveNpcButton_Click(object sender, EventArgs e)
        {
            RunAction("Move NPC", () => _gamePlayDao.MoveNpc(NpcId), "MoveNpcStatus");
        }

        private void RespawnNpcButton_Click(object sender, EventArgs e)
        {
            RunAction("Respawn NPC", () => _gamePlayDao.RespawnNpc(), "RespawnStatus");
        }

        // Shows changes made by other players, timers or the admin
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RefreshScreen();
            Report("Refreshed from the database.", true);
        }

        // 'Return to title screen" logs the player out
        private void LogoutButton_Click(object sender, EventArgs e)
        {
            try
            {
                _loginDao.Logout(PlayerId);
            }
            catch (Exception ex)
            {
                Report("Log out failed: " + ex.Message, false);
                return;
            }

            ReturnToLogin("");
        }

        //a player deletes their own account ( pass their own ID twice)
        private void DeleteAccountButton_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to delete your account? This cannot be undone.",
                "Delete account", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes)
            {
                return;
            }

            string status = RunAction("Delete account",
                                      () => _adminDao.DeletePlayer(PlayerId, PlayerId), "DeletePlayerStatus");
            if (status == "SUCCESS")
            {
                ReturnToLogin("Your account was deleted.");
            }
        }

        // The X button: log the player out and close the whole app
        private void GamePlayForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_returningToLogin)
            {
                return;
            }

            try
            {
                _loginDao.Logout(PlayerId);
            }
            catch
            {
                
            }

            Application.Exit();
        }

        private void ReturnToLogin(string message)
        {
            _returningToLogin = true;

            if (LoginScreen != null)
            {
                LoginScreen.ShowAgain(message);
            }
            else
            {
                new LoginForm().Show();
            }

            Close();
        }

        /// <summary>
        /// Calls one DAO method, logs the status it returns, then reloads the screen.
        /// Returns the status text 
        /// </summary>
        private string RunAction(string action, Func<DataTable> call, string statusColumn)
        {
            string status = "ERROR";
            try
            {
                DataTable result = call();
                status = ResultHelper.GetStatus(result, statusColumn);
                Report(ResultHelper.Describe(action, result, statusColumn), ResultHelper.IsGood(status));
            }
            catch (Exception ex)
            {
                Report(action + ": error - " + ex.Message, false);
            }

            RefreshScreen();
            return status;
        }

        /// <summary>
        /// Reloads everything shown on screen from the database
        /// </summary>
        private void RefreshScreen()
        {
            try
            {
                DataTable player = _gamePlayDao.GetPlayer(PlayerId);
                if (player.Rows.Count == 0)
                {
                    scoreLabel.Text = "Score: -";
                    healthLabel.Text = "Health: -";
                    tileLabel.Text = "Tile: -";
                    holdingLabel.Text = "Holding: -";
                }
                else
                {
                    DataRow row = player.Rows[0];
                    scoreLabel.Text = "Score: " + row["Score"];
                    healthLabel.Text = "Health: " + row["Health"] + " / 4000";
                    tileLabel.Text = row.IsNull("CurrentTileID")
                        ? "Tile: not on the map"
                        : "Tile: " + row["CurrentTileID"] + "  (" + row["X"] + ", " + row["Y"] + ")";
                    holdingLabel.Text = row.IsNull("HeldItem")
                        ? "Holding: nothing"
                        : "Holding: " + row["HeldItem"] + (row.IsNull("HeldAmmo") ? "" : "  (" + row["HeldAmmo"] + " ammo)");
                }

                nestListBox.DataSource = _gamePlayDao.GetNestContents(PlayerId);
                boardGrid.DataSource = _gamePlayDao.GetBoard();
            }
            catch (Exception ex)
            {
                Report("Refresh failed: " + ex.Message, false);
            }
        }

        private void Report(string message, bool ok)
        {
            statusLabel.ForeColor = ok ? Color.Green : Color.DarkRed;
            statusLabel.Text = message;
            logListBox.Items.Insert(0, DateTime.Now.ToString("HH:mm:ss") + "  " + message);
        }
    }
}