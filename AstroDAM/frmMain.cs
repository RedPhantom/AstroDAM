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
    }
}