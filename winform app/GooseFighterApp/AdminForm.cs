using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace GooseFighterApp
{
    /// <summary>
    /// Display area for Administration
    /// The stored procedures check that the caller really is an admin
    /// </summary>
    public partial class AdminForm : Form
    {
        // Set by LoginForm right after construction
        public int AdminId { get; set; }
        public LoginForm LoginScreen { get; set; }

        private readonly AdminDao _adminDao = new AdminDao();
        private readonly LoginDao _loginDao = new LoginDao();
        private bool _returningToLogin;

        //stops the grid's SelectionChanged event reloading the detail boxes
        // while the grid is still being filled with new data
        private bool _loadingPlayers;

        public AdminForm()
        {
            InitializeComponent();
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            Text = "Ultimate Goose Fighter - Administration (Admin ID " + AdminId + ")";

            try
            {
                itemTypeComboBox.DataSource = _adminDao.GetItemTypes();
            }
            catch (Exception ex)
            {
                Report("Could not load the item types: " + ex.Message, false);
            }

            RefreshPlayers(null);
            RefreshBoard();
        }

        // Players tab

        // Selecting a row in the grid loads that player's details into the boxes
        private void PlayersGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (_loadingPlayers)
            {
                return;
            }
            LoadSelectedPlayerDetails();
        }

        // add a new player
        private void AddPlayerButton_Click(object sender, EventArgs e)
        {
            string email = emailTextBox.Text.Trim();
            string username = usernameTextBox.Text.Trim();
            string password = passwordTextBox.Text;
            bool isAdmin = isAdminCheckBox.Checked;

            string status = RunAction("Add player " + username,
                () => _adminDao.AddPlayer(AdminId, email, username, password, isAdmin), "AddPlayerStatus");
            if (status == "SUCCESS")
            {
                RefreshPlayers(GetSelectedPlayerId());
            }
        }

        // update the selected player
        private void UpdatePlayerButton_Click(object sender, EventArgs e)
        {
            int? selected = GetSelectedPlayerId();
            if (selected == null)
            {
                Report("Update player: select a player in the table first.", false);
                return;
            }

            int playerId = selected.Value;
            string email = emailTextBox.Text.Trim();
            string username = usernameTextBox.Text.Trim();
            string password = passwordTextBox.Text;
            int score = (int)scoreInput.Value;
            bool isAdmin = isAdminCheckBox.Checked;
            bool isLockedOut = isLockedOutCheckBox.Checked;

            string status = RunAction("Update player " + playerId,
                () => _adminDao.UpdatePlayer(AdminId, playerId, email, username, password, score, isAdmin, isLockedOut),
                "UpdatePlayerStatus");
            if (status == "SUCCESS")
            {
                RefreshPlayers(playerId);
            }
        }

        // delete the selected player
        private void DeletePlayerButton_Click(object sender, EventArgs e)
        {
            int? selected = GetSelectedPlayerId();
            if (selected == null)
            {
                Report("Delete player: select a player in the table first.", false);
                return;
            }

            int playerId = selected.Value;
            if (playerId == AdminId)
            {
                Report("Delete player: you cannot delete the admin account you are logged in with.", false);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Delete player " + playerId + " and everything they own? This cannot be undone.",
                "Delete player", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes)
            {
                return;
            }

            string status = RunAction("Delete player " + playerId,
                () => _adminDao.DeletePlayer(AdminId, playerId), "DeletePlayerStatus");
            if (status == "SUCCESS")
            {
                RefreshPlayers(null);
                RefreshBoard();
            }
        }

        // Deselects the row so the boxes can be used to add a brand new player
        private void ClearFieldsButton_Click(object sender, EventArgs e)
        {
            playersGrid.CurrentCell = null;
        }

        private void RefreshPlayersButton_Click(object sender, EventArgs e)
        {
            RefreshPlayers(GetSelectedPlayerId());
        }

        // Board tab 

        //place a tile next to an existing one
        private void PlaceTileButton_Click(object sender, EventArgs e)
        {
            int x = (int)tileXInput.Value;
            int y = (int)tileYInput.Value;

            RunAction("Place tile (" + x + "," + y + ")", () => _adminDao.PlaceTile(x, y), "PlaceTileStatus");
            RefreshBoard();
        }

        // remove the selected tile (only allowed if it is empty)
        private void DeleteTileButton_Click(object sender, EventArgs e)
        {
            int? tileId = GetSelectedTileId();
            if (tileId == null)
            {
                Report("Delete tile: select a tile in the table first.", false);
                return;
            }

            int id = tileId.Value;
            RunAction("Delete tile " + id, () => _adminDao.DeleteTile(id), "DeleteTileStatus");
            RefreshBoard();
        }

        // place an item of the chosen type on the selected tile
        private void PlaceItemButton_Click(object sender, EventArgs e)
        {
            int? tileId = GetSelectedTileId();
            if (tileId == null || itemTypeComboBox.SelectedValue == null)
            {
                Report("Place item: choose an item type and select a tile in the table first.", false);
                return;
            }

            int id = tileId.Value;
            int itemTypeId = Convert.ToInt32(itemTypeComboBox.SelectedValue);
            string itemName = itemTypeComboBox.Text;

            RunAction("Place " + itemName + " on tile " + id,
                () => _adminDao.PlaceItem(itemTypeId, id), "PlaceItemStatus");
            RefreshBoard();
        }

        // the rubbish bin tool, removes the item lying on the selected tile
        private void RemoveItemButton_Click(object sender, EventArgs e)
        {
            int? tileId = GetSelectedTileId();
            if (tileId == null)
            {
                Report("Remove item: select a tile in the table first.", false);
                return;
            }

            int id = tileId.Value;
            RunAction("Remove item from tile " + id, () => _adminDao.RemoveItemFromTile(id), "RemoveItemStatus");
            RefreshBoard();
        }

        private void RefreshBoardButton_Click(object sender, EventArgs e)
        {
            RefreshBoard();
        }

        // game tab

        // kill the running game (asks "are you sure" first
        private void KillGameButton_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show(
                "Are you sure? Every other online player will be logged out.",
                "Kill current game", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes)
            {
                return;
            }

            RunAction("Kill current game", () => _adminDao.KillRunningGame(AdminId), "KillStatus");
            RefreshPlayers(GetSelectedPlayerId());
        }

        // Navigation

        // Log the admin out and go back to the login screen
        private void BackButton_Click(object sender, EventArgs e)
        {
            try
            {
                _loginDao.Logout(AdminId);
            }
            catch (Exception ex)
            {
                Report("Log out failed: " + ex.Message, false);
                return;
            }

            _returningToLogin = true;
            if (LoginScreen != null)
            {
                LoginScreen.ShowAgain();
            }
            else
            {
                new LoginForm().Show();
            }
            Close();
        }

        // The X button: log out and close the whole app
        private void AdminForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_returningToLogin)
            {
                return;
            }

            try
            {
                _loginDao.Logout(AdminId);
            }
            catch
            {
            }

            Application.Exit();
        }

        // helpers

        /// <summary>
        /// Calls one DAO method and logs the status it returns.
        /// Returns the status text so a handler can react to it.
        /// </summary>
        private string RunAction(string action, Func<DataTable> call, string statusColumn)
        {
            try
            {
                DataTable result = call();
                string status = ResultHelper.GetStatus(result, statusColumn);
                Report(ResultHelper.Describe(action, result, statusColumn), ResultHelper.IsGood(status));
                return status;
            }
            catch (Exception ex)
            {
                Report(action + ": error - " + ex.Message, false);
                return "ERROR";
            }
        }

        /// <summary>
        /// Reloads the players table. If selectId has a value, that player's row is
        /// selected again afterr
        /// </summary>
        private void RefreshPlayers(int? selectId)
        {
            _loadingPlayers = true;
            try
            {
                playersGrid.DataSource = _adminDao.GetAllPlayers();

                if (selectId.HasValue)
                {
                    foreach (DataGridViewRow row in playersGrid.Rows)
                    {
                        if (Convert.ToInt32(row.Cells["PlayerID"].Value) == selectId.Value)
                        {
                            playersGrid.CurrentCell = row.Cells["Username"];
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Report("Could not load the players: " + ex.Message, false);
            }
            finally
            {
                _loadingPlayers = false;
            }

            LoadSelectedPlayerDetails();
        }

        private void RefreshBoard()
        {
            try
            {
                boardGrid.DataSource = _adminDao.GetBoard();
            }
            catch (Exception ex)
            {
                Report("Could not load the board: " + ex.Message, false);
            }
        }

        // Fills the detail boxes from the database for the selected player,
        // or empty them if no player is selected
        private void LoadSelectedPlayerDetails()
        {
            int? playerId = GetSelectedPlayerId();

            if (playerId == null)
            {
                selectedPlayerLabel.Text = "Selected player: none (fill in the boxes to add a new player)";
                emailTextBox.Clear();
                usernameTextBox.Clear();
                passwordTextBox.Clear();
                scoreInput.Value = 0;
                isAdminCheckBox.Checked = false;
                isLockedOutCheckBox.Checked = false;
                return;
            }

            try
            {
                DataTable player = _adminDao.GetPlayer(playerId.Value);
                if (player.Rows.Count == 0)
                {
                    Report("Player " + playerId + " no longer exists. Click Refresh.", false);
                    return;
                }

                DataRow row = player.Rows[0];
                selectedPlayerLabel.Text = "Selected player ID: " + playerId;
                emailTextBox.Text = Convert.ToString(row["Email"]);
                usernameTextBox.Text = Convert.ToString(row["Username"]);
                passwordTextBox.Text = Convert.ToString(row["Password"]);
                scoreInput.Value = Convert.ToDecimal(row["Score"]);
                isAdminCheckBox.Checked = Convert.ToBoolean(row["IsAdmin"]);
                isLockedOutCheckBox.Checked = Convert.ToBoolean(row["IsLockedOut"]);
            }
            catch (Exception ex)
            {
                Report("Could not load the player: " + ex.Message, false);
            }
        }

        private int? GetSelectedPlayerId()
        {
            if (playersGrid.CurrentRow == null)
            {
                return null;
            }
            return Convert.ToInt32(playersGrid.CurrentRow.Cells["PlayerID"].Value);
        }

        private int? GetSelectedTileId()
        {
            if (boardGrid.CurrentRow == null)
            {
                return null;
            }
            return Convert.ToInt32(boardGrid.CurrentRow.Cells["TileID"].Value);
        }

        private void Report(string message, bool ok)
        {
            statusLabel.ForeColor = ok ? Color.Green : Color.DarkRed;
            statusLabel.Text = message;
            logListBox.Items.Insert(0, DateTime.Now.ToString("HH:mm:ss") + "  " + message);
        }
    }
}