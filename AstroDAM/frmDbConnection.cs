using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data.SqlClient;

namespace AstroDAM
{
    public partial class DbConnectionForm : Form
    {
        readonly BackgroundWorker bgwConnectionTester = new BackgroundWorker();
        readonly bool requestRestart = false;

        /// <summary>
        /// Load the database connection configuration form.
        /// </summary>
        /// <param name="requestRestart">Whether to request the user restart the application
        /// to apply the connection settings.</param>
        public DbConnectionForm(bool requestRestart)
        {
            InitializeComponent();

            this.requestRestart = requestRestart;

            bgwConnectionTester.DoWork += BgwConnectionTester_DoWork;
            bgwConnectionTester.RunWorkerCompleted += BgwConnectionTester_RunWorkerCompleted;

            tbConnectionString.Text = Properties.Settings.Default.ConnectionString;
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
           bgwConnectionTester.RunWorkerAsync(tbConnectionString.Text);
        }

        /// <summary>
        /// Used to validate the connection string.
        /// Called when a connection field value has changed.
        /// </summary>
        private void ParametersChanged(object sender, EventArgs e)
        {
            // attempt to build a connection string and put it in the textbox.

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                ["Data Source"] = tbDataSource.Text,
                ["Integrated Security"] = rbSecurityYes.Checked,
                ["Initial Catalog"] = tbDatabaseFile.Text
            };

            if (int.TryParse(tbConnectionTimeout.Text, out int ConnectTimeout))
            {
                builder["Connect Timeout"] = ConnectTimeout;
            }
            else
            {
                builder["Connect Timeout"] = 30;
            }

            tbConnectionString.Text = builder.ConnectionString;
        }

        private void BgwConnectionTester_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((bool)e.Result)
            {
                lblTestResult.Text = "Test successfull.";
                btnSave.Enabled = true;
            }
            else
            {
                lblTestResult.Text = "Test failed.";
                btnSave.Enabled = false;
            }
        }

        private void BgwConnectionTester_DoWork(object sender, DoWorkEventArgs e)
        {
            bool dbTestResult = DbManager.TestConnection(e.Argument.ToString());

            e.Result = dbTestResult;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ConnectionString = tbConnectionString.Text;
            Properties.Settings.Default.Save();

            MessageBox.Show("Settings updated." + (requestRestart ? "Please restart the application." : ""), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Close();
        }
    }
}
