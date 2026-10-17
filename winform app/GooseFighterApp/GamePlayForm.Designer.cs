namespace GooseFighterApp
{
    partial class GamePlayForm
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

        private System.Windows.Forms.GroupBox playerGroupBox;
        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.Label healthLabel;
        private System.Windows.Forms.Label tileLabel;
        private System.Windows.Forms.Label holdingLabel;
        private System.Windows.Forms.GroupBox moveGroupBox;
        private System.Windows.Forms.Button moveUpButton;
        private System.Windows.Forms.Button moveLeftButton;
        private System.Windows.Forms.Button moveRightButton;
        private System.Windows.Forms.Button moveDownButton;
        private System.Windows.Forms.GroupBox combatGroupBox;
        private System.Windows.Forms.Label facingLabel;
        private System.Windows.Forms.ComboBox facingComboBox;
        private System.Windows.Forms.Button attackButton;
        private System.Windows.Forms.GroupBox inventoryGroupBox;
        private System.Windows.Forms.Button pickUpButton;
        private System.Windows.Forms.Button dropButton;
        private System.Windows.Forms.Label nestLabel;
        private System.Windows.Forms.ListBox nestListBox;
        private System.Windows.Forms.Button takeFromNestButton;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.DataGridView boardGrid;
        private System.Windows.Forms.GroupBox worldGroupBox;
        private System.Windows.Forms.Button spawnItemButton;
        private System.Windows.Forms.Button moveNpcButton;
        private System.Windows.Forms.Button respawnNpcButton;
        private System.Windows.Forms.GroupBox accountGroupBox;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Button logoutButton;
        private System.Windows.Forms.Button deleteAccountButton;
        private System.Windows.Forms.ListBox logListBox;

        private void InitializeComponent()
        {
            this.playerGroupBox = new System.Windows.Forms.GroupBox();
            this.scoreLabel = new System.Windows.Forms.Label();
            this.healthLabel = new System.Windows.Forms.Label();
            this.tileLabel = new System.Windows.Forms.Label();
            this.holdingLabel = new System.Windows.Forms.Label();
            this.moveGroupBox = new System.Windows.Forms.GroupBox();
            this.moveUpButton = new System.Windows.Forms.Button();
            this.moveLeftButton = new System.Windows.Forms.Button();
            this.moveRightButton = new System.Windows.Forms.Button();
            this.moveDownButton = new System.Windows.Forms.Button();
            this.combatGroupBox = new System.Windows.Forms.GroupBox();
            this.facingLabel = new System.Windows.Forms.Label();
            this.facingComboBox = new System.Windows.Forms.ComboBox();
            this.attackButton = new System.Windows.Forms.Button();
            this.inventoryGroupBox = new System.Windows.Forms.GroupBox();
            this.pickUpButton = new System.Windows.Forms.Button();
            this.dropButton = new System.Windows.Forms.Button();
            this.nestLabel = new System.Windows.Forms.Label();
            this.nestListBox = new System.Windows.Forms.ListBox();
            this.takeFromNestButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.boardGrid = new System.Windows.Forms.DataGridView();
            this.worldGroupBox = new System.Windows.Forms.GroupBox();
            this.spawnItemButton = new System.Windows.Forms.Button();
            this.moveNpcButton = new System.Windows.Forms.Button();
            this.respawnNpcButton = new System.Windows.Forms.Button();
            this.accountGroupBox = new System.Windows.Forms.GroupBox();
            this.refreshButton = new System.Windows.Forms.Button();
            this.logoutButton = new System.Windows.Forms.Button();
            this.deleteAccountButton = new System.Windows.Forms.Button();
            this.logListBox = new System.Windows.Forms.ListBox();
            this.playerGroupBox.SuspendLayout();
            this.moveGroupBox.SuspendLayout();
            this.combatGroupBox.SuspendLayout();
            this.inventoryGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.boardGrid)).BeginInit();
            this.worldGroupBox.SuspendLayout();
            this.accountGroupBox.SuspendLayout();
            this.SuspendLayout();
            //
            // playerGroupBox
            //
            this.playerGroupBox.Controls.Add(this.scoreLabel);
            this.playerGroupBox.Controls.Add(this.healthLabel);
            this.playerGroupBox.Controls.Add(this.tileLabel);
            this.playerGroupBox.Controls.Add(this.holdingLabel);
            this.playerGroupBox.Location = new System.Drawing.Point(12, 12);
            this.playerGroupBox.Name = "playerGroupBox";
            this.playerGroupBox.Size = new System.Drawing.Size(300, 118);
            this.playerGroupBox.TabIndex = 0;
            this.playerGroupBox.TabStop = false;
            this.playerGroupBox.Text = "Your goose";
            //
            // scoreLabel
            //
            this.scoreLabel.Location = new System.Drawing.Point(12, 24);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(276, 20);
            this.scoreLabel.TabIndex = 0;
            this.scoreLabel.Text = "Score:";
            //
            // healthLabel
            //
            this.healthLabel.Location = new System.Drawing.Point(12, 46);
            this.healthLabel.Name = "healthLabel";
            this.healthLabel.Size = new System.Drawing.Size(276, 20);
            this.healthLabel.TabIndex = 1;
            this.healthLabel.Text = "Health:";
            //
            // tileLabel
            //
            this.tileLabel.Location = new System.Drawing.Point(12, 68);
            this.tileLabel.Name = "tileLabel";
            this.tileLabel.Size = new System.Drawing.Size(276, 20);
            this.tileLabel.TabIndex = 2;
            this.tileLabel.Text = "Tile:";
            //
            // holdingLabel
            //
            this.holdingLabel.Location = new System.Drawing.Point(12, 90);
            this.holdingLabel.Name = "holdingLabel";
            this.holdingLabel.Size = new System.Drawing.Size(276, 20);
            this.holdingLabel.TabIndex = 3;
            this.holdingLabel.Text = "Holding:";
            //
            // moveGroupBox
            //
            this.moveGroupBox.Controls.Add(this.moveUpButton);
            this.moveGroupBox.Controls.Add(this.moveLeftButton);
            this.moveGroupBox.Controls.Add(this.moveRightButton);
            this.moveGroupBox.Controls.Add(this.moveDownButton);
            this.moveGroupBox.Location = new System.Drawing.Point(12, 138);
            this.moveGroupBox.Name = "moveGroupBox";
            this.moveGroupBox.Size = new System.Drawing.Size(300, 120);
            this.moveGroupBox.TabIndex = 1;
            this.moveGroupBox.TabStop = false;
            this.moveGroupBox.Text = "Move";
            //
            // moveUpButton
            //
            this.moveUpButton.Location = new System.Drawing.Point(110, 22);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new System.Drawing.Size(80, 28);
            this.moveUpButton.TabIndex = 0;
            this.moveUpButton.Tag = "UP";
            this.moveUpButton.Text = "Up";
            this.moveUpButton.UseVisualStyleBackColor = true;
            this.moveUpButton.Click += new System.EventHandler(this.MoveButton_Click);
            //
            // moveLeftButton
            //
            this.moveLeftButton.Location = new System.Drawing.Point(24, 56);
            this.moveLeftButton.Name = "moveLeftButton";
            this.moveLeftButton.Size = new System.Drawing.Size(80, 28);
            this.moveLeftButton.TabIndex = 1;
            this.moveLeftButton.Tag = "LEFT";
            this.moveLeftButton.Text = "Left";
            this.moveLeftButton.UseVisualStyleBackColor = true;
            this.moveLeftButton.Click += new System.EventHandler(this.MoveButton_Click);
            //
            // moveRightButton
            //
            this.moveRightButton.Location = new System.Drawing.Point(196, 56);
            this.moveRightButton.Name = "moveRightButton";
            this.moveRightButton.Size = new System.Drawing.Size(80, 28);
            this.moveRightButton.TabIndex = 2;
            this.moveRightButton.Tag = "RIGHT";
            this.moveRightButton.Text = "Right";
            this.moveRightButton.UseVisualStyleBackColor = true;
            this.moveRightButton.Click += new System.EventHandler(this.MoveButton_Click);
            //
            // moveDownButton
            //
            this.moveDownButton.Location = new System.Drawing.Point(110, 90);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new System.Drawing.Size(80, 28);
            this.moveDownButton.TabIndex = 3;
            this.moveDownButton.Tag = "DOWN";
            this.moveDownButton.Text = "Down";
            this.moveDownButton.UseVisualStyleBackColor = true;
            this.moveDownButton.Click += new System.EventHandler(this.MoveButton_Click);
            //
            // combatGroupBox
            //
            this.combatGroupBox.Controls.Add(this.facingLabel);
            this.combatGroupBox.Controls.Add(this.facingComboBox);
            this.combatGroupBox.Controls.Add(this.attackButton);
            this.combatGroupBox.Location = new System.Drawing.Point(12, 266);
            this.combatGroupBox.Name = "combatGroupBox";
            this.combatGroupBox.Size = new System.Drawing.Size(300, 60);
            this.combatGroupBox.TabIndex = 2;
            this.combatGroupBox.TabStop = false;
            this.combatGroupBox.Text = "Combat";
            //
            // facingLabel
            //
            this.facingLabel.Location = new System.Drawing.Point(12, 27);
            this.facingLabel.Name = "facingLabel";
            this.facingLabel.Size = new System.Drawing.Size(50, 20);
            this.facingLabel.TabIndex = 0;
            this.facingLabel.Text = "Facing:";
            //
            // facingComboBox
            //
            this.facingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.facingComboBox.FormattingEnabled = true;
            this.facingComboBox.Items.AddRange(new object[] {
            "UP",
            "DOWN",
            "LEFT",
            "RIGHT"});
            this.facingComboBox.Location = new System.Drawing.Point(68, 24);
            this.facingComboBox.Name = "facingComboBox";
            this.facingComboBox.Size = new System.Drawing.Size(90, 23);
            this.facingComboBox.TabIndex = 1;
            //
            // attackButton
            //
            this.attackButton.Location = new System.Drawing.Point(168, 21);
            this.attackButton.Name = "attackButton";
            this.attackButton.Size = new System.Drawing.Size(120, 28);
            this.attackButton.TabIndex = 2;
            this.attackButton.Text = "Attack (peck)";
            this.attackButton.UseVisualStyleBackColor = true;
            this.attackButton.Click += new System.EventHandler(this.AttackButton_Click);
            //
            // inventoryGroupBox
            //
            this.inventoryGroupBox.Controls.Add(this.pickUpButton);
            this.inventoryGroupBox.Controls.Add(this.dropButton);
            this.inventoryGroupBox.Controls.Add(this.nestLabel);
            this.inventoryGroupBox.Controls.Add(this.nestListBox);
            this.inventoryGroupBox.Controls.Add(this.takeFromNestButton);
            this.inventoryGroupBox.Location = new System.Drawing.Point(12, 334);
            this.inventoryGroupBox.Name = "inventoryGroupBox";
            this.inventoryGroupBox.Size = new System.Drawing.Size(300, 190);
            this.inventoryGroupBox.TabIndex = 3;
            this.inventoryGroupBox.TabStop = false;
            this.inventoryGroupBox.Text = "Inventory";
            //
            // pickUpButton
            //
            this.pickUpButton.Location = new System.Drawing.Point(12, 22);
            this.pickUpButton.Name = "pickUpButton";
            this.pickUpButton.Size = new System.Drawing.Size(130, 28);
            this.pickUpButton.TabIndex = 0;
            this.pickUpButton.Text = "Pick up item";
            this.pickUpButton.UseVisualStyleBackColor = true;
            this.pickUpButton.Click += new System.EventHandler(this.PickUpButton_Click);
            //
            // dropButton
            //
            this.dropButton.Location = new System.Drawing.Point(158, 22);
            this.dropButton.Name = "dropButton";
            this.dropButton.Size = new System.Drawing.Size(130, 28);
            this.dropButton.TabIndex = 1;
            this.dropButton.Text = "Drop item";
            this.dropButton.UseVisualStyleBackColor = true;
            this.dropButton.Click += new System.EventHandler(this.DropButton_Click);
            //
            // nestLabel
            //
            this.nestLabel.Location = new System.Drawing.Point(12, 58);
            this.nestLabel.Name = "nestLabel";
            this.nestLabel.Size = new System.Drawing.Size(276, 18);
            this.nestLabel.TabIndex = 2;
            this.nestLabel.Text = "Items in your nest:";
            //
            // nestListBox
            //
            this.nestListBox.DisplayMember = "ItemName";
            this.nestListBox.FormattingEnabled = true;
            this.nestListBox.ItemHeight = 15;
            this.nestListBox.Location = new System.Drawing.Point(12, 78);
            this.nestListBox.Name = "nestListBox";
            this.nestListBox.Size = new System.Drawing.Size(170, 94);
            this.nestListBox.TabIndex = 3;
            this.nestListBox.ValueMember = "ItemID";
            //
            // takeFromNestButton
            //
            this.takeFromNestButton.Location = new System.Drawing.Point(188, 78);
            this.takeFromNestButton.Name = "takeFromNestButton";
            this.takeFromNestButton.Size = new System.Drawing.Size(100, 44);
            this.takeFromNestButton.TabIndex = 4;
            this.takeFromNestButton.Text = "Take selected from nest";
            this.takeFromNestButton.UseVisualStyleBackColor = true;
            this.takeFromNestButton.Click += new System.EventHandler(this.TakeFromNestButton_Click);
            //
            // statusLabel
            //
            this.statusLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.statusLabel.ForeColor = System.Drawing.Color.DarkRed;
            this.statusLabel.Location = new System.Drawing.Point(12, 532);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(300, 76);
            this.statusLabel.TabIndex = 4;
            //
            // boardGrid
            //
            this.boardGrid.AllowUserToAddRows = false;
            this.boardGrid.AllowUserToDeleteRows = false;
            this.boardGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.boardGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.boardGrid.Location = new System.Drawing.Point(324, 12);
            this.boardGrid.MultiSelect = false;
            this.boardGrid.Name = "boardGrid";
            this.boardGrid.ReadOnly = true;
            this.boardGrid.RowHeadersVisible = false;
            this.boardGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.boardGrid.Size = new System.Drawing.Size(604, 340);
            this.boardGrid.TabIndex = 5;
            //
            // worldGroupBox
            //
            this.worldGroupBox.Controls.Add(this.spawnItemButton);
            this.worldGroupBox.Controls.Add(this.moveNpcButton);
            this.worldGroupBox.Controls.Add(this.respawnNpcButton);
            this.worldGroupBox.Location = new System.Drawing.Point(324, 360);
            this.worldGroupBox.Name = "worldGroupBox";
            this.worldGroupBox.Size = new System.Drawing.Size(604, 62);
            this.worldGroupBox.TabIndex = 6;
            this.worldGroupBox.TabStop = false;
            this.worldGroupBox.Text = "World events (normally run by timers)";
            //
            // spawnItemButton
            //
            this.spawnItemButton.Location = new System.Drawing.Point(12, 22);
            this.spawnItemButton.Name = "spawnItemButton";
            this.spawnItemButton.Size = new System.Drawing.Size(190, 28);
            this.spawnItemButton.TabIndex = 0;
            this.spawnItemButton.Text = "Spawn random item";
            this.spawnItemButton.UseVisualStyleBackColor = true;
            this.spawnItemButton.Click += new System.EventHandler(this.SpawnItemButton_Click);
            //
            // moveNpcButton
            //
            this.moveNpcButton.Location = new System.Drawing.Point(210, 22);
            this.moveNpcButton.Name = "moveNpcButton";
            this.moveNpcButton.Size = new System.Drawing.Size(190, 28);
            this.moveNpcButton.TabIndex = 1;
            this.moveNpcButton.Text = "Move the NPC one step";
            this.moveNpcButton.UseVisualStyleBackColor = true;
            this.moveNpcButton.Click += new System.EventHandler(this.MoveNpcButton_Click);
            //
            // respawnNpcButton
            //
            this.respawnNpcButton.Location = new System.Drawing.Point(408, 22);
            this.respawnNpcButton.Name = "respawnNpcButton";
            this.respawnNpcButton.Size = new System.Drawing.Size(184, 28);
            this.respawnNpcButton.TabIndex = 2;
            this.respawnNpcButton.Text = "Respawn the NPC";
            this.respawnNpcButton.UseVisualStyleBackColor = true;
            this.respawnNpcButton.Click += new System.EventHandler(this.RespawnNpcButton_Click);
            //
            // accountGroupBox
            //
            this.accountGroupBox.Controls.Add(this.refreshButton);
            this.accountGroupBox.Controls.Add(this.logoutButton);
            this.accountGroupBox.Controls.Add(this.deleteAccountButton);
            this.accountGroupBox.Location = new System.Drawing.Point(324, 430);
            this.accountGroupBox.Name = "accountGroupBox";
            this.accountGroupBox.Size = new System.Drawing.Size(604, 62);
            this.accountGroupBox.TabIndex = 7;
            this.accountGroupBox.TabStop = false;
            this.accountGroupBox.Text = "Account";
            //
            // refreshButton
            //
            this.refreshButton.Location = new System.Drawing.Point(12, 22);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(110, 28);
            this.refreshButton.TabIndex = 0;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            //
            // logoutButton
            //
            this.logoutButton.Location = new System.Drawing.Point(130, 22);
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(230, 28);
            this.logoutButton.TabIndex = 1;
            this.logoutButton.Text = "Back to login (log out)";
            this.logoutButton.UseVisualStyleBackColor = true;
            this.logoutButton.Click += new System.EventHandler(this.LogoutButton_Click);
            //
            // deleteAccountButton
            //
            this.deleteAccountButton.Location = new System.Drawing.Point(368, 22);
            this.deleteAccountButton.Name = "deleteAccountButton";
            this.deleteAccountButton.Size = new System.Drawing.Size(224, 28);
            this.deleteAccountButton.TabIndex = 2;
            this.deleteAccountButton.Text = "Delete my account";
            this.deleteAccountButton.UseVisualStyleBackColor = true;
            this.deleteAccountButton.Click += new System.EventHandler(this.DeleteAccountButton_Click);
            //
            // logListBox
            //
            this.logListBox.FormattingEnabled = true;
            this.logListBox.HorizontalScrollbar = true;
            this.logListBox.ItemHeight = 15;
            this.logListBox.Location = new System.Drawing.Point(324, 500);
            this.logListBox.Name = "logListBox";
            this.logListBox.Size = new System.Drawing.Size(604, 109);
            this.logListBox.TabIndex = 8;
            //
            // GamePlayForm
            //
            this.ClientSize = new System.Drawing.Size(940, 620);
            this.Controls.Add(this.playerGroupBox);
            this.Controls.Add(this.moveGroupBox);
            this.Controls.Add(this.combatGroupBox);
            this.Controls.Add(this.inventoryGroupBox);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.boardGrid);
            this.Controls.Add(this.worldGroupBox);
            this.Controls.Add(this.accountGroupBox);
            this.Controls.Add(this.logListBox);
            this.Name = "GamePlayForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ultimate Goose Fighter - Game Play";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GamePlayForm_FormClosed);
            this.Load += new System.EventHandler(this.GamePlayForm_Load);
            this.playerGroupBox.ResumeLayout(false);
            this.moveGroupBox.ResumeLayout(false);
            this.combatGroupBox.ResumeLayout(false);
            this.inventoryGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.boardGrid)).EndInit();
            this.worldGroupBox.ResumeLayout(false);
            this.accountGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}