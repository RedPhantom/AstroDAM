using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using AstroDAM.Properties;

namespace AstroDAM
{
    public partial class frmSplash : Form
    {
        public frmSplash()
        {
            InitializeComponent();

            if (Properties.Preferences.Default.PlaySplashClip)
                pictureBox1.Image = Properties.Resources.SplashSquence;

            BackgroundWorker bgwStartupCheck = new BackgroundWorker();

            bgwStartupCheck.DoWork += BgwStartupCheck_DoWork;
            bgwStartupCheck.RunWorkerCompleted += BgwStartupCheck_RunWorkerCompleted;

            bool ShowAllSequence = ModifierKeys.HasFlag(Keys.Shift);

            bgwStartupCheck.RunWorkerAsync(ShowAllSequence);

            // Show a random splash message.
            string[] CaptionMessages =
            {
                "Damn.",
                "Quite stunning.",
                "It's really great, actually.",
                "Am I so very happy.",
                "Ever listened to ABBA?",
                "Sweet dreams are seven seas...",
                "THANK YOU StackOverflow.",
                "I heard this song without lyrics, can't find its name...",
                "Play video games, it's cool.",
                "I stand with Hong Kong.",
                "(random message here)",
                "v420.69. Nice.",
                "You're really pretty.",
                "Stay hydrated!",
                "Ask for help and I'll be there.",
                "I'll be there for youuuu.....",
                "At first I was afraid, I was petrified...",
                "And so yeah, I am back, from outer space!",
                "x32 AND x64! I know, right?"
            };

            Random rnd = new Random();

            lblCaption.Text = CaptionMessages[rnd.Next(0, CaptionMessages.Length)];
        }

        private void BgwStartupCheck_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            switch ((int)e.Result)
            {
                // Db connection failed.
                case -1:
                    new frmDbConnection().ShowDialog();
                    Application.Exit();

                    break;

                // Startup successfull.
                case 0:
                    TopMost = false;

                    frmMain frmMain = new frmMain();
                    frmMain.Show();
                    frmMain.Focus();

                    Close();
                    Dispose();
                    break;

                default:
                    break;
            }
        }

        private void BgwStartupCheck_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bgw = (BackgroundWorker)sender;
            System.Threading.Thread.Sleep(1000); // show the splash so the user doest panic over a flashing form.

            // Test db connection:
            UpdateStatusLabel("Testing database connection...");
            bool dbTestResult = Operations.TestConnection();

            if (!dbTestResult)
            {
                e.Result = -1;
                UpdateStatusLabel("Database connection failed.");
            }

            UpdateStatusLabel("All good in the hood." + ((bool)e.Argument ? " Showing film." : ""));
            
            // If we are requested to show the entire startup sequence.
            if ((bool)e.Argument)
            {
                System.Threading.Thread.Sleep(22500);
                pictureBox1.Image = Resources.SplashSquence;
            }
            else
            {
                System.Threading.Thread.Sleep(200);
            }

            e.Result = 0;
        }

        private void UpdateStatusLabel(string Message)
        {
            lblStatus.Invoke((MethodInvoker)(() => lblStatus.Text = Message)); // https://stackoverflow.com/questions/2172467/set-value-of-label-with-c-sharp-cross-threading
        }

        private void frmSplash_Load(object sender, EventArgs e)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            lblVersion.Text = version;
        }
    }
}
