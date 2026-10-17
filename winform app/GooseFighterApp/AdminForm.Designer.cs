namespace GooseFighterApp
{
    partial class AdminForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage playersTabPage;
        private System.Windows.Forms.DataGridView playersGrid;
        private System.Windows.Forms.GroupBox detailsGroupBox;
        private System.Windows.Forms.Label selectedPlayerLabel;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.TextBox emailTextBox;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.NumericUpDown scoreInput;
        private System.Windows.Forms.CheckBox isAdminCheckBox;
        private System.Windows.Forms.CheckBox isLockedOutCheckBox;
        private System.Windows.Forms.Button addPlayerButton;
        private System.Windows.Forms.Button updatePlayerButton;
        private System.Windows.Forms.Button deletePlayerButton;
        private System.Windows.Forms.Button clearFieldsButton;
        private System.Windows.Forms.Button refreshPlayersButton;
        private System.Windows.Forms.TabPage boardTabPage;
        private System.Windows.Forms.DataGridView boardGrid;
        private System.Windows.Forms.Label hintLabel;
        private System.Windows.Forms.GroupBox tileGroupBox;
        private System.Windows.Forms.Label tileXLabel;
        private System.Windows.Forms.NumericUpDown tileXInput;
        private System.Windows.Forms.Label tileYLabel;
        private System.Windows.Forms.NumericUpDown tileYInput;
        private System.Windows.Forms.Button placeTileButton;
        private System.Windows.Forms.Button deleteTileButton;
        private System.Windows.Forms.GroupBox itemGroupBox;
        private System.Windows.Forms.Label itemTypeLabel;
        private System.Windows.Forms.ComboBox itemTypeComboBox;
        private System.Windows.Forms.Button placeItemButton;
        private System.Windows.Forms.Button removeItemButton;
        private System.Windows.Forms.Button refreshBoardButton;
        private System.Windows.Forms.TabPage gameTabPage;
        private System.Windows.Forms.Label gameInfoLabel;
        private System.Windows.Forms.Button killGameButton;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.ListBox logListBox;
        private System.Windows.Forms.Button backButton;

        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.playersTabPage = new System.Windows.Forms.TabPage();
            this.playersGrid = new System.Windows.Forms.DataGridView();
            this.detailsGroupBox = new System.Windows.Forms.GroupBox();
            this.selectedPlayerLabel = new System.Windows.Forms.Label();
            this.emailLabel = new System.Windows.Forms.Label();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.scoreLabel = new System.Windows.Forms.Label();
            this.scoreInput = new System.Windows.Forms.NumericUpDown();
            this.isAdminCheckBox = new System.Windows.Forms.CheckBox();
            this.isLockedOutCheckBox = new System.Windows.Forms.CheckBox();
            this.addPlayerButton = new System.Windows.Forms.Button();
            this.updatePlayerButton = new System.Windows.Forms.Button();
            this.deletePlayerButton = new System.Windows.Forms.Button();
            this.clearFieldsButton = new System.Windows.Forms.Button();
            this.refreshPlayersButton = new System.Windows.Forms.Button();
            this.boardTabPage = new System.Windows.Forms.TabPage();
            this.boardGrid = new System.Windows.Forms.DataGridView();
            this.hintLabel = new System.Windows.Forms.Label();
            this.tileGroupBox = new System.Windows.Forms.GroupBox();
            this.tileXLabel = new System.Windows.Forms.Label();
            this.tileXInput = new System.Windows.Forms.NumericUpDown();
            this.tileYLabel = new System.Windows.Forms.Label();
            this.tileYInput = new System.Windows.Forms.NumericUpDown();
            this.placeTileButton = new System.Windows.Forms.Button();
            this.deleteTileButton = new System.Windows.Forms.Button();
            this.itemGroupBox = new System.Windows.Forms.GroupBox();
            this.itemTypeLabel = new System.Windows.Forms.Label();
            this.itemTypeComboBox = new System.Windows.Forms.ComboBox();
            this.placeItemButton = new System.Windows.Forms.Button();
            this.removeItemButton = new System.Windows.Forms.Button();
            this.refreshBoardButton = new System.Windows.Forms.Button();
            this.gameTabPage = new System.Windows.Forms.TabPage();
            this.gameInfoLabel = new System.Windows.Forms.Label();
            this.killGameButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.logListBox = new System.Windows.Forms.ListBox();
            this.backButton = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.playersTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.playersGrid)).BeginInit();
            this.detailsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scoreInput)).BeginInit();
            this.boardTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.boardGrid)).BeginInit();
            this.tileGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tileXInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tileYInput)).BeginInit();
            this.itemGroupBox.SuspendLayout();
            this.gameTabPage.SuspendLayout();
            this.SuspendLayout();
            //
            // tabControl
            //
            this.tabControl.Controls.Add(this.playersTabPage);
            this.tabControl.Controls.Add(this.boardTabPage);
            this.tabControl.Controls.Add(this.gameTabPage);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(760, 424);
            this.tabControl.TabIndex = 0;
            //
            // playersTabPage
            //
            this.playersTabPage.Controls.Add(this.playersGrid);
            this.playersTabPage.Controls.Add(this.detailsGroupBox);
            this.playersTabPage.Controls.Add(this.addPlayerButton);
            this.playersTabPage.Controls.Add(this.updatePlayerButton);
            this.playersTabPage.Controls.Add(this.deletePlayerButton);
            this.playersTabPage.Controls.Add(this.clearFieldsButton);
            this.playersTabPage.Controls.Add(this.refreshPlayersButton);
            this.playersTabPage.Location = new System.Drawing.Point(4, 24);
            this.playersTabPage.Name = "playersTabPage";
            this.playersTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.playersTabPage.Size = new System.Drawing.Size(752, 396);
            this.playersTabPage.TabIndex = 0;
            this.playersTabPage.Text = "Players";
            this.playersTabPage.UseVisualStyleBackColor = true;
            //
            // playersGrid
            //
            this.playersGrid.AllowUserToAddRows = false;
            this.playersGrid.AllowUserToDeleteRows = false;
            this.playersGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.playersGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.playersGrid.Location = new System.Drawing.Point(8, 8);
            this.playersGrid.MultiSelect = false;
            this.playersGrid.Name = "playersGrid";
            this.playersGrid.ReadOnly = true;
            this.playersGrid.RowHeadersVisible = false;
            this.playersGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.playersGrid.Size = new System.Drawing.Size(736, 150);
            this.playersGrid.TabIndex = 0;
            this.playersGrid.SelectionChanged += new System.EventHandler(this.PlayersGrid_SelectionChanged);
            //
            // detailsGroupBox
            //
            this.detailsGroupBox.Controls.Add(this.selectedPlayerLabel);
            this.detailsGroupBox.Controls.Add(this.emailLabel);
            this.detailsGroupBox.Controls.Add(this.emailTextBox);
            this.detailsGroupBox.Controls.Add(this.usernameLabel);
            this.detailsGroupBox.Controls.Add(this.usernameTextBox);
            this.detailsGroupBox.Controls.Add(this.passwordLabel);
            this.detailsGroupBox.Controls.Add(this.passwordTextBox);
            this.detailsGroupBox.Controls.Add(this.scoreLabel);
            this.detailsGroupBox.Controls.Add(this.scoreInput);
            this.detailsGroupBox.Controls.Add(this.isAdminCheckBox);
            this.detailsGroupBox.Controls.Add(this.isLockedOutCheckBox);
            this.detailsGroupBox.Location = new System.Drawing.Point(8, 164);
            this.detailsGroupBox.Name = "detailsGroupBox";
            this.detailsGroupBox.Size = new System.Drawing.Size(736, 176);
            this.detailsGroupBox.TabIndex = 1;
            this.detailsGroupBox.TabStop = false;
            this.detailsGroupBox.Text = "Player details";
            //
            // selectedPlayerLabel
            //
            this.selectedPlayerLabel.Location = new System.Drawing.Point(12, 22);
            this.selectedPlayerLabel.Name = "selectedPlayerLabel";
            this.selectedPlayerLabel.Size = new System.Drawing.Size(700, 20);
            this.selectedPlayerLabel.TabIndex = 0;
            this.selectedPlayerLabel.Text = "Selected player: none";
            //
            // emailLabel
            //
            this.emailLabel.Location = new System.Drawing.Point(12, 52);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(70, 20);
            this.emailLabel.TabIndex = 1;
            this.emailLabel.Text = "Email:";
            //
            // emailTextBox
            //
            this.emailTextBox.Location = new System.Drawing.Point(90, 49);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(250, 23);
            this.emailTextBox.TabIndex = 2;
            //
            // usernameLabel
            //
            this.usernameLabel.Location = new System.Drawing.Point(12, 84);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(70, 20);
            this.usernameLabel.TabIndex = 3;
            this.usernameLabel.Text = "Username:";
            //
            // usernameTextBox
            //
            this.usernameTextBox.Location = new System.Drawing.Point(90, 81);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(250, 23);
            this.usernameTextBox.TabIndex = 4;
            //
            // passwordLabel
            //
            this.passwordLabel.Location = new System.Drawing.Point(12, 116);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(70, 20);
            this.passwordLabel.TabIndex = 5;
            this.passwordLabel.Text = "Password:";
            //
            // passwordTextBox
            //
            this.passwordTextBox.Location = new System.Drawing.Point(90, 113);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(250, 23);
            this.passwordTextBox.TabIndex = 6;
            //
            // scoreLabel
            //
            this.scoreLabel.Location = new System.Drawing.Point(380, 52);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(60, 20);
            this.scoreLabel.TabIndex = 7;
            this.scoreLabel.Text = "Score:";
            //
            // scoreInput
            //
            this.scoreInput.Location = new System.Drawing.Point(446, 49);
            this.scoreInput.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.scoreInput.Name = "scoreInput";
            this.scoreInput.Size = new System.Drawing.Size(100, 23);
            this.scoreInput.TabIndex = 8;
            //
            // isAdminCheckBox
            //
            this.isAdminCheckBox.Location = new System.Drawing.Point(380, 82);
            this.isAdminCheckBox.Name = "isAdminCheckBox";
            this.isAdminCheckBox.Size = new System.Drawing.Size(200, 24);
            this.isAdminCheckBox.TabIndex = 9;
            this.isAdminCheckBox.Text = "Admin?";
            this.isAdminCheckBox.UseVisualStyleBackColor = true;
            //
            // isLockedOutCheckBox
            //
            this.isLockedOutCheckBox.Location = new System.Drawing.Point(380, 112);
            this.isLockedOutCheckBox.Name = "isLockedOutCheckBox";
            this.isLockedOutCheckBox.Size = new System.Drawing.Size(300, 24);
            this.isLockedOutCheckBox.TabIndex = 10;
            this.isLockedOutCheckBox.Text = "Account locked? (untick to unlock)";
            this.isLockedOutCheckBox.UseVisualStyleBackColor = true;
            //
            // addPlayerButton
            //
            this.addPlayerButton.Location = new System.Drawing.Point(8, 348);
            this.addPlayerButton.Name = "addPlayerButton";
            this.addPlayerButton.Size = new System.Drawing.Size(170, 32);
            this.addPlayerButton.TabIndex = 2;
            this.addPlayerButton.Text = "Add as new player";
            this.addPlayerButton.UseVisualStyleBackColor = true;
            this.addPlayerButton.Click += new System.EventHandler(this.AddPlayerButton_Click);
            //
            // updatePlayerButton
            //
            this.updatePlayerButton.Location = new System.Drawing.Point(186, 348);
            this.updatePlayerButton.Name = "updatePlayerButton";
            this.updatePlayerButton.Size = new System.Drawing.Size(170, 32);
            this.updatePlayerButton.TabIndex = 3;
            this.updatePlayerButton.Text = "Update selected player";
            this.updatePlayerButton.UseVisualStyleBackColor = true;
            this.updatePlayerButton.Click += new System.EventHandler(this.UpdatePlayerButton_Click);
            //
            // deletePlayerButton
            //
            this.deletePlayerButton.Location = new System.Drawing.Point(364, 348);
            this.deletePlayerButton.Name = "deletePlayerButton";
            this.deletePlayerButton.Size = new System.Drawing.Size(170, 32);
            this.deletePlayerButton.TabIndex = 4;
            this.deletePlayerButton.Text = "Delete selected player";
            this.deletePlayerButton.UseVisualStyleBackColor = true;
            this.deletePlayerButton.Click += new System.EventHandler(this.DeletePlayerButton_Click);
            //
            // clearFieldsButton
            //
            this.clearFieldsButton.Location = new System.Drawing.Point(542, 348);
            this.clearFieldsButton.Name = "clearFieldsButton";
            this.clearFieldsButton.Size = new System.Drawing.Size(96, 32);
            this.clearFieldsButton.TabIndex = 5;
            this.clearFieldsButton.Text = "Clear fields";
            this.clearFieldsButton.UseVisualStyleBackColor = true;
            this.clearFieldsButton.Click += new System.EventHandler(this.ClearFieldsButton_Click);
            //
            // refreshPlayersButton
            //
            this.refreshPlayersButton.Location = new System.Drawing.Point(646, 348);
            this.refreshPlayersButton.Name = "refreshPlayersButton";
            this.refreshPlayersButton.Size = new System.Drawing.Size(98, 32);
            this.refreshPlayersButton.TabIndex = 6;
            this.refreshPlayersButton.Text = "Refresh";
            this.refreshPlayersButton.UseVisualStyleBackColor = true;
            this.refreshPlayersButton.Click += new System.EventHandler(this.RefreshPlayersButton_Click);
            //
            // boardTabPage
            //
            this.boardTabPage.Controls.Add(this.boardGrid);
            this.boardTabPage.Controls.Add(this.hintLabel);
            this.boardTabPage.Controls.Add(this.tileGroupBox);
            this.boardTabPage.Controls.Add(this.itemGroupBox);
            this.boardTabPage.Controls.Add(this.refreshBoardButton);
            this.boardTabPage.Location = new System.Drawing.Point(4, 24);
            this.boardTabPage.Name = "boardTabPage";
            this.boardTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.boardTabPage.Size = new System.Drawing.Size(752, 396);
            this.boardTabPage.TabIndex = 1;
            this.boardTabPage.Text = "Board";
            this.boardTabPage.UseVisualStyleBackColor = true;
            //
            // boardGrid
            //
            this.boardGrid.AllowUserToAddRows = false;
            this.boardGrid.AllowUserToDeleteRows = false;
            this.boardGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.boardGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.boardGrid.Location = new System.Drawing.Point(8, 8);
            this.boardGrid.MultiSelect = false;
            this.boardGrid.Name = "boardGrid";
            this.boardGrid.ReadOnly = true;
            this.boardGrid.RowHeadersVisible = false;
            this.boardGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.boardGrid.Size = new System.Drawing.Size(736, 214);
            this.boardGrid.TabIndex = 0;
            //
            // hintLabel
            //
            this.hintLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.hintLabel.Location = new System.Drawing.Point(8, 226);
            this.hintLabel.Name = "hintLabel";
            this.hintLabel.Size = new System.Drawing.Size(736, 18);
            this.hintLabel.TabIndex = 1;
            this.hintLabel.Text = "Click a row in the table to choose a tile for the buttons below.";
            //
            // tileGroupBox
            //
            this.tileGroupBox.Controls.Add(this.tileXLabel);
            this.tileGroupBox.Controls.Add(this.tileXInput);
            this.tileGroupBox.Controls.Add(this.tileYLabel);
            this.tileGroupBox.Controls.Add(this.tileYInput);
            this.tileGroupBox.Controls.Add(this.placeTileButton);
            this.tileGroupBox.Controls.Add(this.deleteTileButton);
            this.tileGroupBox.Location = new System.Drawing.Point(8, 248);
            this.tileGroupBox.Name = "tileGroupBox";
            this.tileGroupBox.Size = new System.Drawing.Size(360, 110);
            this.tileGroupBox.TabIndex = 2;
            this.tileGroupBox.TabStop = false;
            this.tileGroupBox.Text = "Tiles";
            //
            // tileXLabel
            //
            this.tileXLabel.Location = new System.Drawing.Point(12, 28);
            this.tileXLabel.Name = "tileXLabel";
            this.tileXLabel.Size = new System.Drawing.Size(20, 20);
            this.tileXLabel.TabIndex = 0;
            this.tileXLabel.Text = "X:";
            //
            // tileXInput
            //
            this.tileXInput.Location = new System.Drawing.Point(36, 25);
            this.tileXInput.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.tileXInput.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.tileXInput.Name = "tileXInput";
            this.tileXInput.Size = new System.Drawing.Size(70, 23);
            this.tileXInput.TabIndex = 1;
            //
            // tileYLabel
            //
            this.tileYLabel.Location = new System.Drawing.Point(120, 28);
            this.tileYLabel.Name = "tileYLabel";
            this.tileYLabel.Size = new System.Drawing.Size(20, 20);
            this.tileYLabel.TabIndex = 2;
            this.tileYLabel.Text = "Y:";
            //
            // tileYInput
            //
            this.tileYInput.Location = new System.Drawing.Point(144, 25);
            this.tileYInput.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.tileYInput.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.tileYInput.Name = "tileYInput";
            this.tileYInput.Size = new System.Drawing.Size(70, 23);
            this.tileYInput.TabIndex = 3;
            //
            // placeTileButton
            //
            this.placeTileButton.Location = new System.Drawing.Point(12, 62);
            this.placeTileButton.Name = "placeTileButton";
            this.placeTileButton.Size = new System.Drawing.Size(160, 32);
            this.placeTileButton.TabIndex = 4;
            this.placeTileButton.Text = "Place tile at X, Y";
            this.placeTileButton.UseVisualStyleBackColor = true;
            this.placeTileButton.Click += new System.EventHandler(this.PlaceTileButton_Click);
            //
            // deleteTileButton
            //
            this.deleteTileButton.Location = new System.Drawing.Point(180, 62);
            this.deleteTileButton.Name = "deleteTileButton";
            this.deleteTileButton.Size = new System.Drawing.Size(168, 32);
            this.deleteTileButton.TabIndex = 5;
            this.deleteTileButton.Text = "Delete selected tile";
            this.deleteTileButton.UseVisualStyleBackColor = true;
            this.deleteTileButton.Click += new System.EventHandler(this.DeleteTileButton_Click);
            //
            // itemGroupBox
            //
            this.itemGroupBox.Controls.Add(this.itemTypeLabel);
            this.itemGroupBox.Controls.Add(this.itemTypeComboBox);
            this.itemGroupBox.Controls.Add(this.placeItemButton);
            this.itemGroupBox.Controls.Add(this.removeItemButton);
            this.itemGroupBox.Location = new System.Drawing.Point(376, 248);
            this.itemGroupBox.Name = "itemGroupBox";
            this.itemGroupBox.Size = new System.Drawing.Size(368, 110);
            this.itemGroupBox.TabIndex = 3;
            this.itemGroupBox.TabStop = false;
            this.itemGroupBox.Text = "Items";
            //
            // itemTypeLabel
            //
            this.itemTypeLabel.Location = new System.Drawing.Point(12, 28);
            this.itemTypeLabel.Name = "itemTypeLabel";
            this.itemTypeLabel.Size = new System.Drawing.Size(70, 20);
            this.itemTypeLabel.TabIndex = 0;
            this.itemTypeLabel.Text = "Item type:";
            //
            // itemTypeComboBox
            //
            this.itemTypeComboBox.DisplayMember = "Name";
            this.itemTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.itemTypeComboBox.FormattingEnabled = true;
            this.itemTypeComboBox.Location = new System.Drawing.Point(88, 25);
            this.itemTypeComboBox.Name = "itemTypeComboBox";
            this.itemTypeComboBox.Size = new System.Drawing.Size(200, 23);
            this.itemTypeComboBox.TabIndex = 1;
            this.itemTypeComboBox.ValueMember = "ItemTypeID";
            //
            // placeItemButton
            //
            this.placeItemButton.Location = new System.Drawing.Point(12, 62);
            this.placeItemButton.Name = "placeItemButton";
            this.placeItemButton.Size = new System.Drawing.Size(170, 32);
            this.placeItemButton.TabIndex = 2;
            this.placeItemButton.Text = "Place on selected tile";
            this.placeItemButton.UseVisualStyleBackColor = true;
            this.placeItemButton.Click += new System.EventHandler(this.PlaceItemButton_Click);
            //
            // removeItemButton
            //
            this.removeItemButton.Location = new System.Drawing.Point(190, 62);
            this.removeItemButton.Name = "removeItemButton";
            this.removeItemButton.Size = new System.Drawing.Size(168, 32);
            this.removeItemButton.TabIndex = 3;
            this.removeItemButton.Text = "Remove item (bin)";
            this.removeItemButton.UseVisualStyleBackColor = true;
            this.removeItemButton.Click += new System.EventHandler(this.RemoveItemButton_Click);
            //
            // refreshBoardButton
            //
            this.refreshBoardButton.Location = new System.Drawing.Point(8, 364);
            this.refreshBoardButton.Name = "refreshBoardButton";
            this.refreshBoardButton.Size = new System.Drawing.Size(120, 28);
            this.refreshBoardButton.TabIndex = 4;
            this.refreshBoardButton.Text = "Refresh board";
            this.refreshBoardButton.UseVisualStyleBackColor = true;
            this.refreshBoardButton.Click += new System.EventHandler(this.RefreshBoardButton_Click);
            //
            // gameTabPage
            //
            this.gameTabPage.Controls.Add(this.gameInfoLabel);
            this.gameTabPage.Controls.Add(this.killGameButton);
            this.gameTabPage.Location = new System.Drawing.Point(4, 24);
            this.gameTabPage.Name = "gameTabPage";
            this.gameTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.gameTabPage.Size = new System.Drawing.Size(752, 396);
            this.gameTabPage.TabIndex = 2;
            this.gameTabPage.Text = "Game";
            this.gameTabPage.UseVisualStyleBackColor = true;
            //
            // gameInfoLabel
            //
            this.gameInfoLabel.Location = new System.Drawing.Point(12, 16);
            this.gameInfoLabel.Name = "gameInfoLabel";
            this.gameInfoLabel.Size = new System.Drawing.Size(720, 60);
            this.gameInfoLabel.TabIndex = 0;
            this.gameInfoLabel.Text = "There is one shared map, so \"the running game\" means every player who is currently " +
    "online. Killing it logs out every online player except you. Positions, health, scor" +
    "es and items are kept, exactly as with a normal log out.";
            //
            // killGameButton
            //
            this.killGameButton.Location = new System.Drawing.Point(12, 84);
            this.killGameButton.Name = "killGameButton";
            this.killGameButton.Size = new System.Drawing.Size(240, 40);
            this.killGameButton.TabIndex = 1;
            this.killGameButton.Text = "Kill current game";
            this.killGameButton.UseVisualStyleBackColor = true;
            this.killGameButton.Click += new System.EventHandler(this.KillGameButton_Click);
            //
            // statusLabel
            //
            this.statusLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.statusLabel.ForeColor = System.Drawing.Color.DarkRed;
            this.statusLabel.Location = new System.Drawing.Point(12, 444);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(760, 40);
            this.statusLabel.TabIndex = 1;
            //
            // logListBox
            //
            this.logListBox.FormattingEnabled = true;
            this.logListBox.HorizontalScrollbar = true;
            this.logListBox.ItemHeight = 15;
            this.logListBox.Location = new System.Drawing.Point(12, 490);
            this.logListBox.Name = "logListBox";
            this.logListBox.Size = new System.Drawing.Size(760, 109);
            this.logListBox.TabIndex = 2;
            //
            // backButton
            //
            this.backButton.Location = new System.Drawing.Point(12, 608);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(200, 30);
            this.backButton.TabIndex = 3;
            this.backButton.Text = "Back to login (log out)";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.BackButton_Click);
            //
            // AdminForm
            //
            this.ClientSize = new System.Drawing.Size(784, 650);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.logListBox);
            this.Controls.Add(this.backButton);
            this.Name = "AdminForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ultimate Goose Fighter - Administration";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AdminForm_FormClosed);
            this.Load += new System.EventHandler(this.AdminForm_Load);
            this.tabControl.ResumeLayout(false);
            this.playersTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.playersGrid)).EndInit();
            this.detailsGroupBox.ResumeLayout(false);
            this.detailsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scoreInput)).EndInit();
            this.boardTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.boardGrid)).EndInit();
            this.tileGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tileXInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tileYInput)).EndInit();
            this.itemGroupBox.ResumeLayout(false);
            this.gameTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}