using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab_12_Form_BP
{
    public partial class Form1 : Form
    {
        private Label labelPrompt;
        private TextBox textBoxTime;
        private Button buttonStart;
        private System.Windows.Forms.Timer timer;
        private TimeSpan targetTime;
        private Random rng = new Random();

        public Form1()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // --- Form settings ---
            this.Text = "Alarm Clock";
            this.ClientSize = new Size(320, 120);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // --- Label ---
            labelPrompt = new Label()
            {
                Text = "Alarm time (HH:mm:ss):",
                Location = new Point(10, 15),
                AutoSize = true
            };

            // --- TextBox ---
            textBoxTime = new TextBox()
            {
                Location = new Point(labelPrompt.Right + 40, 12),
                Width = 100
            };

            // --- Button ---
            buttonStart = new Button()
            {
                Text = "Start",
                Location = new Point(10, 50),
                AutoSize = true
            };
            buttonStart.Click += ButtonStart_Click;

            // --- Timer ---
            timer = new System.Windows.Forms.Timer()
            {
                Interval = 1000,  // 1 second
                Enabled = false
            };
            timer.Tick += Timer_Tick;

            // --- Add Controls to Form ---
            this.Controls.Add(labelPrompt);
            this.Controls.Add(textBoxTime);
            this.Controls.Add(buttonStart);
        }

        private void ButtonStart_Click(object? sender, EventArgs e)
        {
            // Validate HH:mm:ss
            if (!TimeSpan.TryParseExact(
                    textBoxTime.Text.Trim(),
                    "hh\\:mm\\:ss",
                    null,
                    out targetTime))
            {
                MessageBox.Show(
                    "Please enter a valid time in HH:MM:SS format.",
                    "Invalid Input",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Disable inputs and start color‑changing timer
            textBoxTime.Enabled = false;
            buttonStart.Enabled = false;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            // Change background to a random color each tick
            this.BackColor = Color.FromArgb(
                rng.Next(256),
                rng.Next(256),
                rng.Next(256));

            // Check if we've reached (or passed) the target
            if (DateTime.Now.TimeOfDay >= targetTime)
            {
                timer.Stop();

                // Reset to default background
                this.BackColor = SystemColors.Control;

                // Show alarm message
                MessageBox.Show(
                    "⏰ Alarm! Time's up! ⏰",
                    "Alarm",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Re‑enable inputs for a new alarm
                textBoxTime.Enabled = true;
                buttonStart.Enabled = true;
            }
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}