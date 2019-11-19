using AstroDAM.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AstroDAM
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void lnkHelp1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(this, "Format is in Universal Time (UT) format.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save states & UI preferences
            if (WindowState == FormWindowState.Maximized)
            {
                Settings.Default.Location = RestoreBounds.Location;
                Settings.Default.Size = RestoreBounds.Size;
                Settings.Default.Maximised = true;
                Settings.Default.Minimised = false;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                Settings.Default.Location = Location;
                Settings.Default.Size = Size;
                Settings.Default.Maximised = false;
                Settings.Default.Minimised = false;
            }
            else
            {
                Settings.Default.Location = RestoreBounds.Location;
                Settings.Default.Size = RestoreBounds.Size;
                Settings.Default.Maximised = false;
                Settings.Default.Minimised = true;
            }
            Settings.Default.Save();

            Settings.Default.SplitterPositions = string.Join(";",
                     Controls.OfType<SplitContainer>()
                             .Select(s => s.SplitterDistance));

            Settings.Default.Save();
            Application.Exit();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Load states & UI preferences
            if (Settings.Default.Maximised)
            {
                WindowState = FormWindowState.Maximized;
                Location = Settings.Default.Location;
                Size = Settings.Default.Size;
            }
            else if (Settings.Default.Minimised)
            {
                WindowState = FormWindowState.Minimized;
                Location = Settings.Default.Location;
                Size = Settings.Default.Size;
            }
            else
            {
                Location = Settings.Default.Location;
                Size = Settings.Default.Size;
            }

            if (!string.IsNullOrEmpty(Settings.Default.SplitterPositions))
            {
                var positions = Settings.Default.SplitterPositions
                                        .Split(';')
                                        .Select(int.Parse).ToList();

                var splitContainers = Controls.OfType<SplitContainer>().ToList();

                for (var x = 0; x < positions.Count && x < splitContainers.Count; x++)
                {
                    splitContainers[x].SplitterDistance = positions[x];
                }
            }            
        }

        private void databaseConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmDbConnection().ShowDialog();
        }

        private void camerasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.Cameras).ShowDialog();
        }

        private void cataloguesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.Catalogues).ShowDialog();
        }

        private void colorSpacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.ColorSpaces).ShowDialog();

        }

        private void formatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.FileFormats).ShowDialog();
        }

        private void opticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.Optics).ShowDialog();
        }

        private void photographersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.Photographers).ShowDialog();
        }

        private void scopesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.Scopes).ShowDialog();
        }

        private void sitesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.Sites).ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to quit?", "Quitting?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}