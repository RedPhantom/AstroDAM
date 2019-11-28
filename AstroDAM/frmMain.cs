using AstroDAM.Models;
using AstroDAM.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

            PopulateTreeView();
            ClearData();
        }

        private void ClearData()
        {
            listCameras = Operations.GetCameras();
            listCatalogues = Operations.GetCatalogues();
            listColorSpaces = Operations.GetColorSpaces();
            listFileFormats = Operations.GetFileFormats();
            listOptics = Operations.GetOptics();
            listPhotographers = Operations.GetPhotographers();
            listScopes = Operations.GetScopes();
            listSites = Operations.GetSites();

            tbCollectionId.Text = "(adding)";
            tbDateTime.Text = "";
            tbComments.Text = "";
            tbFile.Text = "";
            tbMetadataFile.Text = "";
            tbObjectId.Text = "";
            tbObjectTitle.Text = "";
            tbResolutionX.Text = "";
            tbResolutionY.Text = "";
            tbTotalFrames.Value = 1;

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

            cbPhotographer.Items.Clear();
            foreach (var item in listPhotographers)
                cbPhotographer.Items.Add(item.GetInformalName());

            cbScope.Items.Clear();
            foreach (var item in listScopes)
                cbScope.Items.Add(item.GetScopeName());

            clbOptics.Items.Clear();
            foreach (var item in listOptics)
                clbOptics.Items.Add(item.Id + "|  " + item.GetOpticName());

            cbSite.Items.Clear();
            foreach (var item in listSites)
                cbSite.Items.Add(item.Name);

            lblStatus.Text = "Loaded all assets.";
        }

        //private string GenerateUUID()
        //{
        //    // Regenerates a UUID.
        //    byte[] uuid = System.Guid.NewGuid().ToByteArray();
        //    string uuidString = Convert.ToBase64String(uuid).Replace("=", "").Replace("+", "").Replace("/", "").Substring(0, 16);

        //    return uuidString;
        //}

        // Buttons

        // Toolstrip
        private void databaseConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmDbConnection().ShowDialog();
        }

        private void camerasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.Cameras).ShowDialog();
            ClearData();
        }

        private void cataloguesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.Catalogues).ShowDialog();
            ClearData();
        }

        private void colorSpacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.ColorSpaces).ShowDialog();
            ClearData();
        }

        private void formatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.FileFormats).ShowDialog();
            ClearData();
        }

        private void opticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.Optics).ShowDialog();
            ClearData();
        }

        private void photographersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.Photographers).ShowDialog();
            ClearData();
        }

        private void scopesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.Scopes).ShowDialog();
            ClearData();
        }

        private void sitesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManager(frmManager.ManagerTabs.Sites).ShowDialog();
            ClearData();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int id = 0;
                int.TryParse(tbCollectionId.Text, out id);

                DateTime captureDateTime = DateTime.Parse(tbDateTime.Text);
                Catalogue catalogue = listCatalogues[cbCatalogue.SelectedIndex];
                int objectId = int.Parse(tbObjectId.Text);
                string objectTitle = tbObjectTitle.Text;
                int numberFrames = Convert.ToInt32(tbTotalFrames.Value);
                FileFormat fileFormat = listFileFormats[cbFileFormat.SelectedIndex];
                ColorSpace colorSpace = listColorSpaces[cbColorSpace.SelectedIndex];
                Camera camera = listCameras[cbCamera.SelectedIndex];
                Scope scope = listScopes[cbScope.SelectedIndex];
                Site site = listSites[cbSite.SelectedIndex];
                List<Optic> optics = new List<Optic>();

                foreach (var item in clbOptics.CheckedItems)
                {
                    int opticId = int.Parse(item.ToString().Split('|')[0]);
                    optics.Add(listOptics.Where(x => x.Id == opticId).ToList()[0]);
                }

                Photographer photographer = listPhotographers[cbPhotographer.SelectedIndex];
                Size resolution = Operations.ParseResolution(tbResolutionX.Text + ";" + tbResolutionY.Text);
                string comments = tbComments.Text;
                string fileName = tbFile.Text;
                string metadataFile = tbMetadataFile.Text;

                Collection collection = new Collection(id, captureDateTime, catalogue, objectId, objectTitle,
                    numberFrames, fileFormat, colorSpace, camera, scope, site, optics, photographer,
                    resolution, comments, fileName, metadataFile);

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
            ClearData();
            PopulateTreeView();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tbCollectionId.Text = "(adding)";
            tbDateTime.Text = "";
            cbCatalogue.SelectedIndex = -1;
            tbObjectId.Text = "";
            tbObjectTitle.Text = "";
            tbTotalFrames.Value = 1;
            cbFileFormat.SelectedIndex = -1;
            cbColorSpace.SelectedIndex = -1;
            cbCamera.SelectedIndex = -1;
            cbScope.SelectedIndex = -1;
            cbSite.SelectedIndex = -1;
            cbPhotographer.SelectedIndex = -1;
            tbResolutionX.Text = "";
            tbResolutionY.Text = "";
            tbComments.Text = "";
            ClearData();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to quit?", "Quitting?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void tvCollections_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Name[0] == 'i') // if the selected node is a collection item.
            {
                int id = int.Parse(e.Node.Name.Substring(1));
                ClearData();
                PopulateFields(id);
            }
        }

        // fills the different fields in the collection view.
        private void PopulateFields(int id)
        {
            Collection collection = Operations.GetCollections(new List<int>() { id })[0];

            tbCollectionId.Text = collection.Id.ToString();
            tbDateTime.Text = collection.CaptureDateTime.ToString("yyyy-MM-Dd hh:mm:ss");
            cbCatalogue.SelectedIndex = listCatalogues.IndexOf(listCatalogues.Find(x => x.Id == collection.Catalogue.Id));
            tbObjectId.Text = collection.Object.ToString();
            tbObjectTitle.Text = collection.ObjectTitle;
            tbMetadataFile.Text = collection.MetaDataFileName;
            tbFile.Text = collection.FileName;
            tbTotalFrames.Value = collection.NumberFrames;
            cbFileFormat.SelectedIndex = listFileFormats.IndexOf(listFileFormats.Find(x => x.Id == collection.FileFormat.Id));
            cbColorSpace.SelectedIndex = listColorSpaces.IndexOf(listColorSpaces.Find(x => x.Id == collection.ColorSpace.Id));
            tbResolutionX.Text = collection.Resolution.Width.ToString();
            tbResolutionY.Text = collection.Resolution.Height.ToString();
            cbPhotographer.SelectedIndex = listPhotographers.IndexOf(listPhotographers.Find(x => x.Id == collection.Photographer.Id));
            cbSite.SelectedIndex = listSites.IndexOf(listSites.Find(x => x.Id == collection.Site.Id));
            tbComments.Text = collection.Comments;
            cbScope.SelectedIndex = listScopes.IndexOf(listScopes.Find(x => x.Id == collection.Scope.Id));
            cbCamera.SelectedIndex = listCameras.IndexOf(listCameras.Find(x => x.Id == collection.Camera.Id));

            clbOptics.Items.Clear();
            bool opticChecked;

            foreach (var item in listOptics)
            {
                opticChecked = collection.Optics.Contains(item);
                clbOptics.Items.Add(item.Id + "|  " + item.OpticType + " " + item.Value, opticChecked);
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

        private void btnLinkMetadataFile_Click(object sender, EventArgs e)
        {
            frmFileImporter importer = new frmFileImporter(frmFileImporter.FileTypes.MetadataFile);

            if (importer.ShowDialog() == DialogResult.OK)
            {
                tbMetadataFile.Text = importer.DialogRes.FilePath;

                if (importer.DialogRes.ImportFileMetadata)
                {
                    ParseMetadataFile(importer.DialogRes.Metadata, importer.DialogRes.UnparsableLines);
                }
            }
        }

        private void ParseMetadataFile(Dictionary<string, string> metadata, string[] unparsableLines)
        {
            int numPramsImported = 0;

            // -- FireCapture file: --

            // -- SharpCap file: --
            
            // camera model - if one of the parsable lines contains "[" and "]".
            var possibleCameras = unparsableLines.Select(x => x.Contains("[") && x.Contains("]"));

            if (possibleCameras.Count() > 0)
            {
                string cameraModel = unparsableLines[0].Replace("[", "").Replace("]", "");
                Camera cameraCandidate = listCameras.Find(x => x.LongName == cameraModel);

                if (cameraCandidate != null)
                {
                    cbCamera.SelectedIndex = listCameras.IndexOf(cameraCandidate);
                    numPramsImported++;
                }
            }
            
            // file extension
            if (metadata.ContainsKey("Output Format"))
            {
                string fileExtension = Regex.Match(metadata["Output Format"], @"\(([^)]*)\)").Groups[1].Value.Substring(2); // skip *.
                FileFormat fileFormatCandidate = listFileFormats.Find(x => x.ShortName.ToLower().Contains(fileExtension));

                if (fileFormatCandidate != null)
                {
                    cbFileFormat.SelectedIndex = listFileFormats.IndexOf(fileFormatCandidate);
                    numPramsImported++;
                }
            }

            // capture area
            if (metadata.ContainsKey("Capture Area"))
            {
                if (metadata["Capture Area"].Contains('x'))
                {
                    string[] captureArea = metadata["Capture Area"].Split('x');

                    tbResolutionX.Text = captureArea[0];
                    tbResolutionY.Text = captureArea[1];
                    numPramsImported++;
                }
            }

            // color space
            if (metadata.ContainsKey("Colour Space"))
            {
                string colorSpace = metadata["Colour Space"];
                ColorSpace colorSpaceCandidate = listColorSpaces.Find(x => x.Name == colorSpace);

                if (colorSpaceCandidate != null)
                {
                    cbColorSpace.SelectedIndex = listColorSpaces.IndexOf(colorSpaceCandidate);
                    numPramsImported++;
                }
            }

            // timestamp
            if (metadata.ContainsKey("TimeStamp"))
            {
                string timestamp = metadata["TimeStamp"];
                tbDateTime.Text = timestamp.Replace("T", " ").Substring(0,19);
                numPramsImported++;
            }

            if (numPramsImported > 0)
                lblStatus.Text = string.Format("Successfully imported {0} parameters from metadata.", numPramsImported);
            else
                lblStatus.Text = "Failed to import data from metadata.";
        }

        private void aboutAstroDAMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmAbout().ShowDialog();
        }

        private void btnLinkFile_Click(object sender, EventArgs e)
        {
            frmFileImporter importer = new frmFileImporter(frmFileImporter.FileTypes.CaptureFile);

            if (importer.ShowDialog() == DialogResult.OK)
            {
                tbFile.Text = importer.DialogRes.FilePath;

                if (importer.DialogRes.ImportFileMetadata)
                {
                    //ParseCaptureFile(importer.DialogRes.FilePath); TODO
                }
            }
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmPreferences().ShowDialog();
        }

        private void PopulateTreeView()
        {
            tvCollections.Nodes.Clear();
            Operations.PopulateTreeView(ref tvCollections, ascendingToolStripMenuItem.Checked);
        }

        private void refreshNavtreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PopulateTreeView();
        }
    }
}