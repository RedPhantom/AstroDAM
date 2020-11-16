using System;
using System.Windows.Forms;
using AstroDAM.Controllers;

namespace AstroDAM
{
    public partial class frmPreferences : Form
    {
        public frmPreferences()
        {
            InitializeComponent();
        }

        private void btnBbDateTime_Click(object sender, EventArgs e)
        {
            tbEndNodeFormat.Text += "{dt|hh:mm:ss}";
        }

        private void btnBbObject_Click(object sender, EventArgs e)
        {
            tbEndNodeFormat.Text += "{o|n}";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // test the navtree building:
            try
            {
                TreeViewController.PopulateTreeView(ref treeView1, true, tbEndNodeFormat.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid values.");
                return;
            }

            if (treeView1.Nodes != null)
            {
                if (treeView1.Nodes.Count == 0)
                {
                    MessageBox.Show("No nodes to populate.");
                    return;
                }
            } else
            {
                MessageBox.Show("No nodes to populate.");
                return;
            }

            Properties.Preferences.Default.TreeNodeFormat = tbEndNodeFormat.Text;
            Properties.Preferences.Default.PlaySplashClip = cbPlaySplashClip.Checked;

            if (cbNodeGrouping.SelectedIndex != -1)
                Properties.Preferences.Default.NodeGrouping = cbNodeGrouping.SelectedIndex;

            Properties.Preferences.Default.Save();
        }

        private void btnBbCollection_Click(object sender, EventArgs e)
        {
            tbEndNodeFormat.Text += "{c|sn}";
        }

        private void frmPreferences_Load(object sender, EventArgs e)
        {
            tbEndNodeFormat.Text = Properties.Preferences.Default.TreeNodeFormat;
            cbPlaySplashClip.Checked = Properties.Preferences.Default.PlaySplashClip;
            cbNodeGrouping.SelectedIndex = Properties.Preferences.Default.NodeGrouping;
        }

        private void btnTestTree_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            TreeViewController.PopulateTreeView(ref treeView1, true, tbEndNodeFormat.Text);
        }
    }
}
