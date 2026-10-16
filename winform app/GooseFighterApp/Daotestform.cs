using System;
using System.Drawing;
using System.Windows.Forms;

namespace GooseFighterApp
{
    /// <summary>
    /// Developer screen that runs the DAO tests and shows the results.
    /// Opened by starting the app with --daotests
    /// </summary>
    public class DaoTestForm : Form
    {
        private readonly Button _runButton = new Button();
        private readonly RichTextBox _output = new RichTextBox();

        public DaoTestForm()
        {
            Text = "Ultimate Goose Fighter - DAO tests";
            ClientSize = new Size(900, 600);

            _runButton.Text = "Rebuild test database and run all DAO tests";
            _runButton.Dock = DockStyle.Top;
            _runButton.Height = 36;
            _runButton.Click += RunButton_Click;

            _output.Dock = DockStyle.Fill;
            _output.ReadOnly = true;
            _output.WordWrap = false;
            _output.Font = new Font("Consolas", 9F);

            Controls.Add(_output);
            Controls.Add(_runButton);
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show(
                "This drops and remakes GooseFighterDB with the test data. Continue?",
                "Run DAO tests", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            _runButton.Enabled = false;
            Cursor = Cursors.WaitCursor;
            try
            {
                _output.Text = new DaoTests().RunAll();
            }
            catch (Exception ex)
            {
                _output.Text = "Test run failed: " + ex.Message;
            }
            finally
            {
                Cursor = Cursors.Default;
                _runButton.Enabled = true;
            }
        }
    }
}