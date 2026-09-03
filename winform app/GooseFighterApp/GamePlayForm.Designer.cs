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

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label tileLabel;
        private System.Windows.Forms.NumericUpDown tileInput;
        private System.Windows.Forms.Button moveButton;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Label statusLabel;

        private void InitializeComponent()
        {
            this.titleLabel = new System.Windows.Forms.Label();
            this.tileLabel = new System.Windows.Forms.Label();
            this.tileInput = new System.Windows.Forms.NumericUpDown();
            this.moveButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tileInput)).BeginInit();
            this.SuspendLayout();
            //
            // titleLabel
            //
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.titleLabel.Location = new System.Drawing.Point(20, 20);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(340, 30);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "GAME PLAY";
            //
            // tileLabel
            //
            this.tileLabel.Location = new System.Drawing.Point(20, 70);
            this.tileLabel.Name = "tileLabel";
            this.tileLabel.Size = new System.Drawing.Size(120, 23);
            this.tileLabel.TabIndex = 1;
            this.tileLabel.Text = "Move to TileID:";
            //
            // tileInput
            //
            this.tileInput.Location = new System.Drawing.Point(150, 68);
            this.tileInput.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.tileInput.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.tileInput.Name = "tileInput";
            this.tileInput.Size = new System.Drawing.Size(100, 23);
            this.tileInput.TabIndex = 2;
            this.tileInput.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // moveButton
            //
            this.moveButton.Location = new System.Drawing.Point(150, 105);
            this.moveButton.Name = "moveButton";
            this.moveButton.Size = new System.Drawing.Size(100, 30);
            this.moveButton.TabIndex = 3;
            this.moveButton.Text = "Move";
            this.moveButton.UseVisualStyleBackColor = true;
            this.moveButton.Click += new System.EventHandler(this.MoveButton_Click);
            //
            // backButton
            //
            this.backButton.Location = new System.Drawing.Point(150, 145);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(150, 30);
            this.backButton.TabIndex = 4;
            this.backButton.Text = "Back to Login";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.BackButton_Click);
            //
            // statusLabel
            //
            this.statusLabel.ForeColor = System.Drawing.Color.DarkRed;
            this.statusLabel.Location = new System.Drawing.Point(20, 190);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(340, 40);
            this.statusLabel.TabIndex = 5;
            //
            // GamePlayForm
            //
            this.ClientSize = new System.Drawing.Size(384, 253);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.tileLabel);
            this.Controls.Add(this.tileInput);
            this.Controls.Add(this.moveButton);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.statusLabel);
            this.Name = "GamePlayForm";
            this.Text = "Ultimate Goose Fighter - Game Play";
            ((System.ComponentModel.ISupportInitialize)(this.tileInput)).EndInit();
            this.ResumeLayout(false);
        }
    }
}