using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data.SqlClient;

namespace AstroDAM
{
    public partial class frmDbConnection : Form
    {
        BackgroundWorker bgwConnectionTester = new BackgroundWorker();

        public frmDbConnection()
        {
            InitializeComponent();
        
            bgwConnectionTester.DoWork += BgwConnectionTester_DoWork;
            bgwConnectionTester.RunWorkerCompleted += BgwConnectionTester_RunWorkerCompleted;

            tbConnectionString.Text = Properties.Settings.Default.ConnectionString;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
           bgwConnectionTester.RunWorkerAsync(tbConnectionString.Text);
        }

        private void ParametersChanged(object sender, EventArgs e)
        {
            int ConnectTimeout;

            // attempt to build a connection string and put it in the textbox.

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder["Data Source"] = tbDataSource.Text;
            builder["Integrated Security"] = rbSecurityYes.Checked;
            builder["Initial Catalog"] = tbDatabaseFile.Text;

            if (int.TryParse(tbConnectionTimeout.Text, out ConnectTimeout))
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
            BackgroundWorker bgw = (BackgroundWorker)sender;
            bool dbTestResult = DbManager.TestConnection(e.Argument.ToString());

            e.Result = dbTestResult;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ConnectionString = tbConnectionString.Text;
            Properties.Settings.Default.Save();

            MessageBox.Show("Settings updated. Please restart the application.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}
