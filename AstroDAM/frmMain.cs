using AstroDAM.Models;
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
        List<Camera> listCameras;
        List<Catalogue> listCatalogues;
        List<ColorSpace> listColorSpaces;
        List<FileFormat> listFileFormats;
        List<Optic> listOptics;
        List<Photographer> listPhotographers;
        List<Scope> listScopes;
        List<Site> listSites;

        public frmMain()
        {
            InitializeComponent();
        }

        frmManager.EditingModes EditingMode = frmManager.EditingModes.Add;

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
            lblStatus.Text = "Adaptig to preferences...";

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

            LoadData();
        }

        private void LoadData()
        {
            listCameras = Operations.GetCameras();
            listCatalogues = Operations.GetCatalogues();
            listColorSpaces = Operations.GetColorSpaces();
            listFileFormats = Operations.GetFileFormats();
            listOptics = Operations.GetOptics();
            listPhotographers = Operations.GetPhotographers();
            listScopes = Operations.GetScopes();
            listSites = Operations.GetSites();

            tbCollectionUuid.Text = GenerateUUID();

            cbCamera.Items.Clear();
            foreach (var item in listCameras)
                cbCamera.Items.Add(item.LongName);

            cbCatalogue.Items.Clear();
            foreach (var item in listCatalogues)
                cbCatalogue.Items.Add(item.LongName);

            cbColorSpace.Items.Clear();
            foreach (var item in listColorSpaces)
                cbColorSpace.Items.Add(item.Name);

            cbFileFormat.Items.Clear();
            foreach (var item in listFileFormats)
                cbFileFormat.Items.Add(item.LongName);

            cbPhotographers.Items.Clear();
            foreach (var item in listPhotographers)
                cbPhotographers.Items.Add(item.GetInformalName());

            cbScope.Items.Clear();
            foreach (var item in listScopes)
                cbScope.Items.Add(item.GetScopeName());

            clbOptics.Items.Clear();
            foreach (var item in listOptics)
                clbOptics.Items.Add(item.Id + "|  " + item.GetOpticName());

            cbSites.Items.Clear();
            foreach (var item in listSites)
                cbSites.Items.Add(item.Name);

            tvCollections.Nodes.Clear();
            Operations.PopulateTreeView(ref tvCollections, ascendingToolStripMenuItem.Checked);

            lblStatus.Text = "Loaded all assets.";
        }

        private string GenerateUUID()
        {
            // Regenerates a UUID.
            byte[] uuid = System.Guid.NewGuid().ToByteArray();
            string uuidString = Convert.ToBase64String(uuid).Replace("=", "").Replace("+", "").Replace("/", "").Substring(0, 16);

            return uuidString;
        }

        // Buttons
        private void btnRegenerateUUID_Click(object sender, EventArgs e)
        {
            tbCollectionUuid.Text = GenerateUUID();
        }

        // Toolstrip
        private void databaseConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmDbConnection().ShowDialog();
        }

        private void camerasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.Cameras).ShowDialog();
            LoadData();
        }

        private void cataloguesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.Catalogues).ShowDialog();
            LoadData();
        }

        private void colorSpacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.ColorSpaces).ShowDialog();
            LoadData();
        }

        private void formatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.FileFormats).ShowDialog();
            LoadData();
        }

        private void opticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.Optics).ShowDialog();
            LoadData();
        }

        private void photographersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.Photographers).ShowDialog();
            LoadData();
        }

        private void scopesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.Scopes).ShowDialog();
            LoadData();
        }

        private void sitesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.Sites).ShowDialog();
            LoadData();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to quit?", "Quitting?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        // Context Menu Strips

        private void ascendingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            descendingToolStripMenuItem.Checked = !ascendingToolStripMenuItem.Checked;
        }

        private void descendingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ascendingToolStripMenuItem.Checked = !descendingToolStripMenuItem.Checked;
        }

        private void btnSortOptions_Click(object sender, EventArgs e)
        {
            cmsSortingOptions.Show(btnSortOptions, new Point(btnSortOptions.Width, btnSortOptions.Height));
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FocusSearchBox();
        }

        // Trick 1

        private void FocusSearchBox()
        {
            tbSearchQuery.Focus();
            timer1.Tick += SearchBoxFade;
        }

        int SearchBoxFadeProgress = 100;

        private void SearchBoxFade(object sender, EventArgs e)
        {
            tbSearchQuery.BackColor = Color.FromArgb(255, 255, SearchBoxFadeProgress);

            if (SearchBoxFadeProgress == 255)
            {
                timer1.Tick -= SearchBoxFade;
                SearchBoxFadeProgress = 100;
            }
            else
                SearchBoxFadeProgress++;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string id = tbCollectionUuid.Text;
                DateTime captureDateTime = DateTime.Parse(tbDateTime.Text);
                Catalogue catalogue = listCatalogues[cbCatalogue.SelectedIndex];
                int objectId = int.Parse(tbObjectId.Text);
                string objectTitle = tbObjectTitle.Text;
                int numberFrames = Convert.ToInt32(tbTotalFrames.Value);
                FileFormat fileFormat = listFileFormats[cbFileFormat.SelectedIndex];
                ColorSpace colorSpace = listColorSpaces[cbColorSpace.SelectedIndex];
                Camera camera = listCameras[cbCamera.SelectedIndex];
                Scope scope = listScopes[cbScope.SelectedIndex];
                Site site = listSites[cbSites.SelectedIndex];
                List<Optic> optics = new List<Optic>();

                foreach (var item in clbOptics.CheckedItems)
                {
                    int opticId = int.Parse(item.ToString().Split('|')[0]);
                    optics.Add(listOptics.Where(x => x.Id == opticId).ToList()[0]);
                }

                Photographer photographer = listPhotographers[cbPhotographers.SelectedIndex];
                Size resolution = Operations.ParseResolution(tbResolutionX.Text + ";" + tbResolutionY.Text);
                string comments = tbComments.Text;

                Collection collection = new Collection(id, captureDateTime, catalogue, objectId, objectTitle,
                    numberFrames, fileFormat, colorSpace, camera, scope, site, optics, photographer,
                    resolution, comments);

                if (EditingMode == frmManager.EditingModes.Add)
                    Operations.AddCollection(collection);
                else
                    Operations.EditCollection(id, collection);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                //return;
                throw;
            }
            LoadData();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tbCollectionUuid.Text = GenerateUUID();
            tbDateTime.Text = "";
            cbCatalogue.SelectedIndex = -1;
            tbObjectId.Text = "";
            tbObjectTitle.Text = "";
            tbTotalFrames.Value = 1;
            cbFileFormat.SelectedIndex = -1;
            cbColorSpace.SelectedIndex = -1;
            cbCamera.SelectedIndex = -1;
            cbScope.SelectedIndex = -1;
            cbSites.SelectedIndex = -1;
            cbPhotographers.SelectedIndex = -1;
            tbResolutionX.Text = "";
            tbResolutionY.Text = "";
            tbComments.Text = "";
            LoadData();
        }
    }
}