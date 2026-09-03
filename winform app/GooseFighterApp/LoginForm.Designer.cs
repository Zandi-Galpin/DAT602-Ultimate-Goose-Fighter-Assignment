namespace GooseFighterApp
{
    partial class LoginForm
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
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Button testConnectionButton;
        private System.Windows.Forms.Label statusLabel;

        private void InitializeComponent()
        {
            this.titleLabel = new System.Windows.Forms.Label();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.testConnectionButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // titleLabel
            //
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.titleLabel.Location = new System.Drawing.Point(20, 20);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(340, 45);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "ULTIMATE GOOSE FIGHTER\r\nLogin and Registration";
            //
            // usernameLabel
            //
            this.usernameLabel.Location = new System.Drawing.Point(20, 80);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(100, 23);
            this.usernameLabel.TabIndex = 1;
            this.usernameLabel.Text = "Username:";
            //
            // usernameTextBox
            //
            this.usernameTextBox.Location = new System.Drawing.Point(130, 78);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(200, 23);
            this.usernameTextBox.TabIndex = 2;
            //
            // passwordLabel
            //
            this.passwordLabel.Location = new System.Drawing.Point(20, 115);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(100, 23);
            this.passwordLabel.TabIndex = 3;
            this.passwordLabel.Text = "Password:";
            //
            // passwordTextBox
            //
            this.passwordTextBox.Location = new System.Drawing.Point(130, 113);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(200, 23);
            this.passwordTextBox.TabIndex = 4;
            //
            // loginButton
            //
            this.loginButton.Location = new System.Drawing.Point(130, 150);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(100, 30);
            this.loginButton.TabIndex = 5;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.LoginButton_Click);
            //
            // testConnectionButton
            //
            this.testConnectionButton.Location = new System.Drawing.Point(130, 185);
            this.testConnectionButton.Name = "testConnectionButton";
            this.testConnectionButton.Size = new System.Drawing.Size(150, 30);
            this.testConnectionButton.TabIndex = 6;
            this.testConnectionButton.Text = "Test DB Connection";
            this.testConnectionButton.UseVisualStyleBackColor = true;
            this.testConnectionButton.Click += new System.EventHandler(this.TestConnectionButton_Click);
            //
            // statusLabel
            //
            this.statusLabel.ForeColor = System.Drawing.Color.DarkRed;
            this.statusLabel.Location = new System.Drawing.Point(20, 220);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(340, 40);
            this.statusLabel.TabIndex = 7;
            //
            // LoginForm
            //
            this.ClientSize = new System.Drawing.Size(384, 271);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.usernameTextBox);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.testConnectionButton);
            this.Controls.Add(this.statusLabel);
            this.Name = "LoginForm";
            this.Text = "Ultimate Goose Fighter - Login";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}