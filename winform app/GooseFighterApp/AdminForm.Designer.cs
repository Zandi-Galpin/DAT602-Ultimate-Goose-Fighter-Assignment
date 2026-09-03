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

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.TextBox emailTextBox;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.CheckBox isAdminCheckBox;
        private System.Windows.Forms.Button addPlayerButton;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Label statusLabel;

        private void InitializeComponent()
        {
            this.titleLabel = new System.Windows.Forms.Label();
            this.emailLabel = new System.Windows.Forms.Label();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.isAdminCheckBox = new System.Windows.Forms.CheckBox();
            this.addPlayerButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // titleLabel
            //
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.titleLabel.Location = new System.Drawing.Point(20, 20);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(360, 30);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "ADMINISTRATION - Add New Player";
            //
            // emailLabel
            //
            this.emailLabel.Location = new System.Drawing.Point(20, 70);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(100, 23);
            this.emailLabel.TabIndex = 1;
            this.emailLabel.Text = "Email:";
            //
            // emailTextBox
            //
            this.emailTextBox.Location = new System.Drawing.Point(130, 68);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(220, 23);
            this.emailTextBox.TabIndex = 2;
            //
            // usernameLabel
            //
            this.usernameLabel.Location = new System.Drawing.Point(20, 105);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(100, 23);
            this.usernameLabel.TabIndex = 3;
            this.usernameLabel.Text = "Username:";
            //
            // usernameTextBox
            //
            this.usernameTextBox.Location = new System.Drawing.Point(130, 103);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(220, 23);
            this.usernameTextBox.TabIndex = 4;
            //
            // passwordLabel
            //
            this.passwordLabel.Location = new System.Drawing.Point(20, 140);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(100, 23);
            this.passwordLabel.TabIndex = 5;
            this.passwordLabel.Text = "Password:";
            //
            // passwordTextBox
            //
            this.passwordTextBox.Location = new System.Drawing.Point(130, 138);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(220, 23);
            this.passwordTextBox.TabIndex = 6;
            //
            // isAdminCheckBox
            //
            this.isAdminCheckBox.Location = new System.Drawing.Point(130, 175);
            this.isAdminCheckBox.Name = "isAdminCheckBox";
            this.isAdminCheckBox.Size = new System.Drawing.Size(100, 24);
            this.isAdminCheckBox.TabIndex = 7;
            this.isAdminCheckBox.Text = "Admin?";
            this.isAdminCheckBox.UseVisualStyleBackColor = true;
            //
            // addPlayerButton
            //
            this.addPlayerButton.Location = new System.Drawing.Point(130, 210);
            this.addPlayerButton.Name = "addPlayerButton";
            this.addPlayerButton.Size = new System.Drawing.Size(220, 30);
            this.addPlayerButton.TabIndex = 8;
            this.addPlayerButton.Text = "Add Player to Database";
            this.addPlayerButton.UseVisualStyleBackColor = true;
            this.addPlayerButton.Click += new System.EventHandler(this.AddPlayerButton_Click);
            //
            // backButton
            //
            this.backButton.Location = new System.Drawing.Point(130, 250);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(150, 30);
            this.backButton.TabIndex = 9;
            this.backButton.Text = "Back to Login";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.BackButton_Click);
            //
            // statusLabel
            //
            this.statusLabel.ForeColor = System.Drawing.Color.DarkRed;
            this.statusLabel.Location = new System.Drawing.Point(20, 290);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(360, 40);
            this.statusLabel.TabIndex = 10;
            //
            // AdminForm
            //
            this.ClientSize = new System.Drawing.Size(404, 353);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.emailLabel);
            this.Controls.Add(this.emailTextBox);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.usernameTextBox);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.isAdminCheckBox);
            this.Controls.Add(this.addPlayerButton);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.statusLabel);
            this.Name = "AdminForm";
            this.Text = "Ultimate Goose Fighter - Administration";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}