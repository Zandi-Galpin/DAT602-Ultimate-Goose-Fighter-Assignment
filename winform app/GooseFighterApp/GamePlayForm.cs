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
    /// Display area for Game Play.
    /// This currently only tests player movement only 
    /// </summary>
    public partial class GamePlayForm : Form
    {
        // Set by LoginForm right after construction, example:
        //   var form = new GamePlayForm { PlayerId = playerId };
        //   form.Show();
        public int PlayerId { get; set; }

        private readonly GamePlayDao _gamePlayDao = new GamePlayDao();

        public GamePlayForm()
        {
            InitializeComponent();
            this.Load += GamePlayForm_Load;
        }

        private void GamePlayForm_Load(object sender, EventArgs e)
        {         
            titleLabel.Text = "GAME PLAY (Player ID " + PlayerId + ")";
        }

        private void MoveButton_Click(object sender, EventArgs e)
        {
            try
            {
                int newTileId = (int)tileInput.Value;
                int rowsAffected = _gamePlayDao.MovePlayer(PlayerId, newTileId);
                statusLabel.ForeColor = Color.Green;
                statusLabel.Text = rowsAffected > 0
                    ? "Moved to tile " + newTileId + "."
                    : "No matching player found.";
            }
            catch (Exception ex)
            {
                statusLabel.ForeColor = Color.DarkRed;
                statusLabel.Text = "Error: " + ex.Message;
            }
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            new LoginForm().Show();
            this.Close();
        }
    }
}