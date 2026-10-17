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
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.TextBox emailTextBox;
        private System.Windows.Forms.Label registerHintLabel;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Button registerButton;
        private System.Windows.Forms.Button testConnectionButton;
        private System.Windows.Forms.Label statusLabel;

        private void InitializeComponent()
        {
            this.titleLabel = new System.Windows.Forms.Label();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.emailLabel = new System.Windows.Forms.Label();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.registerHintLabel = new System.Windows.Forms.Label();
            this.loginButton = new System.Windows.Forms.Button();
            this.registerButton = new System.Windows.Forms.Button();
            this.testConnectionButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // titleLabel
            //
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.titleLabel.Location = new System.Drawing.Point(20, 15);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(340, 45);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "ULTIMATE GOOSE FIGHTER\r\nLogin and Registration";
            //
            // usernameLabel
            //
            this.usernameLabel.Location = new System.Drawing.Point(20, 75);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(100, 23);
            this.usernameLabel.TabIndex = 1;
            this.usernameLabel.Text = "Username:";
            //
            // usernameTextBox
            //
            this.usernameTextBox.Location = new System.Drawing.Point(130, 73);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(220, 23);
            this.usernameTextBox.TabIndex = 2;
            //
            // passwordLabel
            //
            this.passwordLabel.Location = new System.Drawing.Point(20, 110);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(100, 23);
            this.passwordLabel.TabIndex = 3;
            this.passwordLabel.Text = "Password:";
            //
            // passwordTextBox
            //
            this.passwordTextBox.Location = new System.Drawing.Point(130, 108);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(220, 23);
            this.passwordTextBox.TabIndex = 4;
            //
            // emailLabel
            //
            this.emailLabel.Location = new System.Drawing.Point(20, 145);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(100, 23);
            this.emailLabel.TabIndex = 5;
            this.emailLabel.Text = "Email:";
            //
            // emailTextBox
            //
            this.emailTextBox.Location = new System.Drawing.Point(130, 143);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(220, 23);
            this.emailTextBox.TabIndex = 6;
            //
            // registerHintLabel
            //
            this.registerHintLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.registerHintLabel.Location = new System.Drawing.Point(130, 169);
            this.registerHintLabel.Name = "registerHintLabel";
            this.registerHintLabel.Size = new System.Drawing.Size(220, 18);
            this.registerHintLabel.TabIndex = 7;
            this.registerHintLabel.Text = "Only needed when registering";
            //
            // loginButton
            //
            this.loginButton.Location = new System.Drawing.Point(130, 198);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(100, 30);
            this.loginButton.TabIndex = 8;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.LoginButton_Click);
            //
            // registerButton
            //
            this.registerButton.Location = new System.Drawing.Point(240, 198);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(110, 30);
            this.registerButton.TabIndex = 9;
            this.registerButton.Text = "Register";
            this.registerButton.UseVisualStyleBackColor = true;
            this.registerButton.Click += new System.EventHandler(this.RegisterButton_Click);
            //
            // testConnectionButton
            //
            this.testConnectionButton.Location = new System.Drawing.Point(130, 238);
            this.testConnectionButton.Name = "testConnectionButton";
            this.testConnectionButton.Size = new System.Drawing.Size(150, 30);
            this.testConnectionButton.TabIndex = 10;
            this.testConnectionButton.Text = "Test DB Connection";
            this.testConnectionButton.UseVisualStyleBackColor = true;
            this.testConnectionButton.Click += new System.EventHandler(this.TestConnectionButton_Click);
            //
            // statusLabel
            //
            this.statusLabel.ForeColor = System.Drawing.Color.DarkRed;
            this.statusLabel.Location = new System.Drawing.Point(20, 282);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(340, 56);
            this.statusLabel.TabIndex = 11;
            //
            // LoginForm
            //
            this.AcceptButton = this.loginButton;
            this.ClientSize = new System.Drawing.Size(384, 351);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.usernameTextBox);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.emailLabel);
            this.Controls.Add(this.emailTextBox);
            this.Controls.Add(this.registerHintLabel);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.registerButton);
            this.Controls.Add(this.testConnectionButton);
            this.Controls.Add(this.statusLabel);
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ultimate Goose Fighter - Login";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}